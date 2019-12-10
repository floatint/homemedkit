using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using homemedkit_server.Entities;

namespace homemedkit_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedkitController : ControllerBase
    {
        private Entities.DataContext DataContext { set; get; }

        public MedkitController(Entities.DataContext context)
        {
            DataContext = context;
        }

        //=============================READ OPERATIONS============================== 
        [HttpGet("records")]
        public async Task<IActionResult> GetAllRecords()
        {
            return Ok(await DataContext.MedkitRecords.Include(x => x.Medicine).ToListAsync());
        }

        [HttpGet("medicines")]
        public async Task<IActionResult> GetAllMedicines()
        {
            return Ok(await DataContext.Medicines.ToListAsync());
        }

        [HttpGet("diseases")]
        public async Task<IActionResult> GetAllDiseases()
        {
            return Ok(await DataContext.Diseases
                                       .Include(x => x.Medicine)
                                       .Include(x => x.Symptoms)
                                       .ToListAsync());
        }

        [HttpGet("symptoms")]
        public async Task<IActionResult> GetAllSymptoms()
        {
            return Ok(await DataContext.Symptoms.ToListAsync());
        }
        //===========================================================================

        //=============================CREATE OPERATIONS=============================
        [HttpPost("records")]
        public async Task<IActionResult> CreateNewRecord([FromBody] DTO.NewMedkitRecord newRecord)
        {
           if (!ModelState.IsValid)
           {
                return BadRequest(ModelState);
           }
            var med = await DataContext.Medicines.Where(x => x.Id == newRecord.MedicineId).FirstOrDefaultAsync();
            if (med == null)
            {
                return BadRequest("Medicine's not found");
            }
            var record = new MedkitRecord
            {
                Medicine = med,
                ShelfTime = newRecord.ShelfTime
            };
            DataContext.MedkitRecords.Add(record);
            await DataContext.SaveChangesAsync();
            return Ok(record);
        }

        [HttpPost("medicines")]
        public async Task<IActionResult> CreateNewMedicine([FromBody] DTO.NewMedicine newMed)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var med = new Medicine
            {
                Name = newMed.Name,
                Description = newMed.Description
            };
            DataContext.Medicines.Add(med);
            await DataContext.SaveChangesAsync();
            return Ok(med);
        }

        [HttpPost("diseases")]
        public async Task<IActionResult> CreateNewDisease([FromBody] DTO.NewDisease newDis)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var med = await DataContext.Medicines.Where(x => x.Id == newDis.MedicineId).FirstOrDefaultAsync();
            if (med == null)
            {
                return BadRequest("Medicine's not found");
            }
            var disease = new Disease
            {
                Name = newDis.Name,
                Medicine = med
            };
            DataContext.Diseases.Add(disease);
            await DataContext.SaveChangesAsync();
            return Ok(disease);
        }

        [HttpPost("symptoms")]
        public async Task<IActionResult> CreateNewSymptom([FromBody] string symDescr)
        {
            if ((symDescr == null) || symDescr.Length == 0)
            {
                return BadRequest("Symptom's description not defined");
            }
            var symptom = new Symptom { Description = symDescr };
            DataContext.Symptoms.Add(symptom);
            await DataContext.SaveChangesAsync();
            return Ok(symptom);
        }
        //==============================================================================


        //=============================UPDATE OPERATIONS===============================
        [HttpPut("records/{id:int}")]
        public async Task<IActionResult> UpdateRecord(int id, [FromBody] DTO.UpdatedMedkitRecord upRecord)
        {
            var record = await DataContext.MedkitRecords.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (record == null)
            {
                return NotFound(id);
            }
            var med = await DataContext.Medicines.Where(x => x.Id == upRecord.MedicineId).FirstOrDefaultAsync();
            if (med == null)
            {
                return BadRequest("Medicine's not found");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            record.Medicine = med;
            record.ShelfTime = upRecord.ShelfTime;
            DataContext.MedkitRecords.Update(record);
            await DataContext.SaveChangesAsync();
            return Ok(record);
        }

        [HttpPut("medicines/{id:int}")]
        public async Task<IActionResult> UpdateMedicine(int id, [FromBody] DTO.UpdatedMedicine upMed)
        {
            var medicine = await DataContext.Medicines.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (medicine == null)
            {
                return NotFound(id);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            medicine.Name = upMed.Name;
            medicine.Description = upMed.Description;
            DataContext.Medicines.Update(medicine);
            await DataContext.SaveChangesAsync();
            return Ok(medicine);  
        }

        [HttpPut("diseases/{id:int}")]
        public async Task<IActionResult> UpdateDisease(int id, [FromBody] DTO.UpdatedDisease upDis)
        {
            var disease = await DataContext.Diseases
                                           .Where(x => x.Id == id)
                                           .Include(x => x.Medicine)
                                           .Include(x => x.Symptoms)
                                           .FirstOrDefaultAsync();
            if (disease == null)
            {
                return NotFound(id);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var med = await DataContext.Medicines.Where(x => x.Id == upDis.MedicineId).FirstOrDefaultAsync();
            if (med == null)
            {
                return BadRequest(string.Format("Medicine with id={0} not found", upDis.MedicineId));
            }
            //foreach symptoms
            ICollection<Symptom> symCollecttion = new List<Symptom>();
            foreach (var s in upDis.SymptomsId)
            {
                var sym = await DataContext.Symptoms.Where(x => x.Id == s).FirstOrDefaultAsync();
                if (sym == null)
                {
                    return BadRequest(string.Format("Symptom with id={0} not found", s));
                }
                symCollecttion.Add(sym);
            }
            disease.Symptoms = symCollecttion;
            DataContext.Diseases.Update(disease);
            await DataContext.SaveChangesAsync();
            return Ok(disease);
        }

        [HttpPut("symptoms/{id:int}")]
        public async Task<IActionResult> UpdateSymptom(int id, [FromBody] string descr)
        {
            var symptom = await DataContext.Symptoms.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (symptom == null)
            {
                return NotFound(id);
            }
            if (descr == null)
            {
                return BadRequest("Symptom's description not defined");
            }
            symptom.Description = descr;
            DataContext.Symptoms.Update(symptom);
            await DataContext.SaveChangesAsync();
            return Ok(symptom);
        }
        //==============================================================================


        //=============================DELETE OPERATIONS================================
        [HttpDelete("records/{id:int}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            var record = await DataContext.MedkitRecords.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (record == null)
            {
                return NotFound(id);
            }
            DataContext.MedkitRecords.Remove(record);
            await DataContext.SaveChangesAsync();
            return Ok(record);
        }

        [HttpDelete("medicines/{id:int}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicine = await DataContext.Medicines.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (medicine == null)
            {
                return NotFound(id);
            }
            DataContext.Medicines.Remove(medicine);
            await DataContext.SaveChangesAsync();
            return Ok(medicine);
        }

        [HttpDelete("diseases/{id:int}")]
        public async Task<IActionResult> DeleteDisease(int id)
        {
            var disease = await DataContext.Diseases.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (disease == null)
            {
                return NotFound(id);
            }
            DataContext.Diseases.Remove(disease);
            await DataContext.SaveChangesAsync();
            return Ok(disease);
        }

        [HttpDelete("symptoms/{id:int}")]
        public async Task<IActionResult> DeleteSymptom(int id)
        {
            var symptom = await DataContext.Symptoms.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (symptom == null)
            {
                return NotFound(id);
            }
            DataContext.Symptoms.Remove(symptom);
            await DataContext.SaveChangesAsync();
            return Ok(symptom);
        }
        //==============================================================================



    }

}