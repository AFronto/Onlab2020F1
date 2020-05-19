using Google.Cloud.BigQuery.V2;
using StackoverflowGuide.BLL.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models.Tag
{
    public class Tag : BQModel
    {
        public string Name { get; set; }

        public override void FromRow(BigQueryRow row)
        {
            base.FromRow(row);
            Name = row["tag_name"].ToString();
        }
    }
}
