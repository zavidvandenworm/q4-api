using System.Data;
using MySqlConnector;
using System.Configuration;

namespace q4_api;

public static class DatabaseFactory
{
    public static IDbConnection GetConnection()
    {
        var connection = new MySqlConnection(Environment.GetEnvironmentVariable("CONNECTIONSTRING"));
        try
        {
            connection.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return connection;
    }
}