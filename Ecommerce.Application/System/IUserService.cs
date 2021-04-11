using Ecommerce.ViewModel.Common;
using Ecommerce.ViewModel.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.System
{
    public interface IUserService
    {
        Task<string> Authencate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
        Task<PageResult<UserVm>> GetUserPaging(GetUserPagingRequest request);
    }
}
