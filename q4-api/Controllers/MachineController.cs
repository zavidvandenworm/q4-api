using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using q4_api.Dto;

namespace q4_api.Controllers;


[ApiController]
[Route("api/pure/")]
public class MachineController : ControllerBase
{
    private readonly TreeViewRepository _treeViewRepository;

    public MachineController(TreeViewRepository treeViewRepository)
    {
        _treeViewRepository = treeViewRepository;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAll([FromQuery] ListMachineDto dto)
    {
        var results = await _treeViewRepository.ListAsync(dto.Skip, dto.Limit);
        return Ok(results);
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("yahoo");
    }
}