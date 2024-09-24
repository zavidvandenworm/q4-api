using Dapper;

namespace Infrastructure.Repositories;

public class BoardRepository
{
    public async Task<IEnumerable<int>> ListBoards(int limit = 5, int skip = 0)
    {
        using var conn = await DatabaseFactory.GetConnection();

        var props = new DynamicParameters();
        props.Add("@limit", limit);
        props.Add("@skip", skip);

        var result = await conn.QueryAsync<int>("SELECT DISTINCT board FROM machine_monitoring_poorten LIMIT @skip, @limit", props);
        
        return result;
    }

    private async Task<object> GetMoldsByBoard(int boardId)
    {
        using var conn = await DatabaseFactory.GetConnection();
        
        var props = new DynamicParameters();
        props.Add("@boardId", boardId);

        var results = await conn.QueryAsync("SELECT * FROM production_data WHERE board = @boardId", props);
        return results;
    }

    public async Task<object> ListBoardsAndChildren(int limit = 5, int skip = 0)
    {
        using var conn = await DatabaseFactory.GetConnection();
        
        var props = new DynamicParameters();
        props.Add("@limit", limit);
        props.Add("@skip", skip);

        var result = await conn.QueryAsync("SELECT", props);
        return result;
    }

    public async Task<object> ListPortsByBoard(int board, int limit = 5, int skip = 0)
    {
        using var conn = await DatabaseFactory.GetConnection();
        
        var props = new DynamicParameters();
        props.Add("@board", board);
        props.Add("@limit", limit);
        props.Add("@skip", skip);

        var result = await conn.QueryAsync("SELECT * FROM machine_monitoring_poorten WHERE board = @board LIMIT @skip, @limit", props);
        
        return result;
    }
}