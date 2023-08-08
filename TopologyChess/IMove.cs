using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopologyChess
{
    public interface IMove
    {
        MoveType Type { get; }
        bool Legal { get; }
        void Do();
        void Undo();
    }

    public enum MoveType
    {
        TopologyChange, Move, Promotion, Castle
    }
}
