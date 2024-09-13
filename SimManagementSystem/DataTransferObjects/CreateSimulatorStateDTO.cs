namespace SimManagementSystem.DataTransferObjects
{
    public class CreateSimulatorStateDTO
    {
        public DateTime StartupTime { get; set; }

        public int MeterState { get; set; }

        public int Operator { get; set; }
    }
}
