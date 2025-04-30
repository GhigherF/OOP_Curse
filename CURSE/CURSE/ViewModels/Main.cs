    using System;
    using System.Windows;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.ComponentModel.DataAnnotations;


    namespace CURSE.ViewModels
    {



    //Errors
    public class RegisterModel
    {

        [Required(ErrorMessage = "Поле обязательно")]
        [MinLength(8, ErrorMessage = "Пароль должен содержать минимум 8 символов")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[^\s]*$",
            ErrorMessage = "Пароль должен содержать минимум одну цифру, одну заглавную и одну строчную букву, и не содержать пробелов")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Поле обязательно")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно")]
        [MinLength(3, ErrorMessage = "Минимальная длина 3 символа")]
        [RegularExpression("^[a-zA-ZА-Яа-я0-9_!@#$%^&*()+=]+$",
            ErrorMessage = "Недопустимый символ")]
        public string? Nick { get; set; }
    }

    public class LoginModel
    {

        [Required(ErrorMessage = "Поле обязательно")]
        [MinLength(8, ErrorMessage = "Пароль должен содержать минимум 8 символов")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[^\s]*$",
            ErrorMessage = "Пароль должен содержать минимум одну цифру, одну заглавную и одну строчную букву, и не содержать пробелов")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Поле обязательно")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        public string? Email { get; set; }

    }

   
    public class RegisterViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly RegisterModel _model = new();

        public string Nick
        {
            get => _model.Nick ?? "";
            set
            {
                _model.Nick = value;
                OnPropertyChanged(nameof(Nick));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string Email
        {
            get => _model.Email ?? "";
            set
            {
                _model.Email = value;
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string Password
        {
            get => _model.Password ?? "";
            set
            {
                _model.Password = value;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string this[string columnName]
        {
            get
            {
                var context = new ValidationContext(_model)
                {
                    MemberName = columnName
                };

                var results = new List<ValidationResult>();
                bool isValid = Validator.TryValidateProperty(
                    GetProperty(columnName), context, results
                );

                return isValid ? string.Empty : results.First().ErrorMessage!;
            }
        }

        public bool CanRegister =>
           string.IsNullOrEmpty(this[nameof(Email)]) &&
           string.IsNullOrEmpty(this[nameof(Password)])&&
           string.IsNullOrEmpty(this[nameof(Nick)]);

        public string Error => null!;

        private object? GetProperty(string propertyName) =>
            propertyName switch
            {
                nameof(Nick) => _model.Nick,
                nameof(Email) => _model.Email,
                nameof(Password) => _model.Password,
                _ => null
            };

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public class LoginViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly LoginModel _model = new();

        public string Email
        {
            get => _model.Email ?? string.Empty;
            set
            {
                if (_model.Email != value)
                {
                    _model.Email = value;
                    OnPropertyChanged(nameof(Email));
                    OnPropertyChanged(nameof(CanLogin));
                }
            }
        }

        public string Password
        {
            get => _model.Password ?? string.Empty;
            set
            {
                if (_model.Password != value)
                {
                    _model.Password = value;
                    OnPropertyChanged(nameof(Password));
                    OnPropertyChanged(nameof(CanLogin));
                }
            }
        }

        public bool CanLogin =>
            string.IsNullOrEmpty(this[nameof(Email)]) &&
            string.IsNullOrEmpty(this[nameof(Password)]);

        public string this[string columnName]
        {
            get
            {
                var context = new ValidationContext(_model)
                {
                    MemberName = columnName
                };

                var value = columnName switch
                {
                    nameof(Email) => _model.Email,
                    nameof(Password) => _model.Password,
                    _ => null
                };

                var results = new List<ValidationResult>();
                bool isValid = Validator.TryValidateProperty(value, context, results);
                return isValid ? string.Empty : results.First().ErrorMessage!;
            }
        }

        public string Error => null!;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }






    public class MainViewModel : INotifyPropertyChanged
    {
        public RegisterViewModel RegisterVM { get; } = new RegisterViewModel();
        public LoginViewModel LoginVM { get; } = new LoginViewModel();
        private object _currentView;
            public object CurrentView
            {
                get => _currentView;
                set
                {
                    _currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
            }

        public ICommand RegisterCommand { get; }
            public ICommand LogInCommand { get; }
            public ICommand EnterCommand { get; }

            public MainViewModel()
            {
                RegisterCommand = new RelayCommand(ShowRegister);
                LogInCommand = new RelayCommand(ShowLogin);
                EnterCommand = new RelayCommand(Enter);

                ShowLogin(); // начальный экран
            }

            private void ShowRegister()
            {
                CurrentView = new Views.Register(); // можно заменить на ViewModel
            }

            private void ShowLogin()
            {
                CurrentView = new Views.LogIn(); // можно заменить на ViewModel
            }

            private void Enter()
            {
                var b = new Base();
                b.Show();

                // Использовать событие или Messenger, а не прямой доступ к окну
                Application.Current.Windows
                    .OfType<MainWindow>()
                    .FirstOrDefault()?.Close();
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string propertyName) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


