using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Media;

namespace TopologyChess
{
    public class Board : IEnumerable<Cell>
    {
        private readonly Cell[,] _board;

        public Cell this[int x, int y]
        {
            get => _board[y, x];
            set => _board[y, x] = value;
        }

        public Cell this[Point point]
        {
            get => this[(int)point.X, (int)point.Y];
            set => this[(int)point.X, (int)point.Y] = value;
        }

        public int Size { get; set; }

        public Topology CurrentTopology { get; set; } = Topology.Topologies.FirstOrDefault(t => t.Name == "Flat");

        public Board(int size)
        {
            Size = size;
            _board = new Cell[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    this[i, j] = new Cell(i, j);
        }

        public HashSet<Move> TestMoves(Cell cell)
        {
            HashSet<Move> moves = new HashSet<Move>();
            if (cell.Piece.Type == PieceType.Empty) return moves;
            foreach (var d in cell.Piece.MoveDirections)
            {
                int x = cell.X + (int)d.X;
                int y = cell.Y + (int)d.Y;
                if (!OutOfBounds(new Point(x, y)))
                {
                    moves.Add(new Move { From = cell, To = this[x, y] });
                }
            }
            return moves;
        }

        private Move LastMove { get; set; }

        public bool OutOfBounds(Point p) => !(0 <= p.X && p.X < Size && 0 <= p.Y && p.Y < Size);

        public void MarkMoves(Cell from)
        {
            if (from.Piece.Type == PieceType.Empty) return;
            if (from.Piece.Value != PieceValue.Pawn)
            {
                bool once = (from.Piece.Value == PieceValue.Knight || from.Piece.Value == PieceValue.King);
                List<Chain<Step>> leads = new List<Chain<Step>>();
                List<Chain<Step>> new_leads = new List<Chain<Step>>();
                foreach (var d in from.Piece.MoveDirections) leads.Add(
                    new Chain<Step>(new Step(from.Position, d, Matrix.Identity))
                );
                do
                {
                    leads.Sort((a, b) => (OutOfBounds(a.Value.P) ? 1 : 0) - (OutOfBounds(b.Value.P) ? 1 : 0));
                    foreach (var lead in leads)
                    {
                        Step next = lead.Value;
                        next.P += next.V;
                        foreach (Step warp in CurrentTopology.Warp(next, Size))
                        {
                            if (!new_leads.Any(l => l.Value == warp)) new_leads.Add(lead.Add(warp));
                        }
                    }
                    leads.Clear();
                    foreach (var lead in new_leads)
                    {
                        Cell to = this[lead.Value.P];
                        if (to.Piece.Color == from.Piece.Color) continue;
                        if (to.Piece.Type == PieceType.Empty) leads.Add(lead);
                        to.Move ??= new Move(from, to, lead);
                    }
                    new_leads.Clear();
                } while (!once && leads.Any());
            }
            else
            {
                Piece pawn = from.Piece;
                Chain<Step> push = new Chain<Step>(
                    new Step(from.Position, pawn.MoveDirections[1], Matrix.Identity)
                );
                for (int i = 0; i < (pawn.HasMoved ? 1 : 2); i++)
                {
                    Step next = push.Value;
                    next.P += next.V;
                    foreach (Step warp in CurrentTopology.Warp(next, Size))
                    {
                        push = push.Add(warp);
                        Cell to = this[push.Value.P];
                        if (to.Piece.Type == PieceType.Empty) to.Move ??= new Move(from, to, push);
                    }
                }
                Cell enPassant = null;
                if (LastMove != null &&
                    LastMove.MovingPiece.Value == PieceValue.Pawn &&
                    LastMove.Path.Count() == 3)
                {
                    enPassant = this[LastMove.Path.Previous.Value.P];
                }
                for (int i = 0; i <= 2; i += 2)
                {
                    Chain<Step> take = new Chain<Step>(
                        new Step(from.Position, pawn.MoveDirections[i], Matrix.Identity)
                    );
                    Step next = take.Value;
                    next.P += next.V;
                    foreach (Step warp in CurrentTopology.Warp(next, Size))
                    {
                        take = take.Add(warp);
                        Cell to = this[take.Value.P];
                        if (to.Piece.Color != pawn.Color && (to.Piece.Color != Party.None || to == enPassant))
                        {
                            to.Move ??= new Move(from, to, take);
                            if (to == enPassant) to.Move.Capture = LastMove.To;
                        }
                    }
                }
            }
        }

        public void Play(Move move)
        {
            Piece piece = move.MovingPiece;
            move.Capture.Piece = Piece.Empty;
            move.To.Piece = piece;
            move.From.Piece = Piece.Empty;
            piece.HasMoved = true;
            piece.RenderMatrix.Append(move.Path.Value.M);
            if (piece.Value == PieceValue.Pawn)
            {
                piece.RenderMatrix.Transform(piece.MoveDirections);
            }
            LastMove = move;
        }

        public IEnumerator<Cell> GetEnumerator()
            => _board.Cast<Cell>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _board.GetEnumerator();
    }
}