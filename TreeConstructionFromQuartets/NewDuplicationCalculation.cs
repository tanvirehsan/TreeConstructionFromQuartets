using System;
using System.Collections.Generic;
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
       
        public NewDuplicationCalculation()
        {
            string patthDept1 = @"E:\Mizans Research\Inputs\DepethOne.txt";
            string pathInconsistentQuatret = @"E:\Mizans Research\Inputs\InConsistentQuatret.txt";

            depth1Elements = readDepthOneElementInput(patthDept1);
            ListOfInconsistent = readInconsistentQuatret(pathInconsistentQuatret);
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
