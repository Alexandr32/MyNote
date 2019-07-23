using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Note
{
    /// <summary>
    /// Класс для конвертации данных для отображения надписи "Файл не сохранен!" при внесении изменений в заметку
    /// </summary>
    class ConverterTextIsEdited : IValueConverter
    {
        // Данные из иcточника привязки
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isEdited = (bool)value;
            if (isEdited)
            {
                return "Файл не сохранен!";
            }
            else
            {
                return string.Empty;
            }
        }

        // Данные из целевого исочника привязки
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
