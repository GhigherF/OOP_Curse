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
 


    public partial class Base: Window
    {
        //private BaseViewModel _viewModel;
        //private void Bold(object sender, RoutedEventArgs e)
        //{
        //    e.Handled = true;
        //    var rtb = ContextMenuHelper.GetPlacementTargetFromContextMenu<RichTextBox>(sender);
        //    if (rtb != null && _viewModel is BaseViewModel vm)
        //    {
        //        vm.ToggleBoldCommand.Execute(rtb);
        //    }
        //}


        public Base()
        {
            //_viewModel = new BaseViewModel(this);
            InitializeComponent();

            //DataContext = new BaseViewModel(this);
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

    //public class BaseViewModel : INotifyPropertyChanged
    //{
    //    private readonly Window _window;
    //    public ObservableCollection<string> Notes { get; set; }

    //    public ICommand AddItemCommand { get; }
    //    public ICommand DragCommand { get; }
    //    public ICommand CloseCommand { get; }
    //    public ICommand MinimizeCommand { get; }
    //    public ICommand ToggleBoldCommand { get; }
    //    public ICommand NoteKeyDownCommand { get; }


    //    private bool _isDragging;
    //    private Point _clickPosition;

    //    public BaseViewModel(Window window)
    //    {
    //        _window = window;
    //        Notes = new ObservableCollection<string>();
    //        AddItemCommand = new RelayCommand(AddNote);
    //        DragCommand = new RelayCommand(DragWindow);
    //        CloseCommand = new RelayCommand(() => _window.Close());
    //        MinimizeCommand = new RelayCommand(() => _window.WindowState = WindowState.Minimized);
    //        ToggleBoldCommand = new RelayCommand<object>(ToggleBold);
    //        NoteKeyDownCommand = new RelayCommand<KeyEventArgs>(OnKeyDown);
    //    }



    //    private void AddNote()
    //    {
    //        Notes.Add($"Новый элемент {Notes.Count + 1}");
    //    }

    //    private void DragWindow()
    //    {
    //        _window.DragMove();
    //    }

    //    private void OnKeyDown(KeyEventArgs e)
    //    {
    //        // Обработка клавиш Enter и Back
    //    }

    //    private void ToggleBold(object? parameter)
    //    {
    //        if (parameter is RichTextBox rtb)
    //        {
    //            var selection = rtb.Selection;
    //            if (!selection.IsEmpty)
    //            {
    //                var currentWeight = selection.GetPropertyValue(TextElement.FontWeightProperty);
    //                var newWeight = (currentWeight is FontWeight fw && fw == FontWeights.Bold)
    //                    ? FontWeights.Normal
    //                    : FontWeights.Bold;

    //                selection.ApplyPropertyValue(TextElement.FontWeightProperty, newWeight);
    //            }

    //            rtb.Focus();
    //        }
    //    }
    //    public event PropertyChangedEventHandler PropertyChanged;
    //    protected void OnPropertyChanged(string propertyName)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //    }
    //}
}
