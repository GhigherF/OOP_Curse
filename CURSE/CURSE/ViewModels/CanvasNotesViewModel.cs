using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows;

namespace CURSE.ViewModels
{
    public class CanvasNotesViewModel : INotifyPropertyChanged
    {
    
        public ScrollViewer MainScrollViewer { get; set; }
        public ObservableCollection<SmallNote> Notes { get; } = new();
        public ICommand ToggleBoldCommand { get; }
        public ICommand AddSmallNoteCommand { get; }
        public ICommand DeleteSmallNoteCommand { get; }
      
        public CanvasNotesViewModel()
        {
          
            ToggleBoldCommand = new RelayCommand(ToggleBold);
            AddSmallNoteCommand = new RelayCommand(AddSmallNote);
            DeleteSmallNoteCommand = new RelayCommand<SmallNote>(DeleteSmallNote);
        }

       

        private void ToggleBold()
        {
            if (Keyboard.FocusedElement is RichTextBox rtb)
            {
                var selection = rtb.Selection;
                if (!selection.IsEmpty)
                {
                    var weight = selection.GetPropertyValue(TextElement.FontWeightProperty);
                    var newWeight = (weight is FontWeight w && w == FontWeights.Bold)
                        ? FontWeights.Normal
                        : FontWeights.Bold;
                    selection.ApplyPropertyValue(TextElement.FontWeightProperty, newWeight);
                }
                rtb.Focus();
            }
        }

        private void AddSmallNote()
        {
            var newNote = new SmallNote
            {
                X = 100, // Начальная позиция X
                Y = 100  // Начальная позиция Y
            };

            Notes.Add(newNote);
        }

        private void DeleteSmallNote(SmallNote note)
        {
            if (note != null && Notes.Contains(note))
                Notes.Remove(note);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
