using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProdsAndCats.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        [Required]
        [MinLength(2)]
        public string Name { get; set;}
        [Required]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set;} = DateTime.Now;
        public DateTime UpdatedAt { get; set;} = DateTime.Now;
        public List<Association> Associations { get; set; }
    }
}