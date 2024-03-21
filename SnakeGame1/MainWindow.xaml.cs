using SnakeGame1.ViewModels;
using System.Windows;


namespace SnakeGame1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowVM(this);
        }
    }
}