using System;
using System.Collections.Generic;

namespace AdminPortal.Data.Data
{
    public partial class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string? RoleName { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
