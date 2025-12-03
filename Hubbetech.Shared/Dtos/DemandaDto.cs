using Hubbetech.Shared.Enums;

namespace Hubbetech.Shared.Dtos
{
    public class DemandaDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public StatusDemanda Status { get; set; }
        public string? AssignedToUserId { get; set; }
    }
}
