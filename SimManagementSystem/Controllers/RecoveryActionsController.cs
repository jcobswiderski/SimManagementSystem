using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecoveryActionsController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;

        public RecoveryActionsController(SimManagementSystemContext context)
        {
            _context = context;
        }

        [HttpGet("{malfunctionId}")]
        public IActionResult GetRecoveryActionsForMalfunction(int malfunctionId)
        {
            var recoveryActions = _context.RecoveryActions
                .Select(r => new { 
                    r.Id,
                    date = r.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                    r.Description,
                    r.MalfunctionId
                })
                .Where(r => r.MalfunctionId == malfunctionId)
                .ToList();

            return Ok(recoveryActions);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecoveryAction(CreateRecoveryActionDTO newRecoveryAction)
        {
            var recoveryAction = new RecoveryAction
            {
                Date = newRecoveryAction.Date,
                Description = newRecoveryAction.Description,
                MalfunctionId = newRecoveryAction.MalfunctionId
            };

            await _context.RecoveryActions.AddAsync(recoveryAction);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecoveryAction(int id)
        {
            var recoveryActionToDelete = new RecoveryAction
            {
                Id = id
            };

            _context.RecoveryActions.Attach(recoveryActionToDelete);
            _context.RecoveryActions.Remove(recoveryActionToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
