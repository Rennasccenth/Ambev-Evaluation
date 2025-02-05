using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.CreateProduct;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Features.Products;

public sealed class ProductsTests : BaseTest
{
    public ProductsTests(DeveloperEvaluationWebApplicationFactory webApplicationFactory)
        : base(webApplicationFactory)
    { }

    [Fact(DisplayName = "POST /api/products should create product when data is valid")]
    public async Task CreateProduct_WhenDataIsValid_ShouldCreateProduct()
    {
        // Arrange
        await ActAsAuthenticatedUserAsync();

        IServiceScope serviceScope = WebApplicationFactory.Services.CreateScope();
        var createProductRequestValidator = serviceScope.ServiceProvider.GetRequiredService<IValidator<CreateProductRequest>>();
        CreateProductRequest createProductRequest = ProductTestData.GetProductRequest();
        await createProductRequestValidator.ValidateAndThrowAsync(createProductRequest);

        // Act
        var response = await TestServerHttpClient.PostAsJsonAsync("/api/products", createProductRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }

    [Fact(DisplayName = "POST /api/products should return bad request when data is invalid")]
    public async Task CreateProduct_WhenDataIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        await ActAsAuthenticatedUserAsync();
        
        IServiceScope serviceScope = WebApplicationFactory.Services.CreateScope();
        var createProductRequestValidator = serviceScope.ServiceProvider.GetRequiredService<IValidator<CreateProductRequest>>();

        var validProduct = ProductTestData.Dump();
        CreateProductRequest invalidProductRequest = new CreateProductRequest
        {
            Title = validProduct.Title,
            Price = -11100,
            Description = validProduct.Description,
            Category = validProduct.Category,
            Image = validProduct.Image,
            Rating = validProduct.Rating
        };
        ValidationResult validationResult = await createProductRequestValidator.ValidateAsync(invalidProductRequest);

        // Act
        var response = await TestServerHttpClient.PostAsJsonAsync("/api/products", invalidProductRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationResult.IsValid.Should().BeFalse("the price is negative, this request must be flagged as invalid.");
    }

    [Fact(DisplayName = "GET /api/products/{id} should return product when exists")]
    public async Task GetProductById_WhenExists_ShouldReturnProduct()
    {
        // Arrange
        // First create a product
        await ActAsAuthenticatedUserAsync();

        CreateProductRequest createProductRequest = ProductTestData.GetProductRequest();
        var createResponse = await TestServerHttpClient.PostAsJsonAsync("/api/products", createProductRequest);
        createResponse.EnsureSuccessStatusCode();

        // Act
        HttpResponseMessage response = await TestServerHttpClient.GetAsync(createResponse.Headers.Location);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}