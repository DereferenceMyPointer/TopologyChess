using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        }

        public Piece(PieceType type)
        {
            Value = (PieceValue)Math.Abs((int)type);
            Color = (Party)Math.Sign((int)type);
            Type = type;
        }

        public PieceValue Value { get; }
        public Party Color { get; }

        public PieceType Type { get; }

        public bool HasMoved { get; set; } = false;
        public bool IsFirstMove { get; set; } = false;

        public Point Position { get; set; }
        public Vector InternalDirection { get; set; }

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
            var queen_moves = rook_moves.Union(bishop_moves).ToList();
            PieceMoveDirections.Add(PieceValue.Pawn, new List<Vector>());
            PieceMoveDirections.Add(PieceValue.Knight, knight_moves);
            PieceMoveDirections.Add(PieceValue.Bishop, bishop_moves);
            PieceMoveDirections.Add(PieceValue.Rook, rook_moves);
            PieceMoveDirections.Add(PieceValue.Queen, queen_moves);
            PieceMoveDirections.Add(PieceValue.King, queen_moves);
        }

        public List<Vector> MoveDirections => PieceMoveDirections[Value];
    }
}
