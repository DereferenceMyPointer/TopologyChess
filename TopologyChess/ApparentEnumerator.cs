using System.Collections;
using System.Collections.Generic;

namespace TopologyChess
{
    internal class ApparentEnumerator : IEnumerator<Cell>
    {
        private readonly Cell[,] area;
        private readonly Party perspective;
        private readonly int n, L;
        private int k = -1;

        public ApparentEnumerator(Cell[,] area)
        {
            this.area = area;
            n = area.GetLength(0);
            L = area.Length;
        }

        public Cell Current => area[k % n, 7 - k / n];

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