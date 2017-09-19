using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TreeConstructionFromQuartets.Model;

namespace TreeConstructionFromQuartets
{
    public class NewDuplicationCalculation
    {
        #region Variables
        //public FinalPartition finalPartitionAfterDepthOneElement = new FinalPartition();
        public DepthOnleElementModel depth1Elements = new DepthOnleElementModel();
        public List<Quartet> ListOfInconsistent = new List<Quartet>();
        string PathDepthOne = ConfigurationManager.AppSettings["PathDepthOne"].ToString();
        string PathInconsistentQuatret = ConfigurationManager.AppSettings["PathInconsistentQuatret"].ToString();
        string PathOutputAfterNewDuplication = ConfigurationManager.AppSettings["PathOutputAfterNewDuplication"].ToString();
        string PathOutputForWrongPositionedTaxa = ConfigurationManager.AppSettings["PathOutputForWrongPositionedTaxa"].ToString();
        List<WrongTaxa> wrongPositionedTaxaList = new List<WrongTaxa>();
        public List<Pair> ListOfPairWithWrongTaxa = new List<Pair>();

        #endregion


        #region Read input
        public List<Quartet> readInconsistentQuatret(string path)
        {

            List<Quartet> Set_Of_Quartets = new List<Quartet>();
            Quartet quartet;
            int counter = 0;
            string line;
            int QuartetName = 0;
            System.IO.StreamReader file =
               new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line.Trim()) && line.Trim().Count() != 0)
                {
                    foreach (string l in line.Split(';'))
                    {
                        if (l.Contains(","))
                        {
                            if (l.Split(',').Count() == 4)
                            {
                                quartet = new Quartet();
                                quartet._First_Taxa_Value = Helper.getFirstTaxaValue(l.Split(',')[0]);
                                quartet._Second_Taxa_Value = Helper.getSecondTaxaValue(l.Split(',')[1]);
                                quartet._Third_Taxa_Value = Helper.getFirstTaxaValue(l.Split(',')[2]);
                                quartet._Fourth_Taxa_Value = Helper.getSecondTaxaValue(l.Split(',')[3]);
                                quartet._Quartet_Name = "q" + QuartetName.ToString();
                                quartet._Quartet_LeftPart = quartet._First_Taxa_Value.ToString() + quartet._Second_Taxa_Value.ToString();
                                quartet._Quartet_LeftPartReverse = quartet._Second_Taxa_Value.ToString() + quartet._First_Taxa_Value.ToString();
                                quartet._Quartet_RightPart = quartet._Third_Taxa_Value.ToString() + quartet._Fourth_Taxa_Value.ToString();
                                quartet._Quartet_RightPartReverse = quartet._Fourth_Taxa_Value.ToString() + quartet._Third_Taxa_Value.ToString();
                                quartet._Quartet_Input = l + ";";
                                quartet._PartitionStatus = PartitionStatus.None;
                                counter++;
                                QuartetName++;
                                Set_Of_Quartets.Add(quartet);
                            }
                        }
                    }
                }
            }

            file.Close();

            return Set_Of_Quartets;

        }
        public DepthOnleElementModel readDepthOneElementInput(string path)
        {

            string readText = string.Empty;
            var pattern = @"\{(.*?)\}";
            DepthOnleElementModel model = new DepthOnleElementModel();
            if (File.Exists(path))
            {
                readText = File.ReadAllText(path);

            }
            else
            {
                return new DepthOnleElementModel();
            }
            if (!string.IsNullOrEmpty(readText))
            {
                var matches = Regex.Matches(readText, pattern);
                List<TreeNode> TreeNodes = new List<TreeNode>();
                TreeNode node;
                int i = 1;
                foreach (Match m in matches)
                {

                    string values = m.Groups[1].Value.Trim();
                    string[] array = values.Split(',');

                    node = new TreeNode();
                    node._Position = i;
                    List<Taxa> taxas = new List<Taxa>();
                    foreach (string v in array)
                    {

                        Taxa t = new Taxa();
                        t._Taxa_Value = v.Trim();
                        taxas.Add(t);

                    }
                    if (taxas.Count != 0)
                    {
                        node.TaxaList = taxas;
                        TreeNodes.Add(node);
                    }
                    i++;

                }

                if (TreeNodes.Count != 0)
                {
                    model.NodeList = new List<TreeNode>(TreeNodes);
                }

            }

            return model;


        }
        /*
        public FinalPartition readFinalPartitionInput(string path)
        {

            string readText = string.Empty;
            var pattern = @"\{(.*?)\}";
            FinalPartition model = new FinalPartition();
            if (File.Exists(path))
            {
                readText = File.ReadAllText(path);

            }
            else
            {
                return new FinalPartition();
            }
            if (!string.IsNullOrEmpty(readText))
            {
                var matches = Regex.Matches(readText, pattern);
                List<TreeNode> TreeNodes = new List<TreeNode>();
                TreeNode node;
                int i = 1;
                int count = 0;
                foreach (Match m in matches)
                {
                    TreeNodes = new List<TreeNode>();
                    string values = m.Groups[1].Value.Trim();
                    string[] array = values.Split(',');

                    node = new TreeNode();
                    node._Position = i;
                    List<Taxa> taxas = new List<Taxa>();
                    foreach (string v in array)
                    {

                        Taxa t = new Taxa();
                        t._Taxa_Value = v.Trim();
                        taxas.Add(t);

                    }
                    if (taxas.Count != 0)
                    {
                        node.TaxaList = taxas;
                        TreeNodes.Add(node);
                    }

                    if (TreeNodes.Count != 0 && count == 0)
                    {
                        model.LeftPart = new List<TreeNode>(TreeNodes);
                    }
                    if (TreeNodes.Count != 0 && count == 1)
                    {
                        model.RightPart = new List<TreeNode>(TreeNodes);
                    }

                    i++;
                    count++;
                }



            }

            return model;


        }
        */
        #endregion

        //Constructor
        public NewDuplicationCalculation()
        {
            depth1Elements = readDepthOneElementInput(PathDepthOne);
            ListOfInconsistent = readInconsistentQuatret(PathInconsistentQuatret);
            //finalPartitionAfterDepthOneElement = readFinalPartitionInput(PathFinalPartitionAfterDepthOne);
        }
        //Main Method to Calculate Wrong Taxa
        public void CalculateWrongTaxa()
        {

            WrongPositionStatusClass obj;
            WrongTaxa wrongTaxa;
            Quartet qQuartet;
            Pair pairWithWrongTaxa;
            foreach (Quartet q in ListOfInconsistent)
            {
                try
                {
                    #region Calculate Wrong Taxa
                    Taxa t1 = getTaxa(q._First_Taxa_Value);
                    Taxa t2 = getTaxa(q._Second_Taxa_Value);
                    Taxa t3 = getTaxa(q._Third_Taxa_Value);
                    Taxa t4 = getTaxa(q._Fourth_Taxa_Value);
                    int p1 = getPosition(t1);
                    int p2 = getPosition(t2);
                    int p3 = getPosition(t3);
                    int p4 = getPosition(t4);
                    obj = new WrongPositionStatusClass();


                    obj = CalculateWrongPosition(p1, t1._Taxa_Value, p2, t2._Taxa_Value, p3, t3._Taxa_Value, p4, t4._Taxa_Value, q);


                    if (obj.WrongPositionStatus != WrongPositionStatus.None)
                    {
                        wrongTaxa = new WrongTaxa();
                        pairWithWrongTaxa = new Pair();
                        pairWithWrongTaxa.Quatret = q._Quartet_Input.Trim();

                        if (obj.WrongPositionStatus == WrongPositionStatus.aStartLeft)
                        {
                            wrongTaxa.wrongPositionStatus = WrongPositionStatus.aStartLeft;
                            wrongTaxa.Taxa = getTaxa(obj.taStart);
                            wrongTaxa._PositionIntheDepthOne = obj.aStart;
                        }
                        else if (obj.WrongPositionStatus == WrongPositionStatus.aStartRight)
                        {
                            wrongTaxa.wrongPositionStatus = WrongPositionStatus.aStartRight;
                            wrongTaxa.Taxa = getTaxa(obj.taStart);
                            wrongTaxa._PositionIntheDepthOne = obj.aStart;
                        }
                        else if (obj.WrongPositionStatus == WrongPositionStatus.aEndLeft)
                        {
                            wrongTaxa.wrongPositionStatus = WrongPositionStatus.aEndLeft;
                            wrongTaxa.Taxa = getTaxa(obj.taEnd);
                            wrongTaxa._PositionIntheDepthOne = obj.aEnd;
                        }
                        else if (obj.WrongPositionStatus == WrongPositionStatus.aEndRight)
                        {
                            wrongTaxa.wrongPositionStatus = WrongPositionStatus.aEndRight;
                            wrongTaxa.Taxa = getTaxa(obj.taEnd);
                            wrongTaxa._PositionIntheDepthOne = obj.aEnd;
                        }
                        else if (obj.WrongPositionStatus == WrongPositionStatus.bStartLeft)
                        {
                            wrongTaxa.wrongPositionStatus = WrongPositionStatus.bStartLeft;
                            wrongTaxa.Taxa = getTaxa(obj.tbStart);
                            wrongTaxa._PositionIntheDepthOne = obj.bStart;
                        }
                        else if (obj.WrongPositionStatus == WrongPositionStatus.bStartRight)
                        {
                            wrongTaxa.wrongPositionStatus = WrongPositionStatus.bStartRight;
                            wrongTaxa.Taxa = getTaxa(obj.tbStart);
                            wrongTaxa._PositionIntheDepthOne = obj.bStart;
                        }
                        else if (obj.WrongPositionStatus == WrongPositionStatus.bEndLeft)
                        {
                            wrongTaxa.wrongPositionStatus = WrongPositionStatus.bEndLeft;
                            wrongTaxa.Taxa = getTaxa(obj.tbEnd);
                            wrongTaxa._PositionIntheDepthOne = obj.bEnd;
                        }
                        else if (obj.WrongPositionStatus == WrongPositionStatus.bEndRight)
                        {
                            wrongTaxa.wrongPositionStatus = WrongPositionStatus.bEndRight;
                            wrongTaxa.Taxa = getTaxa(obj.tbEnd);
                            wrongTaxa._PositionIntheDepthOne = obj.bEnd;
                        }

                        qQuartet = new Quartet();
                        qQuartet._First_Taxa_Value = obj.taStart;
                        qQuartet._Second_Taxa_Value = obj.taEnd;
                        qQuartet._Third_Taxa_Value = obj.tbStart;
                        qQuartet._Fourth_Taxa_Value = obj.tbEnd;
                        wrongTaxa.Quartet = qQuartet;

                        //if (!isInWrongTaxaWithoutStatus(wrongTaxa.Taxa._Taxa_Value))
                        //{
                        wrongPositionedTaxaList.Add(wrongTaxa);

                        if (qQuartet == null)
                        {

                        }

                        if (wrongTaxa == null)
                        {

                        }

                        if (qQuartet._First_Taxa_Value == wrongTaxa.Taxa._Taxa_Value || qQuartet._Second_Taxa_Value == wrongTaxa.Taxa._Taxa_Value)
                        {
                            pairWithWrongTaxa.tx1 = qQuartet._First_Taxa_Value;
                            pairWithWrongTaxa.tx2 = qQuartet._Second_Taxa_Value;
                            pairWithWrongTaxa.wrongTaxa = wrongTaxa.Taxa._Taxa_Value;
                        }
                        else
                        {
                            pairWithWrongTaxa.tx1 = qQuartet._Third_Taxa_Value;
                            pairWithWrongTaxa.tx2 = qQuartet._Fourth_Taxa_Value;
                            pairWithWrongTaxa.wrongTaxa = wrongTaxa.Taxa._Taxa_Value;
                        }

                        Taxa w1 = getTaxa(pairWithWrongTaxa.tx1);
                        int d1 = getPosition(w1);
                        Taxa w2 = getTaxa(pairWithWrongTaxa.tx2);
                        int d2 = getPosition(w2);
                        int distance = -1;
                        if (d1 > d2)
                        {
                            distance = d1 - d2;
                        }
                        else
                        {
                            distance = d2 - d1;
                        }

                        pairWithWrongTaxa.diff = distance;

                        var maxDifferencedPair = ListOfPairWithWrongTaxa.Where(x => x.wrongTaxa == pairWithWrongTaxa.wrongTaxa).FirstOrDefault();
                        if (maxDifferencedPair != null)
                        {
                            if (maxDifferencedPair.diff < pairWithWrongTaxa.diff)
                            {
                                ListOfPairWithWrongTaxa.RemoveAll(x => x.wrongTaxa == pairWithWrongTaxa.wrongTaxa);
                                ListOfPairWithWrongTaxa.Add(pairWithWrongTaxa);
                            }

                        }
                        else
                            ListOfPairWithWrongTaxa.Add(pairWithWrongTaxa);
                    }
                    #endregion

                }
                catch (Exception ex)
                {

                }
            }



        }

        public void CalculateFinalTree()
        {
            DepthOnleElementModel model = new DepthOnleElementModel();
            List<TreeNode> nodes = new List<TreeNode>();
            List<Taxa> tx = new List<Taxa>();
            TreeNode node = new TreeNode();

            string checkTaxa = string.Empty;

            foreach (Pair p in ListOfPairWithWrongTaxa)
            {
                if (p.tx1 == p.wrongTaxa)
                {
                    checkTaxa = p.tx2;
                }
                else
                {
                    checkTaxa = p.tx1;
                }

                Taxa t1 = getTaxa(checkTaxa);
                int p1 = getPosition(t1);
                Taxa wTaxa = getTaxa(p.wrongTaxa);

                foreach (TreeNode n in depth1Elements.NodeList)
                {
                    if (n._Position == p1)
                    {
                        n.TaxaList.Add(wTaxa);
                        break;
                    }

                }
            }

            OutputProcessing.printDepthOneWithNewDuplicationMethod(PathOutputAfterNewDuplication, depth1Elements.NodeList);
            OutputProcessing.printWrongPositionedTaxa(PathOutputForWrongPositionedTaxa, ListOfPairWithWrongTaxa);

        }



        //public void calculateFinalDuplication()
        //{
        //    foreach (Quartet q in ListOfInconsistent)
        //    {
        //        foreach (WrongTaxa tx in wrongPositionedTaxaList)
        //        {
        //            PartStatus pp = fromwhichPart(tx.Taxa._Taxa_Value);
        //        }

        //    }
        //}

        public WrongPositionStatusClass getWrongPositionStatusClass(WrongPositionStatus status, int aStart, string taStart, int aEnd, string taEnd, int bStart, string tbStart, int bEnd, string tbEnd)
        {
            WrongPositionStatusClass obj = new WrongPositionStatusClass();
            obj.WrongPositionStatus = status;
            obj.aStart = aStart;
            obj.taStart = taStart;
            obj.aEnd = aEnd;
            obj.taEnd = taEnd;
            obj.bStart = bStart;
            obj.tbStart = tbStart;
            obj.bEnd = bEnd;
            obj.tbEnd = tbEnd;
            return obj;

        }
        //Main Method for calculating Wrong Position
        public WrongPositionStatusClass CalculateWrongPosition(int p1, string t1, int p2, string t2, int p3, string t3, int p4, string t4, Quartet q)
        {
            WrongPositionStatusClass statusClass = new WrongPositionStatusClass();
            WrongPositionStatus status = WrongPositionStatus.None;


            if (status == WrongPositionStatus.None)
                status = whoisInWrongPosition(p1, t1, p2, t2, p3, t3, p4, t4, q);

            if (status != WrongPositionStatus.None)
            {
                statusClass = getWrongPositionStatusClass(status, p1, t1, p2, t2, p3, t3, p4, t4);
                //return status;
                return statusClass;
            }

            if (status == WrongPositionStatus.None)
                status = whoisInWrongPosition(p2, t2, p1, t1, p3, t3, p4, t4, q);

            if (status != WrongPositionStatus.None)
            {
                statusClass = getWrongPositionStatusClass(status, p2, t2, p1, t1, p3, t3, p4, t4);
                //return status;
                return statusClass;
            }

            //-----
            if (status == WrongPositionStatus.None)
                status = whoisInWrongPosition(p1, t1, p2, t2, p4, t4, p3, t3, q);

            if (status != WrongPositionStatus.None)
            {
                statusClass = getWrongPositionStatusClass(status, p1, t1, p2, t2, p4, t4, p3, t3);
                //return status;
                return statusClass;
            }

            if (status == WrongPositionStatus.None)
                status = whoisInWrongPosition(p2, t2, p1, t1, p4, t4, p3, t3, q);

            if (status != WrongPositionStatus.None)
            {
                statusClass = getWrongPositionStatusClass(status, p2, t2, p1, t1, p4, t4, p3, t3);
                //return status;
                return statusClass;
            }


            //-----
            if (status == WrongPositionStatus.None)
                status = whoisInWrongPosition(p3, t3, p4, t4, p1, t1, p2, t2, q);

            if (status != WrongPositionStatus.None)
            {
                statusClass = getWrongPositionStatusClass(status, p3, t3, p4, t4, p1, t1, p2, t2);
                //return status;
                return statusClass;
            }

            if (status == WrongPositionStatus.None)
                status = whoisInWrongPosition(p3, t3, p4, t4, p2, t2, p1, t1, q);

            if (status != WrongPositionStatus.None)
            {
                statusClass = getWrongPositionStatusClass(status, p3, t3, p4, t4, p2, t2, p1, t1);
                // return status;
                return statusClass;
            }


            //--
            if (status == WrongPositionStatus.None)
                status = whoisInWrongPosition(p4, t4, p3, t3, p1, t1, p2, t2, q);

            if (status != WrongPositionStatus.None)
            {
                statusClass = getWrongPositionStatusClass(status, p4, t4, p3, t3, p1, t1, p2, t2);
                // return status;
                return statusClass;
            }

            if (status == WrongPositionStatus.None)
                status = whoisInWrongPosition(p4, t4, p3, t3, p2, t2, p1, t1, q);

            if (status != WrongPositionStatus.None)
            {
                statusClass = getWrongPositionStatusClass(status, p4, t4, p3, t3, p2, t2, p1, t1);
                // return status;
                return statusClass;
            }

            // return status;
            return statusClass;

        }

        public bool isInWrongTaxaWithoutStatus(string taxa)
        {

            bool isExist = false;
            foreach (WrongTaxa tx in wrongPositionedTaxaList)
            {
                if (tx.Taxa._Taxa_Value == taxa)
                {
                    isExist = true;
                    break;
                }
            }

            return isExist;
        }
        public bool isInWrongTaxa(string taxa, WrongPositionStatus status)
        {

            bool isExist = false;
            foreach (WrongTaxa tx in wrongPositionedTaxaList)
            {
                if (tx.Taxa._Taxa_Value == taxa && tx.wrongPositionStatus == status)
                {
                    isExist = true;
                    break;
                }
            }

            return isExist;
        }

        /*public int isInWrongTaxaAndGetPosition(string taxa)
        {
            int i = -1;

            foreach (WrongTaxa tx in wrongPositionedTaxaList)
            {
                if (tx.Taxa._Taxa_Value == taxa)
                {

                    i = tx._PositionIntheDepthOne;
                    break;
                }
            }
            return i;
        }*/
        //#endregion
        public bool inBetween(int v1, int v2, int v3)
        {
            if (v1 < v2 && v3 > v2)
            {
                return true;
            }
            return false;
        }



        public WrongPositionStatus whoisInWrongPosition(int aStart, string taStart, int aEnd, string taEnd, int bStart, string tbStart, int bEnd, string tbEnd, Quartet q)
        {
            WrongPositionStatus status = WrongPositionStatus.None;


            //Case 1
            if (inBetween(aStart, bStart, aEnd) && inBetween(aStart, bEnd, aEnd) && bStart < bEnd)
            {

                if (isInWrongTaxa(taStart, WrongPositionStatus.aStartRight))
                    status = WrongPositionStatus.aStartRight;
                else if (isInWrongTaxa(taEnd, WrongPositionStatus.aEndLeft))
                    status = WrongPositionStatus.aEndLeft;
                else
                    // aStart or aEnd in wrong position
                    status = WrongPositionStatus.aEndLeft;

            }
            //Case 2
            else if (inBetween(aStart, bStart, aEnd) && inBetween(bStart, aEnd, bEnd) && bStart < bEnd && aStart < bEnd)
            {
                if (isInWrongTaxa(tbStart, WrongPositionStatus.bStartRight))
                    // bStart in wrong position
                    status = WrongPositionStatus.bStartRight;
                else
                    status = WrongPositionStatus.aEndLeft;


            }
            //Case 3
            else if (inBetween(bStart, aStart, bEnd) && inBetween(aStart, bEnd, aEnd) && bStart < bEnd && bStart < aEnd)
            {
                if (isInWrongTaxa(taStart, WrongPositionStatus.aStartRight))
                    status = WrongPositionStatus.aStartRight;
                else
                    // bEnd in wrong position
                    status = WrongPositionStatus.bEndLeft;
            }
            //Case 4
            else if (aStart == bStart && aEnd == bEnd && aStart < aEnd)
            {
                if (isInWrongTaxa(taStart, WrongPositionStatus.aStartLeft))
                    status = WrongPositionStatus.aStartLeft;
                else if (isInWrongTaxa(taStart, WrongPositionStatus.aStartRight))
                    status = WrongPositionStatus.aStartRight;
                else if (isInWrongTaxa(taEnd, WrongPositionStatus.aEndLeft))
                    status = WrongPositionStatus.aEndLeft;
                else if (isInWrongTaxa(taEnd, WrongPositionStatus.aEndRight))
                    status = WrongPositionStatus.aEndRight;

                else if (isInWrongTaxa(tbStart, WrongPositionStatus.bStartLeft))
                    status = WrongPositionStatus.bStartLeft;
                else if (isInWrongTaxa(tbStart, WrongPositionStatus.bStartRight))
                    status = WrongPositionStatus.bStartRight;
                else if (isInWrongTaxa(tbEnd, WrongPositionStatus.bEndLeft))
                    status = WrongPositionStatus.bEndLeft;
                else if (isInWrongTaxa(tbEnd, WrongPositionStatus.bEndRight))
                    status = WrongPositionStatus.bEndRight;
                else
                    //bStart and bEnd both
                    status = WrongPositionStatus.bStartRight;
            }
            //Case 5
            else if (inBetween(aStart, bStart, aEnd) && aEnd == bEnd && aStart < aEnd)
            {
                if (isInWrongTaxa(taEnd, WrongPositionStatus.aEndLeft))
                    status = WrongPositionStatus.aEndLeft;
                else
                    // aEnd in wrong position
                    status = WrongPositionStatus.aEndLeft;
            }
            //Case 6
            else if (inBetween(aStart, bEnd, aEnd) && aStart == bStart && aStart < aEnd)
            {
                if (isInWrongTaxa(taStart, WrongPositionStatus.aStartRight))
                    status = WrongPositionStatus.aStartRight;
                else
                    // aStart in wrong position
                    status = WrongPositionStatus.aStartRight;
            }

            //Case 7
            else if (aStart < aEnd && aEnd == bStart && bStart < bEnd && aStart < bEnd)
            {
                if (isInWrongTaxa(taEnd, WrongPositionStatus.aEndLeft))
                    status = WrongPositionStatus.aEndLeft;
                else
                    // aStart in wrong position
                    status = WrongPositionStatus.bStartRight;
            }
            return status;
        }

        public Taxa getTaxa(string tV)
        {
            Taxa t = new Taxa();
            t._Taxa_Value = tV;
            return t;
        }

        public int getPosition(Taxa tx)
        {

            int position = -1;
            foreach (TreeNode node in depth1Elements.NodeList)
            {
                foreach (Taxa t in node.TaxaList)
                {

                    if (t._Taxa_Value == tx._Taxa_Value)
                    {
                        position = node._Position;
                        break;
                    }


                }
            }

            return position;
        }




        /*public PartStatus fromwhichPart(string value)
        {
            bool leftPart = false;
            PartStatus partStatus = PartStatus.None;

            foreach (TreeNode node in finalPartitionAfterDepthOneElement.LeftPart)
            {
                var v = node.TaxaList.Where(x => x._Taxa_Value == value).FirstOrDefault();
                if (v != null)
                {
                    if (v._Taxa_Value == value)
                    {
                        leftPart = true;
                        partStatus = PartStatus.LeftPart;
                        break;
                    }
                }
            }

            if (!leftPart)
            {
                foreach (TreeNode node in finalPartitionAfterDepthOneElement.RightPart)
                {
                    var v = node.TaxaList.Where(x => x._Taxa_Value == value).FirstOrDefault();
                    if (v != null)
                    {
                        if (v._Taxa_Value == value)
                        {

                            partStatus = PartStatus.RightPart;
                            break;
                        }
                    }
                }
            }

            return partStatus;


        }*/

    }




    public class PartTaxa
    {
        PartStatus part { get; set; }
        List<string> taxaList { get; set; }
    }

    public enum PartStatus
    {
        None,
        LeftPart,
        RightPart
    }
    public class WrongPositionStatusClass
    {
        public WrongPositionStatus WrongPositionStatus { get; set; } = WrongPositionStatus.None;
        public int aStart { get; set; }
        public string taStart { get; set; }
        public int aEnd { get; set; }
        public string taEnd { get; set; }
        public int bStart { get; set; }
        public string tbStart { get; set; }
        public int bEnd { get; set; }
        public string tbEnd { get; set; }
    }

    public enum WrongPositionStatus
    {
        None,
        aStartLeft,
        aStartRight,
        aEndLeft,
        aEndRight,
        bStartLeft,
        bStartRight,
        bEndLeft,
        bEndRight,
        bStartAndbEndBoth
    }
    public class DepthOnleElementModel
    {
        public List<TreeNode> NodeList { get; set; }
    }


    public class FinalPartition
    {
        public List<TreeNode> LeftPart { get; set; }
        public List<TreeNode> RightPart { get; set; }
    }

    public class TreeNode
    {
        public int _Position { get; set; }
        public List<Taxa> TaxaList { get; set; }
    }

    public class WrongTaxa
    {
        public int _PositionIntheDepthOne { get; set; }
        public Taxa Taxa { get; set; }
        public Quartet Quartet { get; set; }
        public WrongPositionStatus wrongPositionStatus { get; set; } = WrongPositionStatus.None;
    }
    public class Pair
    {
        public string tx1 { get; set; }
        public string tx2 { get; set; }
        public string wrongTaxa { get; set; }
        public string Quatret { get; set; }
        public int diff { get; set; }
    }
}
