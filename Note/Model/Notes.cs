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
        /// <summary>
        /// Название записки
        /// </summary>
        string nameNote;
        /// <summary>
        /// Текст записи
        /// </summary>
        string textNote;
        /// <summary>
        /// Дата создания записи
        /// </summary>
        public DateTime DateTime { get; set; }
   
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// названия записи
        /// </summary>
        public string NameNote
        {
            get => nameNote;
            set
            {               
                nameNote = value;
                OnPropertyChanged("NameNote");
            }
        }
               
        /// <summary>
        /// Текст тела записи
        /// </summary>
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
