using System.Data;
using Dapper;

namespace Infrastructure.Repositories;

public class MoldRepository
{
    public async Task<object> ListMolds(int limit, int skip)
    {
        using var conn = await DatabaseFactory.GetConnection();

        var props = new DynamicParameters();
        props.Add("@limit", limit);
        props.Add("@skip", skip);

        var results = await conn.QueryAsync("SELECT ", props);

        return results;
    }
}