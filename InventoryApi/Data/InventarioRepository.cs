using Dapper;
using InventoryApi.Models;
using System.Data;

namespace InventoryApi.Data;

public class InventarioRepository : IInventarioRepository
{
    private readonly SqlConnectionFactory _factory;
    public InventarioRepository(SqlConnectionFactory factory) => _factory = factory;

    public async Task<IEnumerable<Inventory>> ListAsync()
    {
        using IDbConnection conn = _factory.Create();
        return await conn.QueryAsync<Inventory>(
            "SELECT IdProducto, Existencia, UltimaFechaActualizacion FROM dbo.Inventario ORDER BY IdProducto DESC");
    }

    public async Task<Inventory?> GetByProductoAsync(int idProducto)
    {
        using IDbConnection conn = _factory.Create();
        return await conn.QueryFirstOrDefaultAsync<Inventory>(
            "SELECT IdProducto, Existencia, UltimaFechaActualizacion FROM dbo.Inventario WHERE IdProducto=@id",
            new { id = idProducto });
    }
}
