namespace projekt_programowanie.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public TimeSpan ServiceDuration { get; set; }
        public double ServicePrice { get; set; }

        public List<BookedVisit> BookedVisits { get; set; }
    }
}
