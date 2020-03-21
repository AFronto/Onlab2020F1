using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models
{
    public class Registration
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string RepeatPassword { get; set; }
    }
}
