using SimManagementSystem.DataAccessLayer;

namespace SimManagementSystem.DataTransferObjects
{
    public class CreateTestResultDTO
    {
        public int Test { get; set; }

        public bool IsPassed { get; set; }

        public DateTime Date { get; set; }

        public string Observation { get; set; } = null!;

        public int Executor { get; set; }
    }
}
