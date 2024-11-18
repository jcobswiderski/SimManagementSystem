using SimManagementSystem.DataAccessLayer;

namespace SimManagementSystem.DataTransferObjects
{
    public class CreateMaintenanceDTO
    {
        public int Type { get; set; }

        public DateTime Date { get; set; }
    }
}
