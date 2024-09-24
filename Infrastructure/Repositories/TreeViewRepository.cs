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

        var result = await conn.QueryAsync("SELECT a.board, a.port, TIMESTAMP(b.start_date, b.start_time) as start, c.id as machine, d.id as hothalf FROM machine_monitoring_poorten a \nLEFT JOIN production_data b on a.board = b.board and a.port = b.port\nLEFT JOIN treeview c on c.id = b.treeview_id\nLEFT JOIN treeview d on d.id = b.treeview2_id\nWHERE b.start_date IS NOT NULL AND c.naam IS NOT NULL AND d.naam IS NOT NULL AND b.end_date IS NULL AND c.id = @id", props);
        return result;
    }
}