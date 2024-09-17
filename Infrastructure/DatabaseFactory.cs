using System.Data;
using MySqlConnector;
using System.Configuration;

namespace q4_api;

public static class DatabaseFactory
{
    public static IDbConnection GetConnection()
    {
        var connection = new MySqlConnection(Environment.GetEnvironmentVariable("CONNECTIONSTRING"));
        connection.Open();
        return connection;
    }
}