using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeConstructionFromQuartets.Model;

namespace TreeConstructionFromQuartets
{
    public class ProgramCalculation
    {
        //string PathInconsistentQuatret = ConfigurationManager.AppSettings["PathInconsistentQuatret"].ToString();
        public void StepByStep()
        {
            #region Step:1 Find Depth One Chain and Consistancy Calculation
            ConsistancyCalculation obj = new ConsistancyCalculation();
            obj.CalculateConsistancy();

            // Getting the GainTable After Initial Bipartition
            GainTable GB = obj.getFinalGainTableAfterGainCalculation();
            // Getting Partition with maximum Gained Taxa 
            PartitionSet SetP = GB.PartitionSet;

            // Getting Differred, Isolated And Violated Quatret Before Devide and Conquer
            List<Quartet> _DifferedQuatretListAfterGain = obj.getDifferedQuatretListAfterGain();
            List<Quartet> _IsolatedQuatretListAfterGain = obj.getIsolatedQuatretListAfterGain();
            List<Quartet> _ViolatedQuatretListAfterGain = obj.getViolatedQuatretListAfterGain();
            // Getting All QUatret
            List<Quartet> _ALLQuatretListAfterGain = obj.getALLQuatretListAfterGain();

            // Getting Depth One Chain After Devide And Conquer
            //List<ConsistencyDataModel> _ListConsistencyDataModel = obj.getListConsistencyDataModel();
            //List<ConsistencyDataModel> _ListConsistencyDataModelRandom = obj.getListConsistencyDataModelRandom();
            //List<ConsistencyDataModel> _ListConsistencyDataModelRandomAfterDuplication = new List<ConsistencyDataModel>();

            /*
            #region OriginalDepthOneTree for Duplication with Random Technique
            List<ConsistencyDataModel> _ListConsistencyDataModelOriginal = new List<ConsistencyDataModel>();
            List<ConsistencyDataModel> _ListConsistencyDataModelOriginalAfterRandomDuplication = new List<ConsistencyDataModel>();
            ConsistencyDataModel Cmodel;
            Quartet CQuatet;
            List<Quartet> CListQuatret;
            List<DepthOneTreeNode> CListDepthOneTreeNode = new List<DepthOneTreeNode>();
            DepthOneTreeNode CDepthOneTreeNode;

            foreach (ConsistencyDataModel model in _ListConsistencyDataModel)
            {
                Cmodel = new ConsistencyDataModel();
                
                if (_ListConsistencyDataModelRandom[0]._ALL_Quatret != null)
                {
                    if (_ListConsistencyDataModelRandom[0]._ALL_Quatret.Count != 0)
                    {
                        CListQuatret = new List<Quartet>();
                        foreach (Quartet q in _ListConsistencyDataModelRandom[0]._ALL_Quatret)
                        {
                            CQuatet = new Quartet()
                            {

                                _First_Taxa_Value = q._Fourth_Taxa_Value,
                                _Second_Taxa_Value = q._Third_Taxa_Value,
                                _Third_Taxa_Value = q._Second_Taxa_Value,
                                _Fourth_Taxa_Value = q._First_Taxa_Value,
                                _Quartet_Name = q._Quartet_Name,
                                _Quartet_Input = q._Quartet_Input,
                                _Quartet_LeftPart = q._Quartet_LeftPart,
                                _Quartet_LeftPartReverse = q._Quartet_LeftPartReverse,
                                _Quartet_RightPart = q._Quartet_RightPart,
                                _Quartet_RightPartReverse = q._Quartet_RightPartReverse,
                                _isDistinct = q._isDistinct,
                                _Frequency = q._Frequency,
                                _ConsistancyStatus = ConsistencyStatus.None,
                                _PartitionStatus = q._PartitionStatus

                            };

                            CListQuatret.Add(CQuatet);
                        }
                        Cmodel._ALL_Quatret = new List<Quartet>(CListQuatret);
                    }
                }
                
                CListQuatret = new List<Quartet>();
                foreach (Quartet q in model._Isolated_Quatret)
                {
                    CQuatet = new Quartet()
                    {

                        _First_Taxa_Value = q._Fourth_Taxa_Value,
                        _Second_Taxa_Value = q._Third_Taxa_Value,
                        _Third_Taxa_Value = q._Second_Taxa_Value,
                        _Fourth_Taxa_Value = q._First_Taxa_Value,
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

                    CListQuatret.Add(CQuatet);
                }
                Cmodel._Isolated_Quatret = new List<Quartet>(CListQuatret);


                CListQuatret = new List<Quartet>();
                foreach (Quartet q in model._Violated_Quatret)
                {
                    CQuatet = new Quartet()
                    {

                        _First_Taxa_Value = q._Fourth_Taxa_Value,
                        _Second_Taxa_Value = q._Third_Taxa_Value,
                        _Third_Taxa_Value = q._Second_Taxa_Value,
                        _Fourth_Taxa_Value = q._First_Taxa_Value,
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

                    CListQuatret.Add(CQuatet);
                }
                Cmodel._Violated_Quatret = new List<Quartet>(CListQuatret);


                CListQuatret = new List<Quartet>();
                foreach (Quartet q in model._Differed_Quatret)
                {
                    CQuatet = new Quartet()
                    {

                        _First_Taxa_Value = q._Fourth_Taxa_Value,
                        _Second_Taxa_Value = q._Third_Taxa_Value,
                        _Third_Taxa_Value = q._Second_Taxa_Value,
                        _Fourth_Taxa_Value = q._First_Taxa_Value,
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

                    CListQuatret.Add(CQuatet);
                }
                Cmodel._Differed_Quatret = new List<Quartet>(CListQuatret);


                CListDepthOneTreeNode = new List<DepthOneTreeNode>();
                foreach (DepthOneTreeNode node in model._DepthOneChain)
                {

                    CDepthOneTreeNode = new DepthOneTreeNode();
                    CDepthOneTreeNode._Position = node._Position;
                    CDepthOneTreeNode._Taxa_Value = node._Taxa_Value;
                    CListDepthOneTreeNode.Add(CDepthOneTreeNode);
                }

                Cmodel._DepthOneChain = CListDepthOneTreeNode;

                _ListConsistencyDataModelOriginal.Add(Cmodel);
            }


            #endregion
            */
            // Getting Consistent and Inconsistent Quatret 
            List<Quartet> _DifferredConsistentAfterDevideAndConquer = obj.getDifferedQuatretConsistentAfterDevideAndConquer();
            List<Quartet> _IsolatedConsistentAfterDevideAndConquer = obj.getIsolatedQuatretConsistentAfterDevideAndConquer();
            List<Quartet> _ViolatedConsistentAfterDevideAndConquer = obj.getViolatedQuatretConsistentAfterDevideAndConquer();

            List<Quartet> _IsolatedInConsistentAfterDevideAndConquer = obj.getIsolatedQuatretInConsistentAfterDevideAndConquer();
            List<Quartet> _DifferredInConsistentAfterDevideAndConquer = obj.getDifferedQuatretInConsistentAfterDevideAndConquer();
            List<Quartet> _ViolatedInConsistentAfterDevideAndConquer = obj.getViolatedQuatretInConsistentAfterDevideAndConquer();

            //List<Quartet> _RandomConsistentAfterDevideAndConquer = obj.getRandomQuatretConsistentAfterDevideAndConquer();
            //List<Quartet> _RandomInConsistentAfterDevideAndConquer = obj.getRandomQuatretInConsistentAfterDevideAndConquer();

            #endregion

            #region Step:2 Get The Input (Isolated ,Violated, Differred )

            var vAllInConsistentQuatret = _IsolatedInConsistentAfterDevideAndConquer.Concat(_DifferredInConsistentAfterDevideAndConquer).Concat(_ViolatedInConsistentAfterDevideAndConquer);
            //OutputProcessing.WriteListOfQuatretInConsistancy(vAllInConsistentQuatret.ToList());
            OutputProcessing.WriteListOfQuatretInConsistancyWithNewPath(vAllInConsistentQuatret.ToList()); 
            

            #endregion

            /* #region Step:3 Calculate Super Split List using All Inconsistent Quatret

             SplitCalculation objSplitCalculation = new SplitCalculation();
             SplitModel SuperSplit = new SplitModel();
             if (vAllInConsistentQuatret.ToList().Count != 0)
             {
                 SuperSplit = objSplitCalculation.CalculateSuperSplit(vAllInConsistentQuatret.ToList());
                 OutputProcessing.WriteSplitValues(SuperSplit, "Super Split");
             }

             #endregion

             #region Step:4 Calculate HyBrid DepthOne List

             List<List<string>> HyBridDepthOneList = new List<List<string>>();
             if (SuperSplit._LeftPartOfSplit.Count < SuperSplit._RightPartOfSplit.Count)
             {
                 SuperSplit._RightPartOfSplit = new List<string>();
             }
             else if (SuperSplit._RightPartOfSplit.Count < SuperSplit._LeftPartOfSplit.Count)
             {
                 SuperSplit._LeftPartOfSplit = new List<string>();
             }
             else if (SuperSplit._RightPartOfSplit.Count == SuperSplit._LeftPartOfSplit.Count)
             {
                 SuperSplit._LeftPartOfSplit = new List<string>();
             }
             HyBridDepthOneList = objSplitCalculation.getHyBridDepthOneTaxaListWithoutRemovingCommonTaxa(SetP, SuperSplit);

             List<string> LeftMost = HyBridDepthOneList[0];
             List<string> RightMost = HyBridDepthOneList[1];

             List<DepthOneTreeNode> DepthOneTreeNodeLeft = new List<DepthOneTreeNode>();
             List<DepthOneTreeNode> DepthOneTreeNodeRight = new List<DepthOneTreeNode>();
             DepthOneTreeNode __node;
             int pos = 0;

             foreach (string tx in LeftMost)
             {
                 __node = new DepthOneTreeNode();
                 __node._Position = pos;
                 __node._Taxa_Value = tx;
                 pos++;
                 DepthOneTreeNodeLeft.Add(__node);

             }


             pos = 0;
             foreach (string tx in RightMost)
             {
                 __node = new DepthOneTreeNode();
                 __node._Position = pos;
                 __node._Taxa_Value = tx;
                 pos++;
                 DepthOneTreeNodeRight.Add(__node);

             }

             _ListConsistencyDataModel.Insert(0, new ConsistencyDataModel()
             {
                 _Isolated_Quatret = _ListConsistencyDataModel[0]._Isolated_Quatret,
                 _Differed_Quatret = _ListConsistencyDataModel[0]._Differed_Quatret,
                 _Violated_Quatret = _ListConsistencyDataModel[0]._Violated_Quatret,
                 _DepthOneChain = new List<DepthOneTreeNode>(DepthOneTreeNodeLeft)
             });

             _ListConsistencyDataModel.Add(new ConsistencyDataModel()
             {
                 _Isolated_Quatret = _ListConsistencyDataModel[0]._Isolated_Quatret,
                 _Differed_Quatret = _ListConsistencyDataModel[0]._Differed_Quatret,
                 _Violated_Quatret = _ListConsistencyDataModel[0]._Violated_Quatret,
                 _DepthOneChain = new List<DepthOneTreeNode>(DepthOneTreeNodeRight)
             });

             OutputProcessing.PrintHyBridDepthOneTree(_ListConsistencyDataModel);
             OutputProcessing.GenerateInputForHyBridDepthOneTree(_ListConsistencyDataModel);
             OutputProcessing.GenerateCountOfDuplicateTaxa(DepthOneTreeNodeLeft, DepthOneTreeNodeRight);
             #endregion

             #region Step:5 Calculate Random DepthOne Chain
             string OutputHeaderRandom = "===============================================Consistancy Calculation(Randomized Technique)======================================================";
             OutputProcessing.PrintHyBridDepthOneTree(_ListConsistencyDataModelRandom, "Randomized Divide and Conquer Approach with Split Analysis Technique", "====================================Depth One Element with Randomized Technique======================");
             OutputProcessing.WriteQuatretConsistancy(_ListConsistencyDataModelRandom[0]._ALL_Quatret, PartitionStatus.None, OutputHeaderRandom);
             List<Quartet> all_InconsistentQuatret_for_Random = new List<Quartet>();
             var vInConsistent_for_Random = _ListConsistencyDataModelRandom[0]._ALL_Quatret.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.InConsistent);
             foreach (Quartet q in vInConsistent_for_Random)
             {

                 CQuatet = new Quartet()
                 {

                     _First_Taxa_Value = q._Fourth_Taxa_Value,
                     _Second_Taxa_Value = q._Third_Taxa_Value,
                     _Third_Taxa_Value = q._Second_Taxa_Value,
                     _Fourth_Taxa_Value = q._First_Taxa_Value,
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

                 all_InconsistentQuatret_for_Random.Add(CQuatet);
             }

             #endregion

             #region Step:6 Calculate Duplication For Random DepthOne Chain
             objSplitCalculation.CalculateRandomDepthOneChainWithRandomizedUnionOperationDuplicationNew(_ListConsistencyDataModelRandom[0]._ALL_Quatret, _ListConsistencyDataModelRandom);
             _ListConsistencyDataModelRandomAfterDuplication = objSplitCalculation.getListConsistencyDataModelRandomAfterDuplication();
             bool isAddedtotheleft = objSplitCalculation.getIsAddedToLeft();
             int totalCount = _ListConsistencyDataModelRandomAfterDuplication.Count() - 1;
             //OutputHeaderRandom = "======================================================Consistancy Calculation(Random Technique Duplication)======================================================";
             OutputProcessing.PrintHyBridDepthOneTree(_ListConsistencyDataModelRandomAfterDuplication, "Randomized Depth One Element With Duplicated Taxa");
             if (isAddedtotheleft)
                 OutputProcessing.GenerateCountOfDuplicateTaxaOfRandomDepthTree(_ListConsistencyDataModelRandomAfterDuplication[0]._DepthOneChain, isAddedtotheleft);
             else
                 OutputProcessing.GenerateCountOfDuplicateTaxaOfRandomDepthTree(_ListConsistencyDataModelRandomAfterDuplication[totalCount]._DepthOneChain, isAddedtotheleft);
             #endregion

             #region Step:7 Calculate Super Split for Random DepthOne Chain


             OutputHeaderRandom = "======================================================Inconsistant Quatrets(Randomized Divide and Conquer Approach)======================================================";
             OutputProcessing.WriteQuatretConsistancy(all_InconsistentQuatret_for_Random, PartitionStatus.None, OutputHeaderRandom);

             objSplitCalculation = new SplitCalculation();
             SuperSplit = new SplitModel();
             if (vAllInConsistentQuatret.ToList().Count != 0)
             {
                 SuperSplit = objSplitCalculation.CalculateSuperSplit(all_InconsistentQuatret_for_Random.ToList());
                 OutputProcessing.WriteSplitValues(SuperSplit, "Super Split For Randomized Depth One Chain");
             }

             // Calculate Random DepthOne Chain With SuperSplit
             List<List<string>> HyBridDepthOneListRandom = new List<List<string>>();
             if (SuperSplit._LeftPartOfSplit.Count < SuperSplit._RightPartOfSplit.Count)
             {
                 SuperSplit._RightPartOfSplit = new List<string>();
             }
             else if (SuperSplit._RightPartOfSplit.Count < SuperSplit._LeftPartOfSplit.Count)
             {
                 SuperSplit._LeftPartOfSplit = new List<string>();
             }
             else if (SuperSplit._RightPartOfSplit.Count == SuperSplit._LeftPartOfSplit.Count)
             {
                 SuperSplit._LeftPartOfSplit = new List<string>();
             }
             HyBridDepthOneListRandom = objSplitCalculation.getHyBridDepthOneTaxaListWithoutRemovingCommonTaxa(null, SuperSplit);

             List<string> LeftMostRandom = HyBridDepthOneListRandom[0];
             List<string> RightMostRandom = HyBridDepthOneListRandom[1];

             List<DepthOneTreeNode> DepthOneTreeNodeLeftRandom = new List<DepthOneTreeNode>();
             List<DepthOneTreeNode> DepthOneTreeNodeRightRandom = new List<DepthOneTreeNode>();
             DepthOneTreeNode __nodeRandom;
             pos = 0;

             foreach (string tx in LeftMostRandom)
             {
                 __nodeRandom = new DepthOneTreeNode();
                 __nodeRandom._Position = pos;
                 __nodeRandom._Taxa_Value = tx;
                 pos++;
                 DepthOneTreeNodeLeftRandom.Add(__nodeRandom);

             }


             pos = 0;
             foreach (string tx in RightMostRandom)
             {
                 __nodeRandom = new DepthOneTreeNode();
                 __nodeRandom._Position = pos;
                 __nodeRandom._Taxa_Value = tx;
                 pos++;
                 DepthOneTreeNodeRightRandom.Add(__nodeRandom);

             }

             if (isAddedtotheleft)
             {
                 _ListConsistencyDataModelRandom.RemoveAt(0);
             }
             else
             {
                 _ListConsistencyDataModelRandom.RemoveAt(_ListConsistencyDataModelRandom.Count - 1);
             }

             _ListConsistencyDataModelRandom.Insert(0, new ConsistencyDataModel()
             {
                 _Isolated_Quatret = _ListConsistencyDataModelRandom[0]._Isolated_Quatret,
                 _Differed_Quatret = _ListConsistencyDataModelRandom[0]._Differed_Quatret,
                 _Violated_Quatret = _ListConsistencyDataModelRandom[0]._Violated_Quatret,
                 _DepthOneChain = new List<DepthOneTreeNode>(DepthOneTreeNodeLeftRandom)
             });

             _ListConsistencyDataModelRandom.Add(new ConsistencyDataModel()
             {
                 _Isolated_Quatret = _ListConsistencyDataModelRandom[0]._Isolated_Quatret,
                 _Differed_Quatret = _ListConsistencyDataModelRandom[0]._Differed_Quatret,
                 _Violated_Quatret = _ListConsistencyDataModelRandom[0]._Violated_Quatret,
                 _DepthOneChain = new List<DepthOneTreeNode>(DepthOneTreeNodeRightRandom)
             });

             OutputProcessing.PrintHyBridDepthOneTree(_ListConsistencyDataModelRandom, "Randomized Depth One Element with Minimum Super-Element");
             //OutputProcessing.GenerateInputForHyBridDepthOneTree(_ListConsistencyDataModelRandom);
             OutputProcessing.GenerateCountOfDuplicateTaxa(DepthOneTreeNodeLeftRandom, DepthOneTreeNodeRightRandom);
             //-------
             #endregion

             #region Step 8: Calculate Duplication for Original Depth One Chain with Random Technique


             List<Quartet> all_Quatret = new List<Quartet>();
             all_Quatret = obj.GetConsistancyStatusOfQuatret(_ListConsistencyDataModelOriginal, _ListConsistencyDataModelOriginal[0]._ALL_Quatret);

             foreach (ConsistencyDataModel dmodel in _ListConsistencyDataModelOriginal)
             {
                 dmodel._ALL_Quatret = new List<Quartet>(all_Quatret);
             }

             OutputProcessing.PrintHyBridDepthOneTree(_ListConsistencyDataModelOriginal, "Depth One Element from Bipartition Based Divide and Conquer Approach");
             ConsistencyDataModel dmodelOriginal = _ListConsistencyDataModelOriginal[0];
             OutputHeaderRandom = string.Empty;

             var vConsistentOriginal = dmodelOriginal._ALL_Quatret.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.Consistent);
             var vInConsistentOriginal = dmodelOriginal._ALL_Quatret.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.InConsistent);

             objSplitCalculation.CalculateRandomDepthOneChainWithRandomizedUnionOperationDuplicationNew(vInConsistentOriginal, _ListConsistencyDataModelOriginal);
             _ListConsistencyDataModelOriginalAfterRandomDuplication = objSplitCalculation.getListConsistencyDataModelRandomAfterDuplication();
             isAddedtotheleft = objSplitCalculation.getIsAddedToLeft();
             totalCount = _ListConsistencyDataModelOriginalAfterRandomDuplication.Count() - 1;

             OutputProcessing.WriteQuatretConsistancy(dmodelOriginal._ALL_Quatret, PartitionStatus.None, OutputHeaderRandom);
             OutputProcessing.PrintHyBridDepthOneTree(_ListConsistencyDataModelOriginalAfterRandomDuplication, "Depth One Element with Randomized Duplication");
             if (isAddedtotheleft)
                 OutputProcessing.GenerateCountOfDuplicateTaxaOfRandomDepthTree(_ListConsistencyDataModelOriginalAfterRandomDuplication[0]._DepthOneChain, isAddedtotheleft);
             else
             {
                 OutputProcessing.GenerateCountOfDuplicateTaxaOfRandomDepthTree(_ListConsistencyDataModelOriginalAfterRandomDuplication[totalCount]._DepthOneChain, isAddedtotheleft);
             }

             #endregion
             */
        }

    }
}
