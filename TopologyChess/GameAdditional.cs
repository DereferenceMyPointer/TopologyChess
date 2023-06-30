using System;
using System.Linq;

namespace TopologyChess
{
    public partial class Game
    {

        // Kind of buggy still - will allow topology into check or mate sometimes for some reason (may have misunderstood code)
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
