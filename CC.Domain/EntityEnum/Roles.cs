using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CC.Infrastructure.EntityEnum
{
    public enum Roles
    {
        [EnumMember(Value = "Admin")]
        Admin = 1,

        [EnumMember(Value = "Manager")]
        Manager = 2,

        [EnumMember(Value = "User")]
        User = 3
    }
}
