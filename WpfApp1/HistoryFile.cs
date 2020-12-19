using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Calc.Interfaces;
using Calc.Models;
using Newtonsoft.Json;

namespace WpfApp1
{
    public class HistoryFile : IHistory
    {
        public ObservableCollection<Expression> ExpressionsHistory { get; }
        private string nameFile;

        public HistoryFile(string nameFile)
        {
            this.nameFile = nameFile;
            if (File.Exists(nameFile) == false)
            {
                ExpressionsHistory = new ObservableCollection<Expression>();
                return;
            }

            var text = File.ReadAllText(nameFile);
            ExpressionsHistory = JsonConvert.DeserializeObject<ObservableCollection<Expression>>(text);
        }

        public void Add(Expression expression)
        {
            ExpressionsHistory.Insert(0, expression);
            var textToOutput = JsonConvert.SerializeObject(ExpressionsHistory);
            File.WriteAllText(nameFile, textToOutput);
        }

        public void Delete(int index)
        {
            ExpressionsHistory.RemoveAt(index);
            var textToOutput = JsonConvert.SerializeObject(ExpressionsHistory);
            File.WriteAllText(nameFile, textToOutput);
        }

        public void Clear()
        {
            var textToOutput = JsonConvert.SerializeObject(ExpressionsHistory);
            File.WriteAllText(nameFile, textToOutput);
        }
    }
}
