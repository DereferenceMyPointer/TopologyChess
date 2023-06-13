using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TopologyChess
{
    public class NotationPacker
    {
        private Dictionary<string, Topology> _topologyDictionary;

        public Dictionary<string, Topology> TopologyDictionary { get { return _topologyDictionary; } }

        public NotationPacker()
        {
            _topologyDictionary = new Dictionary<string, Topology>();
            foreach(Topology t in Topology.Topologies)
            {
                _topologyDictionary[t.Id] = t;
            }
        }

        public string PackMoves(ObservableCollection<Move> moves)
        {
            string outp = string.Empty;
            foreach(Move move in moves)
            {
                if(move.TopologyChange != null)
                {
                    outp += move.TopologyChange?.FromTopology + ":" + move.TopologyChange?.ToTopology + ",";
                }
                if(move is Castle castle)
                {
                    // Figure out how to convert castle logic into notation - probably k(king coordinates initial):k(king coordinates final)
                    outp += "OO,";
                }
                else
                {
                    outp += "" + move.From.X + "" + move.From.Y + ":" + move.From.X + "" + move.From.Y + ",";
                }
                
            }
            return outp.Substring(0, outp.Length - 1);
        }   

    }
}