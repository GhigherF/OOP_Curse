using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using CURSE.ViewModels;

namespace CURSE.Views
{
    public partial class Base : Window
    {
        public Base(string NickName)
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                // Убедитесь, что ScrollViewer инициализирован после загрузки
                DataContext = new BaseViewModel(this,NickName);
            };
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
