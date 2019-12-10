using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homemedkit_server.Entities
{
    public class Disease
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public virtual Medicine Medicine { set; get; }
        public virtual ICollection<Symptom> Symptoms { set; get; }
    }
}
