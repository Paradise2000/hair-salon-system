namespace projekt_programowanie.DTOs
{
    public class GetWorkerAvailabilityDto
    {
        public int TempDataId { get; set; }
        public int ServiceId { get; set; }
        public int WorkerId { get; set; }
        public int WorkerAvailabilityId { get; set; }
        public string WorkerFirstName { get; set; }
        public string WorkerPhone { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public double Price { get; set; }
    }
}
