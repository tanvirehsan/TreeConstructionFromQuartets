namespace TreeConstructionFromQuartets.Model
{
    using System;
    using System.Collections.Generic;
 

    public class Partition
    {

        public Partition(string PartionName)
        {

            this._PartitionName = PartionName;
        }
        public string _PartitionName { get; set; }

        private List<Taxa> _TaxaList = new List<Taxa>();
        public List<Taxa> TaxaList
        {
            get
            {

                return _TaxaList;
            }
            set
            {
                _TaxaList = value;
            }
        }

    }
}
