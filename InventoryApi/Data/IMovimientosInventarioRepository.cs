using InventoryApi.Models;

namespace InventoryApi.Data;

public interface IMovimientosInventarioRepository
{
    Task<IEnumerable<InventoryMovement>> ListTopAsync(int top = 100);
    Task<IEnumerable<InventoryMovement>> ListByProductoAsync(int idProducto, int top = 100);
    Task<InventoryResult> EntradaAsync(int idProducto, int cantidad);
    Task<InventoryResult> SalidaAsync(int idProducto, int cantidad);
    Task<IEnumerable<dynamic>> BajoMinimoAsync();
}
