using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace homemedkit_server.DTO
{
    public class UpdatedDisease
    {
        [Required(ErrorMessage = "Disease's name not defined")]
        public string Name { set; get; }
        [Required(ErrorMessage = "Medicine's id not defined")]
        public int MedicineId { set; get; }
        [Required(ErrorMessage = "Symptoms not defined")]
        public ICollection<int> SymptomsId { set; get; }
    }
}
