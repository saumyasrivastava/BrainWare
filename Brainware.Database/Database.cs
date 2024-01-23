using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainWare.Api
{
    using Brainware.Common;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class Database : IDatabase
    {
        private readonly SqlConnection _connection;

        public Database()
        {
            _connection = new SqlConnection(ApplicationConstants.SqlConnectionString);
            _connection.Open();
        }

        public DbDataReader ExecuteReader(string query)
        {
            var sqlQuery = new SqlCommand(query, _connection);

            return sqlQuery.ExecuteReader();
        }

        public int ExecuteNonQuery(string query)
        {
            var sqlQuery = new SqlCommand(query, _connection);

            return sqlQuery.ExecuteNonQuery();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose of managed resources
                if (_connection != null)
                {
                    _connection.Dispose();
                }
            }
        }

        // Destructor (Finalizer)
        ~Database()
        {
            Dispose(false);
        }
    }
}