using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace homemedkit_server.DTO
{
    public class NewMedicine
    {
        [Required(ErrorMessage = "Medicine's name not defined")]
        public string Name { set; get; }
        [Required(ErrorMessage = "Medicine's description not defined")]
        public string Description { set; get; }
    }
}
