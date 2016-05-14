using System.Windows.Controls;
using ExtendedGrid.ExtendedGridControl;

namespace MwoCWDropDeckBuilder.Views
{
    /// <summary>
    /// Interaction logic for BuildListView.xaml
    /// </summary>
    public partial class BuildListView : UserControl
    {
        public BuildListView()
        {
            InitializeComponent();
            BuildExtendedGrid.Theme = ExtendedDataGrid.Themes.Media;
        }
    }
}
