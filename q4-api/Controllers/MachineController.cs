using Dapper;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace q4_api.Controllers;

[ApiController]
[Route("v1")]
public class MachineController : ControllerBase
{
    [HttpGet("machine/all/molds/history")]
    public async Task<IActionResult> GetMachinesMoldsHistory()
    {
        using var conn = await DatabaseFactory.GetConnection();

        var results = await conn.QueryAsync(
            @"SELECT a.board, a.port, TIMESTAMP(b.start_date, b.start_time) as start, TIMESTAMP(b.end_date, b.end_time) as end, c.id as machine, d.id as hothalf FROM machine_monitoring_poorten a LEFT JOIN production_data b on a.board = b.board and a.port = b.port LEFT JOIN treeview c on c.id = b.treeview_id LEFT JOIN treeview d on d.id = b.treeview2_id WHERE b.start_date IS NOT NULL AND c.naam IS NOT NULL AND d.naam IS NOT NULL AND b.end_date IS NOT NULL");

        return Ok(results);
    }

    [HttpGet("machine/{id}/molds/history")]
    public async Task<IActionResult> GetMachineMoldsHistory(int id)
    {
        using var conn = await DatabaseFactory.GetConnection();

        var results = await conn.QueryAsync(
            "SELECT a.board, a.port, TIMESTAMP(b.start_date, b.start_time) as start, TIMESTAMP(b.end_date, b.end_time) as end, c.id as machine, d.id as hothalf FROM machine_monitoring_poorten a \nLEFT JOIN production_data b on a.board = b.board and a.port = b.port\nLEFT JOIN treeview c on c.id = b.treeview_id\nLEFT JOIN treeview d on d.id = b.treeview2_id\nWHERE b.start_date IS NOT NULL AND c.naam IS NOT NULL AND d.naam IS NOT NULL AND b.end_date IS NOT NULL AND c.id = @id",
            new { id });

        return Ok(results);
    }

    [HttpGet("machine/all/molds/current")]
    public async Task<IActionResult> GetMachinesMoldsCurrent()
    {
        using var conn = await DatabaseFactory.GetConnection();

        var results = await conn.QueryAsync(
            "SELECT a.board, a.port, TIMESTAMP(b.start_date, b.start_time) as start, c.id as machine, d.id as hothalf FROM machine_monitoring_poorten a \nLEFT JOIN production_data b on a.board = b.board and a.port = b.port\nLEFT JOIN treeview c on c.id = b.treeview_id\nLEFT JOIN treeview d on d.id = b.treeview2_id\nWHERE b.start_date IS NOT NULL AND c.naam IS NOT NULL AND d.naam IS NOT NULL AND b.end_date IS NULL");

        return Ok(results);
    }

    [HttpGet("machine/{id}/molds/current")]
    public async Task<IActionResult> GetMachinesMoldsCurrent(int id)
    {
        using var conn = await DatabaseFactory.GetConnection();

        var results = await conn.QueryAsync(
            "SELECT a.board, a.port, TIMESTAMP(b.start_date, b.start_time) as start, c.id as machine, d.id as hothalf FROM machine_monitoring_poorten a \nLEFT JOIN production_data b on a.board = b.board and a.port = b.port\nLEFT JOIN treeview c on c.id = b.treeview_id\nLEFT JOIN treeview d on d.id = b.treeview2_id\nWHERE b.start_date IS NOT NULL AND c.naam IS NOT NULL AND d.naam IS NOT NULL AND b.end_date IS NULL AND c.id = @id",
            new { id });

        return Ok(results);
    }

    [HttpGet("machine/list")]
    public async Task<IActionResult> GetMachineList(DateTime timeStart, DateTime timeEnd, int skip = 0, int limit = 10)
    {
        using var conn = await DatabaseFactory.GetConnection();

        var results = await conn.QueryAsync(
            "SELECT DISTINCT(a.treeview_id) as id, COUNT(c.id) as actions, coalesce(SUM(c.shot_time), 0) as total_action_duration, b.laatste_beurt_datum as last_serviced FROM production_data a LEFT JOIN treeview b ON a.treeview2_id = b.id LEFT JOIN (SELECT id, shot_time, port, board from monitoring_data_202009 limit 0,500) c ON a.port = c.port AND a.board = c.board WHERE timestamp(a.start_date, a.start_time) > @timeStart AND timestamp(a.end_date, a.end_time) < @timeEnd GROUP BY a.treeview_id, b.laatste_beurt_datum LIMIT @skip , @limit",
            new { timeStart, timeEnd, skip, limit });

        return Ok(results);
    }

    [HttpGet("machine/{id}/usage/")]
    public async Task<IActionResult> GetMachineHealth(int id)
    {
        using var conn = await DatabaseFactory.GetConnection();

        var result =
            await conn.QueryAsync(
                "SELECT COUNT(a.id) as actions, SUM(a.shot_time) FROM monitoring_data_202009 a LEFT JOIN production_data b ON a.board = @board GROUP BY board, port",
                new { id });

        return Ok(result);
    }
}