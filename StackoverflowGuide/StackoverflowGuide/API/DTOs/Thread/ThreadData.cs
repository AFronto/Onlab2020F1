﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.API.DTOs.Thread
{
    public class ThreadData
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string[] TagList { get; set; }
    }
}
