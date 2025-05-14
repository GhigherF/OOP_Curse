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
    /// Логика взаимодействия для CommunityNotes.xaml
    /// </summary>
    public partial class CommunityNotes : UserControl
    {
        public CommunityNotes(string nickname)
        {
            InitializeComponent();
            this.DataContext = new CommunityNotesViewModel(nickname);
        }
        public CommunityNotes()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is CommunityNotesViewModel vm)
            {
                string nickname = vm.Nickname;
                // Используй nickname как нужно
            }
        }
    }
}
