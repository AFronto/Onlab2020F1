using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackoverflowGuide.API.DTOs.Post;
using StackoverflowGuide.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        IMapper mapper;
        IPostService postService;

        public PostController(IMapper mapper, IPostService postService)
        {
            this.mapper = mapper;
            this.postService = postService;
        }

        [HttpGet("suggestions/{threadId}")]
        public ActionResult<List<PostData>> GetSuggestions(string threadId)
        {
            try
            {
                var suggestions = postService.GetSuggestions(threadId);
                return suggestions.Select(mapper.Map<PostData>).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
