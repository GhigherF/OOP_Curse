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

        public Base()
        { 
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                // Инициализируем ViewModel после загрузки окна
                DataContext = new BaseViewModel(this, MainScrollViewer);
            };
           

          
        }
        private readonly BaseViewModel _viewModel;


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
        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb thumb && thumb.Parent is Grid container)
            {
                const double minWidth = 200;
                const double minHeight = 180;

                double newWidth = container.Width + e.HorizontalChange;
                double newHeight = container.Height + e.VerticalChange;

                // Ограничения по минимальным размерам
                container.Width = Math.Max(newWidth, minWidth);
                container.Height = Math.Max(newHeight, minHeight);
            }
        }
        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer sv)
            {
                const double scrollStep = 40; // Меньше — медленнее
                double newOffset = sv.VerticalOffset - Math.Sign(e.Delta) * scrollStep;

                newOffset = Math.Max(0, Math.Min(newOffset, sv.ExtentHeight - sv.ViewportHeight));
                sv.ScrollToVerticalOffset(newOffset);
                e.Handled = true; // блокируем стандартную прокрутку
            }
        }


    }
}
