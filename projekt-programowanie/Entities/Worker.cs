using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projekt_programowanie.Entities
{
    public class Worker
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        public List<BookedVisit> BookedVisits { get; set; }
        public List<WorkerAvailability> WorkersAvailabilities { get; set; }
    }
}
