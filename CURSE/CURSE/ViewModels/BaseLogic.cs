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
    using System.Text;
using System.Windows.Controls.Primitives;

    namespace CURSE.ViewModels
    {
    public class PreventFocusLossBehavior : Behavior<ComboBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.PreviewMouseDown += OnPreviewMouseDown;
    }

    private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        var comboBox = sender as ComboBox;
        if (comboBox != null)
        {
            var parent = VisualTreeHelper.GetParent(comboBox);
            while (parent != null && !(parent is RichTextBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            if (parent is RichTextBox rtb)
            {
                rtb.Focus();
            }
        }
    }

    protected override void OnDetaching()
    {
        AssociatedObject.PreviewMouseDown -= OnPreviewMouseDown;
        base.OnDetaching();
    }
}
    public class FontSizeBehavior : Behavior<ComboBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
            AssociatedObject.LostFocus += OnLostFocus;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AssociatedObject.SelectedItem is double fontSize)
            {
                var focusedElement = Keyboard.FocusedElement as RichTextBox;
                focusedElement?.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
            }
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.Text is string text && double.TryParse(text, out var fontSize))
            {
                var focusedElement = Keyboard.FocusedElement as RichTextBox;
                focusedElement?.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
            AssociatedObject.LostFocus -= OnLostFocus;
            base.OnDetaching();
        }
    }


    public class ResizeAdorner : Adorner
    {
        private Thumb _resizeThumb;
        private VisualCollection _visuals;
        private FrameworkElement _adornedElement;

        // Отступы от краёв заметки для уголка ресайза
        private const double EdgePadding = 15;

        public ResizeAdorner(UIElement adornedElement) : base(adornedElement)
        {
            _adornedElement = adornedElement as FrameworkElement;

            _resizeThumb = new Thumb
            {
                Width = 10,
                Height = 10,
                Cursor = Cursors.SizeNWSE,
                Template = CreateThumbTemplate()
            };

            _resizeThumb.DragDelta += ResizeThumb_DragDelta;

            _visuals = new VisualCollection(this);
            _visuals.Add(_resizeThumb);
        }

        private ControlTemplate CreateThumbTemplate()
        {
            string xamlTemplate = @"
<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' TargetType='Thumb'>
    <Grid Background='#01000000' Cursor='SizeNWSE'>
        <Path Data='M0,10 L10,10 10,0' Stroke='DarkGray' StrokeThickness='2' Stretch='None' HorizontalAlignment='Right' VerticalAlignment='Bottom'/>
        <Path Data='M0,10 L10,10 10,0' Stroke='White' StrokeThickness='1' Stretch='None' HorizontalAlignment='Right' VerticalAlignment='Bottom'/>
    </Grid>
</ControlTemplate>";
            return (ControlTemplate)System.Windows.Markup.XamlReader.Parse(xamlTemplate);
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_adornedElement == null)
                return;

            double minWidth = 20;
            double minHeight = 20;

            // Найдём родительский RichTextBox (или ближайший контейнер с размерами)
            var parent = FindParentOfType(_adornedElement, typeof(RichTextBox));

            FrameworkElement FindParentOfType(DependencyObject child, Type targetType)
            {
                DependencyObject parentObject = VisualTreeHelper.GetParent(child);
                if (parentObject == null) return null;

                if (targetType.IsInstanceOfType(parentObject))
                    return (FrameworkElement)parentObject;

                return FindParentOfType(parentObject, targetType);
            }

            double maxWidth, maxHeight;

            if (parent != null)
            {
                // Максимальный размер — размеры родителя минус отступы для Thumb
                maxWidth = parent.ActualWidth - EdgePadding;
                maxHeight = parent.ActualHeight - EdgePadding;
            }
            else
            {
                maxWidth = double.MaxValue;
                maxHeight = double.MaxValue;
            }

            double proposedWidth = _adornedElement.Width + e.HorizontalChange;
            double proposedHeight = _adornedElement.Height + e.VerticalChange;

            // Ограничиваем размер в пределах минимум и максимум с учётом отступа
            double newWidth = Math.Min(Math.Max(proposedWidth, minWidth), maxWidth);
            double newHeight = Math.Min(Math.Max(proposedHeight, minHeight), maxHeight);

            _adornedElement.Width = newWidth;
            _adornedElement.Height = newHeight;

            this.InvalidateArrange(); // чтобы Thumb сместился вместе с изменением размера

        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double left = finalSize.Width - _resizeThumb.Width;
            double top = finalSize.Height - _resizeThumb.Height;

            if (left < 0) left = 0;
            if (top < 0) top = 0;

            _resizeThumb.Arrange(new Rect(left, top, _resizeThumb.Width, _resizeThumb.Height));
            return finalSize;
        }


        protected override int VisualChildrenCount => _visuals.Count;

        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }
    }



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
            AssociatedObject.GotFocus += AssociatedObject_GotFocus;
            AssociatedObject.TextChanged += AssociatedObject_TextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
            AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
            AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
            RemoveAllAdorners();
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                var rtb = AssociatedObject;

                if (Clipboard.ContainsImage())
                {
                    var image = Clipboard.GetImage();
                    if (image != null)
                    {
                        InsertImage(rtb, image);
                        e.Handled = true;
                        rtb.Dispatcher.InvokeAsync(() => AddResizeAdorners(rtb), System.Windows.Threading.DispatcherPriority.Background);
                    }
                }
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
                                bitmap.Freeze();
                            }

                            InsertImage(rtb, bitmap);
                            e.Handled = true;
                            rtb.Dispatcher.InvokeAsync(() => AddResizeAdorners(rtb), System.Windows.Threading.DispatcherPriority.Background);
                        }
                    }
                }
            }
        }

        private void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
        {
            AddResizeAdorners(AssociatedObject);
        }

        private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            AddResizeAdorners(AssociatedObject);
        }

        private void InsertImage(RichTextBox rtb, BitmapSource imageSource)
        {
            string imagePath = SaveImageToTempFolder(imageSource);

            var bitmapImage = new BitmapImage(new Uri(imagePath));

            var image = new Image
            {
                Source = bitmapImage,
                Width = 120,
                Height = 90,
                Stretch = Stretch.Uniform,
                SnapsToDevicePixels = true,
                UseLayoutRounding = true,
                Tag = imagePath
            };

            var inlineContainer = new InlineUIContainer(image, rtb.CaretPosition);
            rtb.CaretPosition = inlineContainer.ElementEnd;


            var position = rtb.CaretPosition;

            // Вставляем блок после текущего параграфа
            var paragraph = position.Paragraph;
            if (paragraph != null)
            {
                var flowDoc = rtb.Document;
                var paragraf = rtb.Document.Blocks.FirstBlock as Paragraph;
                if (paragraf == null)
                {
                    paragraf = new Paragraph();
                    rtb.Document.Blocks.Add(paragraf);
                }

                paragraf.Inlines.Add(inlineContainer);
                rtb.CaretPosition = inlineContainer.ElementEnd;
            }
            else
            {
                // fallback - добавляем в конец
                var paragraf = position.Paragraph;
                if (paragraf == null)
                {
                    paragraf = new Paragraph();
                    rtb.Document.Blocks.Add(paragraf);
                }

                paragraf.Inlines.Add(inlineContainer);
                rtb.CaretPosition = inlineContainer.ElementEnd;
            }

            rtb.Focus();
        }

        private string SaveImageToTempFolder(BitmapSource image)
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "MyRtbImages");
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);

            string fileName = Path.Combine(tempDir, Guid.NewGuid() + ".png");

            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }

            return fileName;
        }

        private void AddResizeAdorners(RichTextBox rtb)
        {
            if (rtb == null || rtb.Document == null)
                return;

            var images = FindVisualChildren<Image>(rtb);
            foreach (var image in images)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(image);
                if (adornerLayer != null)
                {
                    var adorner = new ResizeAdorner(image);
                    adornerLayer.Add(adorner);
                }
            }
        }
        private void RemoveAllAdorners()
        {
            var rtb = AssociatedObject;
            if (rtb == null)
                return;

            var images = FindVisualChildren<Image>(rtb);
            foreach (var image in images)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(image);
                if (adornerLayer == null)
                    continue;

                var adorners = adornerLayer.GetAdorners(image);
                if (adorners != null)
                {
                    foreach (var adorner in adorners)
                    {
                        if (adorner is ResizeAdorner)
                            adornerLayer.Remove(adorner);
                    }
                }
            }
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T t)
                        yield return t;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }
    }



    public static class RichTextBoxHelper
        {
            public static readonly DependencyProperty BindableDocumentProperty =
                DependencyProperty.RegisterAttached(
                    "BindableDocument",
                    typeof(string),
                    typeof(RichTextBoxHelper),
                    new FrameworkPropertyMetadata(
                        string.Empty,
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                        OnBindableDocumentChanged));

            public static string GetBindableDocument(DependencyObject obj)
                => (string)obj.GetValue(BindableDocumentProperty);

            public static void SetBindableDocument(DependencyObject obj, string value)
                => obj.SetValue(BindableDocumentProperty, value);

            private static void OnBindableDocumentChanged(
                DependencyObject d,
                DependencyPropertyChangedEventArgs e)
            {
                if (d is RichTextBox rtb && e.NewValue is string xaml)
                {
                    // Загрузка документа из XAML
                    try
                    {
                        var stream = new MemoryStream(Encoding.UTF8.GetBytes(xaml));
                        var doc = new FlowDocument();
                        var range = new TextRange(doc.ContentStart, doc.ContentEnd);
                        range.Load(stream, DataFormats.XamlPackage);
                        rtb.Document = doc;
                    }
                    catch
                    {
                        rtb.Document = new FlowDocument();
                    }
                }
            }

            // Сохранение документа в XAML при изменениях
            public static void SaveDocument(RichTextBox rtb)
            {
                var doc = rtb.Document;
                var range = new TextRange(doc.ContentStart, doc.ContentEnd);
                using var stream = new MemoryStream();
                range.Save(stream, DataFormats.XamlPackage);
                SetBindableDocument(rtb, Encoding.UTF8.GetString(stream.ToArray()));
            }
        }




        public class SmallNote : INotifyPropertyChanged
        {
            private string _documentXaml;
            public string DocumentXaml
            {
                get => _documentXaml;
                set
                {
                    if (_documentXaml != value)
                    {
                        _documentXaml = value;
                        OnPropertyChanged(nameof(DocumentXaml));
                    }
                }
            }

            private double _x;
            public double X
            {
                get => _x;
                set { if (_x != value) { _x = value; OnPropertyChanged(nameof(X)); } }
            }

            private double _y;
            public double Y
            {
                get => _y;
                set { if (_y != value) { _y = value; OnPropertyChanged(nameof(Y)); } }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class NoteCanvas:INotifyPropertyChanged
        {
            public Guid Id { get; } = Guid.NewGuid();
            private string _title;
            public string Title
            {
                get => _title;
                set
                {
                    if (_title != value)
                    {
                        _title = value;
                        OnPropertyChanged(nameof(Title));
                    }
                }
            }
                  public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
            public ObservableCollection<NoteCanvas> Canvases { get; set; } = new ObservableCollection<NoteCanvas>();
            private string _title;  
            public string Title
            {
                get => _selectedCanvas.Title;
                set
                {
                    if (_selectedCanvas.Title != value)
                    {
                        _selectedCanvas.Title = value;
                        OnPropertyChanged(nameof(Title));
                    }
                }
            }
            private NoteCanvas _selectedCanvas;
            public NoteCanvas SelectedCanvas
            {
                get => _selectedCanvas;
                set
                {
                    _selectedCanvas = value;
                    CurrentViewModel = new CanvasNotesViewModel(_selectedCanvas);
                    OnPropertyChanged(nameof(SelectedCanvas));
                    OnPropertyChanged(nameof(CurrentViewModel));
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
            public ICommand SelectCanvasCommand { get; }
            private CanvasNotesViewModel _canvasNotesVM;

            public BaseViewModel(Window window,string nickname)
            {
                AddCanvasCommand = new RelayCommand(AddCanvas);
                Canvases.Add(new NoteCanvas { Title = "Заметки 1" });
                Canvases.Add(new NoteCanvas { Title = "Заметки 2" });
                SelectCanvasCommand = new RelayCommand<NoteCanvas>(canvas => SelectedCanvas = canvas);
                SwitchToCommunityCommand = new RelayCommand(() => CurrentViewModel = new CommunityNotesViewModel(nickname));
                SwitchToCanvasCommand = new RelayCommand(() => CurrentViewModel = new CanvasNotesViewModel(SelectedCanvas));
                CurrentViewModel = new CommunityNotesViewModel(nickname);
            }


            private void AddCanvas()
            {
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
