using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public class InspectionsService : IInspectionsService
    {
        private readonly SimManagementSystemContext _context;

        public InspectionsService(SimManagementSystemContext context)
        {
            _context = context;
        }

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

            if (inspections == null)
            {
                return new NotFoundObjectResult("Inspections not found.");
            }

            return new OkObjectResult(inspections);
        }

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
                return new NotFoundObjectResult("Inspection with given ID not found!");
            }

            return new OkObjectResult(inspection);
        }

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

            return new OkObjectResult(inspection);
        }

        public async Task<IActionResult> DeleteInspection(int id)
        {
            var inspectionToDelete = await _context.Inspections.FirstOrDefaultAsync(d => d.Id == id);

            if (inspectionToDelete == null)
            {
                return new NotFoundObjectResult("Inspection with given ID not found.");
            }

            _context.Inspections.Remove(inspectionToDelete);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<IActionResult> GetInspectionTypes()
        {
            var inspectionTypes = await _context.InspectionTypes.ToListAsync();

            if (inspectionTypes == null)
            {
                return new NotFoundObjectResult("Inspection types not found.");
            }

            return new OkObjectResult(inspectionTypes);
        }
    }
}
