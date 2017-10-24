using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoranWordSearch
{
    [Nest.ElasticsearchType]
    public class VersetWord
    {
        public int VersetWordId { get; set; }
        public int VersetId { get; set; }
        public int WordId { get; set; }
        public string Value { get; set; }
    }
}
