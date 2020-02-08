using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Game.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name="Game Title")]
        public string Name { get; set; }
        [Display(Name = "Release Date")]
        public string ReleaseDate { get; set; }
    }
}
