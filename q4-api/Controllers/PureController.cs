using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using q4_api.Dto;

namespace q4_api.Controllers;

[ApiController]
[Route("v1/pure/")]
public class PureController : ControllerBase
{
    private readonly TreeViewRepository _treeViewRepository;

    public PureController(TreeViewRepository treeViewRepository)
    {
        _treeViewRepository = treeViewRepository;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<Treeview>>> GetAll([FromQuery] ListMachineDto dto)
    {
        var results = await _treeViewRepository.ListAsync(dto.Skip, dto.Limit);
        return Ok(results);
    }
}