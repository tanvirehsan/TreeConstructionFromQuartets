namespace TreeConstructionFromQuartets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TreeConstructionFromQuartets.Model;
    public class Bipartition
    {
        private List<Quartet> Set_Of_Quartets = new List<Quartet>();
        private List<Quartet> Set_Of_Distinct_Quartets = new List<Quartet>();

        private List<Quartet> Set_Of_Duplicate_Quartets = new List<Quartet>();
        private List<Quartet> Set_Of_DistinctDuplicate_Quartets = new List<Quartet>();

        private List<Quartet> Set_Of_Sorted_Distinct_Quartets = new List<Quartet>();

        private List<string> Set_Of_Taxa = new List<string>();

        private List<Taxa> _Set_Of_TaxaWithScore = new List<Taxa>();
        private InputProcessing inputData;


        List<Taxa> _TaxaGainTable = new List<Taxa>();
        List<GainTable> _GainTableList = new List<GainTable>();


        public GainTable getFinalGainTable()
        {
            if (this._GainTableList.Count != 0)
                return this._GainTableList[0];
            return new GainTable();
        }
        public Bipartition(InputProcessing input)
        {
            this.inputData = input;
            this.Set_Of_Quartets = inputData.Get_SetOfQuartets();
            this.Set_Of_Taxa = inputData.Get_SetOfTaxa();
            SetDistinctQuartets();
            SetDistinctDuplicateQuartets();
            SetDistinctQuartetsAfterAddingDuplicateQuartets();
            SetSortedDistinctQuartets();
            InitialBipartition(0);
            this.PartitionSets[0]._ListQuatrets = Set_Of_Sorted_Distinct_Quartets;
            PartitionSet p = this.PartitionSets[0];
            SetSortedDistinctQuartetsWithPartitionStatus(ref p, this.PartitionSets[0]);
            //OutputProcessing.WriteInititalBiPartion(p, this.Set_Of_Taxa); 
            GenerateGainTable(p);
            
        }



        public void GenerateGainTable(PartitionSet p)
        {
            //------------ Calculate Gain Summary
            CalculateGainSummary(p, _TaxaGainTable);

            // ---- Calculate Second Gain Summary ------------------------
            // Cleaning Up all the Data before Calculation

            if (Get_Set_Of_Sorted_Distinct_Quartets_Original().Count != 0)
            {
                _GainTableList[0].PartitionSet._ListQuatrets = new List<Quartet>(Get_Set_Of_Sorted_Distinct_Quartets_Original());

                p = _GainTableList[0].PartitionSet;
                foreach (Partition pp in p.PartitionList)
                {

                    foreach (Taxa tb in pp.TaxaList)
                    {
                        tb._CumulativeGain = 0;
                        tb._DifferedCount = 0;
                        tb._Gain = 0;
                        tb._IsolatedCount = 0;
                        tb._SatisfiedCount = 0;
                        tb._ViotatedCount = 0;
                        tb.IsFreeze = false;
                    }

                }
                p._DifferedCount = 0;
                p._IsolatedCount = 0;
                p._Final_Score = 0;
                p._Gain = 0;
                p._SatisfiedCount = 0;
                p._taxValueForGainCalculation = string.Empty;

                SetSortedDistinctQuartetsWithPartitionStatus(ref p, _GainTableList[0].PartitionSet);
                _TaxaGainTable = new List<Taxa>();
                CalculateGainSummary(p, _TaxaGainTable);

                if (_GainTableList[0]._MaxCumulativeGain >= _GainTableList[1]._MaxCumulativeGain)
                {
                    _GainTableList.RemoveAt(1);
                    return;
                }
                else
                {
                    _GainTableList.RemoveAt(0);
                    GenerateGainTable(_GainTableList[0].PartitionSet);
                }
            }
            else
            {
                return;
            }
            //----------------End Of Calculationn Gain Summary
        }

        public void CalculateGainSummary(PartitionSet p, List<Taxa> _TaxaGainTable)
        {
            GainTable Gt = new GainTable();
            LoadSet_Of_TaxaWithScore();
            LaterPartition(p, this._Set_Of_TaxaWithScore, this.Set_Of_Taxa.Count());
            int Step = 0;
            int _CGain = 0;
            foreach (Taxa tx in _TaxaGainTable)
            {

                _CGain = _CGain + tx._Gain;
                tx._CumulativeGain = _CGain;
                tx.StepK = Step;
                Step++;
            }

            if (_TaxaGainTable.Count != 0)
            {
                var maximumCGain = _TaxaGainTable.Max(x => x._CumulativeGain);
                if (maximumCGain != null)
                {
                    var TotalCount = _TaxaGainTable.Where(x => x._CumulativeGain == maximumCGain).Count();
                    if (TotalCount != null)
                    {
                        if (TotalCount > 0)
                        {
                            foreach (Taxa tt in _TaxaGainTable)
                            {
                                if (tt._CumulativeGain == maximumCGain)
                                {
                                    Gt = new GainTable();
                                    Gt.PartitionSet = tt._TaxaPartitionSet;
                                    Gt.TaxValue = tt._Taxa_Value;
                                    Gt._MaxCumulativeGain = tt._CumulativeGain;
                                    Gt.MaximumGainOfTaxValue = tt._Gain;
                                    Gt._TaxaGainSummary = new List<Taxa>(_TaxaGainTable);
                                    _GainTableList.Add(Gt);
                                    break;
                                }
                            }


                        }
                    }
                }
            }

        }

        public void LoadSet_Of_TaxaWithScore()
        {
            this._Set_Of_TaxaWithScore = new List<Taxa>();
            Taxa t;
            foreach (string taxa in Set_Of_Taxa)
            {
                t = new Taxa();
                t._Taxa_Value = taxa;
                t.IsFreeze = false;
                this._Set_Of_TaxaWithScore.Add(t);
            }
        }


        public bool isTransferrable(List<Taxa> pSet_Of_TaxaWithScore, PartitionSet p)
        {
            List<Taxa> _Set_Of_TaxaWithScore = new List<Taxa>(pSet_Of_TaxaWithScore);
            _Set_Of_TaxaWithScore.RemoveAll(x => x.IsFreeze == true);
            int _NUMBER_OF_FREE_TAXA = 0;
            _NUMBER_OF_FREE_TAXA = _Set_Of_TaxaWithScore.Count();

            if (_NUMBER_OF_FREE_TAXA >= 1 && ((p.PartitionList[0].TaxaList.Count != 1 && p.PartitionList[1].TaxaList.Count > 1) || (p.PartitionList[1].TaxaList.Count != 1 && p.PartitionList[0].TaxaList.Count > 1)))
            {
                return true;
            }

            return false;


        }
        public void LaterPartition(PartitionSet InitialPartitionSet, List<Taxa> pSet_Of_TaxaWithScore, int _GlobalCountTaxaValue)
        {


            int count = 1;
            PartitionSet p;

            List<Taxa> _Set_Of_TaxaWithScore = new List<Taxa>(pSet_Of_TaxaWithScore);
            _Set_Of_TaxaWithScore.RemoveAll(x => x.IsFreeze == true);


            Taxa _Current_Moved_Taxa = new Taxa();

            Taxa MaximumGainedTaxa = new Taxa();
            foreach (Taxa tx in _Set_Of_TaxaWithScore)
            {


                if (tx.IsFreeze == false)
                {
                    p = ChangePositionForGainCalculation(count, InitialPartitionSet, tx._Taxa_Value);



                    SetSortedDistinctQuartetsWithPartitionStatus(ref p, InitialPartitionSet);

                    tx._IsolatedCount = p._IsolatedCount;
                    tx._ViotatedCount = p._ViotatedCount;
                    tx._DifferedCount = p._DifferedCount;
                    tx._SatisfiedCount = p._SatisfiedCount;
                    tx._TaxaPartitionSet = p;
                    tx._Gain = p._Gain;
                    _Current_Moved_Taxa = tx;
                    // Checking the Singleton 
                    if (!isTransferrable(_Set_Of_TaxaWithScore, p))
                    {
                        p = null;
                        p = InitialPartitionSet;
                        tx._TaxaPartitionSet = p;

                    }



                }
                count++;
            }


            if (!string.IsNullOrEmpty(_Current_Moved_Taxa._Taxa_Value))
            {
                var maxGain = _Set_Of_TaxaWithScore.Where(x => x.IsFreeze == false).Max(x => x._Gain);
                if (maxGain != null)
                {

                    var TotCount = _Set_Of_TaxaWithScore.Where(x => x._Gain == maxGain && x.IsFreeze == false).Count();
                    if (TotCount != null)
                    {
                        var maxGainedRow = _Set_Of_TaxaWithScore.Where(x => x._Gain == maxGain && x.IsFreeze == false).ToList();

                        if (maxGainedRow != null)
                        {
                            if (TotCount > 1)
                            {
                                var maximumValueSatisfiedCount = maxGainedRow.Max(x => x._SatisfiedCount);
                                MaximumGainedTaxa = maxGainedRow.Where(x => x._SatisfiedCount == maximumValueSatisfiedCount && x.IsFreeze == false).FirstOrDefault();
                            }
                            else
                                MaximumGainedTaxa = maxGainedRow.Where(x => x._Gain == maxGain && x.IsFreeze == false).FirstOrDefault();

                            foreach (Partition pp in MaximumGainedTaxa._TaxaPartitionSet.PartitionList)
                            {

                                foreach (Taxa ttpp in pp.TaxaList)
                                {
                                    if (ttpp._Taxa_Value == MaximumGainedTaxa._Taxa_Value)
                                    {

                                        ttpp.IsFreeze = true;

                                    }

                                }

                            }
                            foreach (Taxa txx in _Set_Of_TaxaWithScore)
                            {
                                if (txx._Taxa_Value == MaximumGainedTaxa._Taxa_Value)
                                {
                                    txx.IsFreeze = true;

                                }
                            }

                            MaximumGainedTaxa.IsFreeze = true;
                            _TaxaGainTable.Add(MaximumGainedTaxa);
                            _GlobalCountTaxaValue--;

                        }

                    }
                }
               
            }

            else
            {

                

            } 

            if (_GlobalCountTaxaValue == 0)
            {
                return;
            }
            else
            {
                LaterPartition(MaximumGainedTaxa._TaxaPartitionSet, _Set_Of_TaxaWithScore, _GlobalCountTaxaValue);
            }


        }


        public PartitionSet ChangePositionForGainCalculation(int i, PartitionSet InitialPartion, string pTaxaValue)
        {

            PartitionSet p = new PartitionSet("Partition" + i.ToString());

            Partition PA0 = new Partition("PartitionA" + i.ToString());
            Partition PB0 = new Partition("PartitionB" + i.ToString());

            List<Taxa> TaxaListA0 = new List<Taxa>(InitialPartion.PartitionList[0].TaxaList);
            List<Taxa> TaxaListB0 = new List<Taxa>(InitialPartion.PartitionList[1].TaxaList);

            foreach (Taxa t in InitialPartion.PartitionList[0].TaxaList)
            {
                // Need Checking the List count greater than 2 to avoid SingleTone Bipartition 
                //if (t._Taxa_Value == pTaxaValue && t.IsFreeze == false && TaxaListA0.Count > 2)
                if (t._Taxa_Value == pTaxaValue && t.IsFreeze == false)
                {

                    p._taxValueForGainCalculation = pTaxaValue;
                    TaxaListA0.Remove(t);
                    TaxaListB0.Add(t);

                }

            }

            foreach (Taxa t in InitialPartion.PartitionList[1].TaxaList)
            {
                // Need Checking the List count greater than 2 to avoid SingleTone Bipartition 
                //if (t._Taxa_Value == pTaxaValue && t.IsFreeze == false && TaxaListB0.Count > 2)
                if (t._Taxa_Value == pTaxaValue && t.IsFreeze == false)
                {
                    p._taxValueForGainCalculation = pTaxaValue;
                    TaxaListB0.Remove(t);
                    TaxaListA0.Add(t);

                }

            }

            PA0.TaxaList = TaxaListA0;
            PB0.TaxaList = TaxaListB0;


            p.PartitionList = new List<Partition>();
            p._ListQuatrets = new List<Quartet>(Get_Set_Of_Sorted_Distinct_Quartets_Original());

            p.PartitionList.Add(PA0);
            p.PartitionList.Add(PB0);

            return p;
            // this.PartitionSets.Add(p);

        }


        public void SetSortedDistinctQuartetsWithPartitionStatus(ref PartitionSet set, PartitionSet setInitial)
        {

            foreach (Quartet q in set._ListQuatrets)
            {

                q._PartitionStatus = getPartitionStatus(q, set);
            }

            var IsolatedCount = set._ListQuatrets.Where(x => x._PartitionStatus == PartitionStatus.Isolated).Count();
            var ViotatedCount = set._ListQuatrets.Where(x => x._PartitionStatus == PartitionStatus.Viotated).Count();
            var DifferedCount = set._ListQuatrets.Where(x => x._PartitionStatus == PartitionStatus.Differed).Count();
            var SatisfiedCount = set._ListQuatrets.Where(x => x._PartitionStatus == PartitionStatus.Satisfied).Count();
            set._IsolatedCount = IsolatedCount;
            set._ViotatedCount = ViotatedCount;
            set._DifferedCount = DifferedCount;
            set._SatisfiedCount = SatisfiedCount;
            set._Final_Score = set._SatisfiedCount - set._ViotatedCount;
            set._Gain = set._Final_Score - setInitial._Final_Score;

        }



        public void SetSortedDistinctQuartets()
        {
            var list = from element in Set_Of_Distinct_Quartets
                       orderby element._Frequency descending
                       select element;
            Set_Of_Sorted_Distinct_Quartets = new List<Quartet>(list);

        }

        public List<Quartet> Get_Set_Of_Sorted_Distinct_Quartets_Original()
        {
            List<Quartet> Set_Of_Sorted_Distinct_Quartets_Original = new List<Quartet>();
            foreach (Quartet q in Set_Of_Sorted_Distinct_Quartets)
            {
                Quartet qq = new Quartet();
                qq._DuplicateQuatrets = q._DuplicateQuatrets;
                qq._First_Taxa_Value = q._First_Taxa_Value;
                qq._Second_Taxa_Value = q._Second_Taxa_Value;
                qq._Third_Taxa_Value = q._Third_Taxa_Value;
                qq._Fourth_Taxa_Value = q._Fourth_Taxa_Value;
                qq._Quartet_Name = q._Quartet_Name;
                qq._Quartet_Input = q._Quartet_Input;
                qq._Quartet_LeftPart = q._Quartet_LeftPart;
                qq._Quartet_LeftPartReverse = q._Quartet_LeftPartReverse;
                qq._Quartet_RightPart = q._Quartet_RightPart;
                qq._Quartet_RightPartReverse = q._Quartet_RightPartReverse;
                qq._isDistinct = q._isDistinct;
                qq._Frequency = q._Frequency;
                qq._DuplicateQuatrets = q._DuplicateQuatrets;
                qq._PartitionStatus = PartitionStatus.None;
                Set_Of_Sorted_Distinct_Quartets_Original.Add(qq);
            }
            return Set_Of_Sorted_Distinct_Quartets_Original;
        }

        public void SetDistinctQuartetsAfterAddingDuplicateQuartets()
        {
            foreach (Quartet objQuartetInner in Set_Of_DistinctDuplicate_Quartets)
            {
                Set_Of_Distinct_Quartets.Add(objQuartetInner);
            }
        }

        public void SetDistinctQuartets()
        {
            int count = 0;
            List<string> _Quatret = new List<string>();
            foreach (Quartet objQuartet in Set_Of_Quartets)
            {
                count = 0;
                _Quatret = new List<string>();
                foreach (Quartet objQuartetInner in Set_Of_Quartets)
                {
                    if (objQuartet._Quartet_Name != objQuartetInner._Quartet_Name)
                    {
                        if (CheckValue(objQuartet._Quartet_LeftPart, objQuartet._Quartet_RightPart, objQuartetInner._Quartet_LeftPart, objQuartetInner._Quartet_LeftPartReverse, objQuartetInner._Quartet_RightPart, objQuartetInner._Quartet_RightPartReverse))
                        {

                            count++;
                            _Quatret.Add(objQuartetInner._Quartet_Name);
                        }

                    }

                }
                objQuartet._Frequency = count + 1;
                if (count == 0)
                {
                    objQuartet._isDistinct = true;
                    Set_Of_Distinct_Quartets.Add(objQuartet);
                }
                else
                {
                    objQuartet._isDistinct = false;
                    objQuartet._DuplicateQuatrets = _Quatret;


                    Set_Of_Duplicate_Quartets.Add(objQuartet);
                }

            }



        }

        public void SetDistinctDuplicateQuartets()
        {


            for (int i = 0; i < Set_Of_Duplicate_Quartets.Count; i++)
            {
                var findQuatret = Set_Of_Duplicate_Quartets[i];
                var findQuatretName = findQuatret._Quartet_Name;

                if (i == 0)
                {
                    Set_Of_DistinctDuplicate_Quartets.Add(findQuatret);
                }
                else
                {

                    if (isNotExistInDistinctDuplicate(findQuatret))
                    {
                        Set_Of_DistinctDuplicate_Quartets.Add(findQuatret);
                    }

                }
            }
        }

        public bool isNotExistInDistinctDuplicate(Quartet findQuatret)
        {
            int count = 0;
            for (int k = 0; k < Set_Of_DistinctDuplicate_Quartets.Count; k++)
            {
                var CurrQuatret = Set_Of_DistinctDuplicate_Quartets[k];

                if (findQuatret._Quartet_Name != CurrQuatret._Quartet_Name)
                {
                    if (CheckValue(findQuatret._Quartet_LeftPart, findQuatret._Quartet_RightPart, CurrQuatret._Quartet_LeftPart, CurrQuatret._Quartet_LeftPartReverse, CurrQuatret._Quartet_RightPart, CurrQuatret._Quartet_RightPartReverse))
                    {

                        count++;

                    }
                }
            }
            if (count == 0)
                return true;
            return false;

        }

        public bool CheckValue(string _Quartet_LeftPart, string _Quartet_RightPart, string InnerQuartet_LeftPart, string InnerQuartet_LeftPartReverse, string InnerQuartet_RightPart, string InnerQuartet_RightPartReverse)
        {
            if (

                                       (_Quartet_LeftPart.Equals(InnerQuartet_LeftPart) ||
                                       _Quartet_LeftPart.Equals(InnerQuartet_LeftPartReverse) ||
                                       _Quartet_LeftPart.Equals(InnerQuartet_RightPart) ||
                                       _Quartet_LeftPart.Equals(InnerQuartet_RightPartReverse)
                                       )
                                       &&
                                       (_Quartet_RightPart.Equals(InnerQuartet_LeftPart) ||
                                       _Quartet_RightPart.Equals(InnerQuartet_LeftPartReverse) ||
                                       _Quartet_RightPart.Equals(InnerQuartet_RightPart) ||
                                       _Quartet_RightPart.Equals(InnerQuartet_RightPartReverse)
                                       )

                                       )
            {

                return true;

            }

            return false;
        }

        public List<Quartet> getSetOfSortedDistinctQuartets()
        {
            return this.Set_Of_Sorted_Distinct_Quartets;
        }


        private List<PartitionSet> PartitionSets = new List<PartitionSet>();

        public void InitialBipartition(int PartitionSetNo)
        {
            PartitionSet Partition = new PartitionSet("Partition" + PartitionSetNo.ToString());
            List<Partition> Partitions = new List<Partition>();

            Partition _PartitionA;
            Partition _PartitionB;
            int index = 0;
            _PartitionA = new Partition("A" + index.ToString());
            _PartitionB = new Partition("B" + index.ToString());

            List<string> SetOfTaxa = new List<string>(this.Set_Of_Taxa);


            string t1 = string.Empty;
            string t2 = string.Empty;
            string t3 = string.Empty;
            string t4 = string.Empty;


            foreach (Quartet q in Set_Of_Sorted_Distinct_Quartets)
            {
                t1 = q._First_Taxa_Value;
                t2 = q._Second_Taxa_Value;
                t3 = q._Third_Taxa_Value;
                t4 = q._Fourth_Taxa_Value;
                var Qurtet_Name = q._Quartet_Name;

                if (!CheckForNoneOfTheValuesBelongTo(q._Quartet_Name, _PartitionA, _PartitionB, q._First_Taxa_Value, q._Second_Taxa_Value, q._Third_Taxa_Value, q._Fourth_Taxa_Value))
                {
                    // If none of the 4 taxa belongs to either PartitionSet then Insert t1 and t2 in Pa0 and t3 and t4 in Pb0
                    _PartitionA.TaxaList.Add(GetTaxa(t1, Qurtet_Name, 0));
                    SetOfTaxa.Remove(t1);
                    _PartitionA.TaxaList.Add(GetTaxa(t2, Qurtet_Name, 1));
                    SetOfTaxa.Remove(t2);
                    _PartitionB.TaxaList.Add(GetTaxa(t3, Qurtet_Name, 2));
                    SetOfTaxa.Remove(t3);
                    _PartitionB.TaxaList.Add(GetTaxa(t4, Qurtet_Name, 3));
                    SetOfTaxa.Remove(t4);
                }
                else
                {


                    //  insert t1
                    if (!isBelongToPartition(_PartitionA, t1) && !isBelongToPartition(_PartitionB, t1))
                    {
                        if (isBelongToPartition(_PartitionA, t2))
                        {
                            _PartitionA.TaxaList.Add(GetTaxa(t1, Qurtet_Name, 0));
                            SetOfTaxa.Remove(t1);
                        }
                        else if (isBelongToPartition(_PartitionB, t2))
                        {
                            _PartitionB.TaxaList.Add(GetTaxa(t1, Qurtet_Name, 0));
                            SetOfTaxa.Remove(t1);
                        }
                        else
                        {
                            if (isBelongToPartition(_PartitionA, t3))
                            {
                                _PartitionB.TaxaList.Add(GetTaxa(t1, Qurtet_Name, 0));
                                SetOfTaxa.Remove(t1);
                            }
                            else if (isBelongToPartition(_PartitionB, t3))
                            {
                                _PartitionA.TaxaList.Add(GetTaxa(t1, Qurtet_Name, 0));
                                SetOfTaxa.Remove(t1);
                            }
                            else
                            {
                                if (isBelongToPartition(_PartitionA, t4))
                                {
                                    _PartitionB.TaxaList.Add(GetTaxa(t1, Qurtet_Name, 0));
                                    SetOfTaxa.Remove(t1);
                                }
                                else if (isBelongToPartition(_PartitionB, t4))
                                {
                                    _PartitionA.TaxaList.Add(GetTaxa(t1, Qurtet_Name, 0));
                                    SetOfTaxa.Remove(t1);
                                }
                            }
                        }

                    }
                    //insert t2
                    else if (!isBelongToPartition(_PartitionA, t2) && !isBelongToPartition(_PartitionB, t2))
                    {
                        if (isBelongToPartition(_PartitionA, t1))
                        {
                            _PartitionA.TaxaList.Add(GetTaxa(t2, Qurtet_Name, 1));
                            SetOfTaxa.Remove(t2);
                        }
                        else if (isBelongToPartition(_PartitionB, t1))
                        {
                            _PartitionB.TaxaList.Add(GetTaxa(t2, Qurtet_Name, 1));
                            SetOfTaxa.Remove(t2);
                        }
                    }

                    //insert t3
                    else if (!isBelongToPartition(_PartitionA, t3) && !isBelongToPartition(_PartitionB, t3))
                    {
                        if (isBelongToPartition(_PartitionA, t4))
                        {
                            _PartitionA.TaxaList.Add(GetTaxa(t3, Qurtet_Name, 2));
                            SetOfTaxa.Remove(t3);
                        }
                        else if (isBelongToPartition(_PartitionB, t4))
                        {
                            _PartitionB.TaxaList.Add(GetTaxa(t3, Qurtet_Name, 2));
                            SetOfTaxa.Remove(t3);
                        }
                        else
                        {
                            if (isBelongToPartition(_PartitionA, t1))
                            {
                                // Add t3 to _PartitionB
                                _PartitionB.TaxaList.Add(GetTaxa(t3, Qurtet_Name, 2));
                                SetOfTaxa.Remove(t3);
                            }
                            else if (isBelongToPartition(_PartitionB, t1))
                            {
                                // Add t3 to _PartitionA
                                _PartitionA.TaxaList.Add(GetTaxa(t3, Qurtet_Name, 2));
                                SetOfTaxa.Remove(t3);
                            }
                            else if (isBelongToPartition(_PartitionA, t2))
                            {
                                // Add t3 to _PartitionB
                                _PartitionB.TaxaList.Add(GetTaxa(t3, Qurtet_Name, 2));
                                SetOfTaxa.Remove(t3);
                            }
                            else if (isBelongToPartition(_PartitionB, t2))
                            {
                                // Add t3 to _PartitionA
                                _PartitionA.TaxaList.Add(GetTaxa(t3, Qurtet_Name, 2));
                                SetOfTaxa.Remove(t3);
                            }
                        }
                    }
                    //insert t4
                    else if (!isBelongToPartition(_PartitionA, t4) && !isBelongToPartition(_PartitionB, t4))
                    {
                        if (isBelongToPartition(_PartitionA, t3))
                        {
                            _PartitionA.TaxaList.Add(GetTaxa(t4, Qurtet_Name, 3));
                            SetOfTaxa.Remove(t4);
                        }
                        else if (isBelongToPartition(_PartitionB, t3))
                        {
                            _PartitionB.TaxaList.Add(GetTaxa(t4, Qurtet_Name, 3));
                            SetOfTaxa.Remove(t4);
                        }
                    }

                }

                //if (SetOfTaxa.Count == 0)
                //  break;

            }

            //-------------Step 4 if SetOfTaxa Remains Non Empty then  add remaining taxa to either part randomly

            Random rnd = new Random();
            int number = rnd.Next(1, 1000);
            if (number % 2 == 0)
            {
                foreach (string ii in SetOfTaxa)
                {
                    _PartitionA.TaxaList.Add(GetTaxa(ii, "", 0));
                }
            }
            else
            {
                foreach (string ii in SetOfTaxa)
                {
                    _PartitionB.TaxaList.Add(GetTaxa(ii, "", 0));
                }
            }

            //----------------------------------------End of Step 4

            Partitions.Add(_PartitionA);
            Partitions.Add(_PartitionB);
            Partition.PartitionList = Partitions;
            PartitionSets.Add(Partition);

        }

        public bool CheckForNoneOfTheValuesBelongTo(string _Quartet_Name, Partition _PartitionA, Partition _PartitionB, string _First_Taxa_Value, string _Second_Taxa_Value, string _Third_Taxa_Value, string _Fourth_Taxa_Value)
        {
            bool isFound = false;


            if (isBelongToPartition(_PartitionA, _First_Taxa_Value))
            {
                isFound = true;
            }
            if (isBelongToPartition(_PartitionB, _First_Taxa_Value))
            {
                isFound = true;
            }
            else if (isBelongToPartition(_PartitionA, _Second_Taxa_Value))
            {
                isFound = true;
            }
            else if (isBelongToPartition(_PartitionB, _Second_Taxa_Value))
            {
                isFound = true;
            }
            else if (isBelongToPartition(_PartitionA, _Third_Taxa_Value))
            {
                isFound = true;
            }
            else if (isBelongToPartition(_PartitionB, _Third_Taxa_Value))
            {
                isFound = true;
            }
            else if (isBelongToPartition(_PartitionA, _Fourth_Taxa_Value))
            {
                isFound = true;
            }
            else if (isBelongToPartition(_PartitionB, _Fourth_Taxa_Value))
            {
                isFound = true;
            }
            else
            {
                isFound = false;


            }

            return isFound;

        }

        public Taxa GetTaxa(string value, string _Quartet_Name, int position)
        {

            Taxa taxa = new Taxa();
            taxa._Taxa_Value = value;
            taxa._Quartet_Name = _Quartet_Name;
            taxa._Taxa_ValuePosition_In_Quartet = position;

            return taxa;

        }

        private bool isBelongToPartition(Partition _Partition, string taxa)
        {
            var _Taxa_Value = _Partition.TaxaList.Find(x => x._Taxa_Value == taxa);
            if (_Taxa_Value != null)
            {
                return true;
            }
            return false;
        }


        #region PartitionStatus

        public PartitionStatus getPartitionStatus(Quartet q, PartitionSet p)
        {
            if (isSatisfied(q, p))
            {
                return PartitionStatus.Satisfied;
            }
            else if (isDiffered(q, p))
            {
                return PartitionStatus.Differed;
            }
            else if (isIsolated(q, p))
            {
                return PartitionStatus.Isolated;
            }
            else if (isViolated(q, p))
            {
                return PartitionStatus.Viotated;
            }
            else
                return PartitionStatus.Viotated;

        }

        public bool isDiffered(Quartet q, PartitionSet p)
        {
            string t1 = string.Empty;
            string t2 = string.Empty;
            string t3 = string.Empty;
            string t4 = string.Empty;
            t1 = q._First_Taxa_Value;
            t2 = q._Second_Taxa_Value;
            t3 = q._Third_Taxa_Value;
            t4 = q._Fourth_Taxa_Value;

            Partition PartitionA = p.PartitionList[0];
            Partition PartitionB = p.PartitionList[1];


            if (isInPart(PartitionA, t1) && isInPart(PartitionB, t2) && isInPart(PartitionB, t3) && isInPart(PartitionB, t4))
            {
                return true;
            }
            else if (isInPart(PartitionB, t1) && isInPart(PartitionA, t2) && isInPart(PartitionA, t3) && isInPart(PartitionA, t4))
            {
                return true;
            }

            else if (isInPart(PartitionA, t2) && isInPart(PartitionB, t1) && isInPart(PartitionB, t3) && isInPart(PartitionB, t4))
            {
                return true;
            }
            else if (isInPart(PartitionB, t2) && isInPart(PartitionA, t1) && isInPart(PartitionA, t3) && isInPart(PartitionA, t4))
            {
                return true;
            }

            else if (isInPart(PartitionA, t3) && isInPart(PartitionB, t1) && isInPart(PartitionB, t2) && isInPart(PartitionB, t4))
            {
                return true;
            }
            else if (isInPart(PartitionB, t3) && isInPart(PartitionA, t1) && isInPart(PartitionA, t2) && isInPart(PartitionA, t4))
            {
                return true;
            }

            else if (isInPart(PartitionA, t4) && isInPart(PartitionB, t1) && isInPart(PartitionB, t2) && isInPart(PartitionB, t3))
            {
                return true;
            }
            else if (isInPart(PartitionB, t4) && isInPart(PartitionA, t1) && isInPart(PartitionA, t2) && isInPart(PartitionA, t3))
            {
                return true;
            }

            return false;

        }

        public bool isIsolated(Quartet q, PartitionSet p)
        {
            string t1 = string.Empty;
            string t2 = string.Empty;
            string t3 = string.Empty;
            string t4 = string.Empty;
            t1 = q._First_Taxa_Value;
            t2 = q._Second_Taxa_Value;
            t3 = q._Third_Taxa_Value;
            t4 = q._Fourth_Taxa_Value;

            Partition PartitionA = p.PartitionList[0];
            Partition PartitionB = p.PartitionList[1];



            if (isInPart(PartitionA, t1) && isInPart(PartitionA, t2) && isInPart(PartitionA, t3) && isInPart(PartitionA, t4))
            {

                return true;

            }
            else if (isInPart(PartitionB, t1) && isInPart(PartitionB, t2) && isInPart(PartitionB, t3) && isInPart(PartitionB, t4))
            {

                return true;

            }

            return false;

        }

        public bool isViolated(Quartet q, PartitionSet p)
        {
            string t1 = string.Empty;
            string t2 = string.Empty;
            string t3 = string.Empty;
            string t4 = string.Empty;
            t1 = q._First_Taxa_Value;
            t2 = q._Second_Taxa_Value;
            t3 = q._Third_Taxa_Value;
            t4 = q._Fourth_Taxa_Value;

            Partition PartitionA = p.PartitionList[0];
            Partition PartitionB = p.PartitionList[1];



            if ((isInPart(PartitionA, t1) && isInPart(PartitionB, t2)) || (isInPart(PartitionA, t2) && isInPart(PartitionB, t1)))
            {
                if ((isInPart(PartitionA, t2) && isInPart(PartitionB, t3)) || (isInPart(PartitionA, t3) && isInPart(PartitionB, t2)))
                {
                    return true;
                }
            }

            return false;

        }

        public bool isSatisfied(Quartet q, PartitionSet p)
        {
            string t1 = string.Empty;
            string t2 = string.Empty;
            string t3 = string.Empty;
            string t4 = string.Empty;
            t1 = q._First_Taxa_Value;
            t2 = q._Second_Taxa_Value;
            t3 = q._Third_Taxa_Value;
            t4 = q._Fourth_Taxa_Value;

            Partition PartitionA = p.PartitionList[0];
            Partition PartitionB = p.PartitionList[1];

            if (isInSamePart(PartitionA, t1, t2) && isInSamePart(PartitionB, t3, t4))
            {
                return true;
            }
            else if (isInSamePart(PartitionB, t1, t2) && isInSamePart(PartitionA, t3, t4))
            {
                return true;
            }
            //if (isInSamePart(PartitionA, t2, t1) && isInSamePart(PartitionB, t4, t3))
            //{
            //    return true;
            //}
            //else if (isInSamePart(PartitionB, t2, t1) && isInSamePart(PartitionA, t4, t3))
            //{
            //    return true;
            //}
            return false;
        }

        public bool isInSamePart(Partition obj, string t1, string t2)
        {
            var f1 = obj.TaxaList.FindAll(x => x._Taxa_Value == t1);
            if (f1 != null)
            {
                if (f1.Count != 0)
                {
                    var f2 = obj.TaxaList.FindAll(x => x._Taxa_Value == t2);
                    if (f2 != null)
                    {
                        if (f2.Count != 0)
                            return true;
                    }
                }
            }
            return false;
        }

        public bool isInPart(Partition obj, string t1)
        {
            var f1 = obj.TaxaList.FindAll(x => x._Taxa_Value == t1);
            if (f1 != null)
            {
                if (f1.Count != 0)
                    return true;

            }
            return false;
        }




        #endregion




    }
}
