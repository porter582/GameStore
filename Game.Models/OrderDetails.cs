using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Game.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual OrderHeader OrderHeader { get; set; }

        [Required]
        public int GameItemId { get; set; }
        [ForeignKey("GameItemId")]
        public virtual GameModel GameItem { get; set; }

        public int Count { get; set; }
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
