using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace TopologyChess
{
    public partial class Game : INotifyPropertyChanged
    {
        private Game(int size)
        {
            Instance = this;
            Board = new Board(size);
            CurrentTopology = Topology.Topologies.FirstOrDefault(t => t.Name == "Flat");
            DefaultSetup();
            PossibleMoves = CalculateMoves();
        }

        public static Game Instance { get; private set; }

        public static void NewGame(int size = 8)
        {
            new Game(size);
        }

        public static MainWindow Window { get; set; }

        private Board _board;
        public Board Board
        {
            get => _board;
            set
            {
                _board = value;
                OnPropertyChanged();
            }
        }

        private Topology _currentTopology;
        public Topology CurrentTopology
        {
            get => _currentTopology;
            set
            {
                _currentTopology = value;
                TopologyModel.Transform(Mesh, (p) => value.Equation(p.X, p.Y)); //
                BorderPoints = TopologyModel.GetBorder(Mesh);
                OnPropertyChanged();
                OnPropertyChanged(nameof(Mesh));
                OnPropertyChanged(nameof(BorderPoints));
            }
        }

        private MeshGeometry3D _mesh = TopologyModel.GenerateLattice(8 * 20);
        public MeshGeometry3D Mesh
        {
            get => _mesh;
            set
            {
                _mesh = value;
                OnPropertyChanged();
            }
        }

        private Point3DCollection _border_points = new Point3DCollection();
        public Point3DCollection BorderPoints
        {
            get => _border_points;
            set
            {
                _border_points = value;
                OnPropertyChanged();
            }
        }

        //public PieceSelect PieceSelect { get; set; }

        public IMove LastMove { get; set; }

        public ObservableCollection<IMove> History { get; set; } = new();

        public Dictionary<Party, Player> Players { get; } = new()
        {
            { Party.White, new Player(Party.White) },
            { Party.Black, new Player(Party.Black) }
        };

        // players that are controlled from this app instance
        public List<Party> AvailibleParties { get; set; } = new()
        {
            Party.White, Party.Black
        };

        private Party _currentParty = Party.White;
        public Party CurrentParty
        {
            get => _currentParty;
            set
            {
                _currentParty = value;
                OnPropertyChanged();
            }
        }

        public Player CurrentPlayer => Players[CurrentParty];

        public bool IsOver { get; set; } = false;

        public void AddPiece(PieceValue value, Party color, int x, int y)
        {
            Piece piece = Piece.New(value, color);
            Players[color].Add(piece);
            Board[x, y].Piece = piece;
        }

        public void DefaultSetup()
        {
            Board = new Board(8);
            for (int c = -1; c <= 1; c += 2)
            {
                Party color = (Party)c;
                int rank = c == -1 ? 0 : 7;

                AddPiece(PieceValue.Rook, color, 0, rank);
                AddPiece(PieceValue.Knight, color, 1, rank);
                AddPiece(PieceValue.Bishop, color, 2, rank);
                AddPiece(PieceValue.Queen, color, 3, rank);
                AddPiece(PieceValue.King, color, 4, rank);
                AddPiece(PieceValue.Bishop, color, 5, rank);
                AddPiece(PieceValue.Knight, color, 6, rank);
                AddPiece(PieceValue.Rook, color, 7, rank);
                for (int f = 0; f < 8; f++)
                {
                    AddPiece(PieceValue.Pawn, color, f, rank - c);
                }
            }
        }

        public bool IsAttacked(IntVector? position, Move after = null)
        {
            if (position == null) return false;
            Party color = (Party)(-(int)CurrentParty);
            foreach ((IntVector dir, bool slide) in Players[color].AttackDirections)
            {
                int distance = 1;
                List<Step> stepleads = new List<Step>();
                List<Step> new_stepleads = new List<Step>();
                stepleads.Add(new Step((IntVector)position, -dir, Matrix.Identity));
                do
                {
                    foreach (var lead in stepleads)
                    {
                        Step next = lead;
                        next.P += next.V;
                        foreach (Step warp in CurrentTopology.Warp(next, Board.Size))
                        {
                            new_stepleads.Add(warp);
                        }
                    }
                    stepleads.Clear();
                    foreach (var lead in new_stepleads)
                    {
                        Piece piece = Board[lead.P].Piece;
                        if (piece.Color == color &&
                            piece.AttackDirections.Contains(-lead.V) &&
                            ((distance == 1) || piece.Slides) &&
                            piece.Position != after?.Capture?.Position) return true;
                        if (lead.P == after?.To.Position) continue;
                        if (piece.Type == PieceType.Empty || lead.P == after?.From.Position || lead.P == after?.Capture?.Position)
                            stepleads.Add(lead);
                    }
                    new_stepleads.Clear();
                } while (slide && stepleads.Any() && distance++ < Board.Size * Board.Size);
            }
            return false;
        }

        public Dictionary<Cell, List<Move>> PossibleMoves { get; private set; } = new();

        public Dictionary<Cell, List<Move>> CalculateMoves()
        {
            Dictionary<Cell, List<Move>> moves = new();
            foreach (var piece in Players[CurrentParty].Pieces)
            {
                List<Move> piecemoves = piece.GetMoves(false); //
                if (piecemoves.Any()) moves.Add(Board[piece.Position], piecemoves);
            }
            return moves;
        }

        public void Submit(IMove move)
        {
            LastMove = move;
            History.Add(move);
            CurrentParty = (Party)(-(int)CurrentParty);

            PossibleMoves = CalculateMoves();
            if (!PossibleMoves.Any()) End();
        }

        public void End()
        {
            //
            MessageBox.Show("End");
        }

        private bool _isBlocked = false;
        public bool IsBlocked
        {
            get => _isBlocked;
            set
            {
                _isBlocked = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
