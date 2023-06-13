using System;
using System.Linq;

namespace TopologyChess
{
    public partial class Game
    {
        public bool canTopologyMove(TopologyChange? change)
        {
            bool isInCheck = IsAttacked(CurrentPlayer.Pieces.FirstOrDefault(p => p.Value == PieceValue.King).Position);
            CurrentTopology = change?.ToTopology;
            bool willBeInCheck = IsAttacked(CurrentPlayer.Pieces.FirstOrDefault(p => p.Value == PieceValue.King).Position);
            CurrentTopology = change?.FromTopology;
            return !isInCheck && !willBeInCheck;
        }
    }
}
