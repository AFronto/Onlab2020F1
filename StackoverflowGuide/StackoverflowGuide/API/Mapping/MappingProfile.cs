using AutoMapper;
using StackoverflowGuide.API.DTOs;
using StackoverflowGuide.API.DTOs.Thread;
using StackoverflowGuide.BLL.Models;
using StackoverflowGuide.BLL.Models.Auth;
using StackoverflowGuide.BLL.Models.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.API.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterData, Registration>();
            CreateMap<LoginData, Login>();
            CreateMap<ThreadData, Thread>();
            CreateMap<Thread, ThreadData>();
        }
    }
}
