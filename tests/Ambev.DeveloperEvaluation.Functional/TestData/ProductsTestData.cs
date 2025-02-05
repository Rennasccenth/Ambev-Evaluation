using Ambev.DeveloperEvaluation.Domain.Aggregates.Products;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Functional.TestCollections;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.CreateProduct;
using Bogus;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Ambev.DeveloperEvaluation.Functional.TestData;

[Collection(PerCollectionFixture.CollectionName)]
public sealed class ProductsTestData : ICollectionFixture<DeveloperEvaluationWebApplicationFactory>
{
    private readonly TimeProvider _timeProvider;
    private readonly IValidator<Product> _productValidator;
    private readonly Faker _faker = new("pt_BR");

    public ProductsTestData(DeveloperEvaluationWebApplicationFactory webApiFixture)
    {
        IServiceScope serviceScope = webApiFixture.Services.CreateScope();
        _timeProvider = serviceScope.ServiceProvider.GetRequiredService<TimeProvider>();
        _productValidator = serviceScope.ServiceProvider.GetRequiredService<IValidator<Product>>();
    }

    public Product Dump(
        string? title = null,
        decimal? price = null,
        string? description = null,
        string? category = null,
        string? image = null,
        Rating? rating = null)
    {
        return new Product(
            title ?? _faker.Commerce.ProductName(),
            price ?? _faker.Random.Decimal(1, 1000),
            description ?? _faker.Commerce.ProductDescription(),
            category ?? _faker.Commerce.Categories(1)[0],
            image ?? _faker.Image.PicsumUrl(),
            rating ?? new Rating(_faker.Random.Decimal(0, 5), _faker.Random.Number(0, 1000))
        );
    }

    public CreateProductRequest GetProductRequest()
    {
        Product dumpProduct = Dump();
        return new CreateProductRequest
        {
            Title = dumpProduct.Title,
            Price = Math.Round(dumpProduct.Price, 2, MidpointRounding.ToZero),
            Description = dumpProduct.Description,
            Category = dumpProduct.Category,
            Image = dumpProduct.Image,
            Rating = dumpProduct.Rating
        };
    }

    public CreateProductRequest GetProductRequest(
        string title,
        decimal price,
        string description,
        string category,
        string image,
        Rating rating)
    {
        Product validatedProduct = GetValidatedProduct(
            title: title,
            price: price,
            description: description,
            category: category,
            image: image,
            rating: rating
        );
        return new CreateProductRequest
        {
            Title = validatedProduct.Title,
            Price = validatedProduct.Price,
            Description = validatedProduct.Description,
            Category = validatedProduct.Category,
            Image = validatedProduct.Image,
            Rating = validatedProduct.Rating
        };
    }
    
    public Product GetValidatedProduct(
        string title,
        decimal price,
        string description,
        string category,
        string image,
        Rating rating)
    {
        Product product = Dump(
            title: title,
            price: Math.Round(price, 2, MidpointRounding.ToZero),
            description: description,
            category: category,
            image: image,
            rating: rating
        );

        ValidationResult? validationResult = _productValidator.Validate(instance: product);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(message: "generated product is invalid, verify the provided test data.");
        }

        return product;
    }
}