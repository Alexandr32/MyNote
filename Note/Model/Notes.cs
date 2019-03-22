using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note
{
    public class Notes : INotifyPropertyChanged
    {
        // Название записки
        string nameNote;
        // Текст записи
        string textNote;

        // Дата создания записи
        public DateTime DateTime { get; set; }
   
        public event PropertyChangedEventHandler PropertyChanged;

        // Свойство названия записи
        public string NameNote
        {
            get => nameNote;
            set
            {               
                nameNote = value;
                OnPropertyChanged("NameNote");
            }
        }
               
        // Свойство тела записи
        public string TextNote
        {
            get => textNote;
            set
            {
                textNote = value;
                OnPropertyChanged("TextNote");
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

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
