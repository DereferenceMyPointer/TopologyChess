using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TopologyChess
{
    public partial class PieceSelect : UserControl
    {
        public PieceSelect(Party type, Cell target)
        {
            Type = type;
            TargetCell = target;
            Cells = new List<Cell>();
            List<PieceValue> values;
            List<Party> colors;
            if (type == Party.None)
            {
                values = new()
                {
                    PieceValue.Pawn, PieceValue.Knight, PieceValue.Bishop,
                    PieceValue.Rook, PieceValue.Queen, PieceValue.King,
                    PieceValue.None
                };
                colors = new() { Party.White, Party.Black };
            } else {
                values = new()
                {
                    PieceValue.Queen, PieceValue.Knight,
                    PieceValue.Rook, PieceValue.Bishop,
                    PieceValue.Pawn
                };
                colors = new() { type };
            }

            UGRows = values.Count;
            UGColumns = colors.Count;
            int i = 0, j;
            foreach (PieceValue value in values)
            {
                j = 0;
                foreach (Party color in colors)
                {
                    Cells.Add(new Cell(i, j)
                    {
                        Piece = value == PieceValue.None ? Piece.Empty : Piece.New(value, color)
                    });
                    j++;
                }
                i++;
            }
            InitializeComponent();
        }

        private Party Type { get; }

        public int UGColumns { get; }
        public int UGRows { get; }

        private static readonly DependencyProperty CellsProperty =
            DependencyProperty.Register(nameof(Cells), typeof(List<Cell>), typeof(PieceSelect));
        
        private List<Cell> Cells
        {
            get => (List<Cell>)GetValue(CellsProperty);
            set => SetValue(CellsProperty, value);
        }

        public Cell TargetCell { get; }

        public bool IsClosed { get; set; } = false;

        public Piece SelectedPiece { get; set; }

        public event Action<Piece> Selected;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectedPiece = ((Cell)((Button)sender).DataContext).Piece;
            if (Type == Party.None) TargetCell.Piece = SelectedPiece;
            else
            {
                TargetCell.Piece.Value = SelectedPiece.Value;
                TargetCell.Piece.Color = SelectedPiece.Color;
                TargetCell.OnPropertyChanged(nameof(TargetCell.Piece));
            }
            Selected?.Invoke(SelectedPiece);
            Selected = null;
            Close();
        }

        public void Show()
        {
            popup.IsOpen = true;
            Game.Instance.IsBlocked = true;
            popup.CaptureMouse();
        }

        public void Close()
        {
            popup.IsOpen = false;
            Game.Instance.IsBlocked = false;
            popup.ReleaseMouseCapture();
            IsClosed = true;
        }
    }
}
