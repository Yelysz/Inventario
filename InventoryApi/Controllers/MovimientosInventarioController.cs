using Microsoft.AspNetCore.Mvc;
using InventoryApi.Data;


namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovimientosInventarioController : ControllerBase
{
    private readonly IMovimientosInventarioRepository _repo;
    public MovimientosInventarioController(IMovimientosInventarioRepository repo) => _repo = repo;
    public record MovimientoDto(int IdProducto, int Cantidad);

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int top = 100)
        => Ok(await _repo.ListTopAsync(top));

    [HttpGet("producto/{idProducto:int}")]
    public async Task<IActionResult> ListByProducto(int idProducto, [FromQuery] int top = 100)
        => Ok(await _repo.ListByProductoAsync(idProducto, top));

    [HttpPost("entrada")]
    public async Task<IActionResult> Entrada([FromBody] MovimientoDto dto)
        => Ok(await _repo.EntradaAsync(dto.IdProducto, dto.Cantidad));

    [HttpPost("salida")]
    public async Task<IActionResult> Salida([FromBody] MovimientoDto dto)
    {
        try
        {
            var inv = await _repo.SalidaAsync(dto.IdProducto, dto.Cantidad);
            return Ok(inv);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("bajo-minimo")]
    public async Task<IActionResult> BajoMinimo()
    {
        var productos = await _repo.BajoMinimoAsync();

        if (!productos.Any())
            return Ok(new { message = "No hay productos por debajo del mínimo de existencia." });

        return Ok(productos);
    }

}
