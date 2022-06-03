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
        public string? FirstName { get; set; }
        public string? Lastname { get; set; }
        public string? EmployeeNo { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
