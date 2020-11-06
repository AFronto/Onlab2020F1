using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models.ElasticBLL
{
    public class ElasticKeyword
    {
        public string Word { get; set; }

        public int Occurrences
        {
            get { return Occurrences; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Occurrences can be only a positive number!");
                }
                Occurrences = value;
            }
        }

        public double Score { get; set; }

        public double getAverageScore()
        {
            return Score / Occurrences;
        }
    }
}
