using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoranWordSearch
{
    [Nest.ElasticsearchType]
    public class Sourate
    {
        public Sourate()
        {
            Versets = new List<Verset>();
        }
        public List<Verset> Versets { get; set; }

        public int SourateId { get; set; }
        public string Name { get; set; }
        public int VersetsCount { get; set; }
    }
}
