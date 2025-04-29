using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace CURSE
{
    public static class ContextMenuHelper
    {
        public static T? GetPlacementTargetFromContextMenu<T>(object sender) where T : class
        {
            if (sender is DependencyObject depObj)
            {
                var contextMenu = FindVisualParent<ContextMenu>(depObj);
                return contextMenu?.PlacementTarget as T;
            }

            return null;
        }

        public static T? FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null && parent is not T)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as T;
        }
    }
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




    public partial class Base : Window
    {
        private BaseViewModel _viewModel;
        private void Bold(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var rtb = ContextMenuHelper.GetPlacementTargetFromContextMenu<RichTextBox>(sender);
            if (rtb != null && _viewModel is BaseViewModel vm)
            {
                vm.ToggleBoldCommand.Execute(rtb);
            }
        }


        public Base()
        {
            _viewModel = new BaseViewModel(this);
            InitializeComponent();
            DataContext = new BaseViewModel(this);
        }
    }

    public class BaseViewModel : INotifyPropertyChanged
    {
        private readonly Window _window;
        public ObservableCollection<string> Notes { get; set; }

        public ICommand AddItemCommand { get; }
        public ICommand DragCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand MinimizeCommand { get; }
        public ICommand ToggleBoldCommand { get; }
        public ICommand NoteKeyDownCommand { get; }


        private bool _isDragging;
        private Point _clickPosition;

        public BaseViewModel(Window window)
        {
            _window = window;
            Notes = new ObservableCollection<string>();
            AddItemCommand = new RelayCommand(AddNote);
            DragCommand = new RelayCommand(DragWindow);
            CloseCommand = new RelayCommand(() => _window.Close());
            MinimizeCommand = new RelayCommand(() => _window.WindowState = WindowState.Minimized);
            ToggleBoldCommand = new RelayCommand<object>(ToggleBold);
            NoteKeyDownCommand = new RelayCommand<KeyEventArgs>(OnKeyDown);
        }



        private void AddNote()
        {
            Notes.Add($"Новый элемент {Notes.Count + 1}");
        }

        private void DragWindow()
        {
            _window.DragMove();
        }

        private void OnKeyDown(KeyEventArgs e)
        {
            // Обработка клавиш Enter и Back
        }

        private void ToggleBold(object? parameter)
        {
            if (parameter is RichTextBox rtb)
            {
                var selection = rtb.Selection;
                if (!selection.IsEmpty)
                {
                    var currentWeight = selection.GetPropertyValue(TextElement.FontWeightProperty);
                    var newWeight = (currentWeight is FontWeight fw && fw == FontWeights.Bold)
                        ? FontWeights.Normal
                        : FontWeights.Bold;

                    selection.ApplyPropertyValue(TextElement.FontWeightProperty, newWeight);
                }

                rtb.Focus();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
