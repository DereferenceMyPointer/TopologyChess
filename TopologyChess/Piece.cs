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

    public enum PieceColor
    {
        White = 1,
        None = 0,
        Black = -1
    }

    public class Piece
    {
        public Piece(PieceValue value, PieceColor color)
        {
            Value = value;
            Color = color;
            Type = (PieceType)((int)value * (int)color);
        }

        public Piece(PieceType type)
        {
            Value = (PieceValue)Math.Abs((int)type);
            Color = (PieceColor)Math.Sign((int)type);
            Type = type;
        }

        public PieceValue Value { get; }
        public PieceColor Color { get; }

        public PieceType Type { get; }

        public bool HasMoved { get; set; } = false;
        public bool IsFirstMove { get; set; } = false;

        public Point Position { get; set; }
        public Point InternalDirection { get; set; }

        public HashSet<Move> Moves { get; set; } = new HashSet<Move>();

        public HashSet<Move> CalcMoves()
        {
            return Moves;
        }
    }
}
