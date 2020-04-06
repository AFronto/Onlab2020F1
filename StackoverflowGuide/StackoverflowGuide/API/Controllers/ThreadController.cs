using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

            try
            {
                var responseId = new ThreadIdData() { 
                    Id = threadService
                         .CreateNewThread(mapper.Map<Thread>(model))
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

            try
            {
                var responseId = new ThreadIdData()
                {
                    Id = threadService
                         .DeleteThread(id)
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

            try
            {
                var responseId = new ThreadIdData()
                {
                    Id = threadService
                         .EditThread(id, mapper.Map<Thread>(model))
                };
                return responseId;
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }


        [HttpGet]
        public ActionResult<List<ThreadData>> GetAll()
        {
            try
            {
                var threads = threadService.GetAll();
                return threads.Select(mapper.Map<ThreadData>).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
