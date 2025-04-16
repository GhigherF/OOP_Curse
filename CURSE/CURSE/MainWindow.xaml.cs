using Microsoft.Win32;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CURSE
{
    /// <summary>q
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isDragging = false;
        private Point clickPosition;

        private MainViewModel _viewModel;

        public void RegisterTextBlock_MouseLeftButtonUp(object sender, EventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.RegisterCommand.Execute(null);
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
            _viewModel.window(this);
        }
        public void Drag(object sender, MouseButtonEventArgs e)
        {

            this.DragMove();
        }
        public void X(object sender, EventArgs e)
        {
            this.Close();
        }
        public void _ (object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized; // Свернуть окно
        }
    }
    public class LogIn
    {
        string Email;
        string password;       
    }
    public class Register
    {
        string name;
        string Email;
        string password;
    }

    public class MainViewModel : INotifyPropertyChanged
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
        public ICommand RegisterCommand { get; private set; }
        public ICommand LogInCommand { get; private set; }
        public ICommand Enter { get; private set; }
        private MainWindow mainWindow;

        public MainViewModel()
        {
            CurrentView = new LogIn();
            RegisterCommand = new RelayCommand(ShowRegister);
            LogInCommand = new RelayCommand(ShowLogin);
            Enter = new RelayCommand(enter);
        }

       

        public void window(MainWindow e)
        {
            mainWindow = e;
        }

       
        public void enter()
        {
            
          
            var b = new Base();
            b.Show();
            mainWindow.Close();
        }

        public void ShowLogin()
        {
            CurrentView = new LogIn();
        }

        public void ShowRegister()
        {
            CurrentView = new Register();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }







    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public void Execute(object parameter) => _execute();

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }


    }