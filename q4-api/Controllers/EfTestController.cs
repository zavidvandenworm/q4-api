using Domain.Entities;
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
                    Usage = GetPaginatedFilteredMonitoringQueryable(u.Board, u.Port, skip, limit, filterStart,
                        filterEnd).Select(c =>
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

    [HttpGet("monitor/{board:int}/{port:int}")]
    public async Task<IActionResult> ListMonitoring(int board, int port, DateTime? filterStart = null,
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

        var data = await _context.MonitoringData202009s
            .Where(m => m.Board == board && m.Port == port && m.Timestamp > filterStart && m.Timestamp < filterEnd)
            .OrderBy(m => m.Timestamp).ToListAsync();

        return Ok(new PortData
        {
            Port = port,
            Board = board,
            Usage = data.Select(d => new MonitorData
            {
                Codes = [d.Code, d.Code2],
                Duration = d.ShotTime,
                Machine = _context.ProductionData.First(p =>
                    filterStart < p.StartDate.ToDateTime(p.StartTime) &&
                    filterEnd > p.EndDate.ToDateTime(p.EndTime)).TreeviewId,
                Mold = _context.ProductionData.First(p =>
                    filterStart < p.StartDate.ToDateTime(p.StartTime) &&
                    filterEnd > p.EndDate.ToDateTime(p.EndTime)).Treeview2Id
            }).ToList()
        });
    }

    [HttpGet("machine/list")]
    public async Task<IActionResult> GetMachines(int skip = 0, int limit = 10, DateTime? filterStart = null,
        DateTime? filterEnd = null)
    {
        var view = await _context.Machines.Skip(skip).Take(limit).ToListAsync();

        return Ok(view);
    }


    [HttpGet("mold/list")]
    public Task<IActionResult> GetMold(int skip = 0, int limit = 10)
    {
        var view = _context.Molds.Skip(skip).Take(limit);
        return Task.FromResult<IActionResult>(Ok(view));
    }

    private IQueryable<MonitoringData202009> GetPaginatedFilteredMonitoringQueryable(int board, int port, int skip,
        int limit, DateTime? filterStart, DateTime? filterEnd)
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

        return _context.MonitoringData202009s
            .Where(c => c.Board == board && c.Port == port && c.Timestamp > filterStart &&
                        c.Timestamp < filterEnd).Skip(skip).Take(limit);
    }
}