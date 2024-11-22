using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public class RecoveryActionsService : IRecoveryActionsService
    {
        private readonly SimManagementSystemContext _context;

        public RecoveryActionsService(SimManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetRecoveryActionsForMalfunction(int malfunctionId)
        {
            var recoveryActions = await _context.RecoveryActions
                .Select(r => new
                {
                    r.Id,
                    date = r.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                    r.Description,
                    r.MalfunctionId
                })
                .Where(r => r.MalfunctionId == malfunctionId)
                .ToListAsync();

            if (recoveryActions == null)
            {
                return new NotFoundObjectResult("Recovery actions not found.");
            }

            return new OkObjectResult(recoveryActions);
        }

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

            return new CreatedResult();
        }

        public async Task<IActionResult> DeleteRecoveryAction(int id)
        {
            var recoveryActionToDelete = new RecoveryAction
            {
                Id = id
            };

            _context.RecoveryActions.Attach(recoveryActionToDelete);
            _context.RecoveryActions.Remove(recoveryActionToDelete);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
