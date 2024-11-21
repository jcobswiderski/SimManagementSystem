using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Controllers
{
    //[Authorize(Roles = "Admin, Engineer")]
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;

        public DevicesController(SimManagementSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDevices()
        {
            var devices = await _context.Devices.ToListAsync();
            return Ok(devices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDevice(int id)
        {
            var device = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id);

            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDevice(CreateDeviceDTO newDevice)
        {
            var device = new Device
            {
                Name = newDevice.Name,
                Tag = newDevice.Tag
            };

            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();

            return Ok(device);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var deviceToDelete = new Device
            {
                Id = id
            };

            _context.Devices.Attach(deviceToDelete);
            _context.Devices.Remove(deviceToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
