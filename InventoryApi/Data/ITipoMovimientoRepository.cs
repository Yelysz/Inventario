using InventoryApi.Models;

namespace InventoryApi.Data;

public interface ITipoMovimientoRepository
{
    Task<IEnumerable<MovementType>> ListAsync();
}
