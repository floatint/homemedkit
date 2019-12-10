using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace homemedkit_server.DTO
{
    public class UpdatedMedkitRecord
    {
        [Required(ErrorMessage = "Medicine's id not defined")]
        public int MedicineId { set; get; }
        [Required(ErrorMessage = "Shelf time not defined")]
        public DateTime ShelfTime { set; get; }
    }
}
