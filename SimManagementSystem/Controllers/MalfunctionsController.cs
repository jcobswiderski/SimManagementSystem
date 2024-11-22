using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using SimManagementSystem.Services;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MalfunctionsController : ControllerBase
    {
        private readonly IMalfunctionsService _malfunctionsService;

        public MalfunctionsController(SimManagementSystemContext context, IMalfunctionsService malfunctionsService)
        {
            _malfunctionsService = malfunctionsService;
        }

        /// <summary>
        /// Get list of malfunction objects.
        /// </summary>
        /// <returns>List of Malfunction objects</returns>
        [HttpGet]
        public async Task<IActionResult> GetMalfunctions()
        {
            return await _malfunctionsService.GetMalfunctions();
        }

        /// <summary>
        /// Get number of malfunctions between two dates.
        /// </summary>
        /// <param name="dateBegin">Date begin</param>
        /// <param name="dateEnd">Date end</param>
        /// <returns>Integer with value</returns>
        [HttpGet("count")]
        public async Task<IActionResult> GetMalfunctionsCount(DateTime? dateBegin, DateTime? dateEnd)
        {
            return await _malfunctionsService.GetMalfunctionsCount(dateBegin, dateEnd);
        }

        /// <summary>
        /// Get number of all unsolved malfunctions.
        /// </summary>
        /// <returns>Integer with value</returns>
        [HttpGet("count/unsolved")]
        public async Task<IActionResult> GetUnsolvedMalfunctionsCount()
        {
            return await _malfunctionsService.GetUnsolvedMalfunctionsCount();
        }

        /// <summary>
        /// Get single malfunction.
        /// </summary>
        /// <param name="id">Target malfunction id</param>
        /// <returns>Malfunction object</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMalfunction(int id)
        {
            return await _malfunctionsService.GetMalfunction(id);
        }

        /// <summary>
        /// Get all malfuntions for specified device
        /// </summary>
        /// <param name="deviceId">Device id</param>
        /// <returns>List of malfunctions</returns>
        [HttpGet("device/{deviceId}")]
        public async Task<IActionResult> GetMalfunctionForDevice(int deviceId)
        {
            return await _malfunctionsService.GetMalfunctionForDevice(deviceId);
        }

        /// <summary>
        /// Add new malfunction to the system.
        /// </summary>
        /// <param name="newMalfunction">Malfunction data.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateMalfunction(CreateMalfunctionDTO newMalfunction)
        {
            return await _malfunctionsService.CreateMalfunction(newMalfunction);
        }

        /// <summary>
        /// Delete malfunction with given id.
        /// </summary>
        /// <param name="id">Target malfunction id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMalfunction(int id)
        {
            return await _malfunctionsService.DeleteMalfunction(id);
        }

        /// <summary>
        /// Update malfunction state: unsolved -> solved.
        /// </summary>
        /// <param name="id">Target malfunction id</param>
        /// <param name="updatedUser">User id who solve the malfunction</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMalfunctionState(int id, [FromBody] EditMalfunctionDTO updatedUser)
        {
            return await _malfunctionsService.UpdateMalfunctionState(id, updatedUser);
        }
    }
}
