using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                        Piece = value == PieceValue.None ? Piece.Empty : new Piece(value, color)
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

        public event Action<Piece> Selected;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Piece selectedPiece = ((Cell)((Button)sender).DataContext).Piece;
            if (Type == Party.None) TargetCell.Piece = selectedPiece;
            else
            {
                TargetCell.Piece.Value = selectedPiece.Value;
                TargetCell.Piece.Color = selectedPiece.Color;
                TargetCell.OnPropertyChanged(nameof(TargetCell.Piece));
                //MessageBox.Show(nameof(TargetCell.Piece));
            }
            Selected?.Invoke(selectedPiece);
            Selected = null;
            Close();
        }

        public void Show()
        {
            popup.IsOpen = true;
            popup.CaptureMouse();
        }

        public void Close()
        {
            popup.IsOpen = false;
            popup.ReleaseMouseCapture();
            IsClosed = true;
        }
    }
}

//grid.Columns = Colors.Count;
//grid.Rows = Values.Count;
/*int i = 0, j;
foreach (PieceValue value in Values)
{
    j = 0;
    foreach (Party color in Colors)
    {
        CellControl cellControl = new CellControl()
        {
            Cell = new Cell(i, j)
            {
                Piece = new Piece(value, color)
            }
        };
        Button button = new Button()
        {
            OverridesDefaultStyle = true
        };
        FrameworkElementFactory factory = new FrameworkElementFactory();
        button.Click += (sender, e) =>
        {
            Selected = cellControl.Cell.Piece;
            Selected?.Invoke(this, e);
        };
        grid.Children.Add(cellControl);
        j++;
    }
    i++;
}*/