using System.Data;
using MySqlConnector;
using System.Configuration;

namespace q4_api;

public static class DatabaseFactory
{
    public static IDbConnection GetConnection()
    {
        var connection = new MySqlConnection("Server=156.67.83.101;Port=6203;User=mysql;Database=default;Password=zdZCwqeKfmJOPktwK5X0lbLZMd1jkXALUv59PZ9ozOE43VxHv3Ax9Tet4JdtF4Lf");
        connection.Open();
        return connection;
    }
}