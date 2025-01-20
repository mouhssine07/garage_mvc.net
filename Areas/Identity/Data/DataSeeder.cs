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
                    new Product { Name = "Laptop", Description = "A high-performance laptop", Price = 999.99M, Catid = electronicsCategory.Id, Photo = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR3uQBKjrEWHgq0l0CNss5QhcZBl4C79Ol6Xw&s" },
                    new Product { Name = "Smartphone", Description = "A latest model smartphone", Price = 699.99M, Catid = electronicsCategory.Id, Photo = "https://uno.ma/pub/media/catalog/product/cache/af8d7fd2c4634f9c922fba76a4a30c04/l/d/ld0005930188_1.jpeg" },
                    new Product { Name = "Novel", Description = "A best-selling novel", Price = 19.99M, Catid = booksCategory.Id, Photo = "https://www.bookxcess.com/cdn/shop/products/what-is-a-novel-9781406290028-28454831095986.jpg?v=1648788444" },
                    new Product { Name = "T-Shirt", Description = "A comfortable cotton t-shirt", Price = 15.99M, Catid = clothingCategory.Id, Photo = "https://sporteo.ma/4135-large_default/t-shirt-sport-tech-gris.jpg" },
                    new Product { Name = "Jeans", Description = "Stylish denim jeans", Price = 49.99M, Catid = clothingCategory.Id, Photo = "https://thebrands.ma/cdn/shop/files/101.jpg?v=1701196182" },
                    new Product { Name = "Microwave", Description = "A high-power microwave oven", Price = 89.99M, Catid = homeAppliancesCategory.Id, Photo = "https://res.cloudinary.com/sharp-consumer-eu/image/fetch/w_3000,f_auto/https://s3.infra.brandquad.io/accounts-media/SHRP/DAM/origin/c317341c-bbee-11ec-96c6-6ebcd0d8bd62.jpg" },
                    new Product { Name = "Refrigerator", Description = "A large capacity refrigerator", Price = 499.99M, Catid = homeAppliancesCategory.Id, Photo = "https://m.media-amazon.com/images/I/71sWwQyV3jL._AC_SL1494_.jpg" }
                );
                _context.SaveChanges();
            }
        }

        _logger.LogInformation("Seed data completed successfully.");
    }
}
