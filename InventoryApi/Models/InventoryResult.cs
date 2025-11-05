namespace InventoryApi.Models
{
    public class InventoryResult
    {
        public int IdProducto { get; set; }
        public int Existencia { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool BajoMinimo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
