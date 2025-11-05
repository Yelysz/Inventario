using Dapper;
using InventoryApi.Models;
using System.Data;

namespace InventoryApi.Data;

public class TipoMovimientoRepository : ITipoMovimientoRepository
{
    private readonly SqlConnectionFactory _factory;
    public TipoMovimientoRepository(SqlConnectionFactory factory) => _factory = factory;

    public async Task<IEnumerable<MovementType>> ListAsync()
    {
        using IDbConnection conn = _factory.Create();
        return await conn.QueryAsync<MovementType>(
            "SELECT IdTipoMovimiento, Nombre, UltimaFechaActualizacion FROM dbo.TipoMovimiento ORDER BY IdTipoMovimiento");
    }
}
