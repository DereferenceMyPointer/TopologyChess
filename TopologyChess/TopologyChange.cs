using System;

namespace TopologyChess
{
    public class TopologyChange : IMove
    {
        public Topology From { get; set; }
        public Topology To { get; set; }

        public TopologyChange() { }

        public TopologyChange(Topology from, Topology to)
        {
            From = from;
            To = to;
        }

        public MoveType Type { get; } = MoveType.TopologyChange;
        public bool Legal { get; set; } = true;

        public void Do()
        {
            Game.Instance.CurrentTopology = To;
        }

        public void Undo()
        {
            Game.Instance.CurrentTopology = From;
        }
    }

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