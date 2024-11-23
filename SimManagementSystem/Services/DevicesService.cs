using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public class DevicesService : IDevicesService
    {
        private readonly SimManagementSystemContext _context;

        public DevicesService(SimManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetDevices()
        {
            var devices = await _context.Devices.ToListAsync();

            if (devices == null || !devices.Any())
            {
                return new NotFoundObjectResult("Devices not found.");
            }

            return new OkObjectResult(devices);
        }

        public async Task<IActionResult> GetDevice(int id)
        {
            var device = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id);

            if (device == null)
            {
                return new NotFoundObjectResult("Device with given ID not found.");
            }

            return new OkObjectResult(device);
        }

        public async Task<IActionResult> CreateDevice(CreateDeviceDTO newDevice)
        {
            var device = new Device
            {
                Name = newDevice.Name,
                Tag = newDevice.Tag
            };

            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();

            return new CreatedResult();
        }

        public async Task<IActionResult> DeleteDevice(int id)
        {
            var deviceToDelete = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id);

            if (deviceToDelete == null)
            {
                return new NotFoundObjectResult("Device with given ID not found.");
            }

            _context.Devices.Remove(deviceToDelete);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
