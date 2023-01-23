namespace projekt_programowanie.DTOs
{
    public class GetClientsBookingsDto
    {
        public int Id { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientPhone { get; set; }
        public string ServiceName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public double Price { get; set; }
    }
}
