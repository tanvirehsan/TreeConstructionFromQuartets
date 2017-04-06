namespace TreeConstructionFromQuartets
{
    using System; 
    public class Helper
    {

        public static string getFirstTaxaValue(string taxInput)
        {
            int LastIndexOf = taxInput.LastIndexOf("(") + 1;
            string val = taxInput.Substring(LastIndexOf  );
            return val;// Convert.ToInt32(val);
        }

        public static string getSecondTaxaValue(string taxInput)
        {
            int LastIndexOf = taxInput.IndexOf(")")  ;
            string val = taxInput.Substring(0,LastIndexOf);
            return val;// Convert.ToInt32(val);
        }

         
    }
}
