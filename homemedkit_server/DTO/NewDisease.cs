using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace homemedkit_server.DTO
{
    public class NewDisease
    {
        [Required(ErrorMessage = "Diseases's name not defined")]
        public string Name { set; get; }
        [Required(ErrorMessage = "Medicine's id not defined")]
        public int MedicineId { set; get; }
    }
}
