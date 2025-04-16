using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
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
       public partial class Base : Window
    {
        private bool isDragging = false;
        private Point clickPosition;
        private ViewModel _viewModel;
        public Base()
        {
            InitializeComponent();
            _viewModel = new ViewModel();
            DataContext = _viewModel;
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
        //public void Func(object sender, EventArgs e)    
        //{
        //    if (DataContext is ViewModel viewModel)
        //    {
        //        viewModel.textRedactor.Execute(null);
        //    }
        //}


        public void TextRedactor(object sender, KeyEventArgs e)
        { 
       RichTextBox richTextBox = sender as RichTextBox;
            if (richTextBox == null)
                return;

            if (e.Key == Key.Enter)
            {
                TextPointer caretPosition = richTextBox.CaretPosition;
                TextRange range = new TextRange(caretPosition, caretPosition);
                range.Text = "\r\n"; // Используем \r\n для новой строки в RichTextBox
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                TextPointer caretPosition = richTextBox.CaretPosition;

                if (!caretPosition.GetPointerContext(LogicalDirection.Backward).Equals(TextPointerContext.Text))
                {
                    // Если перед курсором нет текста, ничего не делаем
                    return;
                }

                TextRange previousTextRange = new TextRange(caretPosition.GetPositionAtOffset(-1, LogicalDirection.Backward), caretPosition);
                if (previousTextRange.Text == "\r")
                {
                    // Если перед курсором находится символ новой строки, удаляем его
                    TextPointer start = caretPosition.GetPositionAtOffset(-1, LogicalDirection.Backward);
                    TextPointer end = caretPosition;
                    TextRange rangeToDelete = new TextRange(start, end);
                    rangeToDelete.Text = "";
                }
                else
                {
                    // Иначе, удаляем один символ перед курсором
                    TextPointer start = caretPosition.GetPositionAtOffset(-1, LogicalDirection.Backward);
                    TextPointer end = caretPosition;
                    TextRange rangeToDelete = new TextRange(start, end);
                    rangeToDelete.Text = "";
                }
            e.Handled = true;
            }
        }

        public void Bold(object sender,EventArgs e)
        {
            if (MovableTextBox.Selection.IsEmpty)
            {   
                return;
            }

            TextRange selectionRange = new TextRange(MovableTextBox.Selection.Start,MovableTextBox.Selection.End);

            // Проверяем, является ли текст уже жирным
            if (selectionRange.GetPropertyValue(TextElement.FontWeightProperty).ToString() == "Bold")
            {
                // Если жирный, то убираем жирность
                selectionRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            }
            else
            {
                // Если не жирный, то применяем жирность
                selectionRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
        }


        public class ViewModel:INotifyPropertyChanged
        {
            private object _currentView;
            public object CurrentView
            {
                get { return _currentView; }
                set
                {
                    _currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
            }
                 public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        
            public ViewModel()
            {
                //textRedactor = new RelayCommand(TextRedactor);
            }
     
            public ICommand textRedactor { get; private set; }
            
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

                if (newTop >2000)
                    newTop = 2000;

                // Проверяем, если Border перемещается вниз за пределы текущей высоты Canvas
                if (newTop + elementHeight > canvasHeight)
                {

                    double newScrollHeight=2350;
                    if (!(newTop + elementHeight > 2350)) newScrollHeight = newTop + elementHeight + 50; // Добавляем 50 для запаса
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

                // Автоматическая прокрутка ScrollViewer
                double scrollOffsetX = MainScrollViewer.HorizontalOffset;
                double scrollOffsetY = MainScrollViewer.VerticalOffset;

                double viewportWidth = MainScrollViewer.ViewportWidth;
                double viewportHeight = MainScrollViewer.ViewportHeight;

                // Проверяем, нужно ли прокручивать по горизонтали
                if (newLeft < scrollOffsetX)
                {
                    MainScrollViewer.ScrollToHorizontalOffset(newLeft);
                }
                else if (newLeft + elementWidth > scrollOffsetX + viewportWidth)
                {
                    MainScrollViewer.ScrollToHorizontalOffset(newLeft + elementWidth - viewportWidth);
                }

                // Проверяем, нужно ли прокручивать по вертикали
                if (newTop < scrollOffsetY)
                {
                    MainScrollViewer.ScrollToVerticalOffset(newTop);
                }
                else if (newTop + elementHeight > scrollOffsetY + viewportHeight)
                {
                    MainScrollViewer.ScrollToVerticalOffset(newTop + elementHeight - viewportHeight);
                }

                // Обновляем позицию клика
                clickPosition = currentPosition;
            }
        }
    }
}
