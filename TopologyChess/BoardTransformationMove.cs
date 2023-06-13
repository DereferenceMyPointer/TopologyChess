using System;

/** Division of Move dedicated to topology change moves
 * 
 * 
 */
namespace TopologyChess
{
    public struct TopologyChange
    {
        public Topology FromTopology;
        public Topology ToTopology;
    }
    public partial class Move
    {

        public TopologyChange? TopologyChange { get; set; }

        public class BoardTransformation : Move
        {
            public static Move RandomBoardTransformation(Game game)
            {
                Move outputMove = NoMove;
                outputMove.TopologyChange = new TopologyChange
                {
                    FromTopology = game.CurrentTopology,
                    ToTopology = Topology.Topologies[new Random().Next(Topology.Topologies.Count)]
                };
                return outputMove;
            }

            public Move BoardTransformationMove(Topology topology, Game game)
            {
                Move outputMove = NoMove;
                outputMove.TopologyChange = new TopologyChange
                {
                    FromTopology = game.CurrentTopology,
                    ToTopology = topology
                };
                return outputMove;
            }
        }
    }
}