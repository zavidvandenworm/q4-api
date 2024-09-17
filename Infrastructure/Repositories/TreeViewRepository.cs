using Dapper;
using q4_api;

namespace Infrastructure.Repositories;

public class TreeViewRepository
{
    public async Task<object> ListAsync(int skip = 0, int limit = 100)
    {
        Console.WriteLine("VERBINDING NU");
        Console.WriteLine("dbstring is " + Environment.GetEnvironmentVariable("CONNECTIONSTRING"));
        var conn = DatabaseFactory.GetConnection();
        Console.WriteLine("verbinding open");
        var props = new DynamicParameters();
        props.Add("@limit", limit);
        props.Add("@skip", skip);
        
        var result = await conn.QueryAsync("SELECT * FROM treeview LIMIT @limit OFFSET @skip", new { limit, skip });
        Console.WriteLine("query uitgevoerd");
        conn.Close();
        return result;
    }
}