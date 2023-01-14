using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projekt_programowanie.Entities
{
    public class Worker : User
    {
        public string? Description { get; set; }
        public string? ProfilePhotoPath { get; set; }

        public List<BookedVisit> BookedVisits { get; set; }
        public List<WorkerAvailability> WorkersAvailabilities { get; set; }
    }
}
