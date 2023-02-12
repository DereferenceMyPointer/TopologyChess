using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Media;
using System;

namespace TopologyChess
{
    public class Board : IEnumerable<Cell>
    {
        private readonly Cell[,] _board;

        public Cell this[int x, int y]
        {
            get => _board[y, x];
            set => _board[y, x] = value;
        }

        public Cell this[IntVector point]
        {
            get => this[point.X, point.Y];
            set => this[point.X, point.Y] = value;
        }
        public Board(int size)
        {
            Size = size;
            _board = new Cell[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    this[i, j] = new Cell(i, j) { Notation = ToNotation(i, j) };
        }

        public int Size { get; set; }

        public string ToNotation(int x, int y)
        {
            char file = Convert.ToChar(Convert.ToInt32('a') + x);
            string rank = (Size - y).ToString();
            return file + rank;
        }

        public IEnumerator<Cell> GetEnumerator()
            => _board.Cast<Cell>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _board.GetEnumerator();
    }
}