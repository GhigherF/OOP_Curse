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
using System.Windows.Shapes;

namespace CURSE.Views
{
    /// <summary>
    /// Логика взаимодействия для CommunityNoteContextMenu.xaml
    /// </summary>
    public partial class CommunityNoteContextMenu : Window
    {
        public CommunityNoteContextMenu()
        {
            InitializeComponent();
        }
        public void X(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
