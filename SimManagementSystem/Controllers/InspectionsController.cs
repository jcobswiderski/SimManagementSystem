using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using SimManagementSystem.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionsController : ControllerBase
    {
        private readonly IInspectionsService _inspectionsService;

        public InspectionsController(IInspectionsService inspectionsService)
        {
            _inspectionsService = inspectionsService;
        }

        /// <summary>
        /// Get all inspections stored in DB.
        /// </summary>
        /// <returns>List of Inspection objects.</returns>
        [HttpGet]
        public async Task<IActionResult> GetInspections()
        {
            return await _inspectionsService.GetInspections(); 
        }

        /// <summary>
        /// Get single Inspection.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Inspection object or NotFound</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInspection(int id)
        {
            return await _inspectionsService.GetInspection(id);
        }

        /// <summary>
        /// Adding new inspection.
        /// </summary>
        /// <param name="newInspection">Inspection values: type, date, notice, operator</param>
        /// <returns>Single Inspection object</returns>
        [HttpPost]
        public async Task<IActionResult> CreateInspection(CreateInspectionDTO newInspection)
        {
            return await _inspectionsService.CreateInspection(newInspection);
        }

        /// <summary>
        /// Deleting Inspection object with given ID.
        /// </summary>
        /// <param name="id">Target Inspection id</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspection(int id)
        {
            return await _inspectionsService.DeleteInspection(id);
        }

        /// <summary>
        /// Get list of available Inspection types.
        /// </summary>
        /// <returns>List of Inspection types.</returns>
        [HttpGet("types")]
        public async Task<IActionResult> GetInspectionTypes()
        {
            return await _inspectionsService.GetInspectionTypes();
        }
    }
}
