using Microsoft.AspNetCore.Mvc;
using TiendaRapida.Services;

namespace TiendaRapida.Controllers;

public class HomeController : Controller
{
    private readonly ProductService _productService;

    public HomeController(ProductService productService)
    {
        _productService = productService;
    }

    public IActionResult Index()
    {
        ViewBag.Products = _productService.GetProducts();
        return View();
    }
}