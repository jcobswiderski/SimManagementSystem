﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenancesController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;

        public MaintenancesController(SimManagementSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetMaintenances()
        {
            var maintenances = _context.Maintenances
                .OrderByDescending(m => m.Date)
                .Select(m => new
                {
                    m.Id,
                    name = m.TypeNavigation.Name,
                    executor = m.ExecutorNavigation.FirstName + " " + m.ExecutorNavigation.LastName,
                    date = m.Date.ToString("yyyy-MM-dd"),
                    m.Realized
                })
                .ToList();
            return Ok(maintenances);
        }

        [HttpGet("count/incomplete")]
        public IActionResult GetIncompleteMaintenancesCount()
        {
            var maintenancesCount = _context.Maintenances
                .Where(m => m.Date <= DateTime.Now && m.Realized == false)
                .Count();

            return Ok(maintenancesCount);
        }

        [HttpGet("{id}")]
        public IActionResult GetMaintenance(int id)
        {
            var maintenance = _context.Maintenances
                .Select(m => new
                {
                    m.Id,
                    name = m.TypeNavigation.Name,
                    executor = m.ExecutorNavigation.FirstName + " " + m.ExecutorNavigation.LastName,
                    date = m.Date.ToString("yyyy-MM-dd"),
                    m.Realized,
                    tasks = m.TypeNavigation.Tasks
                })
                .Where(m => m.Id == id)
                .FirstOrDefault();
            return Ok(maintenance);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaintenance(CreateMaintenanceDTO newMaintenance)
        {
            var maintenance = new Maintenance
            {
                Type = newMaintenance.Type,
                Date = newMaintenance.Date,
                Realized = false
            };

            await _context.Maintenances.AddAsync(maintenance);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaintenance(int id)
        {
            var maintenanceToDelete = await _context.Maintenances.FirstOrDefaultAsync(m => m.Id == id);

            if (maintenanceToDelete == null)
            {
                return NotFound();
            }

            _context.Maintenances.Remove(maintenanceToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMaintenanceState(int id, [FromBody] EditMaintenanceDTO maintenanceToEdit)
        {
            var maintenance = _context.Maintenances.FirstOrDefault(m => m.Id == id);
            if (maintenance == null)
            {
                return NotFound("Maintenance not found");
            }

            maintenance.Realized = true;
            maintenance.Executor = maintenanceToEdit.Executor;

            _context.SaveChanges();

            return Ok(maintenance);
        }
    }
}
