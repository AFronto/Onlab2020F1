using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackoverflowGuide.API.DTOs.Post;
using StackoverflowGuide.API.DTOs.Tag;
using StackoverflowGuide.API.DTOs.Thread;
using StackoverflowGuide.BLL.Models.Thread;
using StackoverflowGuide.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackoverflowGuide.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class ThreadController : ControllerBase
    {
        IThreadService threadService;
        IMapper mapper;

        public ThreadController(IThreadService threadService, IMapper mapper)
        {
            this.threadService = threadService;
            this.mapper = mapper;
        }

        [HttpPost("create")]
        public ActionResult<ThreadIdData> Post([FromBody]ThreadData model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = this.User.Claims.FirstOrDefault().Value;
            var thread = mapper.Map<Thread>(model);
            thread.Owner = userId;

            try
            {
                var responseId = new ThreadIdData()
                {
                    Id = threadService
                         .CreateNewThread(thread)
                };
                return responseId;
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<ThreadIdData> Delete(string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); 
            var userId = this.User.Claims.FirstOrDefault().Value;

            try
            {
                var responseId = new ThreadIdData()
                {
                    Id = threadService
                         .DeleteThread(id, userId)
                };
                return responseId;
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<ThreadIdData> Edit(string id, [FromBody]ThreadData model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = this.User.Claims.FirstOrDefault().Value;
            var thread = mapper.Map<Thread>(model);
            thread.Owner = userId;

            try
            {
                var responseId = new ThreadIdData()
                {
                    Id = threadService
                         .EditThread(id, thread)
                };
                return responseId;
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpGet("tags")]
        public ActionResult<List<TagData>> GetAllTags()
        {
            try
            {
                var tags = threadService.GetAllTags();
                return tags.Select(mapper.Map<TagData>).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }


        [HttpGet("{id}")]
        public ActionResult<SingleThreadData> GetSingleThread(string id)
        {

            var userId = this.User.Claims.FirstOrDefault().Value;

            try
            {
                var singleThread = threadService.GetSingleThread(id, userId);
                return new SingleThreadData
                {
                    Thread = mapper.Map<ThreadData>(singleThread.Thread),
                    Posts = singleThread.Posts.Select(mapper.Map<PostData>).ToList(),
                    Suggestions = singleThread.Suggestions.Select(mapper.Map<PostData>).ToList()
                };
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpGet]
        public ActionResult<List<ThreadData>> GetAllAsync()
        {
            var userId = this.User.Claims.FirstOrDefault().Value;

            try
            {
                var threads = threadService.GetAll(userId);
                return threads.Select(mapper.Map<ThreadData>).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPost("{id}/search")]
        public ActionResult<SingleThreadData> Search(string id, [FromBody] SearchData model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = this.User.Claims.FirstOrDefault().Value;

            try
            {
                threadService.SetSearchForThread(id,model.SearchTerm,userId);
                var singleThread = threadService.GetSingleThread(id, userId);
                return new SingleThreadData
                {
                    Thread = mapper.Map<ThreadData>(singleThread.Thread),
                    Posts = singleThread.Posts.Select(mapper.Map<PostData>).ToList(),
                    Suggestions = singleThread.Suggestions.Select(mapper.Map<PostData>).ToList()
                };
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
