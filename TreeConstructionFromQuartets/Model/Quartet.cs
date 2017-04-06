namespace TreeConstructionFromQuartets.Model
{
    using System;
    using System.Collections.Generic;

    public class Quartet
    {
        public string _First_Taxa_Value { get; set; }
        public string _Second_Taxa_Value { get; set; }
        public string _Third_Taxa_Value { get; set; }
        public string _Fourth_Taxa_Value { get; set; }
        public string _Quartet_Name { get; set; }
        public string _Quartet_Input { get; set; }

        public string _Quartet_LeftPart { get; set; }
        public string _Quartet_LeftPartReverse { get; set; }

        public string _Quartet_RightPart { get; set; }

        public string _Quartet_RightPartReverse { get; set; }

        public bool _isDistinct { get; set; }


        public int _Frequency { get; set; }


        private List<string> DuplicateQuatrets = new List<string>();
        public List<string> _DuplicateQuatrets  
        {
            get  
            {

                return DuplicateQuatrets;
            }
            set  
            {
                DuplicateQuatrets = value;
            }
        }

        public PartitionStatus _PartitionStatus { get; set; }
        public ConsistencyStatus _ConsistancyStatus { get; set; }

        public List<string> _TaxaSplitLeft { get; set; }
        public List<string> _TaxaSplitRight { get; set; }
    }

    public class QuartetPair
    {
        public string _First_Taxa_Value { get; set; }
        public string _Second_Taxa_Value { get; set; }
    
    }
}
