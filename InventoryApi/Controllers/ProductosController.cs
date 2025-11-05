using Microsoft.AspNetCore.Mvc;
using InventoryApi.Data;
using InventoryApi.Models;

namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductosRepository _repo;
    public ProductosController(IProductosRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> List()
        => Ok(await _repo.ListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> Get(int id)
        => (await _repo.GetAsync(id)) is { } p ? Ok(p) : NotFound();
    public record ProductoCreateDto(string Nombre, string? Descripcion, decimal PrecioVenta, int MinimoExistencia);
    public record ProductoUpdateDto(string Nombre, string? Descripcion, decimal PrecioVenta, int MinimoExistencia);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductoCreateDto dto)
    {
        var created = await _repo.CreateAsync(dto.Nombre, dto.Descripcion, dto.PrecioVenta, dto.MinimoExistencia);
        return CreatedAtAction(nameof(Get), new { id = created.IdProducto }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductoUpdateDto dto)
    {
        var updated = await _repo.UpdateAsync(id, dto.Nombre, dto.Descripcion, dto.PrecioVenta, dto.MinimoExistencia);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => (await _repo.DeleteAsync(id)) ? NoContent() : NotFound();
}
