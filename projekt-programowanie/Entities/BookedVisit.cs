namespace projekt_programowanie.Entities
{
    public class BookedVisit
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool isCancelled { get; set; }

        public int? WorkerId { get; set; }
        public Worker Worker { get; set; }

        public int? ClientId { get; set; }
        public Client Client { get; set; }

        public int? ServiceId { get; set; }
        public Service Service { get; set; }

        public int? WorkerAvailabilityId { get; set; }
        public WorkerAvailability WorkerAvailability { get; set; }
    }
}
