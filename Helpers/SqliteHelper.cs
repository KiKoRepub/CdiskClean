using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdiskClean.Helpers
{
    internal class SqliteHelper
    {

        private readonly string ConnectionSQL;


        public SqliteHelper(string ConnectionSQL)
        {
            this.ConnectionSQL = ConnectionSQL;
        }



        public int executeNonQuery(string sql, List<(string, object)> paramList)
        {
            using (SqliteConnection connection = new SqliteConnection(ConnectionSQL))
            {

                connection.Open();

                using (SqliteCommand command = connection.CreateCommand())
                {

                    foreach (var item in paramList)
                    {
                        command.Parameters.AddWithValue(item.Item1, item.Item2);
                    }
                    


                }



            }

            return 0;
        }
    }
}
