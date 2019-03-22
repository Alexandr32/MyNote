using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Note
{
    // Класс для создания команд
    class DelegateCommand : ICommand
    {
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
        
        // метод выполнения события
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
