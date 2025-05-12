using System.Runtime.Serialization;

namespace CC.Infrastructure.EntityEnum
{
    /// <summary>
    /// Represents the different roles in the system.
    /// </summary>
    /// <remarks>
    /// This enum defines the following roles:
    /// <list type="bullet">
    ///   <item><description>Admin - Highest level of access</description></item>
    ///   <item><description>Manager - Intermediate access with management privileges</description></item>
    ///   <item><description>User - Basic user with limited access</description></item>
    /// </list>
    /// </remarks>
    public enum Roles
    {
        /// <summary>
        /// Represents an administrator role with full access.
        /// </summary>
        [EnumMember(Value = "Admin")]
        Admin = 1,

        /// <summary>
        /// Represents a manager role with management privileges.
        /// </summary>
        [EnumMember(Value = "Manager")]
        Manager = 2,

        /// <summary>
        /// Represents a user role with basic access.
        /// </summary>
        [EnumMember(Value = "User")]
        User = 3
    }
}
