using StackoverflowGuide.BLL.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models.Post
{
    [Table("Post")]
    public class StoredThreadPost : DBModel
    {
        public string ThreadId { get; set; }
        public string PostId { get; set; }
        public int ThreadIndex { get; set; }
        public List<string> ConnectedPosts { get; set; }
    }
}
