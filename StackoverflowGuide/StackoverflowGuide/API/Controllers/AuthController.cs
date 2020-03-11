using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StackoverflowGuide.API.DTOs;
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
        public ActionResult<AuthData> Post([FromBody]LoginData model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return new AuthData();
        }

        [HttpPost("register")]
        public ActionResult<AuthData> Post([FromBody]RegisterData model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return new AuthData();
        }

    }
}
