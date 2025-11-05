namespace InventoryApi.Models;

public class Product
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = default!;
    public string? Descripcion { get; set; }
    public decimal PrecioVenta { get; set; }
    public int MinimoExistencia { get; set; }      // nuevo campo
    public DateTime FechaCreacion { get; set; }
    public DateTime UltimaFechaActualizacion { get; set; }
}