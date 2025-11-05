namespace InventoryApi.Models
{
    public class InventoryMovement
    {
        public int IdMovimiento { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public int IdTipoMovimiento { get; set; }
        public DateTime FechaMovimiento { get; set; }
    }
}
