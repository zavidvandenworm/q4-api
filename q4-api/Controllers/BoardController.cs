using Dapper;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace q4_api.Controllers;

[ApiController]
[Route("v1")]
public class BoardController : ControllerBase
{
    [HttpGet("board/list")]
    public async Task<IActionResult> ListBoardsExtensive()
    {
        using var conn = await DatabaseFactory.GetConnection();

        var results = await conn.QueryAsync("SELECT board, port, name FROM machine_monitoring_poorten");

        var resultsGood = new List<object>();

        var enumerable = results as dynamic[] ?? results.ToArray();
        foreach (var board in enumerable.DistinctBy(r => r.board))
        {
            resultsGood.Add(new
            {
                board.board,
                ports = enumerable.Where(r => r.board == board.board).Select(r => new
                {
                    r.port,
                    r.name
                })
            });
        }

        return Ok(resultsGood);
    }

    [HttpGet("machine/all/molds/history")]
    public async Task<IActionResult> GetMachinesMoldsHistory()
    {
        using var conn = await DatabaseFactory.GetConnection();

        var results = await conn.QueryAsync(
            "SELECT a.board, a.port, TIMESTAMP(b.start_date, b.start_time) as start, TIMESTAMP(b.end_date, b.end_time) as end, c.id as machine, d.id as hothalf FROM machine_monitoring_poorten a \nLEFT JOIN production_data b on a.board = b.board and a.port = b.port\nLEFT JOIN treeview c on c.id = b.treeview_id\nLEFT JOIN treeview d on d.id = b.treeview2_id\nWHERE b.start_date IS NOT NULL AND c.naam IS NOT NULL AND d.naam IS NOT NULL AND b.end_date IS NOT NULL");

        return Ok(results);
    }
    
    [HttpGet("machine/{id}/molds/history")]
    public async Task<IActionResult> GetMachineMoldsHistory(int id)
    {
        using var conn = await DatabaseFactory.GetConnection();

        var results = await conn.QueryAsync(
            "SELECT a.board, a.port, TIMESTAMP(b.start_date, b.start_time) as start, TIMESTAMP(b.end_date, b.end_time) as end, c.id as machine, d.id as hothalf FROM machine_monitoring_poorten a \nLEFT JOIN production_data b on a.board = b.board and a.port = b.port\nLEFT JOIN treeview c on c.id = b.treeview_id\nLEFT JOIN treeview d on d.id = b.treeview2_id\nWHERE b.start_date IS NOT NULL AND c.naam IS NOT NULL AND d.naam IS NOT NULL AND b.end_date IS NOT NULL AND c.id = @id", new {id});

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
            "SELECT a.board, a.port, TIMESTAMP(b.start_date, b.start_time) as start, c.id as machine, d.id as hothalf FROM machine_monitoring_poorten a \nLEFT JOIN production_data b on a.board = b.board and a.port = b.port\nLEFT JOIN treeview c on c.id = b.treeview_id\nLEFT JOIN treeview d on d.id = b.treeview2_id\nWHERE b.start_date IS NOT NULL AND c.naam IS NOT NULL AND d.naam IS NOT NULL AND b.end_date IS NULL AND c.id = @id", new {id});

        return Ok(results);
    }

    [HttpGet("mold/{id}/history")]
    public async Task<IActionResult> GetMoldHistory(int id)
    {
        using var conn = await DatabaseFactory.GetConnection();
        
        var results = await conn.QueryAsync(
            "SELECT a.board, a.port, TIMESTAMP(b.start_date, b.start_time) as start, c.id as machine, d.id as hothalf FROM machine_monitoring_poorten a \nLEFT JOIN production_data b on a.board = b.board and a.port = b.port\nLEFT JOIN treeview c on c.id = b.treeview_id\nLEFT JOIN treeview d on d.id = b.treeview2_id\nWHERE b.start_date IS NOT NULL AND c.naam IS NOT NULL AND d.naam IS NOT NULL AND b.end_date IS NOT NULL AND d.id = @id", new {id});

        return Ok(results);
    }

    [HttpGet("mold/{id}/current")]
    public async Task<IActionResult> GetMoldCurrent(int id)
    {
        using var conn = await DatabaseFactory.GetConnection();
        
        var results = await conn.QueryAsync(
            "SELECT a.board, a.port, TIMESTAMP(b.start_date, b.start_time) as start, c.id as machine, d.id as hothalf FROM machine_monitoring_poorten a \nLEFT JOIN production_data b on a.board = b.board and a.port = b.port\nLEFT JOIN treeview c on c.id = b.treeview_id\nLEFT JOIN treeview d on d.id = b.treeview2_id\nWHERE b.start_date IS NOT NULL AND c.naam IS NOT NULL AND d.naam IS NOT NULL AND b.end_date IS NULL AND d.id = @id", new {id});

        return Ok(results);

    }

    [HttpGet("mold/list")]
    public async Task<IActionResult> GetMoldList()
    {
        using var conn = await DatabaseFactory.GetConnection();

        var results = await conn.QueryAsync("SELECT DISTINCT(a.treeview2_id) as id, b.naam as name FROM production_data a LEFT JOIN treeview b ON a.treeview2_id = b.id");

        return Ok(results);
    }


    [HttpGet("machine/list")]
    public async Task<IActionResult> GetMachineList()
    {
        using var conn = await DatabaseFactory.GetConnection();

        var results = await conn.QueryAsync("SELECT DISTINCT(a.treeview_id) as id, b.naam as name FROM production_data a LEFT JOIN treeview b ON a.treeview2_id = b.id");

        return Ok(results);
    }

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

    [HttpGet("machine/{id}/usage/")]
    public async Task<IActionResult> GetMachineHealth(int id)
    {
        using var conn = await DatabaseFactory.GetConnection();

        var result = await conn.QueryAsync("SELECT COUNT(*) as actions, SUM(shot_time) FROM monitoring_data_202009 GROUP BY board, port", new {id});

        return Ok(result);
    }
}