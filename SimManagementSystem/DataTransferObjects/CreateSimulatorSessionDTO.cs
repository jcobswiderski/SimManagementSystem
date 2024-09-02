using SimManagementSystem.DataAccessLayer;

namespace SimManagementSystem.DataTransferObjects
{
    public class CreateSimulatorSessionDTO
    {
        public int PredefinedSession { get; set; }

        public DateTime Date { get; set; }

        public int? PilotSeat { get; set; }

        public int? CopilotSeat { get; set; }

        public int? SupervisorSeat { get; set; }

        public int? ObserverSeat { get; set; }

        public bool Realized { get; set; }
    }
}
