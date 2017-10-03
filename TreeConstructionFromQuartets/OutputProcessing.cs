namespace TreeConstructionFromQuartets
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using TreeConstructionFromQuartets.Model;

    public class OutputProcessing
    {

        public static void GenerateCountOfDuplicateTaxa(List<DepthOneTreeNode> DepthOneTreeNodeLeft, List<DepthOneTreeNode> DepthOneTreeNodeRight)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("======================================================Total Duplicate Count ======================================================");
            sb.AppendLine("Left Part: " + DepthOneTreeNodeLeft.Count.ToString());
            sb.AppendLine("Right Part: " + DepthOneTreeNodeRight.Count.ToString());
            sb.AppendLine("Total Duplicate Count: " + (DepthOneTreeNodeLeft.Count + DepthOneTreeNodeRight.Count).ToString());
            sb.AppendLine("-----------------------------------------------------------------------------------------------------------");
            //File.WriteAllText(Constant.OutputFilePath, sb.ToString());
            File.AppendAllText(Constant.OutputFilePath, sb.ToString());
        }

        public static void GenerateCountOfDuplicateTaxaOfRandomDepthTree(List<DepthOneTreeNode> DepthOneTreeNodeLeft, bool isAddedTotheLeft)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("======================================================Total Duplicate Count ======================================================");
            if (isAddedTotheLeft)
                sb.AppendLine("Left Part: " + DepthOneTreeNodeLeft.Count.ToString());
            else
                sb.AppendLine("Right Part: " + DepthOneTreeNodeLeft.Count.ToString());

            sb.AppendLine("-----------------------------------------------------------------------------------------------------------");
            //File.WriteAllText(Constant.OutputFilePath, sb.ToString());
            File.AppendAllText(Constant.OutputFilePath, sb.ToString());
        }

        public static string getNodeValuesWithDistantLength(List<DepthOneTreeNode> ListDepthOneChain)
        {
            string val0 = string.Empty;
            int CountValid = 0;
            int n;
            foreach (DepthOneTreeNode node in ListDepthOneChain)
            {
                bool isNumeric = int.TryParse(node._Taxa_Value, out n);
                if (isNumeric)
                {

                    val0 = val0 + "\"" + node._Taxa_Value + "\":0.2" + ",";
                    CountValid++;
                }
            }
            if (CountValid == 1)
            {
                val0 = val0.Substring(0, val0.LastIndexOf(","));
                return val0;
            }
            else if (CountValid > 1)
            {
                val0 = "(" + val0.Substring(0, val0.LastIndexOf(",")) + ")";
                return val0;
            }

            return string.Empty;


        }

        public static string getNodeValues(List<DepthOneTreeNode> ListDepthOneChain)
        {
            string val0 = string.Empty;
            int CountValid = 0;
            int n;
            foreach (DepthOneTreeNode node in ListDepthOneChain)
            {
                bool isNumeric = int.TryParse(node._Taxa_Value, out n);
                if (isNumeric)
                {

                    val0 = val0 + node._Taxa_Value + ",";
                    CountValid++;
                }
            }
            if (CountValid == 1)
            {
                val0 = val0.Substring(0, val0.LastIndexOf(","));
                return val0;
            }
            else if (CountValid > 1)
            {
                val0 = "(" + val0.Substring(0, val0.LastIndexOf(",")) + ")";
                return val0;
            }

            return string.Empty;


        }



        public static void GenerateInputForHyBridDepthOneTree(List<ConsistencyDataModel> _ListConsistencyDataModel, string Header = "")
        {
            string ReturnString = string.Empty;
            StringBuilder sb = new StringBuilder();


            string val0 = string.Empty;
            string val1 = string.Empty;
            foreach (ConsistencyDataModel obj in _ListConsistencyDataModel)
            {
                val0 = getNodeValues(obj._DepthOneChain);
                if (string.IsNullOrEmpty(val1))
                    val1 = val0;
                else
                    val1 = "(" + val1 + "," + val0 + ")";
            }
            sb.Append(val1);
            if (sb.ToString().Contains(","))
                ReturnString = sb.ToString() + ";";
            sb = new StringBuilder();
            if (string.IsNullOrEmpty(Header))
                sb.AppendLine("======================================================MUL Tree in Newick Format ======================================================");
            else
                sb.AppendLine("====================================================== " + Header + " ======================================================");
            sb.AppendLine(ReturnString);
            sb.AppendLine("---------------------------------------------------");

            //File.WriteAllText(Constant.OutputFilePath, sb.ToString());
            File.AppendAllText(Constant.OutputFilePath, sb.ToString());

        }

        public static void PrintHyBridDepthOneTree(List<ConsistencyDataModel> _ListConsistencyDataModel, string Title = "Depth One Element with Minimum Super-Element", string SubTitle = "")
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("======================================================" + Title + " ======================================================");
            if (!string.IsNullOrEmpty(SubTitle))
                sb.AppendLine(SubTitle);

            string val0 = string.Empty;
            string val1 = string.Empty;
            foreach (ConsistencyDataModel obj in _ListConsistencyDataModel)
            {
                val0 = "{";
                foreach (DepthOneTreeNode node in obj._DepthOneChain)
                {
                    val0 = val0 + node._Taxa_Value + ",";
                }
                if (val0.Contains(","))
                    val0 = val0.Substring(0, val0.LastIndexOf(',')) + "}";
                else
                    val0 = val0 + "}";

                sb.Append(" " + val0 + " " + val1);
            }

            sb.AppendLine("");
            sb.AppendLine("---------------------------------------------------");

            //File.WriteAllText(Constant.OutputFilePath, sb.ToString());
            File.AppendAllText(Constant.OutputFilePath, sb.ToString());
        }
        public static void PrintFinalTableOfDivideAndConquerApproach(List<FinalPartionPair> _ListFinalPartionPair, string Title = "Depth One Element")
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("======================================================" + Title + "======================================================");


            string val0 = string.Empty;
            string val1 = string.Empty;
            foreach (FinalPartionPair obj in _ListFinalPartionPair)
            {
                val0 = "{";


                foreach (Taxa tx in obj._P.TaxaList)
                {
                    val0 = val0 + tx._Taxa_Value + ",";
                }
                if (val0.Contains(","))
                    val0 = val0.Substring(0, val0.LastIndexOf(',')) + "}";
                else
                    val0 = val0 + "}";



                sb.Append(" " + val0 + " " + val1);
            }

            sb.AppendLine("");
            sb.AppendLine("---------------------------------------------------");

            //File.WriteAllText(Constant.OutputFilePath, sb.ToString());
            File.AppendAllText(Constant.OutputFilePath, sb.ToString());
        }

        public static void PrintDepthOneElement(List<FinalPartionPair> _ListFinalPartionPair)
        {
            StringBuilder sb = new StringBuilder();

            string val0 = string.Empty;
            string val1 = string.Empty;
            foreach (FinalPartionPair obj in _ListFinalPartionPair)
            {
                val0 = "{";


                foreach (Taxa tx in obj._P.TaxaList)
                {
                    val0 = val0 + tx._Taxa_Value + ",";
                }
                if (val0.Contains(","))
                    val0 = val0.Substring(0, val0.LastIndexOf(',')) + "}";
                else
                    val0 = val0 + "}";



                sb.Append(" " + val0 + " " + val1);
            }

            sb.AppendLine("");
            //sb.AppendLine("---------------------------------------------------");

            //File.WriteAllText(Constant.OutputFilePath, sb.ToString());
            if (File.Exists(Constant.OutputFilePathForDepthOne))
            {
                File.Delete(Constant.OutputFilePathForDepthOne);
            }

            File.AppendAllText(Constant.OutputFilePathForDepthOne, sb.ToString());
        }

        public static void PrintDepthOneTreeRandom(List<FinalPartionPair> _ListFinalPartionPair, string Title = "Depth One Element (Randomized)")
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("======================================================" + Title + "======================================================");


            string val0 = string.Empty;
            string val1 = string.Empty;
            foreach (FinalPartionPair obj in _ListFinalPartionPair)
            {
                val0 = "{";


                foreach (Taxa tx in obj._Root)
                {
                    val0 = val0 + tx._Taxa_Value + ",";
                }
                if (val0.Contains(","))
                    val0 = val0.Substring(0, val0.LastIndexOf(',')) + "}";
                else
                    val0 = val0 + "}";



                sb.Append(" " + val0 + " " + val1);
            }

            sb.AppendLine("");
            sb.AppendLine("---------------------------------------------------");

            //File.WriteAllText(Constant.OutputFilePath, sb.ToString());
            File.AppendAllText(Constant.OutputFilePath, sb.ToString());
        }
        public static void PrintGainSummary(GainTable _TaxaGainTable)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("======================================================Gain Summary======================================================");
            int count = 0;
            int tableIndex = 1;

            count = 0;
            foreach (Taxa tt in _TaxaGainTable._TaxaGainSummary)
            {
                if (count == 0)
                {
                    sb.AppendLine("Table :" + tableIndex.ToString());
                    sb.AppendLine("------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    sb.AppendLine("                " + "K" + "                " + "                " + "Taxon" + "                " + "                " + "Gain" + "                " + "                " + "CGain" + "                ");
                    sb.AppendLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------");

                    sb.AppendLine("                " + (tt.StepK + 1).ToString() + "                " + "                " + tt._Taxa_Value + "                " + "                " + tt._Gain + "                " + "                " + tt._CumulativeGain + "                ");
                }
                else
                {
                    sb.AppendLine("                " + (tt.StepK + 1).ToString() + "                " + "                " + tt._Taxa_Value + "                " + "                " + tt._Gain + "                " + "                " + tt._CumulativeGain + "                ");
                }

                count++;
            }

            tableIndex++;



            // System.Console.WriteLine(sb.ToString());
            //System.Console.ReadLine();
            File.AppendAllText(Constant.OutputFilePath, sb.ToString());
        }
        public static void PrintGainSummary(List<GainTable> _TaxaGainTable)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("======================================================Gain Summary======================================================");
            int count = 0;
            int tableIndex = 1;
            foreach (GainTable ttm in _TaxaGainTable)
            {
                count = 0;
                foreach (Taxa tt in ttm._TaxaGainSummary)
                {
                    if (count == 0)
                    {
                        sb.AppendLine("Table :" + tableIndex.ToString());
                        sb.AppendLine("------------------------------------------------------------------------------------------------------------------------------------------------------------");
                        sb.AppendLine("                " + "K" + "                " + "                " + "Taxon" + "                " + "                " + "Gain" + "                " + "                " + "CGain" + "                ");
                        sb.AppendLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------");

                        sb.AppendLine("                " + (tt.StepK + 1).ToString() + "                " + "                " + tt._Taxa_Value + "                " + "                " + tt._Gain + "                " + "                " + tt._CumulativeGain + "                ");
                    }
                    else
                    {
                        sb.AppendLine("                " + (tt.StepK + 1).ToString() + "                " + "                " + tt._Taxa_Value + "                " + "                " + tt._Gain + "                " + "                " + tt._CumulativeGain + "                ");
                    }

                    count++;
                }

                tableIndex++;

            }

            // System.Console.WriteLine(sb.ToString());
            //System.Console.ReadLine();
            File.AppendAllText(Constant.OutputFilePath, sb.ToString());
        }
        public static void WriteInititalBiPartion(PartitionSet PartitionSets, List<string> Set_Of_Taxa)
        {

            if (File.Exists(Constant.OutputFilePath))
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("======================================================Initial Partition======================================================");
            sb.AppendLine("----------------Set Of Taxa (" + Set_Of_Taxa.Count.ToString() + ")-----------------");
            string SetOfTaxa = "P={";
            foreach (string tx in Set_Of_Taxa)
            {
                SetOfTaxa = SetOfTaxa + tx.ToString() + ",";

            }
            SetOfTaxa = SetOfTaxa.Substring(0, SetOfTaxa.Length - 1);

            sb.AppendLine(SetOfTaxa + "}");


            sb.AppendLine("----------------Set Of Quatret(" + PartitionSets._ListQuatrets.Count.ToString() + ")-----------------");
            string SetOfQuatrets = "Q={";
            foreach (Quartet tx in PartitionSets._ListQuatrets)
            {
                SetOfQuatrets = SetOfQuatrets + "((" + tx._First_Taxa_Value.ToString() + "," + tx._Second_Taxa_Value.ToString() + "),(" + tx._Third_Taxa_Value.ToString() + "," + tx._Fourth_Taxa_Value.ToString() + "));";

            }
            SetOfQuatrets = SetOfQuatrets.Substring(0, SetOfQuatrets.Length - 1);

            sb.AppendLine(SetOfQuatrets + "}");


            sb.AppendLine("----------------Initial Bipartition-----------------");
            string InitialBiPart = string.Empty;
            int ii = 0;
            //foreach (PartitionSet set in PartitionSets)
            //{
            PartitionSet set = PartitionSets;
            foreach (Partition part in set.PartitionList)
            {
                InitialBiPart = InitialBiPart + "P" + part._PartitionName + "={";

                foreach (Taxa t in part.TaxaList)
                {
                    InitialBiPart = InitialBiPart + t._Taxa_Value.ToString() + ",";
                }
                InitialBiPart = InitialBiPart.Substring(0, InitialBiPart.Length - 1);
                InitialBiPart = InitialBiPart + "}";
                sb.AppendLine(InitialBiPart);
                InitialBiPart = string.Empty;
            }


            sb.AppendLine("----------------Set Of Satisfied-----------------");
            SetOfQuatrets = "Satisfied: (" + set._SatisfiedCount.ToString() + ")={";
            foreach (Quartet tx in set._ListQuatrets)
            {
                if (tx._PartitionStatus == PartitionStatus.Satisfied)
                    SetOfQuatrets = SetOfQuatrets + "((" + tx._First_Taxa_Value.ToString() + "," + tx._Second_Taxa_Value.ToString() + "),(" + tx._Third_Taxa_Value.ToString() + "," + tx._Fourth_Taxa_Value.ToString() + "));";

            }
            SetOfQuatrets = SetOfQuatrets.Substring(0, SetOfQuatrets.Length - 1);

            if (set._SatisfiedCount != 0)
                sb.AppendLine(SetOfQuatrets + "}");
            else
                sb.AppendLine(SetOfQuatrets);




            sb.AppendLine("----------------Set Of Violated-----------------");
            SetOfQuatrets = "Viotated : (" + set._ViotatedCount.ToString() + ")={";
            foreach (Quartet tx in set._ListQuatrets)
            {
                if (tx._PartitionStatus == PartitionStatus.Viotated)
                    SetOfQuatrets = SetOfQuatrets + "((" + tx._First_Taxa_Value.ToString() + "," + tx._Second_Taxa_Value.ToString() + "),(" + tx._Third_Taxa_Value.ToString() + "," + tx._Fourth_Taxa_Value.ToString() + "));";

            }
            SetOfQuatrets = SetOfQuatrets.Substring(0, SetOfQuatrets.Length - 1);

            if (set._ViotatedCount != 0)
                sb.AppendLine(SetOfQuatrets + "}");
            else
                sb.AppendLine(SetOfQuatrets);


            sb.AppendLine("----------------Set Of Differed-----------------");
            SetOfQuatrets = "Differed : (" + set._DifferedCount.ToString() + ")={";
            foreach (Quartet tx in set._ListQuatrets)
            {
                if (tx._PartitionStatus == PartitionStatus.Differed)
                    SetOfQuatrets = SetOfQuatrets + "((" + tx._First_Taxa_Value.ToString() + "," + tx._Second_Taxa_Value.ToString() + "),(" + tx._Third_Taxa_Value.ToString() + "," + tx._Fourth_Taxa_Value.ToString() + "));";

            }
            SetOfQuatrets = SetOfQuatrets.Substring(0, SetOfQuatrets.Length - 1);

            if (set._DifferedCount != 0)
                sb.AppendLine(SetOfQuatrets + "}");
            else
                sb.AppendLine(SetOfQuatrets);


            sb.AppendLine("----------------Set Of Isolated-----------------");
            SetOfQuatrets = "Isolated : (" + set._IsolatedCount.ToString() + ")={";
            foreach (Quartet tx in set._ListQuatrets)
            {
                if (tx._PartitionStatus == PartitionStatus.Isolated)
                    SetOfQuatrets = SetOfQuatrets + "((" + tx._First_Taxa_Value.ToString() + "," + tx._Second_Taxa_Value.ToString() + "),(" + tx._Third_Taxa_Value.ToString() + "," + tx._Fourth_Taxa_Value.ToString() + "));";

            }
            SetOfQuatrets = SetOfQuatrets.Substring(0, SetOfQuatrets.Length - 1);

            if (set._IsolatedCount != 0)
                sb.AppendLine(SetOfQuatrets + "}");
            else
                sb.AppendLine(SetOfQuatrets);

            sb.AppendLine("----------------Score Calculation-----------------");
            if (ii == 0)
            {
                sb.AppendLine("Initial Score: (" + set._Final_Score.ToString() + ")");
            }
            else
            {
                sb.AppendLine("Gain Of: (" + set._taxValueForGainCalculation.ToString() + ")");
                sb.AppendLine("Final Score: (" + set._Final_Score.ToString() + ")");
                sb.AppendLine("Gain Score: (" + (set._Final_Score - PartitionSets._Final_Score).ToString() + ")");

            }
            sb.AppendLine("----------------End OF " + set._PartitionSetName);
            ii++;
            // }

            //System.Console.WriteLine(sb.ToString());
            //System.Console.ReadLine();
            File.WriteAllText(Constant.OutputFilePath, sb.ToString());
        }
        public static void WriteCountInformationFromMaxGainTable(GainTable _TaxaGainTable)
        {
            PartitionSet PartitionSets = _TaxaGainTable.PartitionSet;
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("----------------Set Of Quatret(" + PartitionSets._ListQuatrets.Count.ToString() + ")-----------------");
            string SetOfQuatrets = "Q={";
            foreach (Quartet tx in PartitionSets._ListQuatrets)
            {
                SetOfQuatrets = SetOfQuatrets + "((" + tx._First_Taxa_Value.ToString() + "," + tx._Second_Taxa_Value.ToString() + "),(" + tx._Third_Taxa_Value.ToString() + "," + tx._Fourth_Taxa_Value.ToString() + "));";

            }
            SetOfQuatrets = SetOfQuatrets.Substring(0, SetOfQuatrets.Length - 1);

            sb.AppendLine(SetOfQuatrets + "}");


            sb.AppendLine("----------------After Bipartition-----------------");
            string InitialBiPart = string.Empty;
            int ii = 0;

            PartitionSet set = PartitionSets;
            foreach (Partition part in set.PartitionList)
            {
                InitialBiPart = InitialBiPart + "P" + part._PartitionName + "={";

                foreach (Taxa t in part.TaxaList)
                {
                    InitialBiPart = InitialBiPart + t._Taxa_Value.ToString() + ",";
                }
                InitialBiPart = InitialBiPart.Substring(0, InitialBiPart.Length - 1);
                InitialBiPart = InitialBiPart + "}";
                sb.AppendLine(InitialBiPart);
                InitialBiPart = string.Empty;
            }


            sb.AppendLine("----------------Set Of Satisfied-----------------");
            SetOfQuatrets = "Satisfied: (" + set._SatisfiedCount.ToString() + ")={";
            foreach (Quartet tx in set._ListQuatrets)
            {
                if (tx._PartitionStatus == PartitionStatus.Satisfied)
                    SetOfQuatrets = SetOfQuatrets + "((" + tx._First_Taxa_Value.ToString() + "," + tx._Second_Taxa_Value.ToString() + "),(" + tx._Third_Taxa_Value.ToString() + "," + tx._Fourth_Taxa_Value.ToString() + "));";

            }
            SetOfQuatrets = SetOfQuatrets.Substring(0, SetOfQuatrets.Length - 1);

            if (set._SatisfiedCount != 0)
                sb.AppendLine(SetOfQuatrets + "}");
            else
                sb.AppendLine(SetOfQuatrets);




            sb.AppendLine("----------------Set Of Violated-----------------");
            SetOfQuatrets = "Viotated : (" + set._ViotatedCount.ToString() + ")={";
            foreach (Quartet tx in set._ListQuatrets)
            {
                if (tx._PartitionStatus == PartitionStatus.Viotated)
                    SetOfQuatrets = SetOfQuatrets + "((" + tx._First_Taxa_Value.ToString() + "," + tx._Second_Taxa_Value.ToString() + "),(" + tx._Third_Taxa_Value.ToString() + "," + tx._Fourth_Taxa_Value.ToString() + "));";

            }
            SetOfQuatrets = SetOfQuatrets.Substring(0, SetOfQuatrets.Length - 1);

            if (set._ViotatedCount != 0)
                sb.AppendLine(SetOfQuatrets + "}");
            else
                sb.AppendLine(SetOfQuatrets);


            sb.AppendLine("----------------Set Of Differed-----------------");
            SetOfQuatrets = "Differed : (" + set._DifferedCount.ToString() + ")={";
            foreach (Quartet tx in set._ListQuatrets)
            {
                if (tx._PartitionStatus == PartitionStatus.Differed)
                    SetOfQuatrets = SetOfQuatrets + "((" + tx._First_Taxa_Value.ToString() + "," + tx._Second_Taxa_Value.ToString() + "),(" + tx._Third_Taxa_Value.ToString() + "," + tx._Fourth_Taxa_Value.ToString() + "));";

            }
            SetOfQuatrets = SetOfQuatrets.Substring(0, SetOfQuatrets.Length - 1);

            if (set._DifferedCount != 0)
                sb.AppendLine(SetOfQuatrets + "}");
            else
                sb.AppendLine(SetOfQuatrets);


            sb.AppendLine("----------------Set Of Isolated-----------------");
            SetOfQuatrets = "Isolated : (" + set._IsolatedCount.ToString() + ")={";
            foreach (Quartet tx in set._ListQuatrets)
            {
                if (tx._PartitionStatus == PartitionStatus.Isolated)
                    SetOfQuatrets = SetOfQuatrets + "((" + tx._First_Taxa_Value.ToString() + "," + tx._Second_Taxa_Value.ToString() + "),(" + tx._Third_Taxa_Value.ToString() + "," + tx._Fourth_Taxa_Value.ToString() + "));";

            }
            SetOfQuatrets = SetOfQuatrets.Substring(0, SetOfQuatrets.Length - 1);

            if (set._IsolatedCount != 0)
                sb.AppendLine(SetOfQuatrets + "}");
            else
                sb.AppendLine(SetOfQuatrets);



            ii++;

            File.AppendAllText(Constant.OutputFilePath, sb.ToString());


        }

        public static void WriteListOfQuatretInConsistancy(List<Quartet> Listq)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("====================ALL InConsistent Quatret List(" + Listq.Count.ToString() + ")===============================");
            string val = "";
            foreach (Quartet q in Listq)
            {
                val = val + q._Quartet_Input + "  ";

            }
            sb.AppendLine(val);
            File.AppendAllText(Constant.OutputFilePath, sb.ToString());

        }

        public static void WriteListOfQuatretInConsistancyWithNewPath(List<Quartet> Listq )
        {
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine("====================ALL InConsistent Quatret List(" + Listq.Count.ToString() + ")===============================");
            string val = "";
            foreach (Quartet q in Listq)
            {
                val = val + q._Quartet_Input + "  ";

            }
            sb.AppendLine(val);

            if (File.Exists(Constant.OutputFilePathForInconsistent))
            {
                File.Delete(Constant.OutputFilePathForInconsistent);
            }

            File.AppendAllText(Constant.OutputFilePathForInconsistent, sb.ToString());

        }

        public static void WriteQuatretConsistancy(List<Quartet> Listq, PartitionStatus status, string pHeader)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(pHeader))
                sb.AppendLine(pHeader);
            var vIsoConsistent = Listq.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.Consistent);
            var vIsoInConsistent = Listq.FindAll(x => x._ConsistancyStatus == ConsistencyStatus.InConsistent);

            if (status == PartitionStatus.Differed)
            {
                sb.AppendLine("----------------Differed Quatret" + "(" + Listq.Count.ToString() + ")" + "-----------------");
            }
            else if (status == PartitionStatus.Isolated)
            {
                sb.AppendLine("----------------Isolated Quatret" + "(" + Listq.Count.ToString() + ")" + "-----------------");
            }
            else if (status == PartitionStatus.Viotated)
            {
                sb.AppendLine("----------------Viotated Quatret" + "(" + Listq.Count.ToString() + ")" + "-----------------");
            }
            else
            {
                sb.AppendLine("----------------ALL Quatret" + "(" + Listq.Count.ToString() + ")" + "-----------------");
            }
            sb.AppendLine("Consistent Quatret List:" + "(" + vIsoConsistent.Count.ToString() + ")");
            string val = "";
            foreach (Quartet q in vIsoConsistent)
            {
                val = val + q._Quartet_Input + "  ";

            }

            sb.AppendLine(val);
            sb.AppendLine("InConsistent Quatret List:" + "(" + vIsoInConsistent.Count.ToString() + ")");
            val = "";
            foreach (Quartet q in vIsoInConsistent)
            {
                val = val + q._Quartet_Input + "  ";
            }
            sb.AppendLine(val);
            File.AppendAllText(Constant.OutputFilePath, sb.ToString());

        }

        public static void WriteSplitValues(SplitModel model, string pHeader)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(pHeader))
                sb.AppendLine("=================================" + pHeader + "=================================");


            string val1 = "";
            foreach (string q in model._LeftPartOfSplit)
            {
                val1 = val1 + q + ",";

            }

            if (val1.Contains(","))
                val1 = val1.Substring(0, val1.LastIndexOf(','));



            string val2 = "";
            foreach (string q in model._RightPartOfSplit)
            {
                val2 = val2 + q + ",";

            }

            if (val2.Contains(","))
                val2 = val2.Substring(0, val2.LastIndexOf(','));

            sb.AppendLine(val1 + "|" + val2);

            File.AppendAllText(Constant.OutputFilePath, sb.ToString());

        }

        public static void printWrongPositionedTaxa(string path, List<Pair> pairs)
        {

            StringBuilder sb = new StringBuilder();
            string ss = string.Empty;
            foreach (Pair n in pairs)
            {
                ss = "(";
                ss = ss + n.tx1 + "," + n.tx2 + " )" + "----wrong Taxa:" + n.wrongTaxa + "----Quatret:----" + n.Quatret;
                sb.AppendLine(ss);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.AppendAllText(path, sb.ToString());
        }

        public static void printDepthOneWithNewDuplicationMethod(string path, List<TreeNode> NodeList)
        {

            StringBuilder sb = new StringBuilder();
            string ss = string.Empty;
            foreach (TreeNode n in NodeList)
            {
                ss = "{";
                foreach (Taxa tx in n.TaxaList)
                {
                    ss = ss + tx._Taxa_Value + ",";
                }

                if (ss.Contains(","))
                    ss = ss.Substring(0, ss.LastIndexOf(','));

                ss = ss + "}" + " ";
                sb.Append(ss);
            }


            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.AppendAllText(path, sb.ToString());
        }
    }

}
