using Ecommerce.ViewModel.Common;
using Ecommerce.ViewModel.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.AdminApp.Service
{
    public interface IUserAPIClient
    {
        Task<string> Authenticate(LoginRequest request);
        Task<PageResult<UserVm>> GetUserPagings(GetUserPagingRequest request);
        Task<bool> RegisterUser(RegisterRequest register);
    }
}
