using AdminPortal.Common.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface IRolesRepository
    {
        Task<ICollection<RolesViewModel>> GetAllRoles();
    }
}
