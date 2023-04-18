using CarvedRock.Admin.Models;
using CarvedRock.Admin.Repositories;
using FluentValidation;

namespace CarvedRock.Admin.Logic.Validations;

public class ProductValidator : AbstractValidator<ProductModel>
{
    public ProductValidator(ICarvedRockRepository repository)
    {
        RuleFor(p => p).MustAsync(async (productModel, cancelationtoken) =>
        {
            if (productModel.CategoryId == 0)
                return true;

            var categorie = await repository.GetCategoryByAsync(productModel.CategoryId);
            if (categorie?.Name != "Footwear")
                return true;

            return productModel.Price <= 200.00M;
        }).WithMessage("Price cannot be more than 200.00 for footwear");
    }
}
