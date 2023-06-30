using System.Linq;
using System.Text;
using System.Threading.Tasks;

/** Base class for storing moves
 * Design: moves are from one coordinate to another, or castling.
 * Moves contain a Piece, the moving piece.
 * Moves also contain a Path, the steps the piece took to move to the destination
 * Captures are also coordinates; the coordinate of the capture if one exists (maybe could just be boolean since
 * this isn't itself the set of all moves?)
 * Castling is just its own subclass that stores only the rook move (since the king move is deterministic, I guess);
 * adds an additional rook move to the move that happened which does not leave its own trace
 * 
 * Note that piece move logic is all in Game
 *      - Kind of unhinged imo but probably not necessarily a bad solution
 * Note that although this is formatted as a command pattern, the command logic is handled externally
 * 
 * TODO: Make topology change a move
 * Possibilities: 
 * Subclass
 *      - Probably not good, since there would still be coordinates, etc associated with the move
 *      - Could fix by making the other types associated with Piece nullable and adding a topology component
 *          - This would make piece move + topology change possible to implement as well
 * Superclass
 *      - Make piece moves and board transformations subclasses
 *      - Requires some refactoring of existing move code
 * 
 * Needs some additional design considerations, such as: how does the user select / submit a topology change?
 *      - I would like either drop-down + submit button, or grid, or interactable window that previews the desired topology
 * 
 */

namespace TopologyChess
{
    public partial class Move
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
            Path = null,
            TopologyChange = null
        };
    }

    public class Castle : Move
    {
        public Move RookMove { get; set; }
    }

}
