using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopologyChess
{
    public interface IMove
    {
        void Do();
        void Undo();
        void Submit();
    }
}
