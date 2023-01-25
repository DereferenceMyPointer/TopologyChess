using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace TopologyChess
{
    public class Board : IEnumerable<Cell>
    {
        private readonly Cell[,] _area;

        public Cell this[int rank, int file]
        {
            get => _area[rank, file];
            set => _area[rank, file] = value;
        }

        public Cell this[Point point]
        {
            get => _area[(int)point.X, (int)point.Y];
            set => _area[(int)point.X, (int)point.Y] = value;
        }

        public Topology CurrentTopology { get; set; } = Topology.Topologies.FirstOrDefault(t => t.Name == "Flat");

        public Board()
        {
            _area = new Cell[8, 8];
            for (int i = 0; i < _area.GetLength(0); i++)
                for (int j = 0; j < _area.GetLength(1); j++)
                    _area[i, j] = new Cell();
        }

        public IEnumerator<Cell> GetEnumerator()
            => new ApparentEnumerator(_area);

        IEnumerator IEnumerable.GetEnumerator() 
            => new ApparentEnumerator(_area);
    }

    internal class ApparentEnumerator : IEnumerator<Cell>
    {
        private readonly Cell[,] area;
        private readonly int n, L;
        private int k = -1;

        public ApparentEnumerator(Cell[,] area)
        {
            this.area = area;
            n = area.GetLength(0);
            L = area.Length;
        }

        public Cell Current { get => area[k % n, 7 - k / n]; }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            return ++k < L;
        }

        public void Reset()
        {
            k = -1;
        }

        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }
    }
}