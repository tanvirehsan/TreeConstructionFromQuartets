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
        string PathDepthOne = ConfigurationManager.AppSettings["PathDepthOne"].ToString();
        string PathInconsistentQuatret = ConfigurationManager.AppSettings["PathInconsistentQuatret"].ToString();
        List<Taxa> wrongPositionedTaxaList = new List<Taxa>();
        //List<TreeNode> node = new List<TreeNode>();

        public NewDuplicationCalculation()
        {
            depth1Elements = readDepthOneElementInput(PathDepthOne);
            ListOfInconsistent = readInconsistentQuatret(PathInconsistentQuatret);
        }

        public void CalculateWrongTaxa()
        {

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

                if (p1 != -1 || p2 != -1 || p3 != -1 || p4 != -1)
                {

                    if (inBetween(p1, p3, p2) && inBetween(p1, p4, p2))
                    {
                        //wrong Tax p1 or p2
                    }

                    //if (inBetween(p3, p1, p4) && inBetween(p3, p2, p4))
                    //{
                    //    // wrong Tax p3 or p4
                    //}

                    if (inBetween(p1, p3, p2) && p4 > p2)
                    {
                        //wrong Tax p3
                    }

                    if (inBetween(p1, p4, p2) && p3 > p2)
                    {
                        //wrong Tax p4
                    }


                    if (inBetween(p3, p1, p4) && p2 > p4)
                    {
                        //wrong Tax p1
                    }

                    if (inBetween(p3, p2, p4) && p1 > p4)
                    {
                        //wrong Tax p2
                    }



                }

            }

        }

        public bool inBetween(int v1, int v2, int v3)
        {
            if (v1 < v2 && v3 > v2)
            {
                return true;
            }
            return false;
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

        //public List<TreeNode> getRelevantDepthOneElement() {


        //}

        public bool isWorngTaxa()
        {

            return false;
        }

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

        public DepthOnleElementModel depth1Elements = new DepthOnleElementModel();
        public List<Quartet> ListOfInconsistent = new List<Quartet>();
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
}
