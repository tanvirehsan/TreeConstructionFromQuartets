using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeConstructionFromQuartets.Model
{
    public class ConsistencyDataModel
    {
        public List<Quartet> _ALL_Quatret { get; set; }
        public List<Quartet> _Isolated_Quatret { get; set; }
        public List<Quartet> _Differed_Quatret { get; set; }
        public List<Quartet> _Violated_Quatret { get; set; }
        public List<DepthOneTreeNode> _DepthOneChain { get; set; }
    }
}
