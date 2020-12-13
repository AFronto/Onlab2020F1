using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Models.ElasticBLL
{
    public class TagScore
    {
        private int _occurrences;
        public string Tag { get; set; }
        public int Occurrences
        {
            get { return _occurrences; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Occurrences can be only a positive number!");
                }
                _occurrences = value;
            }
        }
    }
}
