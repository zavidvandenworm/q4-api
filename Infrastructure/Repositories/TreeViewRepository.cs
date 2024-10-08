using Dapper;
using q4_api;

namespace Infrastructure.Repositories;

public class TreeViewRepository
{
    public async Task<object> ListAsync(int skip = 0, int limit = 100)
    {
        using var conn = DatabaseFactory.GetConnection();

        var props = new DynamicParameters();
        props.Add("@limit", limit);
        props.Add("@skip", skip);
        
        var result = await conn.QueryAsync("SELECT * FROM treeview LIMIT @skip, @limit", props);

        return result;
    }
}