using Dapper;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace q4_api.Controllers;

[ApiController]
[Route("v1")]
public class MoldController : ControllerBase
{
    [HttpGet("mold/{id}/history")]
    public async Task<IActionResult> GetMoldHistory(int id)
    {
        using var conn = await DatabaseFactory.GetConnection();

        var results = await conn.QueryAsync(
            "SELECT a.board, a.port, TIMESTAMP(b.start_date, b.start_time) as start, c.id as machine, d.id as hothalf FROM machine_monitoring_poorten a \nLEFT JOIN production_data b on a.board = b.board and a.port = b.port\nLEFT JOIN treeview c on c.id = b.treeview_id\nLEFT JOIN treeview d on d.id = b.treeview2_id\nWHERE b.start_date IS NOT NULL AND c.naam IS NOT NULL AND d.naam IS NOT NULL AND b.end_date IS NOT NULL AND d.id = @id",
            new { id });

        return Ok(results);
    }

    [HttpGet("mold/{id}/current")]
    public async Task<IActionResult> GetMoldCurrent(int id)
    {
        using var conn = await DatabaseFactory.GetConnection();

        var results = await conn.QueryAsync(
            "SELECT a.board, a.port, TIMESTAMP(b.start_date, b.start_time) as start, c.id as machine, d.id as hothalf FROM machine_monitoring_poorten a \nLEFT JOIN production_data b on a.board = b.board and a.port = b.port\nLEFT JOIN treeview c on c.id = b.treeview_id\nLEFT JOIN treeview d on d.id = b.treeview2_id\nWHERE b.start_date IS NOT NULL AND c.naam IS NOT NULL AND d.naam IS NOT NULL AND b.end_date IS NULL AND d.id = @id",
            new { id });

        return Ok(results);
    }

    [HttpGet("mold/list")]
    public async Task<IActionResult> GetMoldList()
    {
        using var conn = await DatabaseFactory.GetConnection();

        var results =
            await conn.QueryAsync(
                "SELECT DISTINCT(a.treeview2_id) as id, b.naam as name FROM production_data a LEFT JOIN treeview b ON a.treeview2_id = b.id");

        return Ok(results);
    }
}