using Dapper;
using InventoryApi.Models;
using System.Data;

namespace InventoryApi.Data;

public class ProductosRepository : IProductosRepository
{
    private readonly SqlConnectionFactory _factory;
    public ProductosRepository(SqlConnectionFactory factory) => _factory = factory;

    public async Task<IEnumerable<Product>> ListAsync()
    {
        using IDbConnection conn = _factory.Create();
        return await conn.QueryAsync<Product>("dbo.spProductos_List",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Product?> GetAsync(int id)
    {
        using IDbConnection conn = _factory.Create();
        return await conn.QueryFirstOrDefaultAsync<Product>(
            "dbo.spProductos_GetById",
            new { IdProducto = id },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<Product> CreateAsync(string nombre, string? descripcion, decimal precioVenta, int minimoExistencia, string? codigoProducto)
    {
        using IDbConnection conn = _factory.Create();
        return await conn.QuerySingleAsync<Product>(
            "dbo.spProductos_Create",
            new
            {
                Nombre = nombre,
                Descripcion = descripcion,
                PrecioVenta = precioVenta,
                MinimoExistencia = minimoExistencia,
                CodigoProducto = codigoProducto   // puede ir null o ""
            },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<Product?> UpdateAsync(int id, string nombre, string? descripcion, decimal precioVenta, int minimoExistencia, string? codigoProducto)
    {
        using IDbConnection conn = _factory.Create();
        return await conn.QueryFirstOrDefaultAsync<Product>(
            "dbo.spProductos_Update",
            new
            {
                IdProducto = id,
                Nombre = nombre,
                Descripcion = descripcion,
                PrecioVenta = precioVenta,
                MinimoExistencia = minimoExistencia,
                CodigoProducto = codigoProducto  // null / "" -> se deja a criterio del SP
            },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using IDbConnection conn = _factory.Create();
        var rows = await conn.QuerySingleAsync<int>(
            "dbo.spProductos_Delete",
            new { IdProducto = id },
            commandType: CommandType.StoredProcedure
        );
        return rows > 0;  // true => 204 NoContent, false => 404 NotFound
    }
}
