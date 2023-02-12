using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TopologyChess
{
    public class Cell : INotifyPropertyChanged
    {
        private Piece _piece = null;
        private bool _selected;

        public Cell(int x, int y)
        {
            Position = new IntVector(x, y);
            Color = (x + y) % 2;
            Piece = Piece.Empty;
        }

        public IntVector Position { get; }
        public int Color { get; }

        public Piece Piece
        {
            get => _piece;
            set
            {
                if (value.Type != PieceType.Empty) value.Position = Position;
                _piece = value;
                OnPropertyChanged();
            }
        }

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged();
            }
        }

        private Move _move;
        public Move Move
        {
            get => _move;
            set
            {
                _move = value;
                OnPropertyChanged(nameof(Highlighted));
                OnPropertyChanged(nameof(MoveAngle));
            }
        }

        public bool Highlighted => Move != null;
        public double MoveAngle => Move?.Path.Value.V.Angle ?? 0;

        public string Notation { get; set; }
        public override string ToString() => Notation;

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}