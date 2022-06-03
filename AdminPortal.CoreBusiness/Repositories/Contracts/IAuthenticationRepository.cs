using AdminPortal.Common.Models.AuthenicationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    
    public interface IAuthenticationRepository
    {
        Task<ClaimsPrincipal> Login(LoginViewModel loginViewModel);
    }
}
