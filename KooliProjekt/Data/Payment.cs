using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Payment
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Price { get; set; }
        [Required]
        public string Payment_nr { get; set; }
        [Required]
        public DateTime Deadline { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
