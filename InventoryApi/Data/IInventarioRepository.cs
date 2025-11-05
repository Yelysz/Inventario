using InventoryApi.Models;

namespace InventoryApi.Data;

public interface IInventarioRepository
{
    Task<IEnumerable<Inventory>> ListAsync();
    Task<Inventory?> GetByProductoAsync(int idProducto);
}
