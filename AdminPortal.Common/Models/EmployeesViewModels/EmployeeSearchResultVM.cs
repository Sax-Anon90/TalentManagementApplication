﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.Common.Models.EmployeesViewModels
{
    public class EmployeeSearchResultVM
    {
        public int Id { get; set; }
        public string? EmployeeNo { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
