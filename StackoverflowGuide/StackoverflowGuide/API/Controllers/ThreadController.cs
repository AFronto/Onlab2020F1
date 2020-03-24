using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StackoverflowGuide.API.DTOs.Thread;
using StackoverflowGuide.BLL.Models.Thread;
using StackoverflowGuide.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.API.Controllers
{
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

            try
            {
                var responseId = new ThreadIdData() { 
                    Id = threadService
                         .CreateNewThread(mapper.Map<Thread>(model))
                         .ToString()
                };
                return responseId;
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
