using Note.ViewModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Note.View
{
    public partial class MainWindow : Window
    {
   
        public MainWindow()
        {
            InitializeComponent();
            // Связывание View и ViewModel
            DataContext = new MainWindowViewModel();
        }
        
    }
}
