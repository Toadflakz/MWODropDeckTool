using System.Windows.Controls;
using ExtendedGrid.ExtendedGridControl;

namespace MwoCWDropDeckBuilder.Views
{
    /// <summary>
    /// Interaction logic for DropDeckCreatorView.xaml
    /// </summary>
    public partial class DropDeckCreatorView : UserControl
    {
        public DropDeckCreatorView()
        {
            InitializeComponent();
            ExtendedDataGrid.Theme = ExtendedDataGrid.Themes.Media;
        }
    }
}
