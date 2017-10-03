namespace TreeConstructionFromQuartets
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text;
    using TreeConstructionFromQuartets.Model;
    public class ConsistancyCalculation
    {
        private List<string> _VALID_TAXA_LIST = new List<string>();

        //string PathDepthOne = ConfigurationManager.AppSettings["PathDepthOne"].ToString();

        private List<Quartet> _ALLQuatretListAfterGain = new List<Quartet>();
        private List<Quartet> _DifferedQuatretListAfterGain = new List<Quartet>();
        private List<Quartet> _IsolatedQuatretListAfterGain = new List<Quartet>();
        private List<Quartet> _ViolatedQuatretListAfterGain = new List<Quartet>();

        private List<Quartet> _DifferredConsistentAfterDevideAndConquer = new List<Quartet>();
        private List<Quartet> _DifferredInConsistentAfterDevideAndConquer = new List<Quartet>();

        private List<Quartet> _IsolatedConsistentAfterDevideAndConquer = new List<Quartet>();
        private List<Quartet> _IsolatedInConsistentAfterDevideAndConquer = new List<Quartet>();

        private List<Quartet> _ViolatedConsistentAfterDevideAndConquer = new List<Quartet>();
        private List<Quartet> _ViolatedInConsistentAfterDevideAndConquer = new List<Quartet>();

        private List<Quartet> _RandomConsistentAfterDevideAndConquer = new List<Quartet>();
        private List<Quartet> _RandomInConsistentAfterDevideAndConquer = new List<Quartet>();

        private GainTable _FinalGainTableAfterGainCalculation = new GainTable();
        List<ConsistencyDataModel> _ListConsistencyDataModel = new List<ConsistencyDataModel>();

        List<ConsistencyDataModel> _ListConsistencyDataModelRandom = new List<ConsistencyDataModel>();

        public string DummyTaxaCharacter = "A";

        public void CalculateConsistancy()
        {

            if (File.Exists(Constant.OutputFilePath))
            {
                File.Delete(Constant.OutputFilePath);
            }
            DivideAndConquer divideAndConquer = new DivideAndConquer();
            InputProcessing input = new InputProcessing();
            Bipartition bp = new Bipartition(input);
            GainTable GainTable = new GainTable();
            GainTable = bp.getFinalGainTable();


            int loop = 0;
            ConsistencyDataModel data = new ConsistencyDataModel();
            DepthOneTreeNode node;
            List<DepthOneTreeNode> ListDepthOneTreeNode = new List<DepthOneTreeNode>();


            //ConsistencyDataModel dataRandom = new ConsistencyDataModel();
            //DepthOneTreeNode nodeRandom;
            //List<DepthOneTreeNode> ListDepthOneTreeNodeRandom = new List<DepthOneTreeNode>();


            if (GainTable != null)
            {
                SetFinalGainTableAfterGainCalculation(GainTable);

                PartitionSet setOfMaxGain = GainTable.PartitionSet;


                SetALLQuatretInput(setOfMaxGain._ListQuatrets);

                var vDiffered = setOfMaxGain._ListQuatrets.FindAll(x => x._PartitionStatus == PartitionStatus.Differed);
                if (vDiffered != null)
                    SetDifferedQuatretInput(vDiffered.ToList());

                var vIsolated = setOfMaxGain._ListQuatrets.FindAll(x => x._PartitionStatus == PartitionStatus.Isolated);
                if (vIsolated != null)
                    SetIsolatedQuatretInput(vIsolated.ToList());

                var vViolated = setOfMaxGain._ListQuatrets.FindAll(x => x._PartitionStatus == PartitionStatus.Viotated);
                if (vViolated != null)
                    SetViolatedQuatretInput(vViolated.ToList());

                //OutputProcessing.PrintGainSummary(GainTable);
                //OutputProcessing.WriteCountInformationFromMaxGainTable(GainTable);


                _VALID_TAXA_LIST = input.Get_SetOfTaxa();
                divideAndConquer.Divide(GainTable.PartitionSet, DummyTaxaCharacter, 1, _VALID_TAXA_LIST);

                List<FinalPartionPair> ll = divideAndConquer.getFinalPartionPair();
                //OutputProcessing.PrintFinalTableOfDivideAndConquerApproach(ll);
                OutputProcessing.PrintDepthOneElement(ll);
                

                loop = 0;
                foreach (FinalPartionPair pair in ll)
                {
                    ListDepthOneTreeNode = new List<DepthOneTreeNode>();
                    data = new ConsistencyDataModel();
                    data._Differed_Quatret = this._DifferedQuatretListAfterGain;
                    data._Isolated_Quatret = this._IsolatedQuatretListAfterGain;
                    data._Violated_Quatret = this._ViolatedQuatretListAfterGain;


                    foreach (Taxa tx in pair._P.TaxaList)
                    {
                        node = new DepthOneTreeNode();
                        node._Position = loop;
                        node._Taxa_Value = tx._Taxa_Value;
                        ListDepthOneTreeNode.Add(node);
                    }
                    data._DepthOneChain = ListDepthOneTreeNode;
                    _ListConsistencyDataModel.Add(data);
                    loop++;
                }



                #region finding Consistancy Status

                ConsistencyDataModel dmodel = _ListConsistencyDataModel[0];
                string OutputHeader = "======================================================Consistancy Calculation======================================================";

                #region ISOLATED
                dmodel._Isolated_Quatret = GetConsistancyStatusOfQuatret(_ListConsistencyDataModel, dmodel._Isolated_Quatret);

                var vIsoConsistent = dmodel._Isolated_Quatret.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.Consistent);
                SetIsolatedQuatretConsistentAfterDevideAndConquer(vIsoConsistent);

                var vIsoInConsistent = dmodel._Isolated_Quatret.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.InConsistent);
                SetIsolatedQuatretInConsistentAfterDevideAndConquer(vIsoInConsistent);

                //OutputProcessing.WriteQuatretConsistancy(dmodel._Isolated_Quatret, PartitionStatus.Isolated, OutputHeader);
                #endregion

                #region Violated Quatret
                dmodel._Violated_Quatret = GetConsistancyStatusOfQuatret(_ListConsistencyDataModel, dmodel._Violated_Quatret);

                var vViolatedConsistent = dmodel._Violated_Quatret.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.Consistent);
                SetViolatedQuatretConsistentAfterDevideAndConquer(vViolatedConsistent);

                var vViolatedInConsistent = dmodel._Violated_Quatret.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.InConsistent);
                SetViolatedQuatretInConsistentAfterDevideAndConquer(vViolatedInConsistent);

                //OutputProcessing.WriteQuatretConsistancy(dmodel._Violated_Quatret, PartitionStatus.Viotated, string.Empty);
                #endregion

                #region Differed  Quatret
                dmodel._Differed_Quatret = GetConsistancyStatusOfQuatret(_ListConsistencyDataModel, dmodel._Differed_Quatret);

                var vDiffConsistent = dmodel._Differed_Quatret.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.Consistent);
                SetDifferedQuatretConsistentAfterDevideAndConquer(vDiffConsistent);

                var vDiffInConsistent = dmodel._Differed_Quatret.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.InConsistent);
                SetDifferedQuatretInConsistentAfterDevideAndConquer(vDiffInConsistent);

                //OutputProcessing.WriteQuatretConsistancy(dmodel._Differed_Quatret, PartitionStatus.Differed, string.Empty);
                #endregion


                #endregion


                /*    #region Random Technique


                    var listOfTaxaRandom = input.getSetOfTaxa();
                    divideAndConquer.generateDepthOneTreeRandomly(listOfTaxaRandom, DummyTaxaCharacter, 1, _VALID_TAXA_LIST, this._ALLQuatretListAfterGain);

                    List<FinalPartionPair> llRandom = divideAndConquer.getFinalPartionPairRandom();
                    //OutputProcessing.PrintDepthOneTreeRandom(llRandom, "Random Depth One Tree");
                    loop = 0;
                    foreach (FinalPartionPair pair in llRandom)
                    {
                        ListDepthOneTreeNodeRandom = new List<DepthOneTreeNode>();
                        dataRandom = new ConsistencyDataModel();
                        dataRandom._Differed_Quatret = this._DifferedQuatretListAfterGain;
                        dataRandom._Isolated_Quatret = this._IsolatedQuatretListAfterGain;
                        dataRandom._Violated_Quatret = this._ViolatedQuatretListAfterGain;
                        dataRandom._ALL_Quatret = this._ALLQuatretListAfterGain;

                        foreach (Taxa tx in pair._Root)
                        {
                            nodeRandom = new DepthOneTreeNode();
                            nodeRandom._Position = loop;
                            nodeRandom._Taxa_Value = tx._Taxa_Value;
                            ListDepthOneTreeNodeRandom.Add(nodeRandom);
                        }
                        dataRandom._DepthOneChain = ListDepthOneTreeNodeRandom;
                        _ListConsistencyDataModelRandom.Add(dataRandom);
                        loop++;
                    }

                    ConsistencyDataModel dmodelRandom = _ListConsistencyDataModelRandom[0];
                    //string OutputHeaderRandom = "======================================================Consistancy Calculation(Random Technique)======================================================";

                    dmodelRandom._ALL_Quatret = GetConsistancyStatusOfQuatret(_ListConsistencyDataModelRandom, dmodelRandom._ALL_Quatret);

                    var vConsistentRandom = dmodelRandom._ALL_Quatret.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.Consistent);
                    SetRandomQuatretConsistentAfterDevideAndConquer(vConsistentRandom);

                    var vInConsistentRandom = dmodelRandom._ALL_Quatret.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.InConsistent);
                    SetRandomQuatretInConsistentAfterDevideAndConquer(vInConsistentRandom);

                    //OutputProcessing.WriteQuatretConsistancy(dmodelRandom._ALL_Quatret, PartitionStatus.None, OutputHeaderRandom);

                    #endregion
                */
            }

        }

        public List<ConsistencyDataModel> getListConsistencyDataModel()
        {
            return this._ListConsistencyDataModel;
        }

        public List<ConsistencyDataModel> getListConsistencyDataModelRandom()
        {
            return this._ListConsistencyDataModelRandom;
        }
        public List<Quartet> getViolatedQuatretListAfterGain()
        {
            return this._ViolatedQuatretListAfterGain;
        }

        public List<Quartet> getDifferedQuatretListAfterGain()
        {
            return this._DifferedQuatretListAfterGain;
        }

        public List<Quartet> getIsolatedQuatretListAfterGain()
        {
            return this._IsolatedQuatretListAfterGain;
        }


        public GainTable getFinalGainTableAfterGainCalculation()
        {
            return this._FinalGainTableAfterGainCalculation;
        }
        public void SetFinalGainTableAfterGainCalculation(GainTable m)
        {

            this._FinalGainTableAfterGainCalculation = new GainTable()
            {



                _TaxaGainSummary = new List<Taxa>(m._TaxaGainSummary.Select(x => new Taxa()
                {

                    _Taxa_Value = x._Taxa_Value,
                    _Quartet_Name = x._Quartet_Name,
                    _Taxa_ValuePosition_In_Quartet = x._Taxa_ValuePosition_In_Quartet,
                    _Gain = x._Gain,
                    _CumulativeGain = x._CumulativeGain,
                    IsFreeze = x.IsFreeze,
                    _IsolatedCount = x._IsolatedCount,
                    _ViotatedCount = x._ViotatedCount,
                    _DifferedCount = x._DifferedCount,
                    _SatisfiedCount = x._SatisfiedCount,
                    _TaxaPartitionSet = new PartitionSet(x._TaxaPartitionSet._PartitionSetName, x._TaxaPartitionSet._Final_Score, x._TaxaPartitionSet._IsolatedCount, x._TaxaPartitionSet._ViotatedCount, x._TaxaPartitionSet._SatisfiedCount, x._TaxaPartitionSet._DifferedCount, x._TaxaPartitionSet._taxValueForGainCalculation, x._TaxaPartitionSet._Gain, x._TaxaPartitionSet.PartitionList, x._TaxaPartitionSet._ListQuatrets),
                    StepK = x.StepK
                })),
                _MaxCumulativeGain = m._MaxCumulativeGain,
                TaxValue = m.TaxValue,
                MaximumGainOfTaxValue = m.MaximumGainOfTaxValue,
                PartitionSet = new PartitionSet(m.PartitionSet._PartitionSetName, m.PartitionSet._Final_Score, m.PartitionSet._IsolatedCount, m.PartitionSet._ViotatedCount, m.PartitionSet._SatisfiedCount, m.PartitionSet._DifferedCount, m.PartitionSet._taxValueForGainCalculation, m.PartitionSet._Gain, m.PartitionSet.PartitionList, m.PartitionSet._ListQuatrets)

            };
        }


        public List<Quartet> getDifferedQuatretConsistentAfterDevideAndConquer()
        {
            return this._DifferredConsistentAfterDevideAndConquer;
        }
        public void SetDifferedQuatretConsistentAfterDevideAndConquer(List<Quartet> Q)
        {

            this._DifferredConsistentAfterDevideAndConquer = new List<Quartet>(Q.Select(m => new Quartet()
            {

                _First_Taxa_Value = m._First_Taxa_Value,
                _Second_Taxa_Value = m._Second_Taxa_Value,
                _Third_Taxa_Value = m._Third_Taxa_Value,
                _Fourth_Taxa_Value = m._Fourth_Taxa_Value,
                _Quartet_Name = m._Quartet_Name,
                _Quartet_Input = m._Quartet_Input,
                _Quartet_LeftPart = m._Quartet_LeftPart,
                _Quartet_LeftPartReverse = m._Quartet_LeftPartReverse,
                _Quartet_RightPart = m._Quartet_RightPart,
                _Quartet_RightPartReverse = m._Quartet_RightPartReverse,
                _isDistinct = m._isDistinct,
                _Frequency = m._Frequency,
                _ConsistancyStatus = m._ConsistancyStatus,
                _PartitionStatus = m._PartitionStatus

            }));
        }


        public List<Quartet> getDifferedQuatretInConsistentAfterDevideAndConquer()
        {
            return this._DifferredInConsistentAfterDevideAndConquer;
        }
        public void SetDifferedQuatretInConsistentAfterDevideAndConquer(List<Quartet> Q)
        {

            this._DifferredInConsistentAfterDevideAndConquer = new List<Quartet>(Q.Select(m => new Quartet()
            {

                _First_Taxa_Value = m._First_Taxa_Value,
                _Second_Taxa_Value = m._Second_Taxa_Value,
                _Third_Taxa_Value = m._Third_Taxa_Value,
                _Fourth_Taxa_Value = m._Fourth_Taxa_Value,
                _Quartet_Name = m._Quartet_Name,
                _Quartet_Input = m._Quartet_Input,
                _Quartet_LeftPart = m._Quartet_LeftPart,
                _Quartet_LeftPartReverse = m._Quartet_LeftPartReverse,
                _Quartet_RightPart = m._Quartet_RightPart,
                _Quartet_RightPartReverse = m._Quartet_RightPartReverse,
                _isDistinct = m._isDistinct,
                _Frequency = m._Frequency,
                _ConsistancyStatus = m._ConsistancyStatus,
                _PartitionStatus = m._PartitionStatus

            }));
        }

        public List<Quartet> getIsolatedQuatretConsistentAfterDevideAndConquer()
        {
            return this._IsolatedConsistentAfterDevideAndConquer;
        }
        public void SetIsolatedQuatretConsistentAfterDevideAndConquer(List<Quartet> Q)
        {

            this._IsolatedConsistentAfterDevideAndConquer = new List<Quartet>(Q.Select(m => new Quartet()
            {

                _First_Taxa_Value = m._First_Taxa_Value,
                _Second_Taxa_Value = m._Second_Taxa_Value,
                _Third_Taxa_Value = m._Third_Taxa_Value,
                _Fourth_Taxa_Value = m._Fourth_Taxa_Value,
                _Quartet_Name = m._Quartet_Name,
                _Quartet_Input = m._Quartet_Input,
                _Quartet_LeftPart = m._Quartet_LeftPart,
                _Quartet_LeftPartReverse = m._Quartet_LeftPartReverse,
                _Quartet_RightPart = m._Quartet_RightPart,
                _Quartet_RightPartReverse = m._Quartet_RightPartReverse,
                _isDistinct = m._isDistinct,
                _Frequency = m._Frequency,
                _ConsistancyStatus = m._ConsistancyStatus,
                _PartitionStatus = m._PartitionStatus

            }));
        }


        public List<Quartet> getViolatedQuatretConsistentAfterDevideAndConquer()
        {
            return this._ViolatedConsistentAfterDevideAndConquer;
        }
        public void SetViolatedQuatretConsistentAfterDevideAndConquer(List<Quartet> Q)
        {

            this._ViolatedConsistentAfterDevideAndConquer = new List<Quartet>(Q.Select(m => new Quartet()
            {

                _First_Taxa_Value = m._First_Taxa_Value,
                _Second_Taxa_Value = m._Second_Taxa_Value,
                _Third_Taxa_Value = m._Third_Taxa_Value,
                _Fourth_Taxa_Value = m._Fourth_Taxa_Value,
                _Quartet_Name = m._Quartet_Name,
                _Quartet_Input = m._Quartet_Input,
                _Quartet_LeftPart = m._Quartet_LeftPart,
                _Quartet_LeftPartReverse = m._Quartet_LeftPartReverse,
                _Quartet_RightPart = m._Quartet_RightPart,
                _Quartet_RightPartReverse = m._Quartet_RightPartReverse,
                _isDistinct = m._isDistinct,
                _Frequency = m._Frequency,
                _ConsistancyStatus = m._ConsistancyStatus,
                _PartitionStatus = m._PartitionStatus

            }));
        }

        public List<Quartet> getIsolatedQuatretInConsistentAfterDevideAndConquer()
        {
            return this._IsolatedInConsistentAfterDevideAndConquer;
        }
        public void SetIsolatedQuatretInConsistentAfterDevideAndConquer(List<Quartet> Q)
        {

            this._IsolatedInConsistentAfterDevideAndConquer = new List<Quartet>(Q.Select(m => new Quartet()
            {

                _First_Taxa_Value = m._First_Taxa_Value,
                _Second_Taxa_Value = m._Second_Taxa_Value,
                _Third_Taxa_Value = m._Third_Taxa_Value,
                _Fourth_Taxa_Value = m._Fourth_Taxa_Value,
                _Quartet_Name = m._Quartet_Name,
                _Quartet_Input = m._Quartet_Input,
                _Quartet_LeftPart = m._Quartet_LeftPart,
                _Quartet_LeftPartReverse = m._Quartet_LeftPartReverse,
                _Quartet_RightPart = m._Quartet_RightPart,
                _Quartet_RightPartReverse = m._Quartet_RightPartReverse,
                _isDistinct = m._isDistinct,
                _Frequency = m._Frequency,
                _ConsistancyStatus = m._ConsistancyStatus,
                _PartitionStatus = m._PartitionStatus

            }));
        }


        #region Random Techinique

        public List<Quartet> getALLQuatretListAfterGain()
        {
            return this._ALLQuatretListAfterGain;
        }

        public List<Quartet> getRandomQuatretConsistentAfterDevideAndConquer()
        {
            return this._RandomConsistentAfterDevideAndConquer;
        }
        public void SetRandomQuatretConsistentAfterDevideAndConquer(List<Quartet> Q)
        {

            this._RandomConsistentAfterDevideAndConquer = new List<Quartet>(Q.Select(m => new Quartet()
            {

                _First_Taxa_Value = m._First_Taxa_Value,
                _Second_Taxa_Value = m._Second_Taxa_Value,
                _Third_Taxa_Value = m._Third_Taxa_Value,
                _Fourth_Taxa_Value = m._Fourth_Taxa_Value,
                _Quartet_Name = m._Quartet_Name,
                _Quartet_Input = m._Quartet_Input,
                _Quartet_LeftPart = m._Quartet_LeftPart,
                _Quartet_LeftPartReverse = m._Quartet_LeftPartReverse,
                _Quartet_RightPart = m._Quartet_RightPart,
                _Quartet_RightPartReverse = m._Quartet_RightPartReverse,
                _isDistinct = m._isDistinct,
                _Frequency = m._Frequency,
                _ConsistancyStatus = m._ConsistancyStatus,
                _PartitionStatus = m._PartitionStatus

            }));
        }
        public List<Quartet> getRandomQuatretInConsistentAfterDevideAndConquer()
        {
            return this._RandomInConsistentAfterDevideAndConquer;
        }
        public void SetRandomQuatretInConsistentAfterDevideAndConquer(List<Quartet> Q)
        {

            this._RandomInConsistentAfterDevideAndConquer = new List<Quartet>(Q.Select(m => new Quartet()
            {

                _First_Taxa_Value = m._First_Taxa_Value,
                _Second_Taxa_Value = m._Second_Taxa_Value,
                _Third_Taxa_Value = m._Third_Taxa_Value,
                _Fourth_Taxa_Value = m._Fourth_Taxa_Value,
                _Quartet_Name = m._Quartet_Name,
                _Quartet_Input = m._Quartet_Input,
                _Quartet_LeftPart = m._Quartet_LeftPart,
                _Quartet_LeftPartReverse = m._Quartet_LeftPartReverse,
                _Quartet_RightPart = m._Quartet_RightPart,
                _Quartet_RightPartReverse = m._Quartet_RightPartReverse,
                _isDistinct = m._isDistinct,
                _Frequency = m._Frequency,
                _ConsistancyStatus = m._ConsistancyStatus,
                _PartitionStatus = m._PartitionStatus

            }));
        }

        #endregion

        public List<Quartet> getViolatedQuatretInConsistentAfterDevideAndConquer()
        {
            return this._ViolatedInConsistentAfterDevideAndConquer;
        }
        public void SetViolatedQuatretInConsistentAfterDevideAndConquer(List<Quartet> Q)
        {

            this._ViolatedInConsistentAfterDevideAndConquer = new List<Quartet>(Q.Select(m => new Quartet()
            {

                _First_Taxa_Value = m._First_Taxa_Value,
                _Second_Taxa_Value = m._Second_Taxa_Value,
                _Third_Taxa_Value = m._Third_Taxa_Value,
                _Fourth_Taxa_Value = m._Fourth_Taxa_Value,
                _Quartet_Name = m._Quartet_Name,
                _Quartet_Input = m._Quartet_Input,
                _Quartet_LeftPart = m._Quartet_LeftPart,
                _Quartet_LeftPartReverse = m._Quartet_LeftPartReverse,
                _Quartet_RightPart = m._Quartet_RightPart,
                _Quartet_RightPartReverse = m._Quartet_RightPartReverse,
                _isDistinct = m._isDistinct,
                _Frequency = m._Frequency,
                _ConsistancyStatus = m._ConsistancyStatus,
                _PartitionStatus = m._PartitionStatus

            }));
        }


        public void SetDifferedQuatretInput(List<Quartet> Q)
        {

            this._DifferedQuatretListAfterGain = new List<Quartet>(Q.Select(m => new Quartet()
            {

                _First_Taxa_Value = m._First_Taxa_Value,
                _Second_Taxa_Value = m._Second_Taxa_Value,
                _Third_Taxa_Value = m._Third_Taxa_Value,
                _Fourth_Taxa_Value = m._Fourth_Taxa_Value,
                _Quartet_Name = m._Quartet_Name,
                _Quartet_Input = m._Quartet_Input,
                _Quartet_LeftPart = m._Quartet_LeftPart,
                _Quartet_LeftPartReverse = m._Quartet_LeftPartReverse,
                _Quartet_RightPart = m._Quartet_RightPart,
                _Quartet_RightPartReverse = m._Quartet_RightPartReverse,
                _isDistinct = m._isDistinct,
                _Frequency = m._Frequency,
                _ConsistancyStatus = m._ConsistancyStatus,
                _PartitionStatus = m._PartitionStatus

            }));
        }
        public void SetViolatedQuatretInput(List<Quartet> Q)
        {
            this._ViolatedQuatretListAfterGain = new List<Quartet>(Q.Select(m => new Quartet()
            {

                _First_Taxa_Value = m._First_Taxa_Value,
                _Second_Taxa_Value = m._Second_Taxa_Value,
                _Third_Taxa_Value = m._Third_Taxa_Value,
                _Fourth_Taxa_Value = m._Fourth_Taxa_Value,
                _Quartet_Name = m._Quartet_Name,
                _Quartet_Input = m._Quartet_Input,
                _Quartet_LeftPart = m._Quartet_LeftPart,
                _Quartet_LeftPartReverse = m._Quartet_LeftPartReverse,
                _Quartet_RightPart = m._Quartet_RightPart,
                _Quartet_RightPartReverse = m._Quartet_RightPartReverse,
                _isDistinct = m._isDistinct,
                _Frequency = m._Frequency,
                _ConsistancyStatus = m._ConsistancyStatus,
                _PartitionStatus = m._PartitionStatus

            }));
        }
        public void SetIsolatedQuatretInput(List<Quartet> Q)
        {
            this._IsolatedQuatretListAfterGain = new List<Quartet>(Q.Select(m => new Quartet()
            {

                _First_Taxa_Value = m._First_Taxa_Value,
                _Second_Taxa_Value = m._Second_Taxa_Value,
                _Third_Taxa_Value = m._Third_Taxa_Value,
                _Fourth_Taxa_Value = m._Fourth_Taxa_Value,
                _Quartet_Name = m._Quartet_Name,
                _Quartet_Input = m._Quartet_Input,
                _Quartet_LeftPart = m._Quartet_LeftPart,
                _Quartet_LeftPartReverse = m._Quartet_LeftPartReverse,
                _Quartet_RightPart = m._Quartet_RightPart,
                _Quartet_RightPartReverse = m._Quartet_RightPartReverse,
                _isDistinct = m._isDistinct,
                _Frequency = m._Frequency,
                _ConsistancyStatus = m._ConsistancyStatus,
                _PartitionStatus = m._PartitionStatus

            }));
        }


        public void SetALLQuatretInput(List<Quartet> Q)
        {
            this._ALLQuatretListAfterGain = new List<Quartet>(Q.Select(m => new Quartet()
            {

                _First_Taxa_Value = m._First_Taxa_Value,
                _Second_Taxa_Value = m._Second_Taxa_Value,
                _Third_Taxa_Value = m._Third_Taxa_Value,
                _Fourth_Taxa_Value = m._Fourth_Taxa_Value,
                _Quartet_Name = m._Quartet_Name,
                _Quartet_Input = m._Quartet_Input,
                _Quartet_LeftPart = m._Quartet_LeftPart,
                _Quartet_LeftPartReverse = m._Quartet_LeftPartReverse,
                _Quartet_RightPart = m._Quartet_RightPart,
                _Quartet_RightPartReverse = m._Quartet_RightPartReverse,
                _isDistinct = m._isDistinct,
                _Frequency = m._Frequency,
                _ConsistancyStatus = m._ConsistancyStatus,
                _PartitionStatus = m._PartitionStatus

            }));
        }

        public List<Quartet> GetConsistancyStatusOfQuatret(List<ConsistencyDataModel> _DepthOneChain, List<Quartet> input)
        {

            Quartet dummyQuatret;

            foreach (Quartet q in input)
            {
                q._ConsistancyStatus = ConsistencyStatus.None;
                q._ConsistancyStatus = CheckForInConsistency(_DepthOneChain, q);

                if (q._ConsistancyStatus == ConsistencyStatus.InConsistent)
                {
                    dummyQuatret = new Quartet()
                    {

                        _First_Taxa_Value = q._Third_Taxa_Value,
                        _Second_Taxa_Value = q._Fourth_Taxa_Value,
                        _Third_Taxa_Value = q._First_Taxa_Value,
                        _Fourth_Taxa_Value = q._Second_Taxa_Value,
                        _Quartet_Name = q._Quartet_Name,
                        _Quartet_Input = q._Quartet_Input,
                        _Quartet_LeftPart = q._Quartet_LeftPart,
                        _Quartet_LeftPartReverse = q._Quartet_LeftPartReverse,
                        _Quartet_RightPart = q._Quartet_RightPart,
                        _Quartet_RightPartReverse = q._Quartet_RightPartReverse,
                        _isDistinct = q._isDistinct,
                        _Frequency = q._Frequency,
                        _ConsistancyStatus = q._ConsistancyStatus,
                        _PartitionStatus = q._PartitionStatus

                    };
                    q._ConsistancyStatus = CheckForInConsistency(_DepthOneChain, dummyQuatret);

                }
            }


            return new List<Quartet>(input.Select(m => new Quartet()
            {

                _First_Taxa_Value = m._First_Taxa_Value,
                _Second_Taxa_Value = m._Second_Taxa_Value,
                _Third_Taxa_Value = m._Third_Taxa_Value,
                _Fourth_Taxa_Value = m._Fourth_Taxa_Value,
                _Quartet_Name = m._Quartet_Name,
                _Quartet_Input = m._Quartet_Input,
                _Quartet_LeftPart = m._Quartet_LeftPart,
                _Quartet_LeftPartReverse = m._Quartet_LeftPartReverse,
                _Quartet_RightPart = m._Quartet_RightPart,
                _Quartet_RightPartReverse = m._Quartet_RightPartReverse,
                _isDistinct = m._isDistinct,
                _Frequency = m._Frequency,
                _ConsistancyStatus = m._ConsistancyStatus,
                _PartitionStatus = m._PartitionStatus

            }));
        }

        public ConsistencyStatus CheckForInConsistency(List<ConsistencyDataModel> _DepthOneChain, Quartet input)
        {

            ConsistencyStatus status = ConsistencyStatus.InConsistent;

            int pos1 = 0;
            int pos2 = 0;
            int pos3 = 0;
            int pos4 = 0;

            input._ConsistancyStatus = ConsistencyStatus.None;

            pos1 = getPosition(_DepthOneChain, input._First_Taxa_Value);
            pos2 = getPosition(_DepthOneChain, input._Second_Taxa_Value);
            pos3 = getPosition(_DepthOneChain, input._Third_Taxa_Value);
            pos4 = getPosition(_DepthOneChain, input._Fourth_Taxa_Value);

            if (pos1 != -1 && pos2 != -1 && pos3 != -1 && pos4 != -1)
            {

                if (pos1 == pos2 && pos2 < pos3 && pos3 == pos4)
                {
                    status = ConsistencyStatus.Consistent;
                }
                else if (pos1 < pos2 && pos2 < pos3 && pos3 < pos4)
                {
                    status = ConsistencyStatus.Consistent;
                }
                else if (pos1 < pos2 && pos2 < pos4 && pos4 < pos3)
                {
                    status = ConsistencyStatus.Consistent;
                }
                else if (pos1 > pos2 && pos1 < pos3 && pos3 < pos4)
                {
                    status = ConsistencyStatus.Consistent;
                }
                else if (pos1 > pos2 && pos1 < pos4 && pos4 < pos3)
                {
                    status = ConsistencyStatus.Consistent;
                }
                else if (pos1 == pos2 && pos2 < pos4 && pos4 < pos3)
                {
                    status = ConsistencyStatus.Consistent;
                }
                else if (pos1 == pos2 && pos2 < pos3 && pos4 > pos3)
                {
                    status = ConsistencyStatus.Consistent;
                }
                else if (pos1 < pos2 && pos2 < pos3 && pos4 == pos3)
                {
                    status = ConsistencyStatus.Consistent;
                }
                else if (pos1 > pos2 && pos1 < pos3 && pos4 == pos3)
                {
                    status = ConsistencyStatus.Consistent;
                }
                else if (pos1 < pos2 && pos4 == pos3 && pos1 < pos3 && pos3 < pos2)
                {
                    status = ConsistencyStatus.Consistent;
                }
                else if (pos2 < pos1 && pos4 == pos3 && pos2 < pos3 && pos3 < pos1)
                {
                    status = ConsistencyStatus.Consistent;
                }
                else if (pos3 < pos4 && pos1 == pos2 && pos3 < pos1 && pos1 < pos4)
                {
                    status = ConsistencyStatus.Consistent;
                }
                else if (pos4 < pos3 && pos1 == pos2 && pos4 < pos1 && pos1 < pos3)
                {
                    status = ConsistencyStatus.Consistent;
                }
                else
                    status = ConsistencyStatus.InConsistent;

            }
            else
            {
                status = ConsistencyStatus.InConsistent;

            }

            return status;
        }

        public int getPosition(List<ConsistencyDataModel> ListConsistencyDataModel, string taxa)
        {
            int position = -1;

            foreach (ConsistencyDataModel model in ListConsistencyDataModel)
            {
                var v = model._DepthOneChain.FindAll(x => x._Taxa_Value == taxa).FirstOrDefault();
                if (v != null)
                {
                    position = v._Position;
                    break;
                }
                else
                {
                    position = -1;
                }
            }


            return position;


        }
    }
}
