using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace TopologyChess
{
    public class Board : IEnumerable<Cell>
    {
        private readonly Cell[,] board;
        public Party Perspective { get; set; } = Party.White;

        public Cell this[int rank, int file]
        {
            get => board[rank, file];
            set => board[rank, file] = value;
        }

        public Cell this[Point point]
        {
            get => board[(int)point.X, (int)point.Y];
            set => board[(int)point.X, (int)point.Y] = value;
        }

        public Board()
        {
            board = new Cell[8, 8];
            for (int i = 0; i < board.GetLength(0); i++)
                for (int j = 0; j < board.GetLength(1); j++)
                    board[i, j] = new Cell();
        }

        public IEnumerator<Cell> GetEnumerator()
            => new ApparentEnumerator(board);

        IEnumerator IEnumerable.GetEnumerator()
            => new ApparentEnumerator(board);
    }
}