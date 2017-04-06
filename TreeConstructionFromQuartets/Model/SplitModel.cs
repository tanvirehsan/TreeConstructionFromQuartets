using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeConstructionFromQuartets.Model
{
    public class SplitModel
    {
        public Quartet _InputQuatret { get; set; }

        private int CountTaxa = 0;
        public int _CountTaxa
        {
            get
            {

                return CountTaxa;
            }
            set
            {
                CountTaxa = value;
            }
        }

        private List<string> RightPartOfSplit = new List<string>();
        public List<string> _RightPartOfSplit
        {
            get
            {

                return RightPartOfSplit;
            }
            set
            {
                RightPartOfSplit = value;
            }
        }

        private List<string> LeftPartOfSplit = new List<string>();
        public List<string> _LeftPartOfSplit
        {
            get
            {

                return LeftPartOfSplit;
            }
            set
            {
                LeftPartOfSplit = value;
            }
        }
    }
}
