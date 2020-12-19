using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Calc.Interfaces;

namespace WpfApp1
{
    public class Memory : IMemory
    {
        public ObservableCollection<double> MemoryObservableCollection { get; }

        public Memory()
        {
            MemoryObservableCollection = new ObservableCollection<double>();
        }
        public void Add(double value)
        {
            MemoryObservableCollection.Insert(0, value);
        }

        public void Delete(int index)
        {
            MemoryObservableCollection.RemoveAt(index);
        }

        public void Clear()
        {
            MemoryObservableCollection.Clear();
        }
    }
}
