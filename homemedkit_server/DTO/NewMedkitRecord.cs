using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace homemedkit_server.DTO
{
    public class NewMedkitRecord
    {
        [Required(ErrorMessage = "Medicine's id not defined")]
        public int MedicineId { set; get; }
        [Required(ErrorMessage = "Medicine's shelftime not defined")]
        public DateTime ShelfTime { set; get; }
    }
}
