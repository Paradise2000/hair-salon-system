namespace projekt_programowanie.DTOs
{
    public class GetClientsBookingsListDto
    {
        public int AvailabilityId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<GetClientsBookingsDto> bookedVisits { get; set; }
    }
}
