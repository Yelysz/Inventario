using Microsoft.AspNetCore.Mvc;
using InventoryApi.Data;
using Microsoft.AspNetCore.Authorization;



namespace InventoryApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InventarioController : ControllerBase
{
    private readonly IInventarioRepository _repo;
    public InventarioController(IInventarioRepository repo) => _repo = repo;

    [HttpGet] public async Task<IActionResult> List() => Ok(await _repo.ListAsync());
    [HttpGet("{idProducto:int}")]
    public async Task<IActionResult> Get(int idProducto)
        => (await _repo.GetByProductoAsync(idProducto)) is { } v ? Ok(v) : NotFound();
}
