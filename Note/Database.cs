using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Xml.Serialization;

namespace Note
{
    // Класс для вопроса    
    [Serializable]
    public class Notes : INotifyPropertyChanged
    {
        // Название записки
        string nameNote;
        // Текст записи
        string textNote;

        // Дата создания записи
        public DateTime DateTime { get; }

        // Событие которое происходит при изменении TextBox    
        public event PropertyChangedEventHandler PropertyChanged;

        public string NameNote
        {
            get => nameNote;
            set 
            {
                nameNote = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NameNote"));
            }
        }

        public string TextNote
        {
            get => textNote;
            set
            {
                textNote = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TextNote"));
            }
        }

        public Notes()
        {
            DateTime = DateTime.Now;
        }

        public Notes(string _nameNote, string _textNote)
        {
            nameNote = _nameNote;
            textNote = _textNote;
            DateTime = DateTime.Now;
        }

        public override string ToString()
        {
            return nameNote + "\r\n" + textNote;
        }

        // Привязка по свойсву текста
        public Binding BindingNote
        {
            get
            { 
                Binding bindingNote = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("TextNote"),
                    Mode = BindingMode.TwoWay
                };

                return bindingNote;
            }
        }

        // Привязка по свойсву имени
        public Binding BindingName
        {
            get
            {
                // Привязка имени
                Binding bindingName = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("NameNote"),
                    Mode = BindingMode.TwoWay
                };

                return bindingName;
            }
        }
    }

    class Database
    {
        // Названеи файлов
        string fileName;

        // Список записей
        ObservableCollection<Notes> listNotes;

        // Событие которое происходит при изменении TextBox
        PropertyChangedEventHandler propertyChangedEventHandler;

        // Свойство списка
        public ObservableCollection<Notes> ListNotes
        {
            get { return listNotes; }
        }

        // Св-во имени
        public string FileName
        {
            set { fileName = value; }
            get { return fileName; }
        }

        // Конструктор
        public Database(string _fileName)
        {
            fileName = _fileName;
            listNotes = new ObservableCollection<Notes>();
        }

        // Сохранение в файл
        public void Save()
        {
            // Класс для сериализации объектов типа List<Notes> в xml
            XmlSerializer xmlFormat = new XmlSerializer(typeof(ObservableCollection<Notes>));

            // Создается поток для записи сериализованного списка
            Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);

            // Сериализируем список вопросов
            xmlFormat.Serialize(fStream, listNotes);
            fStream.Close();
        }

        // Загрузка файла
        public void Load()
        {
            if(File.Exists(fileName))
            {
                // Класс для сериализации объектов типа List<Notes> в xml
                XmlSerializer xmlFormat = new XmlSerializer(typeof(ObservableCollection<Notes>));

                // Создается поток для чтения сериализованного списка
                Stream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                // Присваиваем переменной десариализованные данные
                listNotes = (ObservableCollection<Notes>)xmlFormat.Deserialize(fStream);
                fStream.Close();
            }
        }

        // Сохраняет файл в тхт
        public void SaveTxt(Notes note)
        {
            if(note != null)
            {
                StreamWriter sw = new StreamWriter(new FileStream(note.NameNote + ".txt", FileMode.Create, FileAccess.Write));
                sw.Write(note.ToString());
                sw.Close();
            }
        }

        // Уведомление о изменении коллекции
        public void Ghanged(NotifyCollectionChangedEventHandler delegateGhanged, PropertyChangedEventHandler _propertyChangedEventHandler)
        {
            propertyChangedEventHandler = _propertyChangedEventHandler;

            // Передаем событие которое сработает при изменении колекции
            listNotes.CollectionChanged += delegateGhanged;

            foreach(var item in listNotes)
            {
                // Передаем событие которое сработает при изменении свойства
                item.PropertyChanged += _propertyChangedEventHandler;
            }
            
        }

        public void Add(Notes note)
        {
            // Передаем событие которое сработает при изменении свойства
            note.PropertyChanged += propertyChangedEventHandler;
            listNotes.Add(note);
        }
        
        // Сортировка по имени
        public void SortingName()
        {
            listNotes = new ObservableCollection<Notes>(listNotes.OrderBy(u => u.NameNote));
        }

        // Сортировка по дате создания
        public void SortingDateTime()
        {
            listNotes = new ObservableCollection<Notes>(listNotes.OrderBy(u => u.DateTime));
        }

        public int Count() => listNotes.Count();

        public void Remove(Notes note)
        {
            listNotes.Remove(note);
        }
        
    }
}