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
                    MoveDirections = new IntVector[] { new(0, d), new(-1, d), new(1, d) };
                    break;
                case PieceValue.Knight:
                    MoveDirections = new IntVector[] {
                        new(1, 2), new(1, -2), new(-1, -2), new(-1, 2),
                        new(2, 1), new(2, -1), new(-2, -1), new(-2, 1)
                    };
                    break;
                case PieceValue.Bishop:
                    MoveDirections = new IntVector[] { new(1, 1), new(1, -1), new(-1, -1), new(-1, 1) };
                    break;
                case PieceValue.Rook:
                    MoveDirections = new IntVector[] { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
                    break;
                case PieceValue.Queen:
                case PieceValue.King:
                    MoveDirections = new IntVector[] { 
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

        public bool Slides => Value == PieceValue.Bishop || Value == PieceValue.Rook || Value == PieceValue.Queen;

        public bool HasMoved { get; set; } = false;

        public MatrixTransform RenderTransform { get; set; } = new MatrixTransform();

        public IntVector[] MoveDirections { get; set; }
        public IntVector[] AttackDirections => MoveDirections.Skip(Value == PieceValue.Pawn ? 1 : 0).ToArray();
        public IntVector Position { get; set; }

        public static readonly Piece Empty = new Piece(PieceType.Empty);
    }
}


/*static Piece()
        {
            var myResourceDictionary = new ResourceDictionary();
            myResourceDictionary.Source =
                new Uri("/DllName;component/Resources/PiecesDictionary.xaml",
                        UriKind.RelativeOrAbsolute);
        }*/

/*public static readonly Dictionary<PieceValue, IntVector[]> Directions = new()
{
    { PieceValue.None, Array.Empty<IntVector>() },
    { PieceValue.Pawn, Array.Empty<IntVector>() },
    { PieceValue.Knight, new IntVector[] {
        new(1, 2), new(1, -2), new(-1, -2), new(-1, 2),
        new(2, 1), new(2, -1), new(-2, -1), new(-2, 1)
    } },
    { PieceValue.Bishop, new IntVector[] {
        new(1, 1), new(1, -1), new(-1, -1), new(-1, 1)
    } },
    { PieceValue.Rook, new IntVector[] {
        new(0, 1), new(0, -1), new(1, 0), new(-1, 0)
    } },
    { PieceValue.Queen, new IntVector[] {
        new(0, 1), new(0, -1), new(1, 0), new(-1, 0),
        new(1, 1), new(1, -1), new(-1, -1), new(-1, 1)
    } },
    { PieceValue.King, new IntVector[] {
        new(0, 1), new(0, -1), new(1, 0), new(-1, 0),
        new(1, 1), new(1, -1), new(-1, -1), new(-1, 1)
    } },
};

public static readonly IntVector[][] AllDirections = new IntVector[][]
{
    new IntVector[] { // Once
        new(1, 2), new(1, -2), new(-1, -2), new(-1, 2),
        new(2, 1), new(2, -1), new(-2, -1), new(-2, 1)
    },
    new IntVector[] { // Slide
        new(0, 1), new(0, -1), new(1, 0), new(-1, 0),
        new(1, 1), new(1, -1), new(-1, -1), new(-1, 1)
    }
};*/