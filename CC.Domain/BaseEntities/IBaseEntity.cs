namespace CC.Infrastructure.BaseEntities;

public interface IBaseEntity
{
    Guid Id { get; set; }
    bool IsDeleted { get; set; }
    bool IsActive { get; set; }
}
