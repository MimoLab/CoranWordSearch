using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoranWordSearch
{
    [Nest.ElasticsearchType]
    public class Word
    {
        public int WordId { get; set; }
        public string Value { get; set; }
        public Word()
        {
            VersetWords = new List<VersetWord>();
        }
        public List<VersetWord> VersetWords { get; set; }
    }
}
