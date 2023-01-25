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

namespace TopologyChess.Controls
{
    public partial class PieceControl : UserControl
    {
        public static readonly DependencyProperty PieceProperty = DependencyProperty.Register("PieceType", typeof(PieceType), typeof(PieceControl));

        public PieceType PieceType
        {
            get => (PieceType)GetValue(PieceProperty);
            set => SetValue(PieceProperty, value);
        }

        public PieceControl()
        {
            InitializeComponent();
        }
    }
}
