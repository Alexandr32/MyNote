using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Xml.Serialization;

namespace Note.Model
{    
    [Serializable]  
    class Database
    {
        /// <summary>
        /// Расположение файла с записями
        /// </summary>
        string fileName;

        /// <summary>
        /// Делегат вызываемый при изменении данных
        /// </summary>
        Action Action;

        /// <summary>
        /// Список записей
        /// </summary>
        ObservableCollection<Notes> listNotes;

        /// <summary>
        /// Событие которое происходит при внесении изменений
        /// </summary>
        readonly PropertyChangedEventHandler propertyChangedEventHandler;

        /// <summary>
        /// Возвращает созданную коллекцию записей
        /// </summary>
        public ObservableCollection<Notes> ListNotes
        {
            get { return listNotes; }
        }

        /// <summary>
        /// Расположение файла с записями
        /// </summary>
        public string FileName
        {
            set { fileName = value; }
            get { return fileName; }
        }
        
        public Database(string _fileName, PropertyChangedEventHandler _propertyChangedEventHandler, Action _action)
        {
            propertyChangedEventHandler = _propertyChangedEventHandler;
            // Расположение файла с записями
            fileName = _fileName;
            listNotes = new ObservableCollection<Notes>();
            Action = _action;
        }

        /// <summary>
        /// Сохранение данных в файл
        /// </summary>
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

        /// <summary>
        /// Загрузка данных из файла
        /// </summary>
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
                // Добавляем каждой записи событие которое должно сработать при изменении записи
                foreach (var item in listNotes)
                {
                    item.PropertyChanged += propertyChangedEventHandler;
                }
            }
        }

        /// <summary>
        /// Сохраняет файл в тхт
        /// </summary>
        /// <param name="note"></param>
        public void SaveTxt(Notes note)
        {
            if(note != null)
            {
                StreamWriter sw = new StreamWriter(new FileStream(note.NameNote + ".txt", FileMode.Create, FileAccess.Write));
                sw.Write(note.ToString());
                sw.Close();
            }
        }
        
        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="note"></param>
        public void Add(Notes note)
        {
            // Передаем событие которое сработает при изменении свойства
            note.PropertyChanged += propertyChangedEventHandler;
            listNotes.Add(note);
            Action?.Invoke();
        }

        /// <summary>
        /// Сортировка по имени
        /// </summary>
        public void SortingName()
        {
            //throw new NotImplementedException();
            listNotes = new ObservableCollection<Notes>(listNotes.OrderBy(u => u.NameNote));
            Action?.Invoke();
        }

        /// <summary>
        /// Сортировка по дате создания
        /// </summary>
        public void SortingDateTime()
        {
            //throw new NotImplementedException();
            listNotes =  new ObservableCollection<Notes>(listNotes.OrderBy(u => u.DateTime));
            Action?.Invoke();
        }


        /// <summary>
        /// Возвращает кол-во записей
        /// </summary>
        /// <returns></returns>
        public int Count() => listNotes.Count();


        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="note"></param>
        public void Remove(Notes note)
        {
            listNotes.Remove(note);
        }
        
    }
}