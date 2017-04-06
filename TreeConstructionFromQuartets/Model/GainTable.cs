namespace TreeConstructionFromQuartets.Model
{
    using System;
    using System.Collections.Generic;
 
    public class GainTable
    {
        public List<Taxa> _TaxaGainSummary { get; set; }
        public int _MaxCumulativeGain { get; set; }

        public string TaxValue { get; set; }
        public int MaximumGainOfTaxValue { get; set; }
        public PartitionSet PartitionSet { get; set; }
    }
}
