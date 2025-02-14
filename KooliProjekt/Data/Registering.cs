using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Registering : Entity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Klient_Id { get; set; }
        [Required]
        public string Payment_Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Event_Id{ get; set; }
    }
}
