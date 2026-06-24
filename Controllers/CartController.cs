using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TiendaRapida.Models;
using TiendaRapida.Services;

namespace TiendaRapida.Controllers;

public class CartController : Controller
{
    private readonly ProductService _productService;

    public CartController(ProductService productService)
    {
        _productService = productService;
    }

    public IActionResult Index()
    {
        var cart = GetCartFromSession();
        return View(cart);
    }

    [HttpPost]
    public IActionResult Add(int productId, int quantity = 1)
    {
        var product = _productService.GetProduct(productId);
        if (product == null || product.Stock < quantity)
        {
            TempData["Error"] = "Producto no disponible o sin stock";
            return RedirectToAction("Index", "Home");
        }

        var cart = GetCartFromSession();
        var existing = cart.FirstOrDefault(c => c.ProductId == productId);

        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = productId,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = quantity
            });
        }

        SaveCartToSession(cart);
        TempData["Success"] = $"{product.Name} agregado al carrito";
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Remove(int productId)
    {
        var cart = GetCartFromSession();
        var item = cart.FirstOrDefault(c => c.ProductId == productId);
        if (item != null)
        {
            cart.Remove(item);
            SaveCartToSession(cart);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Checkout()
    {
        var cart = GetCartFromSession();
        if (!cart.Any())
        {
            TempData["Error"] = "El carrito está vacío";
            return RedirectToAction("Index");
        }

        // Verificar stock y actualizar
        foreach (var item in cart)
        {
            if (!_productService.UpdateStock(item.ProductId, item.Quantity))
            {
                TempData["Error"] = $"No hay suficiente stock de {item.ProductName}";
                return RedirectToAction("Index");
            }
        }

        // Limpiar carrito
        HttpContext.Session.Remove("Cart");
        TempData["Success"] = "¡Compra realizada con éxito!";
        return RedirectToAction("Index", "Home");
    }

    private List<CartItem> GetCartFromSession()
    {
        var cartJson = HttpContext.Session.GetString("Cart");
        return string.IsNullOrEmpty(cartJson)
            ? new List<CartItem>()
            : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
    }

    private void SaveCartToSession(List<CartItem> cart)
    {
        var cartJson = JsonSerializer.Serialize(cart);
        HttpContext.Session.SetString("Cart", cartJson);
    }
}