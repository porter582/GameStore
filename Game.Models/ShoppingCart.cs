using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Game.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Count = 1;
        }
        public int Id { get; set; }
        public int GameItemId { get; set; }
        [Range(1, 10, ErrorMessage = "Please select a count between 1 and 10")]
        public int Count { get; set; }
        public string ApplicationUserId { get; set; }

        [NotMapped]
        [ForeignKey("GameItemId")]
        public virtual GameModel GameItem { get; set; }

        [NotMapped]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
