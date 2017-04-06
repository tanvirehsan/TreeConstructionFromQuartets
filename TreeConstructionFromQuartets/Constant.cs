namespace TreeConstructionFromQuartets
{
    using System;
    using System.Configuration;
    public class Constant
    {
        //public static string InputFilePath = ConfigurationManager.AppSettings["InputFileName"].ToString();
        //public static string OutputFilePath = ConfigurationManager.AppSettings["OutputFileName"].ToString();

        public static string InputFilePath = string.Empty;
        public static string OutputFilePath = string.Empty;



        public static void SetInputFilePath(string Path)
        {
            InputFilePath = Path;

        }

        public static void SetOutputFilePath(string Path)
        {
            OutputFilePath = Path;

        }

    }
}
