using Google.Apis.Bigquery.v2.Data;
using Google.Cloud.BigQuery.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.BigQueryInterfaces
{
    public interface IBigQuery
    {
        public BigQueryClient GetBigqueryClient();

        public List<BigQueryRow> GetRows(string query);
    }
}

