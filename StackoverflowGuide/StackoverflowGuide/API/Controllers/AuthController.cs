using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StackoverflowGuide.API.DTOs;
using StackoverflowGuide.BLL.Models;
using StackoverflowGuide.BLL.Models.Auth;
using StackoverflowGuide.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        IAuthService authService;
        IMapper mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthData>> PostAsync([FromBody]LoginData model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var id = await authService.LogUserIn(mapper.Map<Login>(model));

                return authService.GetAuthData(id);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthData>> PostAsync([FromBody]RegisterData model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var id = await authService.CreateNewUser(mapper.Map<Registration>(model));

                return authService.GetAuthData(id);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

    }
}
