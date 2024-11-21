using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using SimManagementSystem.Services;

namespace SimManagementSystem.Controllers
{
    //[Authorize(Roles = "Admin, Engineer")]
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDevicesService _devicesService;

        public DevicesController(IDevicesService devicesService)
        {
            _devicesService = devicesService;
        }

        /// <summary>
        /// Get all devices used in system.
        /// </summary>
        /// <returns>List of availiable devices.</returns>
        [HttpGet]
        public async Task<IActionResult> GetDevices()
        {
            return await _devicesService.GetDevices();
        }

        /// <summary>
        /// Get single device.
        /// </summary>
        /// <param name="id">Id of target device.</param>
        /// <returns>Single Device object</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDevice(int id)
        {
            return await _devicesService.GetDevice(id);
        }

        /// <summary>
        /// Endpoint to add new device to the system.
        /// </summary>
        /// <param name="newDevice">Single Device object</param>
        /// <returns>Created</returns>
        [HttpPost]
        public async Task<IActionResult> CreateDevice(CreateDeviceDTO newDevice)
        {
            return await _devicesService.CreateDevice(newDevice);
        }

        /// <summary>
        /// Deleting single device. Device cannot be related with malfunction.
        /// </summary>
        /// <param name="id">Id of single device</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            return await _devicesService.DeleteDevice(id);
        }
    }
}
