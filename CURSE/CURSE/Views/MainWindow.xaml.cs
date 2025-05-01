using Microsoft.Win32;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CURSE.ViewModels;

namespace CURSE
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Drag(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public void X(object sender, EventArgs e)
        {
            this.Close();
        }

        public void _(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
       
    }

}