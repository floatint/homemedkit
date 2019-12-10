using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homemedkit_server.Entities
{
    public class MedkitRecord
    {
        public int Id { set; get; }
        public virtual Medicine Medicine { set; get; }
        public DateTime ShelfTime { set; get; }
    }
}
