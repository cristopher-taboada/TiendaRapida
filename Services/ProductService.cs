using TiendaRapida.Models;

namespace TiendaRapida.Services;

public class ProductService
{
    private List<Product> _products = new();
    private readonly object _lock = new object();

    public ProductService()
    {
        // DATOS DE PRUEBA - PRODUCTOS INICIALES
        _products = new List<Product>
        {
            new Product { Id = 1, Name = "Camiseta Premium", Price = 29.99m, Description = "Camiseta de algodón 100%", Stock = 10 },
            new Product { Id = 2, Name = "Zapatos Deportivos", Price = 79.99m, Description = "Zapatos para correr, talla 42", Stock = 5 },
            new Product { Id = 3, Name = "Mochila Viaje", Price = 49.99m, Description = "Impermeable con compartimento para laptop", Stock = 8 },
            new Product { Id = 4, Name = "Reloj Inteligente", Price = 199.99m, Description = "Con GPS, monitor cardiaco", Stock = 3 },
            new Product { Id = 5, Name = "Auriculares Bluetooth", Price = 59.99m, Description = "Cancelación de ruido, 20h de batería", Stock = 12 }
        };
    }

    public List<Product> GetProducts() => _products;

    public Product? GetProduct(int id) => _products.FirstOrDefault(p => p.Id == id);

    public bool UpdateStock(int productId, int quantity)
    {
        lock (_lock)
        {
            var product = GetProduct(productId);
            if (product == null || product.Stock < quantity)
                return false;

            product.Stock -= quantity;
            return true;
        }
    }

    // Método para agregar productos (opcional, para administración)
    public void AddProduct(Product product)
    {
        lock (_lock)
        {
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        }
    }
}