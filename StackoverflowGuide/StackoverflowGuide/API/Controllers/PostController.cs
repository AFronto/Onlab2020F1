﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackoverflowGuide.API.DTOs.Post;
using StackoverflowGuide.API.DTOs.Thread;
using StackoverflowGuide.BLL.Models.Post;
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
        IThreadService threadService;

        public PostController(IMapper mapper, IPostService postService, IThreadService threadService)
        {
            this.mapper = mapper;
            this.postService = postService;
            this.threadService = threadService;
        }

        [HttpPost("suggestions/{threadId}/declined")]
        public ActionResult<List<PostData>> GetSuggestionsAfterDecline(string threadId, [FromBody]PostData model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = this.User.Claims.FirstOrDefault().Value;

            try
            {
                var suggestions = postService.GetSuggestionsAfterDecline(threadId, mapper.Map<ThreadPost>(model), userId);
                return suggestions.Select(mapper.Map<PostData>).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPost("suggestions/{threadId}/accepted")]
        public ActionResult<NewPostAndSuggestionsData> GetSuggestionsAfterAccept(string threadId, [FromBody]PostData model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = this.User.Claims.FirstOrDefault().Value;

            try
            {
                var response = postService.GetSuggestionsAfterAccept(threadId, mapper.Map<ThreadPost>(model), userId);
                return new NewPostAndSuggestionsData
                {
                    NewPost = mapper.Map<PostData>(response.NewPost),
                    Suggestions = response.Suggestions.Select(mapper.Map<PostData>).ToList()
                };
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpDelete("{threadId}/delete/{postId}")]
        public ActionResult<SingleThreadData> DeleteWatched(string threadId, string postId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = this.User.Claims.FirstOrDefault().Value;

            try
            {
                var responseId = new ThreadIdData()
                {
                    Id = postService
                         .DeletePost(threadId, postId, userId)
                };
                var singleThread = threadService.GetSingleThread(threadId, userId);
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

        [HttpGet("{threadId}/get/{postId}")]
        public ActionResult<SinglePostData> GetSinglePost(string threadId, string postId)
        {
            var userId = this.User.Claims.FirstOrDefault().Value;

            try
            {
                var question = postService.GetSingleById(postId);
                var response = mapper.Map<SinglePostData>(question);
                return response;
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }

        }
    }
}
