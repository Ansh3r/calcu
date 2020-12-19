using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Calc.Interfaces;
using Calc.Models;

namespace WpfApp1
{
    class ViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private ICalculate _calculator;

        public ICalculate Calculator => _calculator ?? new Calculate();

        private IMemory _memory;
        public IMemory Memory { get; }

        public IHistory Journal { get; }
        private Dictionary<string, string> propertyErrors { get; set; }

        public ViewModel()
        {
            Memory = new MemoryDB("db.db");//new MemoryFile("mem.txt");//new Memory();
            Journal = new HistoryDB("db.db");//new HistoryFile("journal.txt");//new History();
            propertyErrors = new Dictionary<string, string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string textValue = "0";
        public string TextValue
        {
            get => textValue;
            set
            {
                textValue = value;
                if (string.IsNullOrWhiteSpace(value))
                    propertyErrors[nameof(TextValue)] = "Empty";
                else if (new Expression(value, Calculator).HasError)
                    propertyErrors[nameof(TextValue)] = "Error in computing";
                else
                    propertyErrors[nameof(TextValue)] = null;
                OnPropertyChanged(nameof(TextValue));
            }
        }

        private ICommand addNumber;
        public ICommand AddNumber
        {
            get => addNumber ?? new RelayCommand<string>(x =>
            {
                TextValue += x;
            }, x => true);
        }

        private ICommand calculate;
        public ICommand Calculate
        {
            get => calculate ?? new RelayCommand<string>(x =>
            {
                var res = Calculator.Parse(new Expression(TextValue, Calculator));
                Journal.Add(res);
                TextValue = res.Result.ToString();
            }, x => Calculator.Parse(new Expression(TextValue, Calculator)).HasError == false);
        }

        private ICommand addMathAction;

        public ICommand AddMathAction
        {
            get => addMathAction ?? new RelayCommand<string>(x =>
            {
                TextValue += x;
            }, x => TextValue != "" && TextValue.Last() != '*' && TextValue.Last() != '+' && 
                    TextValue.Last() != '-' && TextValue.Last() != '/');
        }

        private ICommand _clear;
        public ICommand Clear
        {
            get => _clear ?? new RelayCommand(() =>
            {
                TextValue = "0";
            }, () => true);
        }

        private ICommand deleteMemoryElement;

        public ICommand DeleteMemoryElement
        {
            get => deleteMemoryElement ?? new RelayCommand<TextBox>(x =>
            {
                Memory.Delete((int)x.Tag);
            }, x => true);
        }

        private ICommand takeMemory;

        public ICommand TakeMemory
        {
            get => takeMemory ?? new RelayCommand(() =>
            {
                TextValue = Memory.MemoryObservableCollection.First().ToString();
            }, () => Memory.MemoryObservableCollection.Count > 0);
        }
        private ICommand saveMemory;

        public ICommand SaveMemory
        {
            get => saveMemory ?? new RelayCommand(() =>
            {
                Memory.Add(Calculator.Parse(new Expression(TextValue, Calculator)).Result);
            }, () => Calculator.Parse(new Expression(TextValue, Calculator)).HasError == false);
        }

        private ICommand clearMemory;

        public ICommand ClearMemory
        {
            get => clearMemory ?? new RelayCommand(() =>
            {
                Memory.Clear();
            }, () => Memory.MemoryObservableCollection.Count > 0);
        }

        public string Error
        {
            get => propertyErrors.Any(x => string.IsNullOrWhiteSpace(x.Value))
                ? string.Join(Environment.NewLine, propertyErrors.Where(x => string.IsNullOrWhiteSpace(x.Value) == false).GetEnumerator().Current)
                : null;
        }

        public string this[string columnName] => propertyErrors.ContainsKey(columnName) ? propertyErrors[columnName] : null;
    }
}
