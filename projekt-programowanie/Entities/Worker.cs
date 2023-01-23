using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projekt_programowanie.Entities
{
    public class Worker : User
    {
        public List<BookedVisit> BookedVisits { get; set; }
        public List<WorkerAvailability> WorkersAvailabilities { get; set; }
    }
}
