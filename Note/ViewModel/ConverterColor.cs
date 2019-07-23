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
    /// Класс для конвертации данных для отображения красной полоски при внесении изменений в заметку
    /// </summary>
    class ConverterColor : IValueConverter
    {
        // Данные из иcточника привязки
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isEdited = (bool)value;
            if (isEdited)
            {
                return Brushes.Red;
            }
            else
            {
                return "#FF0F3499";
            }
        }

        // Данные из целевого исочника привязки
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
