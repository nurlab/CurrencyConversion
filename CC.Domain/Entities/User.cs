using CC.Infrastructure.BaseEntities;
using CC.Infrastructure.EntityEnum;

namespace CC.Domain.Entities
{
    public class User : AuditEntity
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string NormalizedUsername { get; set; }
        public Roles Role { get; set; }
    }
}
