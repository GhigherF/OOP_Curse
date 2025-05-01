using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;
using System.Windows.Documents;
using System.ComponentModel.DataAnnotations;

namespace CURSE.ViewModels
{
    ///Behaviors
    public class CanvasDragBehavior : Behavior<FrameworkElement>
    {
        private bool isDragging = false;
        private Point clickOffset; // позиция относительно самого элемента
        private double initialLeft;
        private ScrollViewer? scrollViewer;
        private double initialTop;

        private Canvas parentCanvas => AssociatedObject.Parent as Canvas;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;
            AssociatedObject.MouseMove += OnMouseMove;
            AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseLeftButtonDown -= OnMouseLeftButtonDown;
            AssociatedObject.MouseMove -= OnMouseMove;
            AssociatedObject.MouseLeftButtonUp -= OnMouseLeftButtonUp;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (parentCanvas == null)
                return;

            isDragging = true;
            AssociatedObject.CaptureMouse();

            // Сохраняем смещение мыши относительно элемента (внутри него)
            clickOffset = e.GetPosition(AssociatedObject);

            // Сохраняем начальные координаты
            initialLeft = Canvas.GetLeft(AssociatedObject);
            initialTop = Canvas.GetTop(AssociatedObject);

            // Скрываем курсор
            Mouse.OverrideCursor = Cursors.None;

            e.Handled = true;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            AssociatedObject.ReleaseMouseCapture();

            // Возвращаем курсор
            Mouse.OverrideCursor = null;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging || parentCanvas == null)
                return;

            Point mousePositionOnCanvas = e.GetPosition(parentCanvas);

            double newLeft = mousePositionOnCanvas.X - clickOffset.X;
            double newTop = mousePositionOnCanvas.Y - clickOffset.Y;

            double elementWidth = AssociatedObject.ActualWidth;
            double elementHeight = AssociatedObject.ActualHeight;

            // Получаем окно
            var window = Window.GetWindow(AssociatedObject);
            if (window == null) return;

            // Получаем ScrollViewer
            if (scrollViewer == null)
                scrollViewer = FindScrollViewer(parentCanvas);

            double visibleWidth = window.ActualWidth;
            double visibleHeight = window.ActualHeight;

            if (scrollViewer != null)
            {
                visibleWidth = scrollViewer.ViewportWidth;
                visibleHeight = scrollViewer.ViewportHeight;
            }

            // Ограничение по видимой области (нужно для предотвращения выхода элемента за экран)
            newLeft = Math.Max(0, newLeft);
            newTop = Math.Max(0, newTop);

            if (newLeft + elementWidth > visibleWidth)
                newLeft = visibleWidth - elementWidth;

            // Чтобы не вылезать вниз
            if (newTop + elementHeight > visibleHeight)
                newTop = visibleHeight - elementHeight;

            // Устанавливаем позицию элемента
            Canvas.SetLeft(AssociatedObject, newLeft);
            Canvas.SetTop(AssociatedObject, newTop);

            // Прокрутка содержимого (если нужно)
            if (scrollViewer != null)
            {
                double offsetX = scrollViewer.HorizontalOffset;
                double offsetY = scrollViewer.VerticalOffset;
                double viewportWidth = scrollViewer.ViewportWidth;
                double viewportHeight = scrollViewer.ViewportHeight;

                // Прокрутка по горизонтали
                if (newLeft < offsetX)
                    scrollViewer.ScrollToHorizontalOffset(newLeft);
                else if (newLeft + elementWidth > offsetX + viewportWidth)
                    scrollViewer.ScrollToHorizontalOffset(newLeft + elementWidth - viewportWidth);

                // Прокрутка по вертикали
                if (newTop < offsetY)
                    scrollViewer.ScrollToVerticalOffset(newTop);
                else if (newTop + elementHeight > offsetY + viewportHeight)
                    scrollViewer.ScrollToVerticalOffset(newTop + elementHeight - viewportHeight);
            }
        }



        private ScrollViewer FindScrollViewer(DependencyObject obj)
        {
            while (obj != null)
            {
                if (obj is ScrollViewer sv)
                    return sv;

                obj = VisualTreeHelper.GetParent(obj);
            }

            return null;
        }
    }
    public class RichTextBoxKeyBehavior : Behavior<RichTextBox>
    {
        public static readonly DependencyProperty KeyCommandProperty =
            DependencyProperty.Register(nameof(KeyCommand), typeof(ICommand), typeof(RichTextBoxKeyBehavior));

        public ICommand KeyCommand
        {
            get => (ICommand)GetValue(KeyCommandProperty);
            set => SetValue(KeyCommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += OnKeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.KeyDown -= OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (KeyCommand?.CanExecute(e) == true)
            {
                KeyCommand.Execute(e);
            }
        }
    }
    public class RightClickPopupBehavior : Behavior<Border>
    {
        private Popup? _popup;
        private RichTextBox? _richTextBox;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseRightButtonUp += OnMouseRightButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseRightButtonUp -= OnMouseRightButtonUp;
        }

        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_popup == null || _richTextBox == null)
            {
                if (AssociatedObject.Child is Grid grid)
                {
                    _popup ??= FindChild<Popup>(grid);
                    _richTextBox ??= FindChild<RichTextBox>(grid);
                }
            }

            if (_popup == null)
                return;

            // Получаем позицию курсора в экранных координатах
            Point screenPos = AssociatedObject.PointToScreen(e.GetPosition(AssociatedObject));

            // Получаем окно и границы экрана
            Window window = Application.Current.MainWindow;
            Point windowTopLeft = window.PointToScreen(new Point(0, 0));
            double screenRight = windowTopLeft.X + window.ActualWidth;
            double screenBottom = windowTopLeft.Y + window.ActualHeight;

            // Получаем размеры popup
            _popup.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Size popupSize = _popup.DesiredSize;

            // Корректируем позицию, чтобы popup не выходил за экран
            double adjustedX = screenPos.X;
            double adjustedY = screenPos.Y;

            if (adjustedX + popupSize.Width > screenRight)
                adjustedX = screenRight - popupSize.Width;
            if (adjustedY + popupSize.Height > screenBottom)
                adjustedY = screenBottom - popupSize.Height;

            adjustedX = Math.Max(adjustedX, windowTopLeft.X);
            adjustedY = Math.Max(adjustedY, windowTopLeft.Y);

            // Открываем popup в нужной позиции
            _popup.Placement = PlacementMode.AbsolutePoint;
            _popup.HorizontalOffset = adjustedX;
            _popup.VerticalOffset = adjustedY;
            _popup.IsOpen = true;
        }

        private T? FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T found)
                    return found;

                var result = FindChild<T>(child);
                if (result != null)
                    return result;
            }

            return null;
        }
    }





    //main
    public class BaseViewModel : INotifyPropertyChanged
    {
       
        public ICommand ToggleBoldCommand { get; }
            public ICommand NoteKeyDownCommand { get; }

            public BaseViewModel()
            {
                ToggleBoldCommand = new RelayCommand<object>(ToggleBold);
                NoteKeyDownCommand = new RelayCommand<KeyEventArgs>(OnKeyDown);
            }

            private void ToggleBold(object obj)
            {
                if (obj is RichTextBox rtb)
                {
                    var selection = rtb.Selection;
                    if (!selection.IsEmpty)
                    {
                        var weight = selection.GetPropertyValue(TextElement.FontWeightProperty);
                        var newWeight = (weight is FontWeight w && w == FontWeights.Bold)
                            ? FontWeights.Normal
                            : FontWeights.Bold;

                        selection.ApplyPropertyValue(TextElement.FontWeightProperty, newWeight);
                    }
                    rtb.Focus();
                }
            }

            private void OnKeyDown(KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    MessageBox.Show("Нажата Enter");
                    e.Handled = true;
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;
            protected void OnPropertyChanged(string name) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    
}
