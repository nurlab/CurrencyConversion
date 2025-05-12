namespace CC.Infrastructure.BaseEntities;

/// <summary>
/// Interface for base entities, providing common properties such as Id, IsDeleted, and IsActive.
/// </summary>
public interface IBaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    /// <value>
    /// A Guid representing the unique ID of the entity.
    /// </value>
    Guid Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is deleted.
    /// </summary>
    /// <value>
    /// A boolean flag that marks if the entity is deleted (soft delete).
    /// </value>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is active.
    /// </summary>
    /// <value>
    /// A boolean flag indicating whether the entity is active or not.
    /// </value>
    bool IsActive { get; set; }
}
