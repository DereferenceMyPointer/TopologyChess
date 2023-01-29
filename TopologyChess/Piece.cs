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
            SetMoveDirections();
        }

        public Piece(PieceType type)
        {
            Value = (PieceValue)Math.Abs((int)type);
            Color = (Party)Math.Sign((int)type);
            Type = type;
            SetMoveDirections();
        }

        private void SetMoveDirections()
        {
            switch (Value)
            {
                case PieceValue.Pawn:
                    int d = -(int)Color;
                    MoveDirections = new Vector[] { new(-1, d), new(0, d), new(1, d) };
                    break;
                case PieceValue.Knight:
                    MoveDirections = new Vector[] {
                        new(1, 2), new(1, -2), new(-1, -2), new(-1, 2),
                        new(2, 1), new(2, -1), new(-2, -1), new(-2, 1)
                    };
                    break;
                case PieceValue.Bishop:
                    MoveDirections = new Vector[] { new(1, 1), new(1, -1), new(-1, -1), new(-1, 1) };
                    break;
                case PieceValue.Rook:
                    MoveDirections = new Vector[] { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
                    break;
                case PieceValue.Queen:
                case PieceValue.King:
                    MoveDirections = new Vector[] { 
                        new(0, 1), new(0, -1), new(1, 0), new(-1, 0),
                        new(1, 1), new(1, -1), new(-1, -1), new(-1, 1)
                    };
                    break;
                default:
                    break;
            }
        }

        public PieceValue Value { get; }
        public Party Color { get; }

        public PieceType Type { get; }

        public bool HasMoved { get; set; } = false;

        public MatrixTransform RenderTransform { get; set; } = new MatrixTransform();

        public Vector[] MoveDirections { get; set; }

        public static readonly Piece Empty = new Piece(PieceType.Empty);
    }
}
