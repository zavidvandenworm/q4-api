using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using q4_api.Context;
using q4_api.Dto;

namespace q4_api.Controllers;

[ApiController]
[Route("[controller]")]
public class EfTestController : ControllerBase
{
    private readonly Q4DbContext _context;

    public EfTestController(Q4DbContext context)
    {
        _context = context;
    }

    [HttpGet("monitor")]
    public async Task<IActionResult> ListBoards(int skip = 0, int limit = 10, DateTime? filterStart = null,
        DateTime? filterEnd = null)
    {
        if (filterStart == null)
        {
            if (filterEnd == null)
            {
                filterStart = DateTime.Now.AddMonths(-1);
                filterEnd = DateTime.Now;
            }
            else
            {
                filterStart = filterEnd.Value.AddMonths(-1);
            }
        }
        else
        {
            if (filterEnd == null)
            {
                filterEnd = filterStart.Value.AddMonths(1);
            }
        }

        var portsAndBoards = _context.MachineMonitoringPoortens.Select(m => new { m.Board, m.Port }).Distinct().ToList()
            .Select(
                u => new PortData
                {
                    Port = u.Port,
                    Board = u.Board,
                    Usage = _context.MonitoringData202009s
                        .Where(c => c.Board == u.Board && c.Port == u.Port && c.Timestamp > filterStart &&
                                    c.Timestamp < filterEnd).Skip(skip).Take(limit).Select(c =>
                            new MonitorData
                            {
                                Codes = new List<int> { c.Code, c.Code2 },
                                Duration = c.ShotTime,
                                Timestamp = c.Timestamp ?? DateTime.UnixEpoch,
                                Machine = _context.ProductionData.First(p =>
                                    c.Timestamp > p.StartDate.ToDateTime(p.StartTime) &&
                                    c.Timestamp < p.EndDate.ToDateTime(p.EndTime)).TreeviewId,
                                Mold = _context.ProductionData.First(p =>
                                    c.Timestamp > p.StartDate.ToDateTime(p.StartTime) &&
                                    c.Timestamp < p.EndDate.ToDateTime(p.EndTime)).Treeview2Id
                            }).ToList()
                }).ToList();

        return Ok(portsAndBoards);
    }

    [HttpGet("machine/list")]
    public async Task<IActionResult> GetMachines(int skip = 0, int limit = 10, DateTime? filterStart = null,
        DateTime? filterEnd = null)
    {
        if (filterStart == null)
        {
            if (filterEnd == null)
            {
                filterStart = DateTime.Now.AddMonths(-1);
                filterEnd = DateTime.Now;
            }
            else
            {
                filterStart = filterEnd.Value.AddMonths(-1);
            }
        }
        else
        {
            if (filterEnd == null)
            {
                filterEnd = filterStart.Value.AddMonths(1);
            }
        }

        var view = await _context.Machines.Skip(skip).Take(limit).ToListAsync();

        return Ok(view);
    }


    [HttpGet("mold/list")]
    public Task<IActionResult> GetMold(int skip = 0, int limit = 10)
    {
        var view = _context.Molds.Skip(skip).Take(limit);
        return Task.FromResult<IActionResult>(Ok(view));
    }
}