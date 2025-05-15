using CURSE.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CURSE.ViewModels
{
    public class CommunityNotesViewModel : INotifyPropertyChanged
    {
        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set
            {
                if (_filterText != value)
                {
                    _filterText = value;
                    OnPropertyChanged(nameof(FilterText));
                    CommunityView.Refresh();
                }
            }
        }
        public ICollectionView CommunityView { get; }
        public string Nickname { get; set; }
        public DateTime SelectedDate { get; set; } = DateTime.Now;
        public string Text { get; set; } = string.Empty;
        public ObservableCollection<Community> Community { get; } = new();
        public ICommand SaveAndLoadNotesCommand { get; }
        public ICommand AddCommunityNoteCommand { get; }
        public ICommand DeleteCommunityNoteCommand { get; }

        public ICommand ScrollToTopCommand { get; }
        public CommunityNotesViewModel(string nickname)
        {
            Nickname = nickname;
            AddCommunityNoteCommand = new RelayCommand(OpenCommunityNoteContextMenu);
            SaveAndLoadNotesCommand = new RelayCommand<object>(SaveAndLoadNotes);
            DeleteCommunityNoteCommand = new RelayCommand<Community>(DeleteCommunityNote);
            ScrollToTopCommand = new RelayCommand<ScrollViewer>(ScrollToTop);
            CommunityView = CollectionViewSource.GetDefaultView(Community);
            CommunityView.Filter = FilterNotes;
            LoadCommunityNotes();
        }

        private bool FilterNotes(object obj)
        {
            if (obj is Community note)
            {
                if (string.IsNullOrWhiteSpace(FilterText))
                    return true;

                string lowerFilter = FilterText.ToLower();

                return (note.Nickname != null && note.Nickname.ToLower().Contains(lowerFilter))
                    || (note.Text != null && note.Text.ToLower().Contains(lowerFilter));
            }
            return false;
        }
        private void ScrollToTop(ScrollViewer scrollViewer)
        {
            scrollViewer?.ScrollToTop();
        }
        private void SaveAndLoadNotes(object? parameter)
        {
            if (parameter is Tuple<string, DateTime, Window> values)
            {
                this.Text = values.Item1;
                this.SelectedDate = values.Item2;
                var window = values.Item3;

                using (var db = new dbContext())
                {
                    var note = new Community
                    {
                        Nickname = this.Nickname,
                        SelectedDate = this.SelectedDate,
                        Text = this.Text
                    };

                    db.Community.Add(note);
                    db.SaveChanges();

                    var notesFromDb = db.Community.ToList();

                    Community.Clear();
                    foreach (var dbNote in notesFromDb)
                    {
                        Community.Add(dbNote);
                    }

                    window?.Close(); // Закрываем окно
                }
            }
        }
        private void DeleteCommunityNote(Community note)
        {
           
            if (note == null) return;
            var result = MessageBox.Show(
      "Вы уверены, что хотите удалить эту заметку?",
      "Подтверждение удаления",
      MessageBoxButton.YesNo,
      MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            using (var db = new dbContext())
            {
                var noteInDb = db.Community.FirstOrDefault(n => n.Id == note.Id);
                if (noteInDb != null)
                {
                    db.Community.Remove(noteInDb);
                    db.SaveChanges();
                }
            }

            Community.Remove(note);
        }
        public bool IsAuthor(Community note) => note.Nickname == Nickname;
        private void LoadCommunityNotes()
        {
            using (var db = new dbContext())
            {
                var notesFromDb = db.Community.ToList();

                Community.Clear();
                foreach (var note in notesFromDb)
                {
                    Community.Add(note);
                }
            }
        }
        private void OpenCommunityNoteContextMenu()
        {
            var contextMenuWindow = new CommunityNoteContextMenu
            {
                Owner = Application.Current.MainWindow,
                DataContext = this // <-- ВАЖНО!
            };
            contextMenuWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
