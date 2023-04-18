using CarvedRock.Admin.Logic;
using CarvedRock.Admin.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarvedRock.Admin.Controllers;

[Authorize]
public class ProductsController : Controller
{
    public List<ProductModel> SampleProducts { get; set; }
    private readonly IProductLogic _logic;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IProductLogic logic,
        ILogger<ProductsController> logger)
    {
        _logic = logic;
        _logger = logger;
        SampleProducts = GetSampleProducts();
    }

    public async Task<IActionResult> Index()
    {
        var products = await _logic.GetAllProducts();
        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _logic.GetProductById(id);
        if (product == null)
        {
            _logger.LogInformation("Details not found for id {id}", id);
            return View("NotFound");
        }
        return View(product);
    }

    public async Task<IActionResult> Create()
    {
        var model = await _logic.InitializedProductModel();
        return View(model);
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(ProductModel product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }
        try
        {
            await _logic.AddNewProduct(product);
            return RedirectToAction(nameof(Index));
        }
        catch (ValidationException ex)
        {
            var results = new ValidationResult(ex.Errors);
            results.AddToModelState(ModelState, null);
            await _logic.GetAvailableCategories(product);
            return View(product);
        }

    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            _logger.LogInformation("No id passed for edit");
            return View("NotFound");
        }

        var productModel = await _logic.GetProductById(id.Value);
        if (productModel == null)
        {
            _logger.LogInformation("Details not found for id {id}", id);
            return View("NotFound");
        }
        await _logic.GetAvailableCategories(productModel);
        return View(productModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductModel product)
    {
        if (id != product.Id)
        {
            _logger.LogInformation("Id mismatch in passed information. " +
               "Id value {id} did not match model value of {productId}",
               id, product.Id);
            return View("NotFound");
        }

        if (ModelState.IsValid)
        {
            await _logic.UpdateProduct(product);
            return RedirectToAction(nameof(Index));
        }
        await _logic.GetAvailableCategories(product);
        return View(product);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            _logger.LogInformation("No id value was passed for deletion.");
            return View("NotFound");
        }

        var productModel = await _logic.GetProductById(id.Value);
        if (productModel == null)
        {
            _logger.LogInformation("Id to be deleted ({id}) does not exist in database.", id);
        }

        return View(productModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _logic.RemoveProduct(id);
        return RedirectToAction(nameof(Index));
    }

    private List<ProductModel> GetSampleProducts()
    {
        return new List<ProductModel>()
        {
            new () {Id = 1, Name = "Trailblazer", Price = 69.99M, IsActive = true,
                Description = "Great support in this high-top to take you to great heights and trails." },
            new () {Id = 2, Name = "Coastliner", Price = 49.99M, IsActive = true,
                Description = "Easy in and out with this lightweight but rugged shoe with great ventilation to get your around shores, beaches, and boats."},
            new () {Id = 3, Name = "Woodsman", Price = 64.99M, IsActive = true,
                Description = "All the insulation and support you need when wandering the rugged trails of the woods and backcountry." },
            new () {Id = 4, Name = "Basecamp", Price = 249.99M, IsActive = true,
                Description = "Great insulation and plenty of room for 2 in this spacious but highly-portable tent."},
        };
    }
}
