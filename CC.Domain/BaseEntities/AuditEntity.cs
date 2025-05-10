namespace CC.Infrastructure.BaseEntities
{
    public abstract class AuditEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public long CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public long? UpdatedOn { get; set; }
    }
}
