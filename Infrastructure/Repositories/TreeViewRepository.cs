using Dapper;

namespace Infrastructure.Repositories;

public class TreeViewRepository
{
    public async Task<object> ListAsync(int skip = 0, int limit = 100)
    {
        using var conn = await DatabaseFactory.GetConnection();
        
        var props = new DynamicParameters();
        props.Add("@limit", limit);
        props.Add("@skip", skip);
        
        var result = await conn.QueryAsync("SELECT * FROM treeview LIMIT @limit OFFSET @skip", props);
        return result;
    }

    // GetByIdAsync
    public async Task<object> GetByIdAsync(int id)
    {
        using var conn = await DatabaseFactory.GetConnection();
        
        var props = new DynamicParameters();
        props.Add("@id", id);
        
        var result = await conn.QueryFirstOrDefaultAsync("SELECT * FROM treeview WHERE id = @id", props);
        return result;
    }
}