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











}