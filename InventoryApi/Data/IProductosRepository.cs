using InventoryApi.Models;

namespace InventoryApi.Data;

public interface IProductosRepository
{
    Task<IEnumerable<Product>> ListAsync();
    Task<Product?> GetAsync(int id);
    Task<Product> CreateAsync(string nombre, string? descripcion, decimal precioVenta, int MinimoExistencia);
    Task<Product?> UpdateAsync(int id, string nombre, string? descripcion, decimal precioVenta, int MinimoExistencia);
    Task<bool> DeleteAsync(int id);
}
