﻿using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Organizer : Entity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
