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
            X = x;
            Y = y;
            Position = new Point(x, y);
            Color = (x + y) % 2;
            Piece = Piece.Empty;
        }

        public int X { get; }
        public int Y { get; }
        public Point Position { get; }
        public int Color { get; }

        public Piece Piece
        {
            get => _piece;
            set
            {
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
            }
        }

        public bool Highlighted => Move != null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}