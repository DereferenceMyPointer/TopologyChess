using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TopologyChess
{
    public class Move
    {
        public Move(Cell from, Cell to)
        {
            From = from;
            To = to;
            MovingPiece = from.Piece;
            CapturedPiece = To.Piece;
        }

        public Cell From { get; set; }
        public Cell To { get; set; }
        public Piece MovingPiece { get; set; }
        public Cell Capture { get; set; }
        public Piece CapturedPiece { get; set; }

        public Move Castle { get; set; }
        public Cell EnPassant { get; set; }
    }
}
