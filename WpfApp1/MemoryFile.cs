using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Calc.Interfaces;
using Newtonsoft.Json;

namespace WpfApp1
{
    public class MemoryFile : IMemory
    {
        public ObservableCollection<double> MemoryObservableCollection { get; }
        private string nameFile;

        public MemoryFile(string nameFile)
        {
            this.nameFile = nameFile;
            if (File.Exists(nameFile) == false)
            {
                MemoryObservableCollection = MemoryObservableCollection ?? new ObservableCollection<double>();
                return;
            }

            var text = File.ReadAllText(nameFile);
            MemoryObservableCollection = JsonConvert.DeserializeObject<ObservableCollection<double>>(text);
        }

        public void Add(double value)
        {
            MemoryObservableCollection.Insert(0, value);
            var textToOutput = JsonConvert.SerializeObject(MemoryObservableCollection);
            File.WriteAllText(nameFile, textToOutput);
        }

        public void Delete(int index)
        {
            MemoryObservableCollection.RemoveAt(index);
            var textToOutput = JsonConvert.SerializeObject(MemoryObservableCollection);
            File.WriteAllText(nameFile, textToOutput);
        }

        public void Clear()
        {
            MemoryObservableCollection.Clear();
            var textToOutput = JsonConvert.SerializeObject(MemoryObservableCollection);
            File.WriteAllText(nameFile, textToOutput);
        }
    }
}
