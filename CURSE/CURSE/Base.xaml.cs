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
using System.Windows.Shapes;

namespace CURSE
{
    /// <summary>
    /// Логика взаимодействия для Base.xaml
    /// </summary>
    public partial class Base : Window
    {
        private bool isDragging = false;
        private Point clickPosition;
        public Base()
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


        private void LeftClick(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            ((UIElement)sender).CaptureMouse();
            clickPosition = e.GetPosition(Note);
        }

        private void LeftUnclick(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            ((UIElement)sender).ReleaseMouseCapture();
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentPosition = e.GetPosition(Note);
                double offsetX = currentPosition.X - clickPosition.X;
                double offsetY = currentPosition.Y - clickPosition.Y;

                double currentLeft = Canvas.GetLeft((Border)sender);
                double currentTop = Canvas.GetTop((Border)sender);

                double newLeft = currentLeft + offsetX;
                double newTop = currentTop + offsetY;

                double canvasWidth = Note.ActualWidth;
                double canvasHeight = Note.ActualHeight;
                double elementWidth = ((Border)sender).ActualWidth;
                double elementHeight = ((Border)sender).ActualHeight;

                // Ограничение по горизонтали
                if (newLeft < 0)
                    newLeft = 0;
                if (newLeft + elementWidth > canvasWidth)
                    newLeft = canvasWidth - elementWidth;

                // Ограничение по вертикали
                if (newTop < 0)
                    newTop = 0;

                // Проверяем, если Border перемещается вниз за пределы текущей высоты Canvas
                if (newTop + elementHeight > canvasHeight)
                {
                    // Увеличиваем высоту Canvas и ScrollViewer
                    double newScrollHeight = newTop + elementHeight + 50; // Добавляем 50 для запаса
                    if (newScrollHeight > MainScrollViewer.ViewportHeight)
                    {
                        Note.Height = newScrollHeight;
                        
                    }
                }

                // Обновляем позицию Border
                Canvas.SetLeft((Border)sender, newLeft);
                Canvas.SetTop((Border)sender, newTop);

                // Обновляем позицию TextBox, чтобы он следовал за Border
                Canvas.SetLeft(MovableTextBox, newLeft);
                Canvas.SetTop(MovableTextBox, newTop + elementHeight);

                // Обновляем позицию клика
                clickPosition = currentPosition;
            }
        }
    }
}
