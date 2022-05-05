using System;
using System.Collections.Generic;

namespace AdminPortal.Data.Data
{
    public partial class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
