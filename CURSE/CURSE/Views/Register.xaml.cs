using CURSE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CURSE.Views
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : UserControl
    {
        public Register()
        {
            InitializeComponent();
        }
        private PasswordBox _passwordBox;

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm && sender is PasswordBox pb)
            {
                vm.RegisterVM.Password = pb.Password;

                // Привязка PasswordBox к VM для возможности очистки
                vm.SetPasswordBox(pb);

                // Запуск валидации, если нужна (через Tag, например)
                var binding = BindingOperations.GetBindingExpression(pb, PasswordBox.TagProperty);
                binding?.UpdateSource();
            }
        }

    }
}
