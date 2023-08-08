using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TopologyChess
{
    public class Move : IMove
    {
        public Cell From { get; set; }
        public Cell To { get; set; }
        public Piece MovingPiece { get; set; }
        public Chain<Step> Path { get; set; }
        public Cell Capture { get; set; }
        public Piece CapturedPiece { get; set; } = Piece.Empty;

        public Move() { }
        public Move(Cell from, Cell to, Chain<Step> path)
        {
            From = from;
            To = to;
            Path = path;
            MovingPiece = from.Piece;
            CapturedPiece = to.Piece;
            Capture = to;
        }

        public virtual MoveType Type { get; } = MoveType.Move;
        public bool Legal { get; } = true;

        private bool Done { get; set; } = false;

        private bool init_HasMoved;
        private Matrix init_Matrix;

        public virtual void Do()
        {
            init_HasMoved = MovingPiece.HasMoved;
            init_Matrix = MovingPiece.RenderMatrix;

            MovingPiece.HasMoved = true;
            MovingPiece.RenderMatrix *= Path.Value.M;
            if (CapturedPiece.Color != Party.None)
            {
                Game.Instance.Players[CapturedPiece.Color].Remove(CapturedPiece);
                Capture.Piece = Piece.Empty;
            }
            From.Piece = Piece.Empty;
            To.Piece = MovingPiece;
            Done = true;
        }

        public virtual void Undo()
        {
            if (!Done) return;
            Game game = Game.Instance;
            MovingPiece.HasMoved = init_HasMoved;
            MovingPiece.RenderMatrix = init_Matrix;

            From.Piece = MovingPiece;
            To.Piece = Piece.Empty;
            if (CapturedPiece.Color != Party.None)
            {
                game.Players[CapturedPiece.Color].Add(CapturedPiece);
                Capture.Piece = CapturedPiece;
            }
            Done = false;
        }
    }

    public class Promotion : Move
    {
        public Promotion(Move move)
        {
            From = move.From;
            To = move.To;
            Path = move.Path;
            MovingPiece = move.From.Piece;
            CapturedPiece = move.To.Piece;
            Capture = move.To;
        }

        public override MoveType Type { get; } = MoveType.Promotion;

        public Piece PromotedPiece { get; set; }

        public override void Do()
        {
            base.Do();
            PieceSelect promotion_menu = new PieceSelect(MovingPiece.Color, To);
            promotion_menu.Selected += (piece) =>
            {
                PromotedPiece = piece;
                PromotedPiece.HasMoved = false;
                PromotedPiece.RenderMatrix = MovingPiece.RenderMatrix;
                Player player = Game.Instance.Players[MovingPiece.Color];
                player.Remove(MovingPiece);
                player.Add(PromotedPiece);
                To.Piece = PromotedPiece;
            };
            promotion_menu.Show();
        }

        public override void Undo()
        {
            base.Undo();
            Player player = Game.Instance.Players[MovingPiece.Color];
            player.Remove(PromotedPiece);
            player.Add(MovingPiece);
            PromotedPiece = null;
        }
    }

    public class Castle : Move
    {
        public Move RookMove { get; set; }
        public override MoveType Type { get; } = MoveType.Castle;

        public override void Do()
        {
            RookMove.Do();
            base.Do();
        }

        public override void Undo()
        {
            base.Undo();
            RookMove.Undo();
        }
    }
}
