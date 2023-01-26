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
        public Move(Point from, Point to)
        {
            From = from;
            To = to;

        }

        public Point From { get; set; }
        public Point To { get; set; }
        public Piece MovingPiece { get; set; }
        public Point? Capture { get; set; }
        public Piece CapturedPiece { get; set; }
        public Move Castle { get; set; }
        public Point? EnPassant { get; set; }

    }
}
