using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lista10.Models
{
    public class Article
    {
        public int ArticleId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Too short name")]
        [MaxLength(20, ErrorMessage = "Too long name, do not exceed {1}")]
        [Display(Name = "Product name")]
        public string Name { get; set; }

        [Display(Name="Image")]
        public string ImageName { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "The price must be a positive number")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public double? Price { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        [Display(Name = "Expiration date")]
        public DateTime? ExpirationDate { get; set; }

        
        [Required(ErrorMessage = "Product category is required.")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

/*        public Article(int id, string name, double? price, DateTime? expirationDate, Category productCategory)
        {
            Id = id;
            Name = name;
            Price = price;
            ExpirationDate = expirationDate;
            ProductCategory = productCategory;
        }*/

    }
}
