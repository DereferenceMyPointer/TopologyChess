using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace TopologyChess
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Chess.Board = new Board(8);
            TopologyModel.Transform(Mesh, (Point p) => Equations.Globe(p.X, p.Y));
            BorderPoints = TopologyModel.GetBorder(Mesh);
            Topologies = new ObservableCollection<Topology>(Topology.Topologies);

            /*Step s1 = new Step(new(0, 0), new(0, 0), new Matrix(1, 0, 0, -2, 0, 0));
            Matrix m2 = s1.M;
            m2.Invert();
            MessageBox.Show(m2.ToString());*/
        }

        private Game _chess = new Game();
        private MeshGeometry3D _mesh = TopologyModel.GenerateLattice(8 * 20);
        private Point3DCollection _border_points = new Point3DCollection();

        private ICommand _newGameCommand;
        private ICommand _clearCommand;
        private ICommand _cellCommand;

        public IEnumerable<char> Numbers => "87654321";
        public IEnumerable<char> Letters => "ABCDEFGH";

        public ObservableCollection<Topology> Topologies { get; set; }

        public Game Chess
        {
            get => _chess;
            set
            {
                _chess = value;
                OnPropertyChanged();
            }
        }

        public MeshGeometry3D Mesh
        {
            get => _mesh;
            set
            {
                _mesh = value;
                OnPropertyChanged();
            }
        }

        public Point3DCollection BorderPoints
        { 
            get => _border_points; 
            set
            {
                _border_points = value;
                OnPropertyChanged();
            } 
        }

        private double angle = 0;
        //private RotateTransform _persp = new RotateTransform();
        public double PerspAngle
        {
            get => angle;
            set
            {
                angle = value;
                OnPropertyChanged();
            }
        }

        public ICommand NewGameCommand => _newGameCommand ??= new RelayCommand(parameter => 
        {
            Chess = new Game();
            //SetupBoard();
            //Chess.DefaultSetup();
            //Chess.CurrentTopology = Topology.Topologies.FirstOrDefault(t => t.Name == "Moebius Horizontal");
            /*var top = Board.CurrentTopology;
            string str = "";
            for (int i = 0; i < 4; i++)
            {
                str += i.ToString() + ":\n" + top.Connections[i].ToString() + " " + top.WarpMatrices[i] + "\n\n";
            }
            MessageBox.Show(str);*/
        });

        public ICommand ClearCommand => _clearCommand ??= new RelayCommand(parameter =>
        {
            Chess.Board = new Board(8);
            //PerspAngle = (PerspAngle + 180) % 360;
        });

        private Cell selectedCell;
        public ICommand CellCommand => _cellCommand ??= new RelayCommand(parameter =>
        {
            Cell clickedCell = (Cell)parameter;
            if (selectedCell == null)
            {
                if (!Chess.PossibleMoves.TryGetValue(clickedCell, out List<Move> moves)) return;
                clickedCell.Selected = true;
                selectedCell = clickedCell;
                foreach (Move move in moves)
                {
                    Chess.Board[move.To].Move = move;
                }
                //Chess.MarkMoves(clickedCell);
            }
            else
            {
                if (clickedCell.Highlighted && selectedCell.Piece.Color == Chess.CurrentParty)
                {
                    Chess.Play(clickedCell.Move);
                }
                selectedCell.Selected = false;
                selectedCell = null;
                foreach (Cell c in Chess.Board) c.Move = null;
            }
        }, parameter =>
        {
            if (parameter is not Cell cell) return false;
            if (selectedCell == null && cell.Piece.Color != Chess.CurrentParty) return false;
            return (selectedCell != null || cell.Piece.Type != PieceType.Empty);
        });

        private void SetupBoard()
        {
            Board board = new Board(8);
            board[0, 7].Piece = new Piece(PieceValue.Rook, Party.White);
            board[1, 7].Piece = new Piece(PieceValue.Knight, Party.White);
            board[2, 7].Piece = new Piece(PieceValue.Bishop, Party.White);
            board[3, 7].Piece = new Piece(PieceValue.Queen, Party.White);
            board[4, 7].Piece = new Piece(PieceValue.King, Party.White);
            board[5, 7].Piece = new Piece(PieceValue.Bishop, Party.White);
            board[6, 7].Piece = new Piece(PieceValue.Knight, Party.White);
            board[7, 7].Piece = new Piece(PieceValue.Rook, Party.White);  
            for (int i = 0; i < 8; i++)
            {
                board[i, 6].Piece = new Piece(PieceValue.Pawn, Party.White);
                board[i, 1].Piece = new Piece(PieceValue.Pawn, Party.Black);
            }
            board[0, 0].Piece = new Piece(PieceValue.Rook, Party.Black);
            board[1, 0].Piece = new Piece(PieceValue.Knight, Party.Black);
            board[2, 0].Piece = new Piece(PieceValue.Bishop, Party.Black);
            board[3, 0].Piece = new Piece(PieceValue.Queen, Party.Black);
            board[4, 0].Piece = new Piece(PieceValue.King, Party.Black);
            board[5, 0].Piece = new Piece(PieceValue.Bishop, Party.Black);
            board[6, 0].Piece = new Piece(PieceValue.Knight, Party.Black);
            board[7, 0].Piece = new Piece(PieceValue.Rook, Party.Black);
            Chess.Board = board;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}