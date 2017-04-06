namespace TreeConstructionFromQuartets
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using TreeConstructionFromQuartets.Model;
    public class Program
    {
        static void Main(string[] args)
        {
            ProgramCalculation obj;
            string DuplicationMethod = ConfigurationManager.AppSettings["DuplicationMethod"].ToString();
            string OperationMode = ConfigurationManager.AppSettings["OperationMode"].ToString();
            string FileDirectory = ConfigurationManager.AppSettings["FileDirectory"].ToString();


            //SplitCalculation objSplitCalculation = new SplitCalculation();
            //SplitModel SuperSplit = new SplitModel();
            //List<Quartet> listQuatret = new List<Quartet>();

            //InputProcessing inp = new InputProcessing();

            //listQuatret = inp.Get_SetOfQuartets();

            //SuperSplit = objSplitCalculation.CalculateSuperSplit(listQuatret);
            
           
            try
            {

                if (OperationMode == "Directory")
                {

                    if (Directory.Exists(FileDirectory))
                    {

                        foreach (string file in Directory.GetFiles(FileDirectory))
                        {


                            if (Path.GetExtension(file) == ".txt")
                            {

                                Constant.SetInputFilePath(file);
                                if (DuplicationMethod.ToLower() == "old")

                                    Constant.SetOutputFilePath(FileDirectory + "\\" + Path.GetFileNameWithoutExtension(file) + "OutputOld.txt");
                                else
                                    Constant.SetOutputFilePath(FileDirectory + "\\" + Path.GetFileNameWithoutExtension(file) + "OutputNew.txt");

                                obj = new ProgramCalculation();
                                try
                                {
                                    obj.StepByStep();
                                }
                                catch (Exception ee)
                                {
                                    obj = new ProgramCalculation();
                                }

                            }

                        }

                    }



                }
                else
                {


                    Constant.SetInputFilePath(FileDirectory + "\\" + ConfigurationManager.AppSettings["InputFileName"].ToString());
                    Constant.SetOutputFilePath(FileDirectory + "\\" + ConfigurationManager.AppSettings["OutputFileName"].ToString());
                    try
                    {
                        obj = new ProgramCalculation();
                        obj.StepByStep();

                    }
                    catch (Exception eex)
                    {
                        obj = new ProgramCalculation();
                    }



                }

            }
            catch (Exception ex)
            {
                obj = new ProgramCalculation();
            }
            
           
        }

    }
}
