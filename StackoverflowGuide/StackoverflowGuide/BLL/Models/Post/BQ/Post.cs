using Google.Apis.Bigquery.v2.Data;
using Google.Cloud.BigQuery.V2;
using StackoverflowGuide.BLL.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models.Post
{
    public class Post : BQModel
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public override void FromRow(BigQueryRow row)
        {
            base.FromRow(row);
            Title = row["title"].ToString();
            Body = row["body"].ToString();
        }
    }
}
