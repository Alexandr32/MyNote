using Note.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Note.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        Database database;

        ObservableCollection<Notes> notes;
        // Коллекция записей
        public ObservableCollection<Notes> Notes {
            get => notes;
            private set
            {
                notes = value;
                OnPropertyChanged("Notes");
            }
        }

        // Добавление новой записи
        public ICommand AddCommand { get; }
        // Удаление записи
        public ICommand RemoveCommand { get; }
        // Сохранение записи
        public ICommand SaveCommand { get; }
        // Сортировка записей по имент
        public ICommand SortingNameCommand { get; }
        // Сортировка по времени создания
        public ICommand SortingDateTimeCommand { get; }
        // Закрыть приложение
        public ICommand CloseCommand { get; }
        // Выгрузить текущую запись в .txt
        public ICommand UploadTxtCommand { get; }

        // Выбранная запись
        public Notes selectedNote;
        public Notes SelectedNote
        {
            get => selectedNote;
            set
            {
                selectedNote = value;
                OnPropertyChanged("SelectedNote");
            }
        }

        // Свойство для отображения изменения
        bool isEdited = false;
        public bool IsEdited
        {
            get => isEdited;
            set
            {
                isEdited = value;
                OnPropertyChanged("IsEdited");
            }
        }
        
        // Метод для уведомления что были внемесены изменеия
        void SetIsEdited(object sender, PropertyChangedEventArgs e)
        {
            IsEdited = true;
        }

        public MainWindowViewModel()
        {
            database = new Database("MyNotes", SetIsEdited, UpdateListNotes);
            // Загружаем сохранненые данные
            database.Load();
            
            // Создаем коллекцию с записями из файла
            Notes = database.ListNotes;

            // Добавление 
            AddCommand = new DelegateCommand(AddNewNote);
            // Удаление 
            RemoveCommand = new DelegateCommand(RemoveNote, CanNote);
            // Сохранение
            SaveCommand = new DelegateCommand(Save);
            // Сортировка по имени
            SortingNameCommand = new DelegateCommand(SortingName);
            // Сортировка по дате создания
            SortingDateTimeCommand = new DelegateCommand(SortingDateTime);
            // Команда закрытия записи
            CloseCommand = new DelegateCommand(Close);
            // Выгрузка данных из txt
            UploadTxtCommand = new DelegateCommand(delegate { database.SaveTxt(selectedNote); }, delegate { return (selectedNote as Notes) != null; });
        }
        /// <summary>
        /// Метод закрытия приложения
        /// </summary>
        /// <param name="obj"></param>
        private void Close(object obj)
        {
            //если были изменения ввыводим окно с вопросом о сохранении
            if(isEdited)
            {
                // Диалоговое окно с вопросом о сохранении
                MessageBoxResult result = MessageBox.Show("Сохранить данные?", "Есть несохраненные данные", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    // Сохраняем данные
                    database.Save();
                    // Выход из программы
                    System.Environment.Exit(0);
                }
                else if (result == MessageBoxResult.No)
                {
                    // Выход из программы
                    System.Environment.Exit(0);
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    
                }
            }
            else
            {
                // Выход из программы
                System.Environment.Exit(0);
            }
        }

        /// <summary>
        /// Сортировка по дате
        /// </summary>
        /// <param name="obj"></param>
        private void SortingDateTime(object obj)
        {
            database.SortingDateTime();
        }

        /// <summary>
        /// Сортировка по имени
        /// </summary>
        /// <param name="obj"></param>
        private void SortingName(object obj)
        {
            database.SortingName();

        }


        /// <summary>
        ///  Сохранение записей
        /// </summary>
        /// <param name="obj"></param>
        private void Save(object obj)
        {
            database.Save();
            // Данные сохранены, следовательно переключам флаг изменений в исходное положение
            IsEdited = false;
        }

        /// <summary>
        /// Выключает кнопку удаления в контекстном меню если не выбрана
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CanNote(object arg)
        {
            // Если выбранный элемент является "записью" и не равно 
            // нулю пункт в контекстом меню будет активен
            return (arg as Notes) != null;
        }

        /// <summary>
        ///  Удаление записи
        /// </summary>
        /// <param name="obj"></param>
        private void RemoveNote(object obj)
        {
            Notes note = (Notes)obj;

            // Диаоговое окно удаление записи
            MessageBoxResult result = MessageBox.Show("Удалить запись " + note.NameNote + "?", string.Empty, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                database.Remove(note);
                IsEdited = true;
            }
        }

        /// <summary>
        /// Добавление новой записи
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewNote(object obj)
        {
            Notes note = new Notes()
            {
                NameNote = "Новая запись",
                TextNote = "Текст записи",
            };

            database.Add(note);

            // Уведомляем что есть не сохраненные записи
            IsEdited = true;
        }

        // Обновление колекции
        void UpdateListNotes()
        {
            Notes = database.ListNotes;
        }

        // Метод для создания свойств привязки данных
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
