using StackoverflowGuide.BLL.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackoverflowGuide.BLL.Models.Tag
{
    [Table("Tags")]
    public class DbTag : DBModel
    {
        public string Name { get; set; }
    }
}
