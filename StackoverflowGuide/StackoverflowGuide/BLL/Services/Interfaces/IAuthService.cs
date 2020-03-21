using StackoverflowGuide.API.DTOs;
using StackoverflowGuide.BLL.Models;
using StackoverflowGuide.BLL.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<Guid> CreateNewUser(Registration registration);
        public Task<Guid> LogUserIn(Login login);
        public AuthData GetAuthData(Guid id);
    }
}
