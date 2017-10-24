using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoranWordSearch
{
    [Nest.ElasticsearchType]
    public class Verset
    {
        public int VersetId { get; set; }
        public int NumVerset { get; set; }
        public int SourateId { get; set; }
        public string Content { get; set; }
        public Verset()
        {
            VersetWords = new List<VersetWord>();
        }
        public List<VersetWord> VersetWords { get; set; }
    }
}
