using SimManagementSystem.DataAccessLayer;

namespace SimManagementSystem.DataTransferObjects
{
    public class CreateMalfunctionDTO
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int UserReporter { get; set; }

        public int UserHandler { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        public bool Status { get; set; }

        public List<int> Devices { get; set; }
    }
}
