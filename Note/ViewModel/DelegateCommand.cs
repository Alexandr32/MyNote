using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Note
{
    /// <summary>
    /// Класс для создания команд
    /// </summary>
    class DelegateCommand : ICommand
    {
        /// <summary>
        /// Делагат который сработает при нажатии на элемент
        /// </summary>
        readonly Action<object> execute;
        readonly Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Должно возвращать логическое выражение используется для 
        // включения или выключения элементов UI
        public bool CanExecute(object parameter) => canExecute != null ? canExecute(parameter) : true;

        /// <summary>
        /// Выполнения события
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            execute?.Invoke(parameter);
        }

        // Используется для команд которым не нужна возможность отображение/скрытие элемента UI
        public DelegateCommand(Action<object> executeAcion) : this(executeAcion, null)
        {

        }

        // Второй параметр отвечает за отображение элемента
        public DelegateCommand(Action<object> executeAcion, Func<object, bool> canExecuteAction)
        {
            execute = executeAcion;
            canExecute = canExecuteAction;
        }
    }
}
