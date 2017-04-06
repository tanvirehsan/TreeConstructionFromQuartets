using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeConstructionFromQuartets.Model;

namespace TreeConstructionFromQuartets
{
    public class DivideAndConquer
    {
        private List<FinalPartionPair> _ListFinalPartionPair = new List<FinalPartionPair>();
        private List<FinalPartionPair> _ListFinalPartionPairRandom = new List<FinalPartionPair>();
        public List<Taxa> RandomTaxaListForGeneration = new List<Taxa>();
        public List<Taxa> RandomTaxaListForCalculation = new List<Taxa>();

        public void getRandomTaxaFromListNew()
        {
            Taxa txRandom = new Taxa();
            Random rnd = new Random();
            int r = 0;

            if (RandomTaxaListForCalculation.Count - 1 == 0)
            {
                txRandom = RandomTaxaListForCalculation[r];
            }
            else
            {
                r = rnd.Next(0, RandomTaxaListForCalculation.Count - 1);
                txRandom = RandomTaxaListForCalculation[r];
            }

            RandomTaxaListForGeneration.Add(txRandom);
            RandomTaxaListForCalculation.RemoveAt(r);
            r = rnd.Next(0, RandomTaxaListForCalculation.Count);
            if (r % 2 == 0)
                RandomTaxaListForCalculation.Reverse();

        }
        public Taxa getRandomTaxaFromList(List<Taxa> CurrentTaxaList)
        {
            Taxa txRandom = new Taxa();
            Random rnd = new Random();
            int TaxaListCount = CurrentTaxaList.Count - 1;
            if (TaxaListCount == 0)
            {
                txRandom = CurrentTaxaList[0];
                CurrentTaxaList.RemoveAt(0);
            }
            else
            {
                int r = rnd.Next(0, TaxaListCount);
                txRandom = (Taxa)CurrentTaxaList[r];
                CurrentTaxaList.RemoveAt(r);
            }

            return txRandom;
        }
        public Taxa getRandomTaxaFromListOld(List<Taxa> CurrentTaxaList)
        {
            Taxa txRandom = new Taxa();
            Random rnd = new Random();
            int TaxaListCount = CurrentTaxaList.Count - 1;
            if (TaxaListCount == 0)
            {
                txRandom = CurrentTaxaList[0];
                CurrentTaxaList.RemoveAt(0);
            }
            else
            {
                int r = rnd.Next(0, TaxaListCount);
                txRandom = (Taxa)CurrentTaxaList[r];
                CurrentTaxaList.RemoveAt(r);
            }

            return txRandom;
        }

        public void generateDepthOneTreeRandomly(List<Taxa> TaxaList, string DummyTaxaCharacter, int Level, List<string> _VALID_TAXA_LIST, List<Quartet> inputQuatrets)
        {

            string DummyTaxa = DummyTaxaCharacter + Level.ToString();
            Taxa TaxaDummy = new Taxa();
            TaxaDummy._Taxa_Value = DummyTaxa;

            FinalPartionPair PartionPair = new FinalPartionPair();
            List<Taxa> _LeftPartTaxaList;
            List<Taxa> _RightPartTaxaList;
            RandomTaxaListForCalculation = new List<Taxa>(TaxaList);
            RandomTaxaListForGeneration = new List<Taxa>();
            if (CountValidTaxa(TaxaList, _VALID_TAXA_LIST) > 2)
            {
                _LeftPartTaxaList = new List<Taxa>();
                _RightPartTaxaList = new List<Taxa>();

                //
                Random rnd = new Random();
                int count = 0;
                int isLeft = 0;

                for (int i = 0; i < TaxaList.Count; i++)
                {
                    getRandomTaxaFromListNew();
                }

                for (int i = 0; i < RandomTaxaListForGeneration.Count; i++)
                {
                    if (count == 0)
                    {
                        int r = rnd.Next(0, TaxaList.Count - 1);
                        if (r % 2 == 0)
                        {

                            _LeftPartTaxaList.Add(RandomTaxaListForGeneration[i]);
                            isLeft = 0;
                        }
                        else
                        {

                            _RightPartTaxaList.Add(RandomTaxaListForGeneration[i]);
                            isLeft = 1;
                        }
                        count++;
                    }
                    else
                    {
                        if (isLeft == 1)
                        {
                            _LeftPartTaxaList.Add(RandomTaxaListForGeneration[i]);
                            isLeft = 0;
                        }
                        else
                        {
                            _RightPartTaxaList.Add(RandomTaxaListForGeneration[i]);
                            isLeft = 1;
                        }
                    }
                }



                //RandomTaxaListForGeneration = new List<Taxa>();
                //RandomTaxaListForCalculation = new List<Taxa>();

                //


                _LeftPartTaxaList.Add(TaxaDummy);
                _RightPartTaxaList.Add(TaxaDummy);

                generateDepthOneTreeRandomly(_LeftPartTaxaList, DummyTaxaCharacter, Level + 1, _VALID_TAXA_LIST, inputQuatrets);
                generateDepthOneTreeRandomly(_RightPartTaxaList, DummyTaxaCharacter, Level + 1, _VALID_TAXA_LIST, inputQuatrets);

            }

            else
            {
                //RandomTaxaListForGeneration = new List<Taxa>();
                //RandomTaxaListForCalculation = new List<Taxa>();

                PartionPair = new FinalPartionPair();
                PartionPair._Root = TaxaList;
                PartionPair._Q = GetQuatretWithRandomDividedTaxa(TaxaList, inputQuatrets);
                PartionPair._InputQuatret = inputQuatrets;
                _ListFinalPartionPairRandom.Add(PartionPair);

            }

        }
        public void generateDepthOneTreeRandomlyChanged(List<Taxa> TaxaList, string DummyTaxaCharacter, int Level, List<string> _VALID_TAXA_LIST, List<Quartet> inputQuatrets)
        {

            string DummyTaxa = DummyTaxaCharacter + Level.ToString();
            Taxa TaxaDummy = new Taxa();
            TaxaDummy._Taxa_Value = DummyTaxa;

            FinalPartionPair PartionPair = new FinalPartionPair();
            List<Taxa> _LeftPartTaxaList;
            List<Taxa> _RightPartTaxaList;

            if (TaxaList.Count >= 4 && CountValidTaxa(TaxaList, _VALID_TAXA_LIST) > 0)
            {
                _LeftPartTaxaList = new List<Taxa>();
                _RightPartTaxaList = new List<Taxa>();

                this.RandomTaxaListForGeneration = new List<Taxa>(TaxaList);

                int randomValue = 0;
                Taxa txRandom;
                Random rnd = new Random();
                int CountRand = 0;
                foreach (Taxa ttt in TaxaList)
                {

                    //randomValue = rnd.Next();
                    if (CountRand == 0)
                        randomValue = rnd.Next();
                    else
                        randomValue = CountRand;

                    if (randomValue % 2 == 0)
                    {
                        txRandom = getRandomTaxaFromList(this.RandomTaxaListForGeneration);
                        _LeftPartTaxaList.Add(txRandom);
                        this.RandomTaxaListForGeneration.Remove(txRandom);
                    }
                    else
                    {
                        txRandom = getRandomTaxaFromList(this.RandomTaxaListForGeneration);
                        _RightPartTaxaList.Add(txRandom);
                        this.RandomTaxaListForGeneration.Remove(txRandom);
                    }

                    randomValue++;
                    CountRand++;
                }

                this.RandomTaxaListForGeneration = new List<Taxa>();

                _LeftPartTaxaList.Add(TaxaDummy);
                generateDepthOneTreeRandomly(_LeftPartTaxaList, DummyTaxaCharacter, Level + 1, _VALID_TAXA_LIST, inputQuatrets);
                _RightPartTaxaList.Add(TaxaDummy);
                generateDepthOneTreeRandomly(_RightPartTaxaList, DummyTaxaCharacter, Level + 1, _VALID_TAXA_LIST, inputQuatrets);

            }

            else
            {

                PartionPair = new FinalPartionPair();
                PartionPair._Root = TaxaList;
                PartionPair._Q = GetQuatretWithRandomDividedTaxa(TaxaList, inputQuatrets);
                PartionPair._InputQuatret = inputQuatrets;
                _ListFinalPartionPairRandom.Add(PartionPair);


            }

        }
        public void generateDepthOneTreeRandomlyOld(List<Taxa> TaxaList, string DummyTaxaCharacter, int Level, List<string> _VALID_TAXA_LIST, List<Quartet> inputQuatrets)
        {

            string DummyTaxa = DummyTaxaCharacter + Level.ToString();
            Taxa TaxaDummy = new Taxa();
            TaxaDummy._Taxa_Value = DummyTaxa;

            FinalPartionPair PartionPair = new FinalPartionPair();
            List<Taxa> _LeftPartTaxaList;
            List<Taxa> _RightPartTaxaList;

            if (CountValidTaxa(TaxaList, _VALID_TAXA_LIST) > 2)
            {
                _LeftPartTaxaList = new List<Taxa>();
                _RightPartTaxaList = new List<Taxa>();


                int medianposition = TaxaList.Count / 2 - 1;

                for (int i = 0; i <= medianposition; i++)
                {
                    _LeftPartTaxaList.Add(TaxaList[i]);
                }

                for (int i = medianposition + 1; i < TaxaList.Count; i++)
                {
                    _RightPartTaxaList.Add(TaxaList[i]);
                }

                _LeftPartTaxaList.Add(TaxaDummy);
                _RightPartTaxaList.Add(TaxaDummy);

                generateDepthOneTreeRandomly(_LeftPartTaxaList, DummyTaxaCharacter, Level + 1, _VALID_TAXA_LIST, inputQuatrets);
                generateDepthOneTreeRandomly(_RightPartTaxaList, DummyTaxaCharacter, Level + 1, _VALID_TAXA_LIST, inputQuatrets);

            }

            else
            {
                PartionPair = new FinalPartionPair();
                PartionPair._Root = TaxaList;
                PartionPair._Q = GetQuatretWithRandomDividedTaxa(TaxaList, inputQuatrets);
                PartionPair._InputQuatret = inputQuatrets;
                _ListFinalPartionPairRandom.Add(PartionPair);

            }

        }
        public List<Quartet> GetQuatretWithRandomDividedTaxa(List<Taxa> TaxaList, List<Quartet> inputQuatrets)
        {

            List<Quartet> QA = new List<Quartet>();

            int count = 0;
            foreach (Quartet q in inputQuatrets)
            {
                count = 0;
                foreach (Taxa t in TaxaList)
                {
                    if (q._First_Taxa_Value == t._Taxa_Value || q._Second_Taxa_Value == t._Taxa_Value || q._Third_Taxa_Value == t._Taxa_Value || q._Fourth_Taxa_Value == t._Taxa_Value)
                    {
                        count++;
                    }
                }

                if (count == 3 || count == 4)
                {
                    QA.Add(q);
                }


            }

            return QA;
        }
        public void Divide(PartitionSet pPartitionSet, string DummyTaxaCharacter, int Level, List<string> _VALID_TAXA_LIST)
        {


            string DummyTaxa = DummyTaxaCharacter + Level.ToString();
            Taxa TaxaDummy = new Taxa();
            TaxaDummy._Taxa_Value = DummyTaxa;


            //Adding Dummy Taxa to the Set PA and PB
            FinalPartionPair PartionPair;
            List<FinalPartionPair> ListPartionPair = new List<FinalPartionPair>();
            foreach (Partition p in pPartitionSet.PartitionList)
            {
                PartionPair = new FinalPartionPair();
                p.TaxaList.Add(TaxaDummy);
                PartionPair = SubDivideQuatret(pPartitionSet._ListQuatrets, p, DummyTaxa);
                PartionPair._Root = p.TaxaList;

                ListPartionPair.Add(PartionPair);
            }



            foreach (FinalPartionPair pair in ListPartionPair)
            {
                if (CountValidTaxa(pair._P.TaxaList, _VALID_TAXA_LIST) >= 1 && (pair._Q == null))
                {
                    _ListFinalPartionPair.Add(pair);
                }
                else if (CountValidTaxa(pair._P.TaxaList, _VALID_TAXA_LIST) >= 3 && (pair._Q != null))
                {
                    InputProcessing input = new InputProcessing(pair._Q, pair._P.TaxaList);
                    Bipartition bpA = new Bipartition(input);
                    GainTable GTable = bpA.getFinalGainTable();
                    Divide(GTable.PartitionSet, DummyTaxaCharacter, Level + 1, _VALID_TAXA_LIST);
                }
                else if (CountValidTaxa(pair._P.TaxaList, _VALID_TAXA_LIST) < 3 && (pair._Q != null))
                {
                    _ListFinalPartionPair.Add(pair);
                }
            }




        }

        public List<FinalPartionPair> getFinalPartionPair()
        {
            return this._ListFinalPartionPair;
        }

        public List<FinalPartionPair> getFinalPartionPairRandom()
        {
            return this._ListFinalPartionPairRandom;
        }

        public static int CountValidTaxa(List<Taxa> PListOfTaxa, List<string> _VALID_TAXA_LIST)
        {
            int count = 0;

            foreach (string validTaxa in _VALID_TAXA_LIST)
            {
                var TotalCount = PListOfTaxa.Where(x => x._Taxa_Value == validTaxa).Count();
                if (TotalCount != null)
                {
                    if (TotalCount > 0)
                    {
                        count++;
                    }
                }
            }


            return count;


        }

        public FinalPartionPair SubDivideQuatret(List<Quartet> inputQuatretList, Partition p, string DummyTaxa)
        {
            List<Quartet> Q = new List<Quartet>();
            List<Quartet> QA = new List<Quartet>();
            FinalPartionPair PartionPair = new FinalPartionPair();
            PartionPair._P = p;

            var vDiffered = inputQuatretList.FindAll(x => x._PartitionStatus == PartitionStatus.Differed);
            var vIsolated = inputQuatretList.FindAll(x => x._PartitionStatus == PartitionStatus.Isolated);

            if (vIsolated != null)
            {
                if (vIsolated.Count != 0)
                {
                    Q = vIsolated.ToList();
                }
            }

            if (vDiffered != null)
            {
                if (vDiffered.Count != 0)
                {
                    foreach (Quartet q in vDiffered.ToList())
                    {
                        Q.Add(q);
                    }

                }
            }

            if (Q.Count() != 0)
            {
                QA = GetQuatretWithDummy(p, Q, DummyTaxa);
                PartionPair._Q = QA;
                PartionPair._InputQuatret = Q;
            }



            return PartionPair;
        }

        public List<Quartet> GetQuatretWithDummy(Partition PA, List<Quartet> Q, string DummyTaxa)
        {

            List<Quartet> QA = new List<Quartet>();
            //------------------PA=Set of Taxa 
            //------------------QA= Set of Quatret
            int count = 0;
            foreach (Quartet q in Q)
            {
                count = 0;
                foreach (Taxa t in PA.TaxaList)
                {
                    if (q._First_Taxa_Value == t._Taxa_Value || q._Second_Taxa_Value == t._Taxa_Value || q._Third_Taxa_Value == t._Taxa_Value || q._Fourth_Taxa_Value == t._Taxa_Value)
                    {
                        count++;
                    }


                }

                if (count == 3 || count == 4)
                {
                    QA.Add(q);
                }


            }



            foreach (Quartet q in QA)
            {


                var v1 = PA.TaxaList.Where(x => x._Taxa_Value == q._First_Taxa_Value).Count();
                var v2 = PA.TaxaList.Where(x => x._Taxa_Value == q._Second_Taxa_Value).Count();
                var v3 = PA.TaxaList.Where(x => x._Taxa_Value == q._Third_Taxa_Value).Count();
                var v4 = PA.TaxaList.Where(x => x._Taxa_Value == q._Fourth_Taxa_Value).Count();

                if (v1 == 0)
                {
                    q._First_Taxa_Value = DummyTaxa;

                }

                else if (v2 == 0)
                {
                    q._Second_Taxa_Value = DummyTaxa;

                }

                else if (v3 == 0)
                {

                    q._Third_Taxa_Value = DummyTaxa;
                }

                else if (v4 == 0)
                {
                    q._Fourth_Taxa_Value = DummyTaxa;

                }
                q._Quartet_Input = "((" + q._First_Taxa_Value + "," + q._Second_Taxa_Value + "),(" + q._Third_Taxa_Value + "," + q._Fourth_Taxa_Value + "));";
                q._Quartet_LeftPart = q._First_Taxa_Value.ToString() + q._Second_Taxa_Value.ToString();
                q._Quartet_LeftPartReverse = q._Second_Taxa_Value.ToString() + q._First_Taxa_Value.ToString();
                q._Quartet_RightPart = q._Third_Taxa_Value.ToString() + q._Fourth_Taxa_Value.ToString();
                q._Quartet_RightPartReverse = q._Fourth_Taxa_Value.ToString() + q._Third_Taxa_Value.ToString();
                q._PartitionStatus = PartitionStatus.None;
                q._isDistinct = false;
                q._Frequency = 0;
                q._DuplicateQuatrets = new List<string>();


            }

            return QA;

        }

    }
}
