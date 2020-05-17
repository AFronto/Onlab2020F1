using Google.Apis.Auth.OAuth2;
using Google.Apis.Bigquery.v2.Data;
using Google.Cloud.BigQuery.V2;
using StackoverflowGuide.BLL.BigQueryInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StackoverflowGuide.DATA.BigQuerry
{
    public class BigQuery : IBigQuery
    {
        public string ProjectId { get; set; }

        public BigQueryClient GetBigqueryClient()
        {
            var config = "bq-secrets.json";

            GoogleCredential credential = null;
            using (var jsonStream = new FileStream(config, FileMode.Open, FileAccess.Read, FileShare.Read))
                credential = GoogleCredential.FromStream(jsonStream);

            ProjectId = ((ServiceAccountCredential)credential.UnderlyingCredential).ProjectId;

            return BigQueryClient.Create(ProjectId, credential);
        }

        public List<BigQueryRow> GetRows(string query)
        {
            var bqClient = GetBigqueryClient();
            var result = bqClient.ExecuteQuery(query, parameters: null).ToList();
            return result;
        }
    }
}
