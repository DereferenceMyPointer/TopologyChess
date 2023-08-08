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
    public partial class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Topologies = new ObservableCollection<Topology>(Topology.Topologies);
        }

        private ICommand _newGameCommand;
        private ICommand _clearCommand;
        private ICommand _cellCommand;

        public ObservableCollection<Topology> Topologies { get; set; }

        public Game Chess => Game.Instance;

        private double angle = 0;
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
            Game.NewGame();
            OnPropertyChanged(nameof(Chess));
        });

        public ICommand ClearCommand => _clearCommand ??= new RelayCommand(parameter =>
        {
            //Chess.Board = new Board(8);
            //PerspAngle = (PerspAngle + 180) % 360;
        });

        private ICommand _randomTopologyCommand;
        public ICommand RandomTopologyCommand => _randomTopologyCommand ??= new RelayCommand(parameter =>
        {
            /*Move desiredMove = Move.BoardTransformation.RandomBoardTransformation(Chess);
            if (Chess != null && Chess.canTopologyMove(desiredMove.TopologyChange))
                Chess.Play(Move.BoardTransformation.RandomBoardTransformation(Chess));
            else
                MessageBox.Show("Attempted Illegal Topology");*/
        });

        private Cell selectedCell;
        public ICommand CellCommand => _cellCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is not Cell cell) return;
            if (!Chess.AvailibleParties.Contains(Chess.CurrentParty)) return;
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
                    move.To.Move = move;
                }
                //Chess.MarkMoves(clickedCell);
            }
            else
            {
                if (clickedCell.Highlighted && selectedCell.Piece.Color == Chess.CurrentParty)
                {
                    clickedCell.Move.Do();
                    AttemptedMove = clickedCell.Move;
                }
                selectedCell.Selected = false;
                selectedCell = null;
                foreach (Cell c in Chess.Board) c.Move = null;
            }
        }, parameter => !Chess.IsBlocked && AttemptedMove == null);

        private IMove AttemptedMove { get; set; }

        private ICommand _undoCommand;
        public ICommand UndoCommand => _undoCommand ??= new RelayCommand(parameter =>
        {
            AttemptedMove.Undo();
            AttemptedMove = null;
        }, parameter => AttemptedMove != null);

        private ICommand _submitCommand;
        public ICommand SubmitCommand => _submitCommand ??= new RelayCommand(parameter =>
        {
            Chess.Submit(AttemptedMove);
            AttemptedMove = null;
        }, parameter => AttemptedMove != null);

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