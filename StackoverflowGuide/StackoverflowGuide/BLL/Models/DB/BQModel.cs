using Google.Apis.Bigquery.v2.Data;
using Google.Cloud.BigQuery.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models.DB
{
    public class BQModel
    {
        public string Id { get; set; }

        public virtual void FromRow(BigQueryRow row)
        {
            Id = row["id"].ToString();
        }
    }
}
