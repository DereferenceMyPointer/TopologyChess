using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TopologyChess
{
    public enum PieceValue
    {
        None = 0,
        Pawn = 1,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    public enum Party
    {
        White = 1,
        None = 0,
        Black = -1
    }

    public class Piece
    {
        public Piece(PieceValue value, Party color)
        {
            Value = value;
            Color = color;
            Type = (PieceType)((int)value * (int)color);
            MoveDirections = PieceMoveDirections[Value];
            if (Type == PieceType.BlackPawn)
            {
                for (int i = 0; i < 3; i++)
                {
                    MoveDirections[i] = -MoveDirections[i];
                }
            }
        }

        public Piece(PieceType type)
        {
            Value = (PieceValue)Math.Abs((int)type);
            Color = (Party)Math.Sign((int)type);
            Type = type;
            MoveDirections = PieceMoveDirections[Value];
            if (Type == PieceType.BlackPawn)
            {
                for (int i = 0; i < 3; i++)
                {
                    MoveDirections[i] = -MoveDirections[i];
                }
            }
        }

        public PieceValue Value { get; }
        public Party Color { get; }

        public PieceType Type { get; }

        public bool HasMoved { get; set; } = false;

        public Point Position { get; set; }
        public Matrix RenderMatrix { get; set; } = Matrix.Identity;

        private static readonly Dictionary<PieceValue, List<Vector>> PieceMoveDirections;

        static Piece()
        {
            PieceMoveDirections = new Dictionary<PieceValue, List<Vector>>();
            var rook_moves = new List<Vector>() { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
            var bishop_moves = new List<Vector>() { new(1, 1), new(1, -1), new(-1, -1), new(-1, 1) };
            var knight_moves = new List<Vector>() {
                new(1, 2), new(1, -2), new(-1, -2), new(-1, 2),
                new(2, 1), new(2, -1), new(-2, -1), new(-2, 1)
            };
            var pawn_moves = new List<Vector>() { new(0, 1), new(-1, 1), new(1, 1) };
            var queen_moves = rook_moves.Union(bishop_moves).ToList();
            PieceMoveDirections.Add(PieceValue.None, new List<Vector>());
            PieceMoveDirections.Add(PieceValue.Pawn, pawn_moves);
            PieceMoveDirections.Add(PieceValue.Knight, knight_moves);
            PieceMoveDirections.Add(PieceValue.Bishop, bishop_moves);
            PieceMoveDirections.Add(PieceValue.Rook, rook_moves);
            PieceMoveDirections.Add(PieceValue.Queen, queen_moves);
            PieceMoveDirections.Add(PieceValue.King, queen_moves);
            Empty = new Piece(PieceType.Empty);
        }

        public List<Vector> MoveDirections { get; set; }

        public static Piece Empty;
    }
}
