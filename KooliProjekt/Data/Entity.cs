using System.ComponentModel.DataAnnotations;
namespace KooliProjekt.Data
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}