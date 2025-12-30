using LinqToDB;
using LinqToDB.Data;
using Millionaire.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Data.DataContext
{
    public class DBContext : DataConnection
    {
        public DBContext(string connectionString) : base(new DataOptions().UsePostgreSQL(connectionString)){ }
        public ITable<GamesModel> games => this.GetTable<GamesModel>();
    }
}
