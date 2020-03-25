using StackoverflowGuide.BLL.Models.DB;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackoverflowGuide.BLL.Models.Thread
{
    [Table("Thread")]
    public class Thread: DBModel
    {
        public string Name { get; set; }

        public string[] TagList { get; set; }
    }
}
