using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using SimManagementSystem.Services;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecoveryActionsController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;
        private readonly IRecoveryActionsService _recoveryActionsService;

        public RecoveryActionsController(IRecoveryActionsService recoveryActionsService)
        {
            _recoveryActionsService = recoveryActionsService;
        }

        /// <summary>
        /// Get all recovery actions for malfunction.
        /// </summary>
        /// <param name="malfunctionId">Target malfunction id</param>
        /// <returns></returns>
        [HttpGet("{malfunctionId}")]
        public async Task<IActionResult> GetRecoveryActionsForMalfunction(int malfunctionId)
        {
            return await _recoveryActionsService.GetRecoveryActionsForMalfunction(malfunctionId);
        }

        /// <summary>
        /// Add new recovery action for malfunction.
        /// </summary>
        /// <param name="newRecoveryAction">All recovery action required data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateRecoveryAction(CreateRecoveryActionDTO newRecoveryAction)
        {
            return await _recoveryActionsService.CreateRecoveryAction(newRecoveryAction);
        }

        /// <summary>
        /// Delete single recovery action.
        /// </summary>
        /// <param name="id">Target recovery action id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecoveryAction(int id)
        {
            return await _recoveryActionsService.DeleteRecoveryAction(id);
        }
    }
}
