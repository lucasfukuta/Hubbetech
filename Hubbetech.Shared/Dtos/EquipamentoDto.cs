namespace Hubbetech.Shared.Dtos
{
    public class EquipamentoDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // e.g., Available, InUse, Maintenance
    }
}
