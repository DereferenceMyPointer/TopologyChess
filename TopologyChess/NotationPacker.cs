using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

/** Notation conversion and packing class
 * Should support txt and possibly other export and import formats
 * Currently projected to store board states as move sequences
 */


namespace TopologyChess
{
    /*public class NotationPacker
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
                    outp += "k" + move.From.X + "" + move.From.Y + ":" + move.From.X + "" + move.From.Y + ",";
                }
                else
                {
                    outp += "" + move.From.X + "" + move.From.Y + ":" + move.From.X + "" + move.From.Y + ",";
                }
                
            }
            return outp[..^1];
        }

        public List<Move> StringToMoveList(string text)
        {
            List<Move> moves = new List<Move>();
            foreach (string s in text.Split(','))
            {
                if (s[0] == 'k')
                {
                    // Implement using Game castling strategy - would be much easier with delegated move handling
                    moves.Add(new Castle().RookMove);
                } else if (Char.IsLetter(s[0])) {
                    moves.Add(new Move.BoardTransformation(new TopologyChange()
                    {
                        FromTopology = TopologyDictionary[s[0..1]],
                        ToTopology = TopologyDictionary[s[3..4]]
                    }));
                } else
                {
                    // Need access to moving piece for current system - would need to
                    // actually perform moves on a board to do this as of right now
                    moves.Add(new Move(){
                        From = new IntVector(s[0], s[1]),
                        To = new IntVector(s[3], s[4])
                    });
                }
            }

            return moves;
        }

    }*/
}