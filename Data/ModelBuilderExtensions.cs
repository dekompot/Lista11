using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lista10.Models;

namespace Lista10.Data
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    CategoryId = 1,
                    Name = "Vegetables"
                },
                new Article()
                {
                    CategoryId = 2,
                    Name = "Others"
                }
                ); 
            modelBuilder.Entity<Article>().HasData(
                new Article()
                {
                    ArticleId = 1,
                    Name = "carrot",
                    Price = 1.49,
                    ExpirationDate = new DateTime(),
                    CategoryId = 1
                },
                new Article()
                {
                    ArticleId = 2,
                    Name = "broccoli",
                    Price = 2.49,
                    ExpirationDate = new DateTime(2024,12,20),
                    CategoryId = 2
                }
                );

        }
    }
}
