namespace projekt_programowanie.Entities
{
    public class WorkerAvailability
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
    }
}
