namespace TreeConstructionFromQuartets.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PartitionSet
    {

        public PartitionSet(string PartitionSetName)
        {

            this._PartitionSetName = PartitionSetName;

            this._Final_Score = 0;
            this._IsolatedCount = 0;
            this._ViotatedCount = 0;
            this._SatisfiedCount = 0;
            this._DifferedCount = 0;
            this._taxValueForGainCalculation = string.Empty;
            this._Gain = 0;
            this.PartitionList = new List<Partition>();
            this._ListQuatrets = new List<Quartet>();
        }

        public PartitionSet(string PartitionSetName, int _Final_Score, int _IsolatedCount, int _ViotatedCount, int _SatisfiedCount, int _DifferedCount, string _taxValueForGainCalculation, int _Gain, List<Partition> PartitionList, List<Quartet> _ListQuatrets)
        {

            this._PartitionSetName = PartitionSetName;
            this._Final_Score = _Final_Score;
            this._IsolatedCount = _IsolatedCount;
            this._ViotatedCount = _ViotatedCount;
            this._SatisfiedCount = _SatisfiedCount;
            this._DifferedCount = _DifferedCount;
            this._taxValueForGainCalculation = _taxValueForGainCalculation;
            this._Gain = _Gain;


            this.PartitionList = new List<Partition>(PartitionList.Select(x => new Partition(x._PartitionName)
            {
                _PartitionName = x._PartitionName,
                TaxaList = new List<Taxa>(x.TaxaList.Select(m => new Taxa()
            {
                _Taxa_Value = m._Taxa_Value,
                _Quartet_Name = m._Quartet_Name,
                _Taxa_ValuePosition_In_Quartet = m._Taxa_ValuePosition_In_Quartet,
                _Gain = m._Gain,
                _CumulativeGain = m._CumulativeGain,
                IsFreeze = m.IsFreeze,
                _IsolatedCount = m._IsolatedCount,
                _ViotatedCount = m._ViotatedCount,
                _DifferedCount = m._DifferedCount,
                _SatisfiedCount = m._SatisfiedCount,
                _TaxaPartitionSet = m._TaxaPartitionSet != null ? new PartitionSet(m._TaxaPartitionSet._PartitionSetName, m._TaxaPartitionSet._Final_Score, m._TaxaPartitionSet._IsolatedCount, m._TaxaPartitionSet._ViotatedCount, m._TaxaPartitionSet._SatisfiedCount, m._TaxaPartitionSet._DifferedCount, m._TaxaPartitionSet._taxValueForGainCalculation, m._TaxaPartitionSet._Gain, m._TaxaPartitionSet.PartitionList, m._TaxaPartitionSet._ListQuatrets) : null,
                StepK = m.StepK
            }))
            }));

            this._ListQuatrets = new List<Quartet>(_ListQuatrets.Select(x => new Quartet()
            {

                _First_Taxa_Value = x._First_Taxa_Value,
                _Second_Taxa_Value = x._Second_Taxa_Value,
                _Third_Taxa_Value = x._Third_Taxa_Value,
                _Fourth_Taxa_Value = x._Fourth_Taxa_Value,
                _Quartet_Name = x._Quartet_Name,
                _Quartet_Input = x._Quartet_Input,
                _Quartet_LeftPart = x._Quartet_LeftPart,
                _Quartet_LeftPartReverse = x._Quartet_LeftPartReverse,
                _Quartet_RightPart = x._Quartet_RightPart,
                _Quartet_RightPartReverse = x._Quartet_RightPartReverse,
                _isDistinct = x._isDistinct,
                _Frequency = x._Frequency,
                _DuplicateQuatrets = x._DuplicateQuatrets,
                _PartitionStatus = x._PartitionStatus,
                _ConsistancyStatus = x._ConsistancyStatus,
                _TaxaSplitLeft = x._TaxaSplitLeft,
                _TaxaSplitRight = x._TaxaSplitRight
            }));

        }

        public string _PartitionSetName { get; set; }

        public string _taxValueForGainCalculation { get; set; }


        public int _Gain { get; set; }

        public int _Final_Score { get; set; }

        public int _IsolatedCount { get; set; }
        public int _ViotatedCount { get; set; }
        public int _DifferedCount { get; set; }
        public int _SatisfiedCount { get; set; }

        public List<Quartet> _ListQuatrets { get; set; }
        private List<Partition> _PartitionList = new List<Partition>();
        public List<Partition> PartitionList
        {
            get
            {

                return _PartitionList;
            }
            set
            {
                _PartitionList = value;
            }
        }
    }
}
