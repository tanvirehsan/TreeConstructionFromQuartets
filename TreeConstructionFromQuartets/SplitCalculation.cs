using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeConstructionFromQuartets.Model;

namespace TreeConstructionFromQuartets
{
    public class SplitCalculation
    {

        private List<SplitModel> FinalSplit = new List<SplitModel>();

        private List<ConsistencyDataModel> _DepthOneChainRandom = new List<ConsistencyDataModel>();
        private int _DepthOneChainRandomCount = 0;

        private bool _isAddedTotheLeft = false;

        //private List<ConsistencyDataModel> _DepthOneChainMain = new List<ConsistencyDataModel>();
        //private int _DepthOneChainMainCount = 0;

        public void GetFinalSplitCalculation(List<Quartet> _listOfViolatedQuatret, SplitModel Model, int countTotalInConsistentQuatret)
        {
            List<Quartet> listOfViolatedQuatret = new List<Quartet>(_listOfViolatedQuatret);

            List<SplitModel> Split = new List<SplitModel>();
            SplitModel objSplitCurrent;
            Quartet CurrentInput = new Quartet();


            if (listOfViolatedQuatret.Count == 0)
            {
                SplitModel objModel = new SplitModel();
                objModel._LeftPartOfSplit = Model._LeftPartOfSplit;
                objModel._RightPartOfSplit = Model._RightPartOfSplit;
                objModel._LeftPartOfSplit.Sort();
                objModel._RightPartOfSplit.Sort();
                objModel._CountTaxa = objModel._LeftPartOfSplit.Count() + objModel._RightPartOfSplit.Count();
                FinalSplit.Add(objModel);
                return;
            }


            objSplitCurrent = new SplitModel();
            CurrentInput = listOfViolatedQuatret[0];
            objSplitCurrent._LeftPartOfSplit.Add(CurrentInput._First_Taxa_Value);
            objSplitCurrent._LeftPartOfSplit.Add(CurrentInput._Second_Taxa_Value);
            objSplitCurrent._RightPartOfSplit.Add(CurrentInput._Third_Taxa_Value);
            objSplitCurrent._RightPartOfSplit.Add(CurrentInput._Fourth_Taxa_Value);
            objSplitCurrent._LeftPartOfSplit.Sort();
            objSplitCurrent._RightPartOfSplit.Sort();
            objSplitCurrent._CountTaxa = objSplitCurrent._LeftPartOfSplit.Count() + objSplitCurrent._RightPartOfSplit.Count();
            List<SplitModel> _splits;
            if (listOfViolatedQuatret.Count == countTotalInConsistentQuatret)
            {
                if (listOfViolatedQuatret.Count >= 1)
                    listOfViolatedQuatret.RemoveAt(0);
                GetFinalSplitCalculation(listOfViolatedQuatret, objSplitCurrent, countTotalInConsistentQuatret);
            }
            else
            {
                _splits = getSplitArrays(CurrentInput, Model);
                if (listOfViolatedQuatret.Count >= 1)
                    listOfViolatedQuatret.RemoveAt(0);

                if (_splits[0]._CountTaxa < _splits[1]._CountTaxa)
                {
                    GetFinalSplitCalculation(listOfViolatedQuatret, _splits[0], countTotalInConsistentQuatret);
                }
                else if (_splits[0]._CountTaxa > _splits[1]._CountTaxa)
                {
                    GetFinalSplitCalculation(listOfViolatedQuatret, _splits[1], countTotalInConsistentQuatret);
                }
                else
                {
                    GetFinalSplitCalculation(listOfViolatedQuatret, _splits[0], countTotalInConsistentQuatret);
                }

                //GetFinalSplitCalculation(listOfViolatedQuatret, _splits[0], countTotalInConsistentQuatret);
                //GetFinalSplitCalculation(listOfViolatedQuatret, _splits[1], countTotalInConsistentQuatret);
            }







        }

        public List<SplitModel> getFinalSplitList()
        {

            return this.FinalSplit;
        }

        public SplitModel getSuperSplitModel(List<SplitModel> ListSplitModel)
        {
            SplitModel SuperSplit = new SplitModel();
            var MinValue = ListSplitModel.Min(x => x._CountTaxa);
            var result = ListSplitModel.First(x => x._CountTaxa == MinValue);
            if (result != null)
                SuperSplit = result;
            return SuperSplit;
        }

