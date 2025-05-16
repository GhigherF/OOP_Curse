using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows;


namespace CURSE.ViewModels
{
    public class CanvasNotesViewModel : INotifyPropertyChanged
    {
        private double _selectedFontSize = 12;
        private TextRange _lastSelection;
        public TextRange LastSelection
        {
            get => _lastSelection;
            set
            {
                _lastSelection = value;
                OnPropertyChanged(nameof(LastSelection));
            }
        }
        public double SelectedFontSize
        {
            get => _selectedFontSize;
            set
            {
                _selectedFontSize = value;
                OnPropertyChanged(nameof(SelectedFontSize));
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Принудительное обновление фокуса
                    var focusedElement = Keyboard.FocusedElement;
                    if (focusedElement is RichTextBox rtb)
                    {
                        rtb.Focus();
                    }
                });
            }
        }
        private void ApplyFontSizeToSelectedText(double fontSize)
{
    if (LastSelection != null && !LastSelection.IsEmpty)
    {
        LastSelection.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
    }
}
        public string Title
        {
            get => _noteCanvas.Title;
            set
            {
                if (_noteCanvas.Title != value)
                {
                    _noteCanvas.Title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }       
        public NoteCanvas _noteCanvas;

        public ObservableCollection<SmallNote> Notes => _noteCanvas.Notes;  

        public ScrollViewer MainScrollViewer { get; set; }

        public ICommand ToggleBoldCommand { get; }
        public ICommand ToggleItalicCommand { get; }
        public ICommand ToggleUnderlineCommand { get; }
        public ICommand AddSmallNoteCommand { get; }
        public ICommand DeleteSmallNoteCommand { get; }
         public ICommand SaveDocumentCommand { get; }
        public ICommand AddFontSizeCommand { get; }
        public ICommand RemoveFontSizeCommand { get; }

        public CanvasNotesViewModel(NoteCanvas noteCanvas)
        {
            _noteCanvas = noteCanvas ?? throw new ArgumentNullException(nameof(noteCanvas));
            _noteCanvas.PropertyChanged += (s, e) =>
            {   
                if (e.PropertyName == nameof(NoteCanvas.Title))
                    OnPropertyChanged(nameof(Title));
            };

            ToggleBoldCommand = new RelayCommand(ToggleBold);
            ToggleItalicCommand = new RelayCommand(ToggleItalic);
            ToggleUnderlineCommand = new RelayCommand(ToggleUnderline);
            AddSmallNoteCommand = new RelayCommand(AddSmallNote);
            DeleteSmallNoteCommand = new RelayCommand<SmallNote>(DeleteSmallNote);
        }

        private void AddFontSize()
        {
            // Логика добавления нового размера шрифта
            // Например, добавить в список FontSizes
            // Здесь можно показать диалог для ввода нового значения
        }

        private void RemoveFontSize()
        {
            // Логика удаления выбранного размера шрифта
            // Убедитесь, что выбранный размер шрифта не используется
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
        private void ToggleItalic()
        {
            if (Keyboard.FocusedElement is RichTextBox rtb)
            {
                var selection = rtb.Selection;
                if (!selection.IsEmpty)
                {
                    var weight = selection.GetPropertyValue(TextElement.FontStyleProperty);
                    var newWeight = (weight is FontStyle w && w == FontStyles.Italic)
                        ? FontStyles.Normal
                        : FontStyles.Italic;
                    selection.ApplyPropertyValue(TextElement.FontStyleProperty, newWeight);
                }
                rtb.Focus();
            }
        }
        private void ToggleUnderline()
        {
            if (Keyboard.FocusedElement is not RichTextBox rtb) return;

            var selection = rtb.Selection;
            if (selection.IsEmpty) return;

            // Безопасное получение текущих декораций
            var currentDecorations = selection.GetPropertyValue(Inline.TextDecorationsProperty);
            TextDecorationCollection decorationsCollection = currentDecorations as TextDecorationCollection;

            // Создаём новую коллекцию на основе существующей
            TextDecorationCollection newDecorations = decorationsCollection != null
                ? new TextDecorationCollection(decorationsCollection)
                : new TextDecorationCollection();

            // Проверяем наличие подчёркивания
            bool hasUnderline = decorationsCollection?
                .Any(d => d.Location == TextDecorationLocation.Underline) ?? false;

            // Переключаем подчёркивание
            if (hasUnderline)
            {
                // Удаляем все подчёркивания
                var underlinesToRemove = newDecorations
                    .Where(d => d.Location == TextDecorationLocation.Underline)
                    .ToList();

                foreach (var underline in underlinesToRemove)
                {
                    newDecorations.Remove(underline);
                }
            }
            else
            {
                // Добавляем стандартное подчёркивание
                newDecorations.Add(TextDecorations.Underline[0]);
            }

            // Применяем изменения
            selection.ApplyPropertyValue(
                Inline.TextDecorationsProperty,
                newDecorations.Count > 0 ? newDecorations : null
            );

            rtb.Focus();
        }


        private void AddSmallNote()
        {
            var newNote = new SmallNote
            {
                X = 100,
                Y = 100
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
