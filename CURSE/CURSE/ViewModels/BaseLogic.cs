using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
using CURSE.Views;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.IO;

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

            if (canScroll) return;
            e.Handled = true;
        }
    }

    public class DisableParentScrollOnHoverBehavior : Behavior<TextBlock>
    {
        private ScrollViewer? _scrollViewer;
        private bool _isScrollViewerCached;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.PreviewMouseWheel += OnPreviewMouseWheel;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.PreviewMouseWheel -= OnPreviewMouseWheel;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_isScrollViewerCached) return;

            _scrollViewer = FindScrollViewer(AssociatedObject);
            _isScrollViewerCached = true;
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!_isScrollViewerCached || _scrollViewer == null) return;

            bool isScrollingUp = e.Delta > 0;
            bool canScroll = isScrollingUp
                ? _scrollViewer.VerticalOffset > 0
                : _scrollViewer.VerticalOffset < _scrollViewer.ExtentHeight - _scrollViewer.ViewportHeight;

            if (canScroll) return;
            e.Handled = true;
        }

        private ScrollViewer? FindScrollViewer(DependencyObject parent)
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
                rtb.CaretPosition.InsertLineBreak();
            }
            else if (e.Key == Key.Tab)
            {
                e.Handled = true;
                rtb.CaretPosition.InsertTextInRun("    ");
            }
        }
    }

    public class PasteImageBehavior : Behavior<RichTextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                var rtb = AssociatedObject;

                // Вставка из буфера как изображения
                if (Clipboard.ContainsImage())
                {
                    var image = Clipboard.GetImage();
                    if (image != null)
                    {
                        InsertImage(rtb, image);
                        e.Handled = true;
                    }
                }

                // Вставка из буфера как файла изображения
                else if (Clipboard.ContainsFileDropList())
                {
                    var files = Clipboard.GetFileDropList();
                    if (files.Count > 0)
                    {
                        var path = files[0];
                        if (File.Exists(path))
                        {
                            var bitmap = new BitmapImage();
                            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                            {
                                bitmap.BeginInit();
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.StreamSource = stream;
                                bitmap.EndInit();
                            }

                            InsertImage(rtb, bitmap);
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private void InsertImage(RichTextBox rtb, BitmapSource imageSource)
        {
            var imageControl = new Image
            {
                Source = imageSource,
                Width = imageSource.Width,
                Stretch = System.Windows.Media.Stretch.Uniform
            };

            var container = new InlineUIContainer(imageControl, rtb.CaretPosition);
            rtb.CaretPosition = container.ElementEnd;
            rtb.Focus();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
        }
    }

    public class SmallNote : INotifyPropertyChanged
    {
        private string _content;
        public string Content
        {
            get => _content;
            set { if (_content != value) { _content = value; OnPropertyChanged(); } }
        }

        private double _x;
        public double X
        {
            get => _x;
            set { if (_x != value) { _x = value; OnPropertyChanged(); } }
        }

        private double _y;
        public double Y
        {
            get => _y;
            set { if (_y != value) { _y = value; OnPropertyChanged(); } }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class NoteCanvas
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; set; }
        public ObservableCollection<SmallNote> Notes { get; set; } = new ObservableCollection<SmallNote>();
    }

    public class Community
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public DateTime SelectedDate { get; set; } = DateTime.Today;
        public string Text { get; set; } = string.Empty;
    }

    public class BaseViewModel : INotifyPropertyChanged
    {
        private NoteCanvas _selectedCanvas;
        public ObservableCollection<NoteCanvas> Canvases { get; set; } = new ObservableCollection<NoteCanvas>();
        public NoteCanvas SelectedCanvas
        {
            get => _selectedCanvas;
            set
            {
                if (_selectedCanvas != value)
                {
                    _selectedCanvas = value;
                    OnPropertyChanged(nameof(SelectedCanvas));
                }
            }
        }
        public ScrollViewer MainScrollViewer { get; set; }
        private string _nickName;
        public string NickName
        {
            get => _nickName;
            set
            {
                if (_nickName != value)
                {
                    _nickName = value;
                    OnPropertyChanged(nameof(NickName));
                }
            }
        }

        private object _currentViewModel;
        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }
           public ScrollViewer ScrollViewer { get; set; }
        public ICommand SwitchToCommunityCommand { get; }
        public ICommand SwitchToCanvasCommand { get; }
        public ICommand AddCanvasCommand { get; }

        public BaseViewModel(Window window, string nickname)
        {
            AddCanvasCommand = new RelayCommand(AddCanvas);
            AddCanvas();
            AddCanvas();
            NickName = nickname;

            SwitchToCommunityCommand = new RelayCommand(() =>
                CurrentViewModel = new CommunityNotesViewModel(NickName));

            SwitchToCanvasCommand = new RelayCommand(() =>
                CurrentViewModel = new CanvasNotesViewModel());

            CurrentViewModel = new CommunityNotesViewModel(NickName);
        }

        private void AddCanvas()
        {
            MessageBox.Show("gg");
            var newCanvas = new NoteCanvas
            {
                Title = $"canvas {Canvases.Count + 1}"
            };
            Canvases.Add(newCanvas);
            SelectedCanvas = newCanvas;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
