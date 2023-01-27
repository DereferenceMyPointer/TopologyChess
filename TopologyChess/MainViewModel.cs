﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
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
            TopologyModel.Transform(Mesh, (Point p) => Equations.Bicone(p.X, p.Y));
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
            Cell clickedCell = (Cell)parameter;
            Cell selectedCell = Board.FirstOrDefault(x => x.Selected);
            if (selectedCell == null)
            {
                clickedCell.Selected = true;
                foreach (var m in Board.TestMoves(clickedCell))
                {
                    m.To.Highlighted = true;
                }
            }
            else
            {
                if (clickedCell.Highlighted)
                {
                    clickedCell.Piece = selectedCell.Piece;
                    selectedCell.Piece = Piece.Empty;
                }
                selectedCell.Selected = false;
                foreach (Cell c in Board) c.Highlighted = false;
            }
        }, parameter => parameter is Cell cell && (Board.Any(x => x.Selected) || cell.Piece.Type != PieceType.Empty));

        private void SetupBoard()
        {
            Board board = new Board();
            board[0, 0].Piece = new Piece(PieceValue.Rook, Party.White);
            board[1, 0].Piece = new Piece(PieceValue.Knight, Party.White);
            board[2, 0].Piece = new Piece(PieceValue.Bishop, Party.White);
            board[3, 0].Piece = new Piece(PieceValue.Queen, Party.White);
            board[4, 0].Piece = new Piece(PieceValue.King, Party.White);
            board[5, 0].Piece = new Piece(PieceValue.Bishop, Party.White);
            board[6, 0].Piece = new Piece(PieceValue.Knight, Party.White);
            board[7, 0].Piece = new Piece(PieceValue.Rook, Party.White);  
            for (int i = 0; i < 8; i++)
            {
                board[i, 1].Piece = new Piece(PieceValue.Pawn, Party.White);
                board[i, 6].Piece = new Piece(PieceValue.Pawn, Party.Black);
            }
            board[0, 7].Piece = new Piece(PieceValue.Rook, Party.Black);
            board[1, 7].Piece = new Piece(PieceValue.Knight, Party.Black);
            board[2, 7].Piece = new Piece(PieceValue.Bishop, Party.Black);
            board[3, 7].Piece = new Piece(PieceValue.Queen, Party.Black);
            board[4, 7].Piece = new Piece(PieceValue.King, Party.Black);
            board[5, 7].Piece = new Piece(PieceValue.Bishop, Party.Black);
            board[6, 7].Piece = new Piece(PieceValue.Knight, Party.Black);
            board[7, 7].Piece = new Piece(PieceValue.Rook, Party.Black);
            Board = board;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}