using Microsoft.AspNetCore.Mvc;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public interface IDevicesService
    {
        Task<IActionResult> GetDevices();
        Task<IActionResult> GetDevice(int id);
        Task<IActionResult> CreateDevice(CreateDeviceDTO newDevice);
        Task<IActionResult> DeleteDevice(int id);
    }
}
