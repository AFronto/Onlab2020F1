using StackoverflowGuide.BLL.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models.Post
{
    [Table("ClusteredPost")]
    public class PostInCluster : DBModel
    {
        public int PostId { get; set; }
        public List<int> Clusters { get; set; }
        public List<string> TagList { get; set; }
    }
}
