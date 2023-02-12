using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace TopologyChess
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Topologies = new ObservableCollection<Topology>(Topology.Topologies);
        }

        private Game _chess = new Game();

        private ICommand _newGameCommand;
        private ICommand _clearCommand;
        private ICommand _cellCommand;

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

        private string hoverCellNotation = "";
        public string HoverCellNotation
        {
            get => hoverCellNotation;
            set
            {
                hoverCellNotation = value;
                OnPropertyChanged();
            }
        }

        public ICommand NewGameCommand => _newGameCommand ??= new RelayCommand(parameter => 
        {
            Chess = new Game();
            //SetupBoard();
            //Chess.DefaultSetup();
            //Chess.CurrentTopology = Topology.Topologies.FirstOrDefault(t => t.Name == "Moebius Horizontal");
        });

        public ICommand ClearCommand => _clearCommand ??= new RelayCommand(parameter =>
        {
            //Chess.Board = new Board(8);
            //PerspAngle = (PerspAngle + 180) % 360;
        });

        private Cell selectedCell;
        public ICommand CellCommand => _cellCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is not Cell cell) return;
            if (selectedCell == null && cell.Piece.Color != Chess.CurrentParty) return;
            if (!(selectedCell != null || cell.Piece.Type != PieceType.Empty)) return;

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
        }, parameter => !Chess.IsBlocked
        /*, parameter =>
        {
            if (parameter is not Cell cell) return false;
            if (selectedCell == null && cell.Piece.Color != Chess.CurrentParty) return false;
            return (selectedCell != null || cell.Piece.Type != PieceType.Empty);
        }*/);

        private ICommand _setPieceCommand;
        public ICommand SetPieceCommand => _setPieceCommand ??= new RelayCommand(parameter =>
        {
            Cell cell = (Cell)parameter;
            /*PieceSelect select = new PieceSelect(Party.None, cell);
            //select.Selected += (sender, e) => { };
            select.Show();*/
            MessageBox.Show(cell.Piece.Value.ToString());
        });

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}