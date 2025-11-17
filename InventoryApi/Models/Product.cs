namespace InventoryApi.Models;

public class Product
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = default!;
    public string? Descripcion { get; set; }
    public decimal PrecioVenta { get; set; }
    public int MinimoExistencia { get; set; }
    public string? CodigoProducto { get; set; }  // puede venir null o vacío
    public DateTime FechaCreacion { get; set; }
    public DateTime UltimaFechaActualizacion { get; set; }
    public bool IsDeleted { get; set; }          // para borrado lógico
}