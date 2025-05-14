using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CURSE.ViewModels
{
    public class SmallNotesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SmallNote> SmallNotes { get; } = new ObservableCollection<SmallNote>();

        private readonly ObservableCollection<NoteCanvas> _globalNotes;

        public ICommand AddSmallNoteCommand { get; }
       

        public SmallNotesViewModel(ObservableCollection<NoteCanvas> globalNotes)
        {
            _globalNotes = globalNotes;

            AddSmallNoteCommand = new RelayCommand<NoteCanvas>(AddSmallNote);
          
        }

        private void AddSmallNote(NoteCanvas parentNote)
        {
            var newNote = new SmallNote
            {
                Content = string.Empty,
                X = 0,
                Y = 0
            };

            parentNote.Notes.Add(newNote);
            SmallNotes.Add(newNote);
        }

        

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
