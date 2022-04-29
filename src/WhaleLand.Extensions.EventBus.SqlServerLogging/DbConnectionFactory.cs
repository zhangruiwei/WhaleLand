﻿using System.Data.SqlClient;

namespace WhaleLand.Extensions.EventBus.SqlServerLogging
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        public readonly string ConnectionString;

        public DbConnectionFactory(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        public System.Data.Common.DbConnection GetDbConnection()
        {
            var connection = new SqlConnection(this.ConnectionString);
            return connection;
        }
    }
}
