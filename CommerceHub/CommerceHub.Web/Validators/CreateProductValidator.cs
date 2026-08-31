using CommerceHub.Web.Features.Products.Commands.CreateNewProduct;
using CommerceHub.Web.Models;
using CommerceHub.Web.Repositories;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace CommerceHub.Web.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator(EFCategoryRepository categoryRepository, IProductReader productReader)
        {
            RuleFor(p => p.Name)
                   .NotEmpty().WithMessage("Ürün adı boş olamaz")
                   .MaximumLength(200).WithMessage("Ürün adı en fazla 200 karakter olmalı");

            RuleFor(p => p.BasePrice)
                   .GreaterThan(0).WithMessage("Fiyat, 0'dan büyük olmalı");

            RuleFor(p => p.CategoryId)
                   .GreaterThan(0).WithMessage("Geçerli bir kategori seçin")
                   .MustAsync(async (categoryId, cancellation) =>
                   {
                       var categories = await categoryRepository.GetAllAsync();
                       return categories.Any(c => c.Id == categoryId);
                   })
                   .WithMessage("Belirtilen katogori, kayıtlı değil!");

            RuleFor(p => p.SKU)
                   .NotEmpty().WithMessage("SKU değeri boş olamaz")
                   .CustomAsync(async (sku, context, cancellation) =>
                   {
                       if (string.IsNullOrWhiteSpace(sku))
                       {
                           return;
                       }

                       var allProducts = await productReader.GetProductsAsync();
                       var duplicate = allProducts.Count(p=>p.SKU == sku);

                       if (duplicate > 1)
                       {
                           context.AddFailure(nameof(Product.SKU), $"'{sku}' SKU değeri, başka bir ürün tarafından kullanılıyor ");
                       }

                   });
        }
    }
}
