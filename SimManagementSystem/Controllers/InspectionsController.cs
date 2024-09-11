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
        public IActionResult GetInspections()
        {
            var inspections = _context.Inspections
                .Select(i => new {
                    i.Id,
                    Date = i.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                    InspectionType = i.InspectionType.Name,
                    Operator = i.OperatorNavigation.FirstName + " " + i.OperatorNavigation.LastName
                })
                .ToList();
            return Ok(inspections);
        }

        [HttpGet("{id}")]
        public IActionResult GetInspection(int id)
        {
            var inspection = _context.Inspections
                 .Where(i => i.Id == id)
                 .Select(i => new {
                     i.Id,
                     i.Date,
                     InspectionType = i.InspectionType.Name,
                     Operator = i.OperatorNavigation.FirstName + " " + i.OperatorNavigation.LastName
                 })
                 .FirstOrDefault();

            if (inspection == null)
            {
                return NotFound($"No inspection found for the id {id}.");
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
                Operator = newInspection.Operator
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
        public IActionResult GetInspectionTypes()
        {
            var inspectionTypes = _context.InspectionTypes.ToList();

            if (inspectionTypes == null)
            {
                return NotFound("There's no any inspection types.");
            }

            return Ok(inspectionTypes);
        }
    }
}
