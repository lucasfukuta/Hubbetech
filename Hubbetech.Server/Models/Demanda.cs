using Hubbetech.Shared.Enums;

namespace Hubbetech.Server.Models
{
    public class Demanda
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public StatusDemanda Status { get; set; }
        public string? AssignedToUserId { get; set; }
        public ApplicationUser? AssignedToUser { get; set; }
    }
}
