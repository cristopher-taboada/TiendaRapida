using Microsoft.AspNetCore.Mvc;

public class InventoryController : Controller
{
    private readonly ProductService _productService;

    public InventoryController(ProductService productService)
    {
        _productService = productService;
    }

    public IActionResult Index()
    {
        var products = _productService.GetProducts();
        return View(products);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            _productService.AddProduct(product);
            TempData["Success"] = "Producto agregado exitosamente";
            return RedirectToAction("Index");
        }
        return View(product);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var product = _productService.GetProduct(id);
        if (product == null)
        {
            TempData["Error"] = "Producto no encontrado";
            return RedirectToAction("Index");
        }
        return View(product);
    }

    [HttpPost]
    public IActionResult Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            if (_productService.UpdateProduct(product))
            {
                TempData["Success"] = "Producto actualizado exitosamente";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Error al actualizar el producto";
        }
        return View(product);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        if (_productService.DeleteProduct(id))
        {
            TempData["Success"] = "Producto eliminado exitosamente";
        }
        else
        {
            TempData["Error"] = "Error al eliminar el producto";
        }
        return RedirectToAction("Index");
    }
}