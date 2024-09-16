using SimManagementSystem.DataAccessLayer;

namespace SimManagementSystem.DataTransferObjects
{
    public class CreateInspectionDTO
    {
        public int InspectionTypeId { get; set; }

        public DateTime Date { get; set; }

        public int Operator { get; set; }

        public string? Notice { get; set; }
    }
}
