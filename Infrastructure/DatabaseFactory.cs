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
            // connectie is gelukt
            Console.WriteLine("Connection to database established");
            connection.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error 029329923923: " + e.Message);
            throw;
        }
        return connection;
    }
}