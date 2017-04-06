namespace TreeConstructionFromQuartets.Model
{
    using System;
    using System.Collections.Generic;

    public class Taxa
    {
        public string _Taxa_Value { get; set; }
        public string _Quartet_Name { get; set; }
        public int _Taxa_ValuePosition_In_Quartet { get; set; }
        public int _Gain { get; set; }
        public int _CumulativeGain { get; set; }

        private bool _IsFreeze = false;
        public bool IsFreeze
        {
            get
            {

                return _IsFreeze;
            }
            set
            {
                _IsFreeze = value;
            }
        }

        public int _IsolatedCount { get; set; }
        public int _ViotatedCount { get; set; }
        public int _DifferedCount { get; set; }
        public int _SatisfiedCount { get; set; }

        public PartitionSet _TaxaPartitionSet { get; set; }


        private int _StepK = 0;
        public int StepK { get; set; }
    }
}
