using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Services
{
    public interface IPasswordHashService
    {
        string GeneratePasswordHash(string password);
    }
}
