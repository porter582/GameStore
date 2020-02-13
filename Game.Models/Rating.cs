using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Game.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name="Rating Title")]
        public string Name { get; set; }
        public string Description { get; set; }
        //[Display(Name = "Release Date")]
        //public string ReleaseDate { get; set; }
    }
}
