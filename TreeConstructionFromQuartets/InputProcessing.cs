namespace TreeConstructionFromQuartets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TreeConstructionFromQuartets.Model;


    public class InputProcessing
    {
        private List<Quartet> Set_Of_Quartets = new List<Quartet>();
        private List<Quartet> Set_Of_Distinct_Quartets = new List<Quartet>();
        private List<Quartet> Sorted_Of_Quartets = new List<Quartet>();
        private List<string> Set_Of_Taxa = new List<string>();


        public InputProcessing()
        {
            readInput();
            SettingOfTaxas(this.Set_Of_Quartets);
        }

        public InputProcessing(List<Quartet> InputQuatrets, List<Taxa> list_Set_Of_Quartets)
        {
            readInputForDivideAndConquer(InputQuatrets);
            SettingOfTaxas(list_Set_Of_Quartets);
        }

        public void readInputForDivideAndConquer(List<Quartet> InputQuatrets)
        {

            //Set_Of_Quartets = new List<Quartet>();
            //Set_Of_Distinct_Quartets = new List<Quartet>();
            //Sorted_Of_Quartets = new List<Quartet>();
            //Set_Of_Taxa = new List<string>(); 
            int counter = 0;
            int QuartetName = 0;
            foreach (Quartet quartet in InputQuatrets)
            {

                quartet._Quartet_Name = "q" + QuartetName.ToString();
                quartet._Quartet_LeftPart = quartet._First_Taxa_Value.ToString() + quartet._Second_Taxa_Value.ToString();
                quartet._Quartet_LeftPartReverse = quartet._Second_Taxa_Value.ToString() + quartet._First_Taxa_Value.ToString();
                quartet._Quartet_RightPart = quartet._Third_Taxa_Value.ToString() + quartet._Fourth_Taxa_Value.ToString();
                quartet._Quartet_RightPartReverse = quartet._Fourth_Taxa_Value.ToString() + quartet._Third_Taxa_Value.ToString();
                quartet._PartitionStatus = PartitionStatus.None;
                counter++;
                QuartetName++;
                this.Set_Of_Quartets.Add(quartet);

            }

        }

        public void readInput()
        {
            Quartet quartet;
            int counter = 0;
            string line;
            int QuartetName = 0;
            System.IO.StreamReader file =
               new System.IO.StreamReader(Constant.InputFilePath);
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
                                this.Set_Of_Quartets.Add(quartet);
                            }
                        }
                    }
                }
            }

            file.Close();


        }
        private void SettingOfTaxas(List<Quartet> list)
        {

            List<string> Taxas = new List<string>();

            foreach (Quartet q in list)
            {
                Taxas.Add(q._First_Taxa_Value);
                Taxas.Add(q._Second_Taxa_Value);
                Taxas.Add(q._Third_Taxa_Value);
                Taxas.Add(q._Fourth_Taxa_Value);

            }
            this.Set_Of_Taxa = Taxas.Distinct().ToList();

        }

        private void SettingOfTaxas(List<Taxa> listOfTaxa)
        {
            List<string> Taxas = new List<string>();

            foreach (Taxa q in listOfTaxa)
            {
                Taxas.Add(q._Taxa_Value);


            }

            this.Set_Of_Taxa = Taxas.Distinct().ToList();

        }

        public List<Quartet> Get_SetOfQuartets()
        {
            return this.Set_Of_Quartets;
        }

        public List<string> Get_SetOfTaxa()
        {
            return this.Set_Of_Taxa;
        }

        public List<Taxa> getSetOfTaxa()
        {

            List<Taxa> _pSet_Of_Taxa = new List<Taxa>();
            Taxa t;
            foreach (string taxa in this.Set_Of_Taxa)
            {
                t = new Taxa();
                t._Taxa_Value = taxa;
                t.IsFreeze = false;
                _pSet_Of_Taxa.Add(t);
            }
            return _pSet_Of_Taxa;

        }


    }
}
