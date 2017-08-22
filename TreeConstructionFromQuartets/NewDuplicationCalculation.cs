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
        public FinalPartition finalPartitionAfterDepthOneElement = new FinalPartition();
        public DepthOnleElementModel depth1Elements = new DepthOnleElementModel();
        public List<Quartet> ListOfInconsistent = new List<Quartet>();
        string PathDepthOne = ConfigurationManager.AppSettings["PathDepthOne"].ToString();
        string PathInconsistentQuatret = ConfigurationManager.AppSettings["PathInconsistentQuatret"].ToString();
        string PathFinalPartitionAfterDepthOne = ConfigurationManager.AppSettings["PathFinalPartitionAfterDepthOne"].ToString();
        List<WrongTaxa> wrongPositionedTaxaList = new List<WrongTaxa>();
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
        #endregion

        //Constructor
        public NewDuplicationCalculation()
        {
            depth1Elements = readDepthOneElementInput(PathDepthOne);
            ListOfInconsistent = readInconsistentQuatret(PathInconsistentQuatret);
            finalPartitionAfterDepthOneElement = readFinalPartitionInput(PathFinalPartitionAfterDepthOne);
        }
        //Main Method to Calculate Wrong Taxa
        public void CalculateWrongTaxa()
        {

            WrongPositionStatusClass obj;
            WrongTaxa wrongTaxa;
            Quartet qQuartet;
            foreach (Quartet q in ListOfInconsistent)
            {

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




                wrongTaxa = new WrongTaxa();

                if (obj.WrongPositionStatus == WrongPositionStatus.aStart)
                {
                    wrongTaxa.wrongPositionStatus = WrongPositionStatus.aStart;
                    wrongTaxa.Taxa = getTaxa(obj.taStart);
                    wrongTaxa._PositionIntheDepthOne = obj.aStart;
                }
                else if (obj.WrongPositionStatus == WrongPositionStatus.aEnd)
                {
                    wrongTaxa.wrongPositionStatus = WrongPositionStatus.aEnd;
                    wrongTaxa.Taxa = getTaxa(obj.taEnd);
                    wrongTaxa._PositionIntheDepthOne = obj.aEnd;
                }
                else if (obj.WrongPositionStatus == WrongPositionStatus.bStart)
                {
                    wrongTaxa.wrongPositionStatus = WrongPositionStatus.bStart;
                    wrongTaxa.Taxa = getTaxa(obj.tbStart);
                    wrongTaxa._PositionIntheDepthOne = obj.bStart;
                }
                else if (obj.WrongPositionStatus == WrongPositionStatus.bEnd)
                {
                    wrongTaxa.wrongPositionStatus = WrongPositionStatus.bEnd;
                    wrongTaxa.Taxa = getTaxa(obj.tbEnd);
                    wrongTaxa._PositionIntheDepthOne = obj.bEnd;
                }

                qQuartet = new Quartet();
                qQuartet._First_Taxa_Value = obj.taStart;
                qQuartet._Second_Taxa_Value = obj.taEnd;
                qQuartet._Third_Taxa_Value = obj.tbStart;
                qQuartet._Fourth_Taxa_Value = obj.tbEnd;
                wrongTaxa.Quartet = qQuartet;

                if (!isInWrongTaxa(wrongTaxa.Taxa._Taxa_Value))
                    wrongPositionedTaxaList.Add(wrongTaxa);

            }

           

        }

        public void calculateFinalDuplication()
        {
            foreach (Quartet q in ListOfInconsistent)
            {
                foreach (WrongTaxa tx in wrongPositionedTaxaList)
                {
                    PartStatus pp = fromwhichPart(tx.Taxa._Taxa_Value);
                }

            }
        }

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


        public bool isInWrongTaxa(string taxa)
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

        public int isInWrongTaxaAndGetPosition(string taxa)
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
        }
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

                if (isInWrongTaxa(taStart))
                    status = WrongPositionStatus.aStart;
                else if (isInWrongTaxa(taEnd))
                    status = WrongPositionStatus.aEnd;
                else
                    // aStart or aEnd in wrong position
                    status = WrongPositionStatus.aStart;

            }
            //Case 2
            else if (inBetween(aStart, bStart, aEnd) && inBetween(bStart, aEnd, bEnd) && bStart < bEnd && aStart < bEnd)
            {
                if (isInWrongTaxa(taEnd))
                    status = WrongPositionStatus.aEnd;
                else
                    // bStart in wrong position
                    status = WrongPositionStatus.bStart;
            }
            //Case 3
            else if (inBetween(bStart, aStart, bEnd) && inBetween(aStart, bEnd, aEnd) && bStart < bEnd && bStart < aEnd)
            {
                if (isInWrongTaxa(taStart))
                    status = WrongPositionStatus.aStart;
                else
                    // bEnd in wrong position
                    status = WrongPositionStatus.bEnd;
            }
            //Case 4
            else if (aStart == bStart && aEnd == bEnd && aStart < aEnd)
            {
                if (isInWrongTaxa(taStart))
                    status = WrongPositionStatus.aStart;
                else if (isInWrongTaxa(taEnd))
                    status = WrongPositionStatus.aEnd;
                else if (isInWrongTaxa(tbEnd))
                    status = WrongPositionStatus.bEnd;
                else
                    //bStart and bEnd both
                    status = WrongPositionStatus.bStart;
            }
            //Case 5
            else if (inBetween(aStart, bStart, aEnd) && aEnd == bEnd && aStart < aEnd)
            {
                if (isInWrongTaxa(taStart))
                    status = WrongPositionStatus.aStart;
                else
                    // bStart in wrong position
                    status = WrongPositionStatus.bStart;
            }
            //Case 6
            else if (inBetween(aStart, bEnd, aEnd) && aStart == bStart && aStart < aEnd)
            {
                if (isInWrongTaxa(tbEnd))
                    status = WrongPositionStatus.bEnd;
                else
                    // aStart in wrong position
                    status = WrongPositionStatus.aStart;
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




        public PartStatus fromwhichPart(string value)
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


        }

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
        aStart,
        aEnd,
        bStart,
        bEnd,
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
}
