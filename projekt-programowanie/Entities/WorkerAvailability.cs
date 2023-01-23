namespace projekt_programowanie.Entities
{
    public class WorkerAvailability
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool isCancelled { get; set; }

        public int WorkerId { get; set; }
        public Worker Worker { get; set; }

        public List<BookedVisit> BookedVisits { get; set; }
    }
}
