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
        public DepthOnleElementModel depth1Elements = new DepthOnleElementModel();
        public List<Quartet> ListOfInconsistent = new List<Quartet>();
        string PathDepthOne = ConfigurationManager.AppSettings["PathDepthOne"].ToString();
        string PathInconsistentQuatret = ConfigurationManager.AppSettings["PathInconsistentQuatret"].ToString();
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
        #endregion

        //Constructor
        public NewDuplicationCalculation()
        {
            depth1Elements = readDepthOneElementInput(PathDepthOne);
            ListOfInconsistent = readInconsistentQuatret(PathInconsistentQuatret);
        }
        //Main Method to Calculate Wrong Taxa
        public void CalculateWrongTaxa()
        {
            int i = 0;
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

                if (i == 0)
                {
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


                    wrongPositionedTaxaList.Add(wrongTaxa);
                }
                else
                {
                    int aStart = 0;
                    string taStart = "";

                    int aEnd = 0;
                    string taEnd = "";

                    int bStart = 0;
                    string tbStart = "";

                    int bEnd = 0;
                    string tbEnd = "";

                    if (isInWrongTaxa(t1._Taxa_Value))
                    {
                        //aStart
                        taStart = t1._Taxa_Value;

                    }
                    else
                    {

                    }
                    if (isInWrongTaxa(t2._Taxa_Value))
                    {
                        //aEnd
                        taEnd = t2._Taxa_Value;
                    }
                    if (isInWrongTaxa(t3._Taxa_Value))
                    {
                        //bStart
                        tbStart = t3._Taxa_Value;
                    }
                    if (isInWrongTaxa(t4._Taxa_Value))
                    {
                        //bEnd
                        tbEnd = t4._Taxa_Value;
                    }

                    obj = CalculateWrongPosition(p1, t1._Taxa_Value, p2, t2._Taxa_Value, p3, t3._Taxa_Value, p4, t4._Taxa_Value, q);
                }
                i++;
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
        //#region Supperss
        //public void recursiveChecking(int p1, string t1, int p2, string t2, int p3, string t3, int p4, string t4, Quartet q)
        //{
        //    //WrongTaxa wrongTaxa;
        //    if (isOverlap(p1, p2, p3, p4))
        //    {
        //        calculateWrongTaxa(p1, t1, p2, t2, p3, t3, p4, t4, q);

        //    }
        //    else if (isOverlap(p1, p2, p4, p3))
        //    {
        //        calculateWrongTaxa(p1, t1, p2, t2, p4, t4, p3, t3, q);
        //    }
        //    else if (isOverlap(p2, p1, p3, p4))
        //    {
        //        calculateWrongTaxa(p2, t2, p1, t1, p3, t3, p4, t4, q);
        //    }
        //    else if (isOverlap(p2, p1, p4, p3))
        //    {
        //        calculateWrongTaxa(p2, t2, p1, t1, p4, t4, p3, t3, q);
        //    }
        //    else if (isOverlap(p3, p4, p1, p2))
        //    {
        //        calculateWrongTaxa(p3, t3, p4, t4, p1, t1, p2, t2, q);
        //    }
        //    else if (isOverlap(p3, p4, p2, p1))
        //    {
        //        calculateWrongTaxa(p3, t3, p4, t4, p2, t2, p1, t1, q);
        //    }
        //    else if (isOverlap(p4, p3, p1, p2))
        //    {
        //        calculateWrongTaxa(p4, t4, p3, t3, p1, t1, p2, t2, q);
        //    }
        //    else if (isOverlap(p4, p3, p2, p1))
        //    {
        //        calculateWrongTaxa(p4, t4, p3, t3, p2, t2, p1, t1, q);
        //    }
        //}

        //public void calculateWrongTaxa(int p1, string t1, int p2, string t2, int p3, string t3, int p4, string t4, Quartet q)
        //{
        //    ExpressionStatus status = checkingExpression(p1, p2, p3, p4);
        //    WrongTaxa wrongTaxa;
        //    // bool expression1 = p1 < p4;
        //    // bool expression2 = p3 < p2;

        //    if (status == ExpressionStatus.TrueTrue)
        //    {
        //        if (isInWrongTaxa(t1))
        //        {
        //            status = ExpressionStatus.FalseTrue;
        //        }
        //        else if (isInWrongTaxa(t2))
        //        {
        //            status = ExpressionStatus.TrueFalse;
        //        }
        //        else if (isInWrongTaxa(t4))
        //        {
        //            status = ExpressionStatus.FalseTrue;
        //        }
        //        else if (isInWrongTaxa(t3))
        //        {
        //            status = ExpressionStatus.TrueFalse;
        //        }
        //        else
        //        {
        //            //Random rnd = new Random();
        //            //int val = rnd.Next(1, 100);
        //            //if (val % 2 == 0)
        //            //{
        //            //    status = ExpressionStatus.TrueFalse;
        //            //}
        //            //else
        //            //{
        //            status = ExpressionStatus.FalseTrue;
        //            // }

        //        }

        //    }

        //    if (status == ExpressionStatus.FalseFalse)
        //    {
        //        //p1,p2 or p3,p4
        //    }
        //    else if (status == ExpressionStatus.FalseTrue)
        //    {
        //        //p1 or p4
        //        if (isInWrongTaxa(t1))
        //        {

        //        }
        //        else if (isInWrongTaxa(t4))
        //        {

        //        }
        //        else
        //        {

        //            wrongTaxa = new WrongTaxa();
        //            wrongTaxa.Taxa = getTaxa(t1);
        //            wrongTaxa.Quartet = q;
        //            wrongTaxa._PositionIntheDepthOne = p1;
        //            wrongPositionedTaxaList.Add(wrongTaxa);
        //        }

        //    }
        //    else if (status == ExpressionStatus.TrueFalse)
        //    {
        //        //p3 or p2

        //        if (isInWrongTaxa(t2))
        //        {

        //        }
        //        else if (isInWrongTaxa(t3))
        //        {

        //        }
        //        else
        //        {

        //            wrongTaxa = new WrongTaxa();
        //            wrongTaxa.Taxa = getTaxa(t2);
        //            wrongTaxa.Quartet = q;
        //            wrongTaxa._PositionIntheDepthOne = p2;
        //            wrongPositionedTaxaList.Add(wrongTaxa);
        //        }

        //    }

        //}

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
        //#endregion
        public bool inBetween(int v1, int v2, int v3)
        {
            if (v1 < v2 && v3 > v2)
            {
                return true;
            }
            return false;
        }

        //#region Supperss
        //public ExpressionStatus checkingExpression(int p1, int p2, int p3, int p4)
        //{

        //    bool expression1 = p1 < p4;
        //    bool expression2 = p3 < p2;

        //    if (expression1 && !expression2)
        //    {
        //        return ExpressionStatus.TrueFalse;
        //    }
        //    else if (!expression1 && expression2)
        //    {
        //        return ExpressionStatus.FalseTrue;
        //    }
        //    else if (!expression1 && !expression2)
        //    {
        //        return ExpressionStatus.FalseFalse;
        //    }
        //    else if (expression1 && expression2)
        //    {
        //        return ExpressionStatus.TrueTrue;
        //    }
        //    return ExpressionStatus.None;
        //}

        //public bool isOverlap(int p1, int p2, int p3, int p4)
        //{
        //    bool overlap = false;
        //    overlap = p1 < p4 && p3 < p2;
        //    return overlap;
        //}
        //#endregion

        public WrongPositionStatus whoisInWrongPosition(int aStart, string taStart, int aEnd, string taEnd, int bStart, string tbStart, int bEnd, string tbEnd, Quartet q)
        {
            WrongPositionStatus status = WrongPositionStatus.None;
             

            if (inBetween(aStart, bStart, aEnd) && inBetween(aStart, bEnd, aEnd) && bStart < bEnd)
            {
                // aStart or aEnd in wrong position
                status = WrongPositionStatus.aStart;
            }
            else if (inBetween(aStart, bStart, aEnd) && inBetween(bStart, aEnd, bEnd) && bStart < bEnd && aStart < bEnd)
            {
                // bStart in wrong position
                status = WrongPositionStatus.bStart;
            }
            else if (inBetween(bStart, aStart, bEnd) && inBetween(aStart, bEnd, aEnd) && bStart < bEnd && bStart < aEnd)
            {
                // bEnd in wrong position
                status = WrongPositionStatus.bEnd;
            }
            else if (aStart == bStart && aEnd == bEnd && aStart < aEnd)
            {
                //bStart and bEnd 
                status = WrongPositionStatus.bStartAndbEndBoth;
            }
            else if (inBetween(aStart, bStart, aEnd) && aEnd == bEnd && aStart < aEnd)
            {
                // bStart in wrong position
                status = WrongPositionStatus.bStart;
            }
            else if (inBetween(aStart, bEnd, aEnd) && aStart == bStart && aStart < aEnd)
            {
                // bStart in wrong position
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






    }
    //public enum ExpressionStatus
    //{
    //    None,
    //    TrueFalse,
    //    FalseTrue,
    //    FalseFalse,
    //    TrueTrue
    //}

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
