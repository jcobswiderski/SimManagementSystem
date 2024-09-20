using SimManagementSystem.DataAccessLayer;

namespace SimManagementSystem.DataTransferObjects
{
    public class CreateRecoveryActionDTO
    {
        public DateTime Date { get; set; }

        public string Description { get; set; } = null!;

        public int MalfunctionId { get; set; }
    }
}
