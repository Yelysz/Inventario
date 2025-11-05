using Dapper;
using InventoryApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InventoryApi.Data;

public class MovimientosInventarioRepository : IMovimientosInventarioRepository
{
    private readonly SqlConnectionFactory _factory;
    public MovimientosInventarioRepository(SqlConnectionFactory factory) => _factory = factory;

    public async Task<IEnumerable<InventoryMovement>> ListTopAsync(int top = 100)
    {
        using IDbConnection conn = _factory.Create();
        return await conn.QueryAsync<InventoryMovement>(
            $"SELECT TOP (@top) IdMovimiento, IdProducto, Fecha, Cantidad, IdTipoMovimiento, UltimaFechaActualizacion " +
            "FROM dbo.MovimientosInventario ORDER BY IdMovimiento DESC",
            new { top });
    }

    public async Task<IEnumerable<InventoryMovement>> ListByProductoAsync(int idProducto, int top = 100)
    {
        using IDbConnection conn = _factory.Create();
        return await conn.QueryAsync<InventoryMovement>(
            $"SELECT TOP (@top) IdMovimiento, IdProducto, Fecha, Cantidad, IdTipoMovimiento, UltimaFechaActualizacion" +
            "FROM dbo.MovimientosInventario WHERE IdProducto=@id ORDER BY IdMovimiento DESC",
            new { id = idProducto, top });
    }

    public async Task<InventoryResult> EntradaAsync(int idProducto, int cantidad)
    {
        using var conn = _factory.Create();
        try
        {
            return await conn.QuerySingleAsync<InventoryResult>(
                "dbo.spInventario_Entrada",
                new { IdProducto = idProducto, Cantidad = cantidad },
                commandType: CommandType.StoredProcedure
            );
        }
        catch (SqlException ex)
        {
            if (ex.Message.Contains("Cantidad debe ser positiva"))
                throw new InvalidOperationException("La cantidad debe ser mayor que cero.");
            throw;
        }
    }

    public async Task<InventoryResult> SalidaAsync(int idProducto, int cantidad)
    {
        using var conn = _factory.Create();
        try
        {
            return await conn.QuerySingleAsync<InventoryResult>(
                "dbo.spMovimientosInventario_Salida",
                new { IdProducto = idProducto, Cantidad = cantidad },
                commandType: CommandType.StoredProcedure
            );
        }
        catch (SqlException ex)
        {
            if (ex.Message.Contains("Stock insuficiente"))
                throw new InvalidOperationException("No hay suficiente stock para realizar la salida.");
            if (ex.Message.Contains("No hay inventario registrado"))
                throw new InvalidOperationException("El producto no tiene inventario registrado.");
            if (ex.Message.Contains("Cantidad debe ser positiva"))
                throw new InvalidOperationException("La cantidad debe ser mayor que cero.");
            throw;
        }
    }


    public async Task<IEnumerable<dynamic>> BajoMinimoAsync()
    {
        using var conn = _factory.Create();

        var result = await conn.QueryAsync(
            "dbo.spProductos_BajoMinimo",
            commandType: CommandType.StoredProcedure
        );

        return result;
    }
}
