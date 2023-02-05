using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopologyChess
{
    public class Move
    {
        public IntVector From { get; set; }
        public IntVector To { get; set; }
        public Piece MovingPiece { get; set; }
        public Chain<Step> Path { get; set; }
        public IntVector? Capture { get; set; }

        public Move() { }
        public Move(Cell from, Cell to, Chain<Step> path)
        {
            From = from.Position;
            To = to.Position;
            Path = path;
            MovingPiece = from.Piece;
            Capture = to.Position;
        }

        public static readonly Move NoMove = new()
        {
            From = new(-1, -1),
            To = new(-1, -1),
            Capture = null,
            MovingPiece = null,
            Path = null
        };
    }

    public class Castle : Move
    {
        public Move RookMove { get; set; }
    }
}
