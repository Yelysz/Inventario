using Microsoft.AspNetCore.Mvc;
using InventoryApi.Data;
using Microsoft.AspNetCore.Authorization;


namespace InventoryApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TipoMovimientoController : ControllerBase
{
    private readonly ITipoMovimientoRepository _repo;
    public TipoMovimientoController(ITipoMovimientoRepository repo) => _repo = repo;

    [HttpGet] public async Task<IActionResult> List() => Ok(await _repo.ListAsync());
}
