using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Interfaces
{
    interface IMemory
    {
        ObservableCollection<double> MemoryObservableCollection { get; }
        void Add(double value);
        void Delete(int index);
        void Clear();
    }
}
