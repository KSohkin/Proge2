using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Event : Entity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Seats { get; set; }
        [Required]
        public string Price { get; set; }
        [Required]
        public string Summary{ get; set; }
        [Required]
        public string Organizer { get; set; }
    }
}
