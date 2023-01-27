using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace TopologyChess
{
    public class Board : IEnumerable<Cell>
    {
        private readonly Cell[,] _board;

        public Cell this[int rank, int file]
        {
            get => _board[7 - file, rank];
            set => _board[7 - file, rank] = value;
        }

        public Cell this[Point point]
        {
            get => this[(int)(point.X - 0.5), (int)(point.Y - 0.5)];
            set => this[(int)(point.X - 0.5), (int)(point.Y - 0.5)] = value;
        }

        public Board()
        {
            _board = new Cell[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    this[i, j] = new Cell(i, j);
        }

        public HashSet<Move> TestMoves(Cell cell)
        {
            HashSet<Move> moves = new HashSet<Move>();
            if (cell.Piece.Type == PieceType.Empty) return moves;
            foreach (var d in cell.Piece.MoveDirections)
            {
                int x = cell.X + (int)d.X;
                int y = cell.Y + (int)d.Y;
                if (0 <= x && x < 8 && 0 <= y && y < 8)
                {
                    moves.Add(new Move(cell, this[x, y]));
                }
            }
            return moves;
        }

        public HashSet<Move> BasicMoves(int x, int y)
        {
            HashSet<Move> moves = new HashSet<Move>();
            Cell cell = this[x, y];
            if (cell.Piece.Type == PieceType.Empty) return moves;
            Piece piece = cell.Piece;
            Point pos = new Point(x + 0.5, y + 0.5);
            if (piece.Value != PieceValue.Pawn)
            {
                bool once = (piece.Value == PieceValue.Knight || piece.Value == PieceValue.King);
                List<Point> leads = new List<Point>();
                foreach (var d in piece.MoveDirections) leads.Add(pos);
                while (leads.Any())
                {
                    
                    if (once) break;
                }
            }
            return moves;
        }

        public IEnumerator<Cell> GetEnumerator()
            => _board.Cast<Cell>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _board.GetEnumerator();
    }
}