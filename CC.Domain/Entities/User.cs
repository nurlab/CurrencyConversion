using CC.Infrastructure.BaseEntities;
using CC.Infrastructure.EntityEnum;

namespace CC.Domain.Entities
{
    /// <summary>
    /// Represents a user entity in the system.
    /// </summary>
    /// <remarks>
    /// The user entity includes the following properties:
    /// <list type="bullet">
    ///   <item><description>Username - The unique identifier for the user.</description></item>
    ///   <item><description>Password - The user's password.</description></item>
    ///   <item><description>NormalizedUsername - The normalized version of the username for case-insensitive comparison.</description></item>
    ///   <item><description>Role - The role assigned to the user (Admin, Manager, or User).</description></item>
    /// </list>
    /// </remarks>
    public class User : AuditEntity
    {
        /// <summary>
        /// Gets or sets the unique username for the user.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the password for the user.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the normalized version of the username for case-insensitive comparison.
        /// </summary>
        public required string NormalizedUsername { get; set; }

        /// <summary>
        /// Gets or sets the role assigned to the user.
        /// </summary>
        /// <remarks>
        /// The role is one of the values from the <see cref="Roles"/> enum:
        /// <list type="bullet">
        ///   <item><description>Admin</description></item>
        ///   <item><description>Manager</description></item>
        ///   <item><description>User</description></item>
        /// </list>
        /// </remarks>
        public Roles Role { get; set; }
    }
}
