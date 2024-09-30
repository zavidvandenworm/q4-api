using System.Data;
using MySqlConnector;

namespace Infrastructure;

public static class DatabaseFactory
{
    public static async Task<IDbConnection> GetConnection()
    {
        var connection = new MySqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
        await connection.OpenAsync();
        return connection;
    }
}