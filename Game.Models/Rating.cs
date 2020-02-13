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
        [Display(Name="Rating")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
