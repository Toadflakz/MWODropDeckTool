using System.Windows;
using Microsoft.Practices.ServiceLocation;
using MwoCWDropDeckBuilder.ViewModel;

namespace MwoCWDropDeckBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel MainWindowViewModel
        {
            get { return (MainWindowViewModel)GetValue(MainWindowViewModelProperty); }
            set { SetValue(MainWindowViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainWindowViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainWindowViewModelProperty =
            DependencyProperty.Register("MainWindowViewModel", typeof(MainWindowViewModel), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel = ServiceLocator.Current.GetInstance<MainWindowViewModel>();
        }
    }
}
