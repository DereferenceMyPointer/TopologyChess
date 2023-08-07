using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace TopologyChess
{
    public enum PieceValue
    {
        None = 0,
        Pawn = 1,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    public enum Party
    {
        White = 1,
        None = 0,
        Black = -1
    }

    public class Piece
    {
        protected Piece(PieceValue value, Party color)
        {
            Color = color;
            Value = value;
            Game.Players[color].Add(this);
            SetMovement();
            if (Color == Party.Black) RenderMatrix = new Matrix(-1, 0, 0, -1, 0, 0);
        }

        public static Piece New(PieceValue value, Party color)
        {
            switch (value)
            {
                case PieceValue.Pawn:
                    return new Pawn(color);
                case PieceValue.Knight:
                    return new Knight(color);
                case PieceValue.Bishop:
                    return new Bishop(color);
                case PieceValue.Rook:
                    return new Rook(color);
                case PieceValue.Queen:
                    return new Queen(color);
                case PieceValue.King:
                    return new King(color);
                default:
                    return Empty;
            }
        }

        public static Piece New(PieceType type)
        {
            return New((PieceValue)Math.Abs((int)type), (Party)Math.Sign((int)type));
        }

        protected Game Game => Game.Instance;

        protected virtual void SetMovement() { }

        public PieceValue Value { get; set; }

        public Party Color { get; set; }

        public PieceType Type => (PieceType)((int)Value * (int)Color);

        public bool Slides { get; set; }

        public bool HasMoved { get; set; } = false;

        public MatrixTransform RenderTransform { get; } = new MatrixTransform();

        public Matrix RenderMatrix
        {
            get => RenderTransform.Matrix;
            set
            {
                RenderTransform.Matrix = value;
                for (int i = 0; i < MoveDirections.Length; i++)
                {
                    MoveDirections[i] = (IntVector)value.Transform((Vector)MoveDirections[i]);
                }
            }
        }

        public IntVector[] MoveDirections { get; set; }
        
        public virtual IntVector[] AttackDirections { get => MoveDirections; }
        
        public IntVector Position { get; set; }

        private Piece()
        {
            Value = PieceValue.None;
            Color = Party.None;
        }

        public static readonly Piece Empty = new Piece();

        public virtual List<Move> GetMoves(bool include_illegal)
        {
            List<Move> moves = new List<Move>();
            Cell from = Game.Board[Position];
            Piece king = Game.Players[Color].King;
            int distance = 0;
            List<Chain<Step>> leads = new List<Chain<Step>>();
            List<Chain<Step>> new_leads = new List<Chain<Step>>();
            foreach (var d in MoveDirections) leads.Add(
                new Chain<Step>(new Step(Position, d, Matrix.Identity))
            );
            do
            {
                leads.Sort((a, b) =>
                    Topology.Sides(a.Value.P, Game.Board.Size).Count -
                    Topology.Sides(b.Value.P, Game.Board.Size).Count
                );
                foreach (var lead in leads)
                {
                    Step next = lead.Value;
                    next.P += next.V;
                    foreach (Step warp in Game.CurrentTopology.Warp(next, Game.Board.Size))
                    {
                        if (!new_leads.Any(l => l.Value == warp)) new_leads.Add(lead.Add(warp));
                    }
                }
                leads.Clear();
                foreach (var lead in new_leads)
                {
                    Cell to = Game.Board[lead.Value.P];
                    if (to.Piece.Color == Color) continue;
                    if (to.Piece.Type == PieceType.Empty) leads.Add(lead);
                    if (!moves.Any(m => m.To == to.Position))
                    {
                        Move move = new Move(from, to, lead);
                        if (!Game.IsAttacked(this == king ? to.Position : king.Position, move))
                            moves.Add(move);
                        else if (include_illegal)
                            moves.Add(move);
                    }
                }
                new_leads.Clear();
            } while (Slides && leads.Any() && distance++ < Game.Board.Size * Game.Board.Size);
            return moves;
        }
    }

    public class Pawn : Piece
    {
        public Pawn(Party color) : base(PieceValue.Pawn, color) { }

        protected override void SetMovement()
        {
            Slides = false;
            MoveDirections = new IntVector[] { new(0, -1), new(-1, -1), new(1, -1) };
        }

        public override IntVector[] AttackDirections
        {
            get => MoveDirections.Skip(1).ToArray();
        }

        public override List<Move> GetMoves(bool include_illegal)
        {
            List<Move> moves = new List<Move>();
            Cell from = Game.Board[Position];
            Piece king = Game.Players[Color].King;
            Chain<Step> push = new Chain<Step>(
                new Step(Position, MoveDirections[0], Matrix.Identity)
            );
            for (int i = 0; i < (HasMoved ? 1 : 2); i++)
            {
                Step next = push.Value;
                next.P += next.V;
                foreach (Step warp in Game.CurrentTopology.Warp(next, Game.Board.Size))
                {
                    push = push.Add(warp);
                    Cell to = Game.Board[push.Value.P];
                    if (to.Piece.Type == PieceType.Empty && !moves.Any(m => m.To == to.Position))
                    {
                        Move move = new Move(from, to, push);
                        if (!Game.IsAttacked(this == king ? to.Position : king.Position, move))
                            moves.Add(move);
                        else if (include_illegal)
                            moves.Add(move);
                    }
                }
            }
            Cell enPassant = null;
            // ERROR BANDAID FIX - IGNORE TOPOLOGY MOVE
            if (Game.LastMove != null && Game.LastMove.TopologyChange == null &&
                Game.LastMove.MovingPiece.Value == PieceValue.Pawn &&
                Game.LastMove.MovingPiece.Color != Color &&
                Game.LastMove.Path.Count() == 3)
            {
                enPassant = Game.Board[Game.LastMove.Path.Previous.Value.P];
            }
            for (int i = 1; i <= 2; i++)
            {
                Chain<Step> take = new Chain<Step>(
                    new Step(Position, MoveDirections[i], Matrix.Identity)
                );
                Step next = take.Value;
                next.P += next.V;
                foreach (Step warp in Game.CurrentTopology.Warp(next, Game.Board.Size))
                {
                    Chain<Step> take1 = take.Add(warp);
                    Cell to = Game.Board[take1.Value.P];
                    if (to.Piece.Color != Color && (to.Piece.Color != Party.None || to == enPassant))
                    {
                        Move takemove = new Move(from, to, take1);
                        if (to == enPassant) takemove.Capture = Game.LastMove.To;
                        if (!moves.Any(m => m.To == to.Position))
                        {
                            if (!Game.IsAttacked(king.Position, takemove))
                                moves.Add(takemove);
                            else if (include_illegal)
                                moves.Add(takemove);
                        }
                    }
                }
            }
            return moves;
        }
    }

    public class Knight : Piece
    {
        public Knight(Party color) : base(PieceValue.Knight, color) { }

        protected override void SetMovement()
        {
            Slides = false;
            MoveDirections = new IntVector[] {
                new(1, 2), new(1, -2), new(-1, -2), new(-1, 2),
                new(2, 1), new(2, -1), new(-2, -1), new(-2, 1)
            };
        }
    }

    public class Bishop : Piece
    {
        public Bishop(Party color) : base(PieceValue.Bishop, color) { }
        
        protected override void SetMovement()
        {
            Slides = true;
            MoveDirections = new IntVector[] { new(1, 1), new(1, -1), new(-1, -1), new(-1, 1) };
        }
    }

    public class Rook : Piece
    {
        public Rook(Party color) : base(PieceValue.Rook, color) { }

        protected override void SetMovement()
        {
            Slides = true;
            MoveDirections = new IntVector[] { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
        }
    }

    public class Queen : Piece
    {
        public Queen(Party color) : base(PieceValue.Queen, color) { }
        
        protected override void SetMovement()
        {
            Slides = true;
            MoveDirections = new IntVector[] {
                new(0, 1), new(0, -1), new(1, 0), new(-1, 0),
                new(1, 1), new(1, -1), new(-1, -1), new(-1, 1)
            };
        }
    }

    public class King : Piece
    {
        public King(Party color) : base(PieceValue.King, color)
        {
            Game.Players[color].King = this;
        }

        protected override void SetMovement()
        {
            Slides = false;
            MoveDirections = new IntVector[] {
                new(0, 1), new(0, -1), new(1, 0), new(-1, 0),
                new(1, 1), new(1, -1), new(-1, -1), new(-1, 1)
            };
        }

        public override List<Move> GetMoves(bool include_illegal)
        {
            List<Move> moves = base.GetMoves(include_illegal);
            if (HasMoved || Game.IsAttacked(Position) ||
                    !Game.Players[Game.CurrentParty].Pieces.Any(p => p.Value == PieceValue.Rook && !p.HasMoved))
                return moves;
            
            IntVector[] directions = new IntVector[4] { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
            foreach (var dir in directions)
            {
                int distance = 1;
                List<Chain<Step>> leads = new List<Chain<Step>>();
                List<Chain<Step>> new_leads = new List<Chain<Step>>();
                Piece rook = null;
                leads.Add(new Chain<Step>(new Step(Position, dir, Matrix.Identity)));
                do
                {
                    foreach (var lead in leads)
                    {
                        Step next = lead.Value;
                        next.P += next.V;
                        foreach (Step warp in Game.CurrentTopology.Warp(next, Game.Board.Size))
                        {
                            new_leads.Add(lead.Add(warp));
                        }
                    }
                    leads.Clear();
                    foreach (var lead in new_leads)
                    {
                        Piece piece1 = Game.Board[lead.Value.P].Piece;
                        if (piece1.Color == (Party)(-(int)Color)) continue;
                        if (piece1.Color == Color && (piece1.HasMoved ||
                            piece1.Value != PieceValue.Rook)) continue;
                        if (distance <= 2 && Game.IsAttacked(lead.Value.P)) continue;
                        if (piece1.Value == PieceValue.Rook) rook = piece1;
                        if (rook != null && distance > 1)
                        {
                            Chain<Step> old = lead;
                            Chain<Step> rpath = null;
                            while (old.Value.P != rook.Position) old = old.Previous;
                            Matrix inv = old.Value.M;
                            inv.Invert();
                            while (old.Previous != null)
                            {
                                rpath = new Chain<Step>
                                {
                                    Value = new Step(old.Value.P, -old.Value.V, old.Value.M * inv),
                                    Previous = rpath
                                };
                                old = old.Previous;
                            }
                            Chain<Step> kpath = lead;
                            while (kpath.Previous.Previous.Previous != null) kpath = kpath.Previous;
                            Move rookMove = new Move()
                            {
                                From = rook.Position,
                                To = rpath.Value.P,
                                MovingPiece = rook,
                                Path = rpath
                            };
                            if (Game.IsAttacked(kpath.Value.P, rookMove)) continue;
                            Castle castle = new Castle()
                            {
                                From = Position,
                                To = kpath.Value.P,
                                MovingPiece = this,
                                Path = kpath,
                                RookMove = rookMove
                            };
                            moves.Add(castle);
                        }
                        else leads.Add(lead);
                    }
                    new_leads.Clear();
                } while (leads.Any() && distance++ < Game.Board.Size * Game.Board.Size);
            }
            return moves;
        }
    }
}
