using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace TopologyChess
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            TopologyModel.Transform(Mesh, (Point p) => Equations.Globe(p.X, p.Y));
            BorderPoints = TopologyModel.GetBorder(Mesh);
        }
        
        private Board _board = new Board();
        private MeshGeometry3D _mesh = TopologyModel.GenerateLattice(8 * 20);
        private Point3DCollection _border_points = new Point3DCollection();

        private ICommand _newGameCommand;
        private ICommand _clearCommand;
        private ICommand _cellCommand;

        public IEnumerable<char> Numbers => "87654321";
        public IEnumerable<char> Letters => "ABCDEFGH";

        public Board Board 
        {
            get => _board;
            set
            {
                _board = value;
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

        public ICommand NewGameCommand => _newGameCommand ??= new RelayCommand(parameter => 
        {
            SetupBoard();
        });

        public ICommand ClearCommand => _clearCommand ??= new RelayCommand(parameter =>
        {
            Board = new Board();
        });

        public ICommand CellCommand => _cellCommand ??= new RelayCommand(parameter =>
        {
            Cell cell = (Cell)parameter;
            Cell selectedCell = Board.FirstOrDefault(x => x.Selected);
            if (selectedCell == null || selectedCell == cell)
            {
                cell.Selected = !cell.Selected;
            }
            else
            {
                selectedCell.Selected = false;
                cell.Piece = selectedCell.Piece;
                selectedCell.Piece = null;
            }
        }, parameter => parameter is Cell cell && (Board.Any(x => x.Selected) || cell.PieceType != PieceType.Empty));

        private void SetupBoard()
        {
            Board board = new Board();
            board[0, 0].Piece = new Piece(PieceValue.Rook, PieceColor.White);
            board[1, 0].Piece = new Piece(PieceValue.Knight, PieceColor.White);
            board[2, 0].Piece = new Piece(PieceValue.Bishop, PieceColor.White);
            board[3, 0].Piece = new Piece(PieceValue.Queen, PieceColor.White);
            board[4, 0].Piece = new Piece(PieceValue.King, PieceColor.White);
            board[5, 0].Piece = new Piece(PieceValue.Bishop, PieceColor.White);
            board[6, 0].Piece = new Piece(PieceValue.Knight, PieceColor.White);
            board[7, 0].Piece = new Piece(PieceValue.Rook, PieceColor.White);  
            for (int i = 0; i < 8; i++)
            {
                board[i, 1].Piece = new Piece(PieceValue.Pawn, PieceColor.White);
                board[i, 6].Piece = new Piece(PieceValue.Pawn, PieceColor.Black);
            }
            board[0, 7].Piece = new Piece(PieceValue.Pawn, PieceColor.Black);
            board[1, 7].Piece = new Piece(PieceValue.Knight, PieceColor.Black);
            board[2, 7].Piece = new Piece(PieceValue.Bishop, PieceColor.Black);
            board[3, 7].Piece = new Piece(PieceValue.Queen, PieceColor.Black);
            board[4, 7].Piece = new Piece(PieceValue.King, PieceColor.Black);
            board[5, 7].Piece = new Piece(PieceValue.Bishop, PieceColor.Black);
            board[6, 7].Piece = new Piece(PieceValue.Knight, PieceColor.Black);
            board[7, 7].Piece = new Piece(PieceValue.Pawn, PieceColor.Black);
            Board = board;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}