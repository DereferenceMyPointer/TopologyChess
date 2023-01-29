using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopologyChess
{
    public class Move
    {
        public Cell From { get; set; }
        public Cell To { get; set; }
        public Piece MovingPiece { get; set; }
        public Chain<Step> Path { get; set; }
        public Cell Capture { get; set; }

        public Move() { }
        public Move(Cell from, Cell to, Chain<Step> path)
        {
            From = from;
            To = to;
            Path = path;
            MovingPiece = from.Piece;
            Capture = to;
        }
    }

    public class Castle : Move
    {
        public Move RookMove { get; set; }
    }
}
