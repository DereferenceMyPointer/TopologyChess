using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopologyChess
{
    public class Move
    {
        public Move() { }

        public Cell From { get; set; }
        public Cell To { get; set; }
        public Piece Captured { get; set; }
    }
}
