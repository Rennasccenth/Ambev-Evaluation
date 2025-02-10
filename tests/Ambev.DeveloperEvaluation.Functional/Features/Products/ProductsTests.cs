using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using Ambev.DeveloperEvaluation.Functional.Features.Users;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Queries.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUser;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Sdk;

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
        HttpClient adminAuthenticatedClient = await AuthenticateAsAdminAsync();

        IServiceScope serviceScope = WebApplicationFactory.Services.CreateScope();
        var createProductRequestValidator = serviceScope.ServiceProvider.GetRequiredService<IValidator<CreateProductRequest>>();
        CreateProductRequest createProductRequest = ProductTestData.GetProductRequest();
        await createProductRequestValidator.ValidateAndThrowAsync(createProductRequest);

        // Act
        var response = await adminAuthenticatedClient.PostAsJsonAsync("/api/products", createProductRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }

    [Fact(DisplayName = "POST /api/products should return bad request when data is invalid")]
    public async Task CreateProduct_WhenDataIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        HttpClient adminAuthenticatedClient = await AuthenticateAsAdminAsync();
        
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
        var response = await adminAuthenticatedClient.PostAsJsonAsync("/api/products", invalidProductRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationResult.IsValid.Should().BeFalse("the price is negative, this request must be flagged as invalid.");
    }

    [Fact(DisplayName = "GET /api/products/{productId} should return product when exists")]
    public async Task GetProductById_WhenExists_ShouldReturnProduct()
    {
        // Arrange
        // First lets create a product
        HttpClient adminAuthenticatedClient = await AuthenticateAsAdminAsync();

        CreateProductRequest createProductRequest = ProductTestData.GetProductRequest();
        var createResponse = await adminAuthenticatedClient.PostAsJsonAsync("/api/products", createProductRequest);
        createResponse.EnsureSuccessStatusCode();

        // Act
        HttpResponseMessage response = await adminAuthenticatedClient.GetAsync(createResponse.Headers.Location);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact(DisplayName = "GET /api/products/{productId} should return 404 Not Found when product doesn't exists")]
    public async Task GetProductById_WhenDoesntExists_ShouldReturnNotFound()
    {
        // Arrange
        HttpClient adminAuthenticatedClient = await AuthenticateAsAdminAsync();

        // Act
        HttpResponseMessage response = await adminAuthenticatedClient.GetAsync($"api/Products/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact(DisplayName = "DELETE /api/products/{productId} should delete the product when it exists.")]
    public async Task DeleteProduct_WhenExists_ShouldReturnNoContent()
    {
        // Arrange
        // First let's create a product as an ADMIN
        HttpClient adminAuthenticatedClient = await AuthenticateAsAdminAsync();

        CreateProductRequest createProductRequest = ProductTestData.GetProductRequest();
        HttpResponseMessage createResponse = await adminAuthenticatedClient.PostAsJsonAsync("/api/products", createProductRequest);
        createResponse.EnsureSuccessStatusCode();
        HttpResponseMessage response = await adminAuthenticatedClient.GetAsync(createResponse.Headers.Location);
        response.EnsureSuccessStatusCode();

        var createdProduct = await response.Content.ReadFromJsonAsync<GetProductResponse>();
        if (createdProduct is null) throw new XunitException("Fail to get the product response"); 
        
        // Act
        HttpResponseMessage deleteResponse = await adminAuthenticatedClient.DeleteAsync($"/api/products/{createdProduct.Id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        HttpResponseMessage retrievedProductResponse = await adminAuthenticatedClient.GetAsync($"/api/products/{createdProduct.Id}");
        retrievedProductResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact(DisplayName = "DELETE /api/products/{productId} should return 404 Not Found when product doesn't exists.")]
    public async Task DeleteProduct_WhenDoesntExists_ShouldReturnNotFound()
    {
        // Arrange
        HttpClient adminAuthenticatedClient = await AuthenticateAsAdminAsync();
        
        // Act
        HttpResponseMessage deleteResponse = await adminAuthenticatedClient.DeleteAsync($"/api/products/{Guid.NewGuid()}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact(DisplayName = "DELETE /api/products/{productId} when client is a Customer user, returns 403 Forbidden")]
    public async Task DeleteProduct_WhenProductExistsButUserIsAnCustomer_ShouldReturn403Forbidden()
    {
        // Arrange
        // First let's create a user and a product as an ADMIN
        HttpClient adminAuthenticatedClient = await AuthenticateAsAdminAsync();
        
        var createUserRequest = TestUserBuilder.CreateUserRequest(UserTestData.DumpUser(
            role: UserRole.Customer,
            status: UserStatus.Active));
        string preHashPassword = createUserRequest.Password;

        // Create the Customer user as an ADMIN user
        HttpResponseMessage createUserHttpResponse = await adminAuthenticatedClient.PostAsJsonAsync(
            requestUri: "api/users",
            value: createUserRequest);
        createUserHttpResponse.EnsureSuccessStatusCode();
        
        // Create the Product as ADMIN user
        CreateProductRequest createProductRequest = ProductTestData.GetProductRequest();
        HttpResponseMessage createResponse = await adminAuthenticatedClient.PostAsJsonAsync("/api/products", createProductRequest);
        createResponse.EnsureSuccessStatusCode();
        
        // Get the created Product
        HttpResponseMessage getProductResponse = await adminAuthenticatedClient.GetAsync(createResponse.Headers.Location);
        getProductResponse.EnsureSuccessStatusCode();

        // Parse the created Product
        var createdProduct = await getProductResponse.Content.ReadFromJsonAsync<GetProductResponse>();
        if (createdProduct is null) throw new XunitException("Fail to get the product response"); 
        
        // Act as the created CUSTOMER
        HttpClient customerAuthenticatedClient = await AuthenticateAsCustomerAsync(createUserRequest.Email, preHashPassword);
        HttpResponseMessage deleteResponse = await customerAuthenticatedClient.DeleteAsync($"/api/products/{createdProduct.Id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}