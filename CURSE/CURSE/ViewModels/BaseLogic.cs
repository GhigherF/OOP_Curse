using System;
using System.Runtime.InteropServices;
using System.Windows;       // Для WPF-элементов
using Microsoft.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
using System.Runtime.CompilerServices;

namespace CURSE.ViewModels
{
    public class CanvasDragBehavior : Behavior<FrameworkElement>
    {
        private bool isDragging;
        private Point clickOffset;
        private double initialLeft;
        private double initialTop;
        private ScrollViewer? scrollViewer;
        private Canvas? parentCanvas => AssociatedObject.Parent as Canvas;

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
            if (parentCanvas == null) return;

            isDragging = true;
            AssociatedObject.CaptureMouse();
            clickOffset = e.GetPosition(AssociatedObject);
            initialLeft = Canvas.GetLeft(AssociatedObject);
            initialTop = Canvas.GetTop(AssociatedObject);
            Mouse.OverrideCursor = Cursors.None;
            e.Handled = true;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            AssociatedObject.ReleaseMouseCapture();
            Mouse.OverrideCursor = null;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging || parentCanvas == null) return;

            if (scrollViewer == null)
                scrollViewer = FindScrollViewer(parentCanvas);

            Point mousePositionOnCanvas = e.GetPosition(parentCanvas);
            double elementWidth = AssociatedObject.ActualWidth;
            double elementHeight = AssociatedObject.ActualHeight;

            // Новые координаты относительно Canvas
            double newLeft = mousePositionOnCanvas.X - clickOffset.X;
            double newTop = mousePositionOnCanvas.Y - clickOffset.Y;

            // Ограничения прокрутки
            double scrollMargin = 5;

            if (scrollViewer != null)
            {
                Point posInScroll = e.GetPosition(scrollViewer);

                // Горизонтальная прокрутка
                if (posInScroll.X > scrollViewer.ViewportWidth - scrollMargin)
                    scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + scrollMargin);
                else if (posInScroll.X < scrollMargin)
                    scrollViewer.ScrollToHorizontalOffset(Math.Max(0, scrollViewer.HorizontalOffset - scrollMargin));

                // Вертикальная прокрутка
                if (posInScroll.Y > scrollViewer.ViewportHeight - scrollMargin)
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + scrollMargin);
                else if (posInScroll.Y < scrollMargin)
                    scrollViewer.ScrollToVerticalOffset(Math.Max(0, scrollViewer.VerticalOffset - scrollMargin));

                // ⛔ Ограничения: не выходить за правый/нижний край видимой области
                double maxLeft = scrollViewer.HorizontalOffset + scrollViewer.ViewportWidth - elementWidth;
                double maxTop = scrollViewer.VerticalOffset + scrollViewer.ViewportHeight - elementHeight;

                newLeft = Math.Min(newLeft, maxLeft);
                newTop = Math.Min(newTop, maxTop);
            }

            // Ограничение сверху и слева
            newLeft = Math.Max(0, newLeft);
            newTop = Math.Max(0, newTop);

            Canvas.SetLeft(AssociatedObject, newLeft);
            Canvas.SetTop(AssociatedObject, newTop);
        }



        private ScrollViewer? FindScrollViewer(DependencyObject obj)
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

    public class DisableParentScrollOnFocusBehavior : Behavior<RichTextBox>
    {
        private ScrollViewer _scrollViewer;
        private bool _isScrollViewerCached;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnRichTextBoxLoaded;
            AssociatedObject.PreviewMouseWheel += OnPreviewMouseWheel;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= OnRichTextBoxLoaded;
            AssociatedObject.PreviewMouseWheel -= OnPreviewMouseWheel;
        }

        private void OnRichTextBoxLoaded(object sender, RoutedEventArgs e)
        {
            // Кешируем ScrollViewer один раз при загрузке
            _scrollViewer = FindScrollViewer(AssociatedObject);
            _isScrollViewerCached = true;
        }

        private ScrollViewer FindScrollViewer(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is ScrollViewer sv) return sv;
                var result = FindScrollViewer(child);
                if (result != null) return result;
            }
            return null;
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!_isScrollViewerCached || _scrollViewer == null) return;

            var isScrollingUp = e.Delta > 0;
            var canScroll = isScrollingUp
                ? _scrollViewer.VerticalOffset > 0
                : _scrollViewer.VerticalOffset < _scrollViewer.ExtentHeight - _scrollViewer.ViewportHeight;

            // Если можем скроллить - позволяем работать нативному механизму
            if (canScroll) return;

            // Если достигли границы - блокируем всплытие
            e.Handled = true;
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
            var rtb = AssociatedObject;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                rtb.CaretPosition.InsertLineBreak(); // Вставляет LineBreak
            }
            else if (e.Key == Key.Tab)
            {
                e.Handled = true;
                rtb.CaretPosition.InsertTextInRun("    "); // Вставляет 4 пробела
            }
        }
    }











    public class BaseViewModel : INotifyPropertyChanged
    {
        public class Note : INotifyPropertyChanged
        {
            public string Title { get; set; }

            private double _x;
            public double X
            {
                get => _x;
                set
                {
                    if (_x != value)
                    {
                        _x = value;
                        OnPropertyChanged(nameof(X));
                    }
                }
            }

            private double _y;
            public double Y
            {
                get => _y;
                set
                {
                    if (_y != value)
                    {
                        _y = value;
                        OnPropertyChanged(nameof(Y));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private readonly Window _window;
        private ScrollViewer? _scrollViewer;

        public ObservableCollection<Note> Notes { get; }
        public ICommand AddItemCommand { get; }
        public ICommand ToggleBoldCommand { get; }
        public ICommand NoteKeyDownCommand { get; }

        public BaseViewModel(Window window, ScrollViewer? scrollViewer)
        {
            _window = window;
            _scrollViewer = scrollViewer;
            Notes = new ObservableCollection<Note>();
            AddItemCommand = new RelayCommand(AddNote);
            ToggleBoldCommand = new RelayCommand(ToggleBold);
        }

        private void AddNote()
        {
            if (_scrollViewer == null) return;

            // Получаем текущие параметры прокрутки
            double horizontalOffset = _scrollViewer.HorizontalOffset;
            double verticalOffset = _scrollViewer.VerticalOffset;
            double viewportWidth = _scrollViewer.ViewportWidth;
            double viewportHeight = _scrollViewer.ViewportHeight;

            // Рассчитываем центр видимой области
            double centerX = horizontalOffset + (viewportWidth / 2) - 100; // 100 - половина ширины заметки
            double centerY = verticalOffset + (viewportHeight / 2) - 90;   // 90 - половина высоты заметки

            // Создаем новую заметку и добавляем её в коллекцию
            var newNote = new Note
            {
                Title = $"Новая заметка {Notes.Count + 1}",
                X = centerX,
                Y = centerY
            };

            Notes.Add(newNote);
        }




        private void ToggleBold()
        {
            if (Keyboard.FocusedElement is RichTextBox rtb)
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

                rtb.Focus(); // Обновляет фокус, если нужно
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}