using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeConstructionFromQuartets.Model
{
    public class FinalPartionPair
    {
        public List<Taxa> _Root { get; set; }
        public Partition _P { get; set; }
        public List<Quartet> _Q { get; set; }

        public List<Quartet> _InputQuatret { get; set; }
    }
}
