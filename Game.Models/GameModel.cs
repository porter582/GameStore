using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Game.Models
{
    public class GameModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name="Game Title")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public double Price { get; set; }

        [Display(Name = "Genre Type")]
        public int GenreId { get; set; }

        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; }

        [Display(Name = "Rating Type")]
        public int RatingId { get; set; }

        [ForeignKey("RatingId")]
        public virtual Rating Rating { get; set; }
    }
}
