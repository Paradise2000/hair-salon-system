using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projekt_programowanie.Entities
{
    public class Client : User
    {
        public List<BookedVisit> BookedVisits { get; set; }
    }
}
