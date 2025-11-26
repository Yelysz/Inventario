using InventoryApi.Data;
using InventoryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;


namespace InventoryApi.Controllers;

[Authorize]
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
        => (await _repo.GetAsync(id)) is { } p ? Ok(p) : NotFound(new { message = "Producto no encontrado." });

    // Ahora incluyen CodigoProducto opcional
    public record ProductoCreateDto(
        string Nombre,
        string? Descripcion,
        decimal PrecioVenta,
        int MinimoExistencia,
        string? CodigoProducto
    );

    public record ProductoUpdateDto(
        string Nombre,
        string? Descripcion,
        decimal PrecioVenta,
        int MinimoExistencia,
        string? CodigoProducto
    );

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductoCreateDto dto)
    {
        try
        {
            var created = await _repo.CreateAsync(
                dto.Nombre,
                dto.Descripcion,
                dto.PrecioVenta,
                dto.MinimoExistencia,
                dto.CodigoProducto
            );

            return CreatedAtAction(nameof(Get), new { id = created.IdProducto }, created);
        }
        catch (Exception ex)
        {
            // Aquí normalmente llegará el mensaje del RAISERROR (código duplicado, etc.)
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductoUpdateDto dto)
    {
        try
        {
            var updated = await _repo.UpdateAsync(
                id,
                dto.Nombre,
                dto.Descripcion,
                dto.PrecioVenta,
                dto.MinimoExistencia,
                dto.CodigoProducto
            );

            return updated is null
                ? NotFound(new { message = "Producto no encontrado." })
                : Ok(updated);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _repo.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "El producto no existe o ya estaba eliminado." });

            return Ok(new { message = "Producto eliminado correctamente." });
        }
        catch (SqlException ex) when (ex.Number == 50000) // RAISERROR custom
        {
            return BadRequest(new
            {
                message = ex.Message // "Este producto no puede eliminarse porque tiene inventario asociado."
            });
        }
        catch (SqlException ex) when (ex.Number == 547) // FK safety
        {
            return BadRequest(new
            {
                message = "Este producto no puede eliminarse porque está relacionado con Inventario o Movimientos."
            });
        }
    }

    //[HttpDelete("{id:int}")]
    //public async Task<IActionResult> Delete(int id)
    //{
    //    var deleted = await _repo.DeleteAsync(id);

    //    if (!deleted)
    //        return NotFound(new { message = "El producto no existe o ya estaba eliminado." });

    //    return Ok(new { message = "Producto eliminado correctamente." });
    //}
}
