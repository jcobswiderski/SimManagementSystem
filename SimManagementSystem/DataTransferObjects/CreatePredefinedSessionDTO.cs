namespace SimManagementSystem.DataTransferObjects
{
    public class CreatePredefinedSessionDTO
    {
        public int Category { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Duration { get; set; }

        public string Abbreviation { get; set; } = null!;
    }
}
