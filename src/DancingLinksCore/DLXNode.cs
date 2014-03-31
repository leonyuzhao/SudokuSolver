using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DancingLinks
{
    public class DLXNode
    {
        public DLXNode Up { get; set; }
        public DLXNode Down { get; set; }
        public DLXNode Left { get; set; }
        public DLXNode Right { get; set; }

        public int Column { get; set; }
        public int Row { get; set; }
    }

    public class DLXColumn : DLXNode
    {
        public int Count { get; set; }
    }
}
