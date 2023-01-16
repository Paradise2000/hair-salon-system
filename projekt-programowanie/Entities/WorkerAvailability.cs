namespace projekt_programowanie.Entities
{
    public class WorkerAvailability
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
    }
}
