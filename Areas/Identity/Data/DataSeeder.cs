using garage.Areas.Identity.Data;
using garage.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

public class DataSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(ApplicationDbContext context, ILogger<DataSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public void SeedData()
    {
        if (!_context.Categories.Any())
        {
            _context.Categories.AddRange(
                new Category { Name = "Electronics", Description = "Electronic items", Photo = "bi bi-ev-front" },
                new Category { Name = "Books", Description = "Books and literature", Photo = "bi bi-book" },
                new Category { Name = "Clothing", Description = "Apparel and accessories", Photo = "bi bi-shirt" },
                new Category { Name = "Home Appliances", Description = "Appliances for home use", Photo = "bi bi-house" }
            );
            _context.SaveChanges();
        }

        if (!_context.Products.Any())
        {
            var electronicsCategory = _context.Categories.FirstOrDefault(c => c.Name == "Electronics");
            var booksCategory = _context.Categories.FirstOrDefault(c => c.Name == "Books");
            var clothingCategory = _context.Categories.FirstOrDefault(c => c.Name == "Clothing");
            var homeAppliancesCategory = _context.Categories.FirstOrDefault(c => c.Name == "Home Appliances");

            if (electronicsCategory != null && booksCategory != null && clothingCategory != null && homeAppliancesCategory != null)
            {
                _context.Products.AddRange(
                    new Product { Name = "Laptop", Description = "A high-performance laptop", Price = 999.99M, Catid = electronicsCategory.Id, Photo = "https/images&1.jpg" },
                    new Product { Name = "Smartphone", Description = "A latest model smartphone", Price = 699.99M, Catid = electronicsCategory.Id, Photo = "https/images&1.jpg" },
                    new Product { Name = "Novel", Description = "A best-selling novel", Price = 19.99M, Catid = booksCategory.Id, Photo = "https/images&1.jpg" },
                    new Product { Name = "T-Shirt", Description = "A comfortable cotton t-shirt", Price = 15.99M, Catid = clothingCategory.Id, Photo = "https/images&2.jpg" },
                    new Product { Name = "Jeans", Description = "Stylish denim jeans", Price = 49.99M, Catid = clothingCategory.Id, Photo = "https/images&3.jpg" },
                    new Product { Name = "Microwave", Description = "A high-power microwave oven", Price = 89.99M, Catid = homeAppliancesCategory.Id, Photo = "https/images&4.jpg" },
                    new Product { Name = "Refrigerator", Description = "A large capacity refrigerator", Price = 499.99M, Catid = homeAppliancesCategory.Id, Photo = "https/images&5.jpg" }
                );
                _context.SaveChanges();
            }
        }

        _logger.LogInformation("Seed data completed successfully.");
    }
}
