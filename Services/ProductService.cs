using System.Text.Json;

public class ProductService
{
    private List<Product> _products;
    private int _nextId;

    public ProductService()
    {
        // Datos iniciales de ejemplo
        _products = new List<Product>
        {
            new Product { Id = 1, Name = "Camiseta Premium", Description = "Camiseta de algodón 100%", Stock = 10, Price = 29.99m, RegistrationDate = DateTime.Now },
            new Product { Id = 2, Name = "Laptop Gamer", Description = "Laptop de alta gama", Stock = 10, Price = 5000.00m, RegistrationDate = DateTime.Now },
            new Product { Id = 3, Name = "Iphone 17", Description = "Celular de alta gama", Stock = 20, Price = 7000.00m, RegistrationDate = DateTime.Now },
            new Product { Id = 4, Name = "Mouse", Description = "Mouse con DPI alta", Stock = 50, Price = 70.00m, RegistrationDate = DateTime.Now },
            new Product { Id = 5, Name = "MousePad", Description = "Mouse con diseño", Stock = 40, Price = 20.00m, RegistrationDate = DateTime.Now },
            new Product { Id = 6, Name = "Monitor Samsung 2025", Description = "Monitor marca samsung", Stock = 100, Price = 5200.00m, RegistrationDate = DateTime.Now }
        };
        _nextId = _products.Max(p => p.Id) + 1;
    }

    public List<Product> GetProducts() => _products;

    public Product GetProduct(int id) => _products.FirstOrDefault(p => p.Id == id);

    public void AddProduct(Product product)
    {
        product.Id = _nextId++;
        product.RegistrationDate = DateTime.Now;
        _products.Add(product);
    }

    public bool UpdateProduct(Product updatedProduct)
    {
        var product = GetProduct(updatedProduct.Id);
        if (product == null) return false;

        product.Name = updatedProduct.Name;
        product.Description = updatedProduct.Description;
        product.Stock = updatedProduct.Stock;
        product.Price = updatedProduct.Price;
        return true;
    }

    public bool DeleteProduct(int id)
    {
        var product = GetProduct(id);
        if (product == null) return false;
        return _products.Remove(product);
    }

    public bool UpdateStock(int productId, int newStock)
    {
        var product = GetProduct(productId);
        if (product == null) return false;
        product.Stock = newStock;
        return true;
    }
}