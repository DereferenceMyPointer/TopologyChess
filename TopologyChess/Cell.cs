using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TopologyChess
{
    public class Cell : INotifyPropertyChanged
    {
        private Piece _piece = null;
        private bool _selected;
        private bool _highlighted;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            Color = (x + y) % 2;
            Piece = Piece.Empty;
        }

        public int X { get; }
        public int Y { get; }
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

        public bool Highlighted
        {
            get => _highlighted;
            set
            {
                _highlighted = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}