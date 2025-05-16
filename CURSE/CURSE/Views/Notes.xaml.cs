using CURSE.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace CURSE.Views
{
    public partial class Notes : UserControl
    {

      
        public Notes()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                if (DataContext is CanvasNotesViewModel vm)
                {
                    vm.MainScrollViewer = MainScrollViewer;
                }
            };
        }


        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (thumb == null) return;

            var grid = FindParent<Grid>(thumb);
            if (grid == null) return;

            var note = grid.DataContext as SmallNote;
            if (note == null || MainScrollViewer == null) return;

            var currentWidth = grid.ActualWidth;
            var currentHeight = grid.ActualHeight;

            var newWidth = currentWidth + e.HorizontalChange;
            var newHeight = currentHeight + e.VerticalChange;

            // Получаем правый и нижний край области прокрутки
            var visibleRight = MainScrollViewer.HorizontalOffset + MainScrollViewer.ViewportWidth;
            var visibleBottom = MainScrollViewer.VerticalOffset + MainScrollViewer.ViewportHeight;

            newWidth = Math.Clamp(newWidth, 200, visibleRight - note.X);
            newHeight = Math.Clamp(newHeight,180, visibleBottom - note.Y);

            grid.Width = newWidth;
            grid.Height = newHeight;

            // Проверяем, нужно ли прокручивать
            CheckScrollBounds(grid, MainScrollViewer);
        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (true)
            {
                var parentObject = VisualTreeHelper.GetParent(child);
                if (parentObject == null) return null;
                if (parentObject is T parent) return parent;
                child = parentObject;
            }
        }

        private void CheckScrollBounds(FrameworkElement element, ScrollViewer scrollViewer)
        {
            var itemRight = element.Margin.Left + element.ActualWidth;
            var itemBottom = element.Margin.Top + element.ActualHeight;

            var viewportRight = scrollViewer.HorizontalOffset + scrollViewer.ViewportWidth;
            var viewportBottom = scrollViewer.VerticalOffset + scrollViewer.ViewportHeight;

            if (itemRight > viewportRight)
            {
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + (itemRight - viewportRight));
            }

            if (itemBottom > viewportBottom)
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + (itemBottom - viewportBottom));
            }
        }
        private void ComboBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null && !comboBox.IsDropDownOpen)
            {
                comboBox.IsHitTestVisible = false; // Принудительное сброс фокуса
                comboBox.IsHitTestVisible = true;
            }
        }

        private void ToggleButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Фикс двойного срабатывания
            e.Handled = true;
            var toggleButton = sender as ToggleButton;
            toggleButton?.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer sv)
            {
                const double scrollStep = 40;
                double newOffset = sv.VerticalOffset - Math.Sign(e.Delta) * scrollStep;

                newOffset = Math.Max(0, Math.Min(newOffset, sv.ExtentHeight - sv.ViewportHeight));
                sv.ScrollToVerticalOffset(newOffset);
                e.Handled = true;
            }
        }
        private void ComboBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            ComboBox combo = (ComboBox)sender;
            combo.IsDropDownOpen = true;

            // предотвращаем потерю фокуса RichTextBox
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(combo), Keyboard.FocusedElement);
        }
    }
}
