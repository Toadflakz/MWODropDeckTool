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
