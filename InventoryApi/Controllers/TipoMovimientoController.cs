using Microsoft.AspNetCore.Mvc;
using InventoryApi.Data;

namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TipoMovimientoController : ControllerBase
{
    private readonly ITipoMovimientoRepository _repo;
    public TipoMovimientoController(ITipoMovimientoRepository repo) => _repo = repo;

    [HttpGet] public async Task<IActionResult> List() => Ok(await _repo.ListAsync());
}
