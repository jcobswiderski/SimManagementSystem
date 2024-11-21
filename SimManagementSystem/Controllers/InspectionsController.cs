using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionsController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;

        public InspectionsController(SimManagementSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetInspections()
        {
            var inspections = await _context.Inspections
                .OrderByDescending(i => i.Date)
                .Select(i => new
                {
                    i.Id,
                    Date = i.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                    InspectionType = i.InspectionType.Name,
                    Operator = i.OperatorNavigation.FirstName + " " + i.OperatorNavigation.LastName,
                    i.Notice
                })
                .ToListAsync();
            return Ok(inspections);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInspection(int id)
        {
            var inspection = await _context.Inspections
                 .Where(i => i.Id == id)
                 .Select(i => new
                 {
                     i.Id,
                     i.Date,
                     InspectionType = i.InspectionType.Name,
                     Operator = i.OperatorNavigation.FirstName + " " + i.OperatorNavigation.LastName,
                     i.Notice
                 })
                 .FirstOrDefaultAsync();

            if (inspection == null)
            {
                return NotFound("Inspection with given ID not found!");
            }


            return Ok(inspection);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInspection(CreateInspectionDTO newInspection)
        {
            var inspection = new Inspection
            {
                InspectionTypeId = newInspection.InspectionTypeId,
                Date = newInspection.Date,
                Operator = newInspection.Operator,
                Notice = newInspection.Notice
            };

            await _context.Inspections.AddAsync(inspection);
            await _context.SaveChangesAsync();

            return Ok(inspection);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspection(int id)
        {
            var inspectionToDelete = new Inspection
            {
                Id = id
            };

            _context.Inspections.Attach(inspectionToDelete);
            _context.Inspections.Remove(inspectionToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetInspectionTypes()
        {
            var inspectionTypes = await _context.InspectionTypes.ToListAsync();

            if (inspectionTypes == null)
            {
                return NotFound("Inspection types not found.");
            }

            return Ok(inspectionTypes);
        }
    }
}
