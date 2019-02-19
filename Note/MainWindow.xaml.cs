using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Note
{
    public partial class MainWindow : Window
    {
        Database database;
        // Отображает были ли внесены изменения
        bool isEdited = false;

        public MainWindow()
        {
            InitializeComponent();

            database = new Database("MyNotes");
            database.Load();
          
            textBoxNote.Background = Brushes.White;
            
            // Делает нективным поля ввода при отсутствии выбора записи
            EnebleTextBox();

            // Связываем listBox и коллекцию
            listBoxNote.ItemsSource = database.ListNotes;

            // Привязка Label в head к имени записи
            Binding bindingName = new Binding
            {
                Source = textBoxName,
                Path = new PropertyPath("Text"),
                Mode = BindingMode.TwoWay
            };

            // Связывает данные
            labelName.SetBinding(Label.ContentProperty, bindingName);

            // Фиксация метода который должен сработать при изменении данных
            database.Ghanged(Users_CollectionChanged, Item_PropertyChanged);
        }

        private void ListBoxNote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUI();
        }

        // Обновление интерфейся и привязка данных
        private void UpdateUI()
        {
            // Получение данных из listBox
            Notes note = (Notes) listBoxNote.SelectedValue;

            if (note != null)
            {
                // Связывает значение
                textBoxName.SetBinding(TextBox.TextProperty, note.BindingName);
                textBoxNote.SetBinding(TextBox.TextProperty, note.BindingNote);
            }

            EnebleTextBox();
        }


        // Делает нективным поля ввода при отсутствии выбора записи
        private void EnebleTextBox()
        {
            if (database.Count() == 0 || listBoxNote.SelectedValue == null)
            {
                textBoxName.IsEnabled = false;
                textBoxNote.IsEnabled = false;
            }
            else
            {
                textBoxName.IsEnabled = true;
                textBoxNote.IsEnabled = true;
            }
        }

        private void MenuItem_Click_Deleted(object sender, RoutedEventArgs e)
        {
            Notes note = (Notes)listBoxNote.SelectedValue;
            MessageBoxResult result = MessageBox.Show("Удалить запись " + note.NameNote + "?", string.Empty, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                database.Remove(note);
                textBoxName.Text = string.Empty;
                textBoxNote.Text = string.Empty;
            }
        }

        private void Click_AddNote(object sender, RoutedEventArgs e)
        {
            Notes note = new Notes()
            {
                NameNote = "Новая запись",
                TextNote = "Текст записи",
            };

            database.Add(note);            
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            database.Save();

            head_status.Background = new SolidColorBrush(Color.FromRgb(15, 52, 153));
            textStatusFile.Visibility = Visibility.Hidden;
            isEdited = false;
        }

        // Контекстное меню при нажатии на клавиши
        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            btnSetting.ContextMenu.IsOpen = true;
        }

        private void CloseApp_Clic(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Срабатывает если были изменения в списке записей
        public void Users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateUINotSaved();
        }

        // Свойство срабатывает при внесении изменении какого-либо свойства записи
        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateUINotSaved();
        }

        // Изменяет интерфейса при не сохраненном файле
        private void UpdateUINotSaved()
        {
            // Создает красную полоску
            head_status.Background = Brushes.Red;
            // Выводит касное сообщение о не сохраненным файле
            textStatusFile.Visibility = Visibility.Visible;
            isEdited = true;
        }

        // Метод по сортировки имени
        private void Sorting_Click(object sender, RoutedEventArgs e)
        {
            // Сортировка 
            database.SortingName();
            // Связывание данных с отсортированной коллекцией
            listBoxNote.ItemsSource = database.ListNotes;
        }

        private void SortingDataTime_Click(object sender, RoutedEventArgs e)
        {
            // Сортировка 
            database.SortingDateTime();
            // Связывание данных с отсортированной коллекцией
            listBoxNote.ItemsSource = database.ListNotes;
        }

        private void Head_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            head.ContextMenu.IsOpen = true;
        }
        
        // Перетаскивает окно при нажатии на head
        private void LabelName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isEdited)
            {
                MessageBoxResult result = MessageBox.Show("Сохранить данные?", "Есть несохраненные данные", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    database.Save();
                }
                else if (result == MessageBoxResult.No)
                {
                    
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        // Сохраняет текущую запись в .txt
        private void ButtonSaveTxt_Click(object sender, RoutedEventArgs e)
        {
            Notes note = (Notes)listBoxNote.SelectedValue;
            database.SaveTxt(note);
        }
    }
}
