using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projekt_programowanie.Entities
{
    public class Client
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        public List<BookedVisit> BookedVisits { get; set; }
    }
}
