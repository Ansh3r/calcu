using System;
using System.Collections.ObjectModel;
using Calc.Models;

namespace Calc.Interfaces
{
    public interface IHistory
    {
        ObservableCollection<Expression> ExpressionsHistory { get; }
        void Add(Expression expression);
        void Delete(int index);
        void Clear();
    }
}
