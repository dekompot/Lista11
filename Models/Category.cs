using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace Lista10.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Too short name")]
        [MaxLength(20, ErrorMessage = "Too long name, do not exceed {1}")]
        [Display(Name = "Category name")]
        public string Name { get; set; }
    }
}
