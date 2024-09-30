using Dapper;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace q4_api.Controllers;

[ApiController]
[Route("v1")]
public class MonitoringController : ControllerBase
{
    [HttpGet("monitoring")]
    public async Task<IActionResult> Monitoring(int board, int port, int skip = 0, int limit = 10)
    {
        using var conn = await DatabaseFactory.GetConnection();

        var results =
            await conn.QueryAsync(
                "SELECT timestamp, id, shot_time as duration FROM monitoring_data_202009 WHERE port = @port AND board = @board ORDER BY timestamp DESC LIMIT @skip, @limit",
                new { board, port, skip, limit });

        return Ok(results);
    }
}