using System;

/** Division of Move dedicated to topology change moves
 * 
 * 
 */
namespace TopologyChess
{
    /*public struct TopologyChange
    {
        public Topology FromTopology;
        public Topology ToTopology;
    }
    public class TopologyMove// : IMove
    {

        public TopologyChange? TopologyChange { get; set; }

        public class BoardTransformation : Move
        {
            public static TopologyMove RandomBoardTransformation(Game game)
            {
                TopologyMove outputMove = NoMove;
                outputMove.TopologyChange = new TopologyChange
                {
                    FromTopology = game.CurrentTopology,
                    ToTopology = Topology.Topologies[new Random().Next(Topology.Topologies.Count)]
                };
                return outputMove;
            }

            public BoardTransformation(Topology topology, Game game)
            {
                this.From = NoMove.From;
                this.To = NoMove.To;
                this.MovingPiece = NoMove.MovingPiece;
                this.Path = NoMove.Path;
                this.TopologyChange = new TopologyChange
                {
                    FromTopology = game.CurrentTopology,
                    ToTopology = topology
                };
            }

            public BoardTransformation(TopologyChange change)
            {
                this.From = NoMove.From;
                this.To = NoMove.To;
                this.MovingPiece = NoMove.MovingPiece;
                this.Path = NoMove.Path;
                this.TopologyChange = change;
            }
        }
    }*/
}