using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Calc.Interfaces;
using Dapper;

namespace WpfApp1
{
    public class MemoryDB : IMemory
    {
        class Item
        {
            public int Id { get; set; }
            public double Value { get; set; }
        }
        public ObservableCollection<double> MemoryObservableCollection { get; }
        private string nameFile;

        public MemoryDB(string nameFile)
        {
            this.nameFile = nameFile;
            MemoryObservableCollection = new ObservableCollection<double>();
            using (var connection = new SQLiteConnection($"Data Source={nameFile};Version=3;"))
            {
                connection.Open();
                var items = connection.Query<Item>("select * from Memory");
                if (items.Any())
                {
                    foreach (var item in items)
                    {
                        MemoryObservableCollection.Add(item.Value);
                    }
                }
            }
        }

        public void Add(double value)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameFile};Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand(connection);
                command.CommandText =
                    @"Insert into Memory(Value) 
                    Values(@Value)";
                command.Parameters.AddWithValue("@Value", value);

                command.ExecuteNonQuery();
                MemoryObservableCollection.Add(value);
            }
        }

        public void Delete(int index)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameFile};Version=3;"))
            {
                connection.Open();
                connection.Query($"Delete from Memory Where Id = {index + 1}");
                MemoryObservableCollection.RemoveAt(index);
            }
        }

        public void Clear()
        {
            using (var connection = new SQLiteConnection($"Data Source={nameFile};Version=3;"))
            {
                connection.Open();
                connection.Query($"Delete from Memory");
                MemoryObservableCollection.Clear();
            }
            throw new NotImplementedException();
        }
    }
}
