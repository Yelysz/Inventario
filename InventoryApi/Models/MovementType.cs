namespace InventoryApi.Models
{
    public class MovementType
    {
        public int IdTipoMovimiento { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}
