using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopologyChess
{
    public class Player
    {
        public Player(Party party)
        {
            Party = party;
        }

        public Party Party { get; }

        private List<Piece> pieces = new List<Piece>();
        public ReadOnlyCollection<Piece> Pieces => pieces.AsReadOnly();

        public Piece King { get; set; }

        public void Add(Piece piece)
        {
            pieces.Add(piece);
            attackDirections = null;
        }

        public void Remove(Piece piece)
        {
            pieces.Remove(piece);
            attackDirections = null;
        }

        public void Refresh() => attackDirections = null;

        private Dictionary<IntVector, bool> attackDirections;
        public Dictionary<IntVector, bool> AttackDirections
        {
            get
            {
                if (attackDirections != null) return attackDirections;
                else
                {
                    Dictionary<IntVector, bool> directions = new Dictionary<IntVector, bool>();
                    foreach (Piece piece in pieces)
                    {
                        foreach (var dir in piece.AttackDirections)
                        {
                            bool slide = piece.Slides;
                            if (!directions.ContainsKey(dir)) directions.Add(dir, slide);
                            else directions[dir] |= slide;
                        }
                    }
                    return attackDirections = directions;
                }
            }
            set => attackDirections = value;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Party.ToString());
            foreach (Piece piece in Pieces)
            {
                sb.Append(piece.Color.ToString()[0]);
                sb.Append(piece.Value.ToString());
                sb.Append(' ');
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