        public List<SplitModel> getSplitArrays(Quartet q, SplitModel model)
        {
            List<SplitModel> list = new List<SplitModel>();

            Quartet Left = q;
            Quartet Right = Reverse(q);

            list.Add(getSplitModelAfterCalculation(Left, model));
            list.Add(getSplitModelAfterCalculation(Right, model));


            return list;
        }

        public SplitModel getSplitModelAfterCalculation(Quartet q, SplitModel model)
        {
            SplitModel objSplit = new SplitModel();
            objSplit._InputQuatret = q;
            objSplit._LeftPartOfSplit.Add(q._First_Taxa_Value);
            objSplit._LeftPartOfSplit.Add(q._Second_Taxa_Value);
            objSplit._LeftPartOfSplit = getUnionOfTaxaList(objSplit._LeftPartOfSplit, model._LeftPartOfSplit);

            objSplit._RightPartOfSplit.Add(q._Third_Taxa_Value);
            objSplit._RightPartOfSplit.Add(q._Fourth_Taxa_Value);
            objSplit._RightPartOfSplit = getUnionOfTaxaList(objSplit._RightPartOfSplit, model._RightPartOfSplit);

            objSplit._LeftPartOfSplit.Sort();
            objSplit._RightPartOfSplit.Sort();
            objSplit._CountTaxa = objSplit._LeftPartOfSplit.Count() + objSplit._RightPartOfSplit.Count();

            return objSplit;
        }

        public List<string> getUnionOfTaxaList(List<string> root, List<string> current)
        {
            var distinct = root.Union(current).ToList();
            return distinct;
        }


        public Quartet Reverse(Quartet q)
        {

            Quartet Right = new Quartet();
            Right._First_Taxa_Value = q._Third_Taxa_Value;
            Right._Second_Taxa_Value = q._Fourth_Taxa_Value;
            Right._Third_Taxa_Value = q._First_Taxa_Value;
            Right._Fourth_Taxa_Value = q._Second_Taxa_Value;
            return Right;

        }


        #region Main Calculation

        public SplitModel CalculateSuperSplit(List<Quartet> violted)
        {

            SplitCalculation Splitc;
            SplitModel SuperSplit = new SplitModel();
            if (violted.Count != 0)
            {
                Splitc = new SplitCalculation();
                if (violted.Count > 1)
                {
                    Splitc.GetFinalSplitCalculation(violted, new SplitModel(), violted.Count);
                    List<SplitModel> FinalSplit = new List<SplitModel>();
                    FinalSplit = Splitc.getFinalSplitList();
                    SuperSplit = Splitc.getSuperSplitModel(FinalSplit);
                }
                else if (violted.Count == 1)
                {
                    SuperSplit = new SplitModel();
                    SuperSplit._InputQuatret = violted[0];
                    SuperSplit._LeftPartOfSplit.Add(violted[0]._First_Taxa_Value);
                    SuperSplit._LeftPartOfSplit.Add(violted[0]._Second_Taxa_Value);
                    SuperSplit._RightPartOfSplit.Add(violted[0]._Third_Taxa_Value);
                    SuperSplit._RightPartOfSplit.Add(violted[0]._Fourth_Taxa_Value);
                    SuperSplit._LeftPartOfSplit.Sort();
                    SuperSplit._RightPartOfSplit.Sort();
                    SuperSplit._CountTaxa = SuperSplit._LeftPartOfSplit.Count() + SuperSplit._RightPartOfSplit.Count();
                }
            }
            return SuperSplit;
        }

        public List<List<string>> getHyBridDepthOneTaxaListWithoutRemovingCommonTaxa(PartitionSet SetP, SplitModel DuplicateSplit)
        {
            List<List<string>> list = new List<List<string>>();
            List<string> _Left = new List<string>();
            List<string> _Right = new List<string>();


            _Left = DuplicateSplit._LeftPartOfSplit.Distinct().ToList();
            _Right = DuplicateSplit._RightPartOfSplit.Distinct().ToList();

            _Left.Sort();
            _Right.Sort();

            list.Add(_Left);
            list.Add(_Right);

            return list;


        }

        #endregion

        #region Random Calculation

        public bool getIsAddedToLeft()
        {
            return this._isAddedTotheLeft;
        }
        public List<ConsistencyDataModel> getListConsistencyDataModelRandomAfterDuplication()
        {
            int countToStop = this._DepthOneChainRandom.Count - this._DepthOneChainRandomCount;

            int i = 0;
            foreach (ConsistencyDataModel model in this._DepthOneChainRandom)
            {
                if (i > 0 && i < countToStop)
                {
                    foreach (DepthOneTreeNode node in model._DepthOneChain)
                    {

                        var vMatchedNode = this._DepthOneChainRandom[0]._DepthOneChain.FindAll(x => x._Taxa_Value == node._Taxa_Value);

                        if (vMatchedNode != null)
                        {
                            if (vMatchedNode.Count == 0)
                                this._DepthOneChainRandom[0]._DepthOneChain.Add(node);
                        }
                    }
                }
                i++;
            }

            try
            {
                for (i = 1; i < countToStop; i++)
                {
                    this._DepthOneChainRandom.RemoveAt(1);
                }
            }
            catch (Exception ex)
            {
            }
            return this._DepthOneChainRandom;
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


        #region New Duplication

        #region Random Depth One Chain With Randomized Union Operation Technique
        public void CalculateRandomDepthOneChainWithRandomizedUnionOperationDuplicationNew(List<Quartet> QuatretRandom, List<ConsistencyDataModel> _pDepthOneChainRandom)
        {
            Random rnd = new Random();
            int number = 0;
            this._DepthOneChainRandom = _pDepthOneChainRandom;
            this._DepthOneChainRandomCount = _pDepthOneChainRandom.Count;

            SetPositioningInDepthOneChainRandom();

            List<Quartet> _ALL_Quatret = GetConsistancyStatusOfQuatret(this._DepthOneChainRandom, QuatretRandom);
            var vConsistentRandom = _ALL_Quatret.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.Consistent);
            var vInConsistentRandom = _ALL_Quatret.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.InConsistent).ToList();

            List<string> Left = new List<string>();
            List<string> Right = new List<string>();
            bool isLeft = false;
            foreach (Quartet q in vInConsistentRandom)
            {
                number = rnd.Next(1, 10000);
                if (number % 2 == 0)
                {
                    isLeft = true;

                }
                else
                {
                    isLeft = false;

                }

                if (isLeft)
                {
                    Left.Add(q._First_Taxa_Value);
                    Left.Add(q._Second_Taxa_Value);
                    Right.Add(q._Third_Taxa_Value);
                    Right.Add(q._Fourth_Taxa_Value);
                    isLeft = false;
                }
                else
                {
                    Left.Add(q._Third_Taxa_Value);
                    Left.Add(q._Fourth_Taxa_Value);
                    Right.Add(q._First_Taxa_Value);
                    Right.Add(q._Second_Taxa_Value);
                    isLeft = true;
                }
            }

            isLeft = false;

            var _Left = Left.Distinct();
            var _Right = Right.Distinct();

            if (_Left.Count() == _Right.Count())
            {

                number = rnd.Next(1, 10000);
                if (number % 2 == 0)
                {
                    isLeft = false;
                    this._isAddedTotheLeft = isLeft;
                }
                else
                {
                    isLeft = true;
                    this._isAddedTotheLeft = isLeft;
                }

            }
            else if (_Left.Count() < _Right.Count())
            {
                isLeft = true;
                this._isAddedTotheLeft = isLeft;
            }
            else if (_Left.Count() > _Right.Count())
            {
                isLeft = false;
                this._isAddedTotheLeft = isLeft;
            }
            if (isLeft)
                AddDuplicationIntoTheChainNew(_Left.ToList(), isLeft);
            else
                AddDuplicationIntoTheChainNew(_Right.ToList(), isLeft);

            SetPositioningInDepthOneChainRandom();
        }
        #endregion

        #region Random Depth One Chain With Normal Union Operation Technique


        public void CalculateRandomDepthOneChainWithNormalUnionOperationDuplicationNew(List<Quartet> QuatretRandom, List<ConsistencyDataModel> _pDepthOneChainRandom)
        {
            this._DepthOneChainRandom = _pDepthOneChainRandom;
            this._DepthOneChainRandomCount = _pDepthOneChainRandom.Count;

            SetPositioningInDepthOneChainRandom();

            List<Quartet> _ALL_Quatret = GetConsistancyStatusOfQuatret(this._DepthOneChainRandom, QuatretRandom);
            var vConsistentRandom = _ALL_Quatret.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.Consistent);
            var vInConsistentRandom = _ALL_Quatret.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.InConsistent).ToList();

            List<string> Left = new List<string>();
            List<string> Right = new List<string>();
            foreach (Quartet q in vInConsistentRandom)
            {
                Left.Add(q._First_Taxa_Value);
                Left.Add(q._Second_Taxa_Value);
                Right.Add(q._Third_Taxa_Value);
                Right.Add(q._Fourth_Taxa_Value);
            }

            bool isLeft = false;

            var _Left = Left.Distinct();
            var _Right = Right.Distinct();

            if (_Left.Count() == _Right.Count())
            {
                Random rnd = new Random();
                int number = rnd.Next(1, 10000);
                if (number % 2 == 0)
                {
                    isLeft = false;
                    this._isAddedTotheLeft = isLeft;
                }
                else
                {
                    isLeft = true;
                    this._isAddedTotheLeft = isLeft;
                }

            }
            else if (_Left.Count() < _Right.Count())
            {
                isLeft = true;
                this._isAddedTotheLeft = isLeft;
            }
            else if (_Left.Count() > _Right.Count())
            {
                isLeft = false;
                this._isAddedTotheLeft = isLeft;
            }
            if (isLeft)
                AddDuplicationIntoTheChainNew(_Left.ToList(), isLeft);
            else
                AddDuplicationIntoTheChainNew(_Right.ToList(), isLeft);

            SetPositioningInDepthOneChainRandom();
        }

        #endregion

        private void SetPositioningInDepthOneChainRandom()
        {
            int pos = 0;
            foreach (ConsistencyDataModel tx in _DepthOneChainRandom)
            {
                foreach (DepthOneTreeNode nd in tx._DepthOneChain)
                {
                    nd._Position = pos;

                }
                pos++;
            }
        }


        private void AddDuplicationIntoTheChainNew(List<string> _List_Taxa_Value, bool isLeft)
        {

            List<DepthOneTreeNode> DepthOneTreeNodeLeft = new List<DepthOneTreeNode>();

            if (isLeft)
            {
                foreach (string _Taxa_Value in _List_Taxa_Value)
                {
                    DepthOneTreeNodeLeft.Add(new DepthOneTreeNode()
                    {
                        _Position = 0,
                        _Taxa_Value = _Taxa_Value
                    });
                    _DepthOneChainRandom.Insert(0, new ConsistencyDataModel()
                    {
                        _Isolated_Quatret = _DepthOneChainRandom[0]._Isolated_Quatret,
                        _Differed_Quatret = _DepthOneChainRandom[0]._Differed_Quatret,
                        _Violated_Quatret = _DepthOneChainRandom[0]._Violated_Quatret,
                        _DepthOneChain = new List<DepthOneTreeNode>(DepthOneTreeNodeLeft)
                    });

                }

            }
            else
            {
                foreach (string _Taxa_Value in _List_Taxa_Value)
                {
                    DepthOneTreeNodeLeft.Add(new DepthOneTreeNode()
                    {
                        _Position = 0,
                        _Taxa_Value = _Taxa_Value
                    });

                }
                _DepthOneChainRandom.Add(new ConsistencyDataModel()
                {
                    _Isolated_Quatret = _DepthOneChainRandom[0]._Isolated_Quatret,
                    _Differed_Quatret = _DepthOneChainRandom[0]._Differed_Quatret,
                    _Violated_Quatret = _DepthOneChainRandom[0]._Violated_Quatret,
                    _DepthOneChain = new List<DepthOneTreeNode>(DepthOneTreeNodeLeft)
                });

            }

        }
        #endregion

        #endregion

    }
}
