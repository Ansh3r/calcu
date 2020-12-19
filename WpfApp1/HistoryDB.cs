using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Calc.Interfaces;
using Calc.Models;
using Dapper;

namespace WpfApp1
{
    public class HistoryDB : IHistory
    {
        public ObservableCollection<Expression> ExpressionsHistory { get; }
        private string nameFile;

        public HistoryDB(string nameFile)
        {
            this.nameFile = nameFile;
            ExpressionsHistory = new ObservableCollection<Expression>();
            using (var connection = new SQLiteConnection($"Data Source={nameFile};Version=3;"))
            {
                connection.Open();
                var expressions = connection.Query<Expression>("select * from Expressions");
                if (expressions.Any())
                {
                    foreach (var expression in expressions)
                    {
                        ExpressionsHistory.Add(expression);
                    }
                }
            }
        }

        public void Add(Expression expression)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameFile};Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand(connection);
                command.CommandText =
                    @"Insert into Expressions(Action, MathExpression, Result, ErrorMessage, HasError, Steps) 
                    Values(@Action, @MathExpression, @Result, @ErrorMessage, @HasError, @Steps)";
                command.Parameters.AddWithValue("@Action", expression.Action);
                command.Parameters.AddWithValue("@MathExpression", expression.MathExpression);
                command.Parameters.AddWithValue("@Result", expression.Result);
                command.Parameters.AddWithValue("@ErrorMessage", expression.ErrorMessage);
                command.Parameters.AddWithValue("@HasError", expression.HasError);
                command.Parameters.AddWithValue("@Steps", expression.Steps);

                command.ExecuteNonQuery();
                ExpressionsHistory.Add(expression);
            }
        }

        public void Delete(int index)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameFile};Version=3;"))
            {
                connection.Open();
                connection.Query($"Delete from Expressions Where Id = {index + 1}");
                ExpressionsHistory.RemoveAt(index);
            }
        }

        public void Clear()
        {
            using (var connection = new SQLiteConnection($"Data Source={nameFile};Version=3;"))
            {
                connection.Open();
                connection.Query($"Delete from Expressions");
                ExpressionsHistory.Clear();
            }
        }
    }
}
