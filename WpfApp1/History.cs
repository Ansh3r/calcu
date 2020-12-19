using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Calc.Interfaces;
using Calc.Models;

namespace WpfApp1
{
    public class History : IHistory
    {
        public ObservableCollection<Expression> ExpressionsHistory { get; }

        public History()
        {
            ExpressionsHistory = new ObservableCollection<Expression>();
        }
        public void Add(Expression expression)
        {
            ExpressionsHistory.Add(expression);
        }

        public void Delete(int index)
        {
            ExpressionsHistory.RemoveAt(index);
        }

        public void Clear()
        {
            ExpressionsHistory.Clear();
        }
    }
}
