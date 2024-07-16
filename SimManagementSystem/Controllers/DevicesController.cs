using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetDevices()
        {
            var devices = _context.Devices.ToList();
            return Ok(devices);
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
