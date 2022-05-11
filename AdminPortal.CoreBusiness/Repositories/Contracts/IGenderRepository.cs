using AdminPortal.Common.Models.CoursesViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface IGenderRepository
    {
        Task<ICollection<GenderVM>> GetAllGenders();
    }
}
