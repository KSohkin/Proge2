using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.WpfApp.Api
{
    public class Client
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phonenumber { get; set; }
    }
}
    