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
    public partial class GlueArrow : UserControl
    {
        public GlueArrow()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ArrowBrushProperty = 
            DependencyProperty.Register(nameof(ArrowBrush), typeof(Brush), typeof(GlueArrow));

        public Brush ArrowBrush
        {
            get => (Brush)GetValue(ArrowBrushProperty);
            set => SetValue(ArrowBrushProperty, value);
        }
    }
}
