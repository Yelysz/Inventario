namespace InventoryApi.Models
{
    public class Inventory
    {
        public int IdProducto { get; set; }
        public int Existencia { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
