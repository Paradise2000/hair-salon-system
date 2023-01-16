using FluentValidation;

namespace projekt_programowanie.DTOs
{
    public class AddWorkerAvailabilityDto
    {
        public DateTime Date { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}
