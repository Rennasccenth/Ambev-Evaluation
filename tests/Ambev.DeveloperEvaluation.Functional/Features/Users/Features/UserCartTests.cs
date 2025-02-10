using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.CreateUserCart;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Sdk;

namespace Ambev.DeveloperEvaluation.Functional.Features.Users.Features;

public sealed class UserCartTests : BaseTest
{
    public UserCartTests(DeveloperEvaluationWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    { }

    [Fact(DisplayName = "POST api/users/me/cart should return 404 Not Found when cart contains a unregistered product")]
    public async Task CreateUserCart_WhenCartHasUnregisteredProduct_Returns404NotFound()
    {
        // Arrange
        HttpClient adminAuthenticatedClient = await AuthenticateAsAdminAsync();

        // {ADMIN} Creates Customer User
        var dumpedCustomerUser = UserTestData.DumpUser(role: UserRole.Customer, status: UserStatus.Active);
        var createUserRequest = TestUserBuilder.CreateUserRequest(dumpedCustomerUser);
        string preHashPassword = createUserRequest.Password;
        
        HttpResponseMessage createUserHttpResponse = await adminAuthenticatedClient.PostAsJsonAsync("api/users", createUserRequest);
        createUserHttpResponse.EnsureSuccessStatusCode();
        
        // {ADMIN} Creates Three Products
        CreateProductRequest createProductRequestOne = ProductTestData.GetProductRequest();
        HttpResponseMessage createResponseOne = await adminAuthenticatedClient.PostAsJsonAsync("/api/products", createProductRequestOne);
        createResponseOne.EnsureSuccessStatusCode();
        
        CreateProductRequest createProductRequestTwo = ProductTestData.GetProductRequest();
        HttpResponseMessage createResponseTwo = await adminAuthenticatedClient.PostAsJsonAsync("/api/products", createProductRequestTwo);
        createResponseTwo.EnsureSuccessStatusCode();

        CreateProductRequest createProductRequestThree = ProductTestData.GetProductRequest();
        HttpResponseMessage createResponseThree = await adminAuthenticatedClient.PostAsJsonAsync("/api/products", createProductRequestThree);
        createResponseThree.EnsureSuccessStatusCode();

        // Prepare those Created Products to cart addition.
        var productOne = await createResponseOne.Content.ReadFromJsonAsync<CreateProductResponse>();
        var productTwo = await createResponseTwo.Content.ReadFromJsonAsync<CreateProductResponse>();
        var productThree = await createResponseThree.Content.ReadFromJsonAsync<CreateProductResponse>();

        if (!(productOne is not null && productTwo is not null && productThree is not null))
        {
            throw new XunitException("Created products responses were invalid.");
        }
        
        // {CUSTOMER} Act as the created CUSTOMER
        HttpClient customerAuthenticatedClient = await AuthenticateAsCustomerAsync(createUserRequest.Email, preHashPassword);
        CreateUserCartRequestSummary userCartRequestSummary = new()
        {
            Date = WebApplicationFactory.Services.CreateScope().ServiceProvider.GetRequiredService<TimeProvider>()
                .GetUtcNow().DateTime,
            Products =
            [
                new ProductQuantifier
                {
                    ProductId = productOne.Id,
                    Quantity = 1
                },
                new ProductQuantifier
                {
                    ProductId = productTwo.Id,
                    Quantity = 4
                },
                new ProductQuantifier
                {
                    ProductId = productThree.Id,
                    Quantity = 9
                },
                new ProductQuantifier // This one wasn't registered before.
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 1
                }
            ]
        };

        // {CUSTOMER} Creates User Cart without the created products.
        HttpResponseMessage createUserCartResponseMessage = await customerAuthenticatedClient.PostAsJsonAsync("api/users/me/cart", userCartRequestSummary);

        // Assert
        using (var _ = new AssertionScope())
        {
            createUserCartResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
    
    [Fact(DisplayName = "DELETE api/users/me/cart when user has a cart should return 204 No Content and the deleted cart.")]
    public async Task DeleteUserCart_WhenCartWasNotCreated_Returns404NotFound()
    {
        // Arrange
        HttpClient adminAuthenticatedClient = await AuthenticateAsAdminAsync();

        // {ADMIN} Creates Customer User
        var dumpedCustomerUser = UserTestData.DumpUser(role: UserRole.Customer, status: UserStatus.Active);
        var createUserRequest = TestUserBuilder.CreateUserRequest(dumpedCustomerUser);
        string preHashPassword = createUserRequest.Password;
        
        HttpResponseMessage createUserHttpResponse = await adminAuthenticatedClient.PostAsJsonAsync("api/users", createUserRequest);
        createUserHttpResponse.EnsureSuccessStatusCode();
        
        // {ADMIN} Creates Three Products
        CreateProductRequest createProductRequestOne = ProductTestData.GetProductRequest();
        HttpResponseMessage createResponseOne = await adminAuthenticatedClient.PostAsJsonAsync("/api/products", createProductRequestOne);
        createResponseOne.EnsureSuccessStatusCode();
        
        CreateProductRequest createProductRequestTwo = ProductTestData.GetProductRequest();
        HttpResponseMessage createResponseTwo = await adminAuthenticatedClient.PostAsJsonAsync("/api/products", createProductRequestTwo);
        createResponseTwo.EnsureSuccessStatusCode();

        CreateProductRequest createProductRequestThree = ProductTestData.GetProductRequest();
        HttpResponseMessage createResponseThree = await adminAuthenticatedClient.PostAsJsonAsync("/api/products", createProductRequestThree);
        createResponseThree.EnsureSuccessStatusCode();

        // Prepare those Created Products to cart addition.
        var productOne = await createResponseOne.Content.ReadFromJsonAsync<CreateProductResponse>();
        var productTwo = await createResponseTwo.Content.ReadFromJsonAsync<CreateProductResponse>();
        var productThree = await createResponseThree.Content.ReadFromJsonAsync<CreateProductResponse>();

        if (!(productOne is not null && productTwo is not null && productThree is not null))
        {
            throw new XunitException("Created products responses were invalid.");
        }
        
        // {CUSTOMER} Act as the created CUSTOMER
        HttpClient customerAuthenticatedClient = await AuthenticateAsCustomerAsync(createUserRequest.Email, preHashPassword);
        CreateUserCartRequestSummary userCartRequestSummary = new()
        {
            Date = WebApplicationFactory.Services.CreateScope().ServiceProvider.GetRequiredService<TimeProvider>()
                .GetUtcNow().DateTime,
            Products =
            [
                new ProductQuantifier
                {
                    ProductId = productOne.Id,
                    Quantity = 1
                },
                new ProductQuantifier
                {
                    ProductId = productTwo.Id,
                    Quantity = 4
                },
                new ProductQuantifier
                {
                    ProductId = productThree.Id,
                    Quantity = 9
                }
            ]
        };
        
        // {CUSTOMER} Creates User Cart with created products.
        HttpResponseMessage createUserCartResponseMessage = await customerAuthenticatedClient.PostAsJsonAsync("api/users/me/cart", userCartRequestSummary);
        createUserCartResponseMessage.EnsureSuccessStatusCode();

        // {CUSTOMER} Gets User Cart.
        HttpResponseMessage getUserCartResponseMessage = await customerAuthenticatedClient.GetAsync(createUserCartResponseMessage.Headers.Location);
        getUserCartResponseMessage.EnsureSuccessStatusCode();

        // {CUSTOMER} Delete User Cart.
        HttpResponseMessage deleteUserCartResponseMessage = await customerAuthenticatedClient.DeleteAsync("api/users/me/cart");
        deleteUserCartResponseMessage.EnsureSuccessStatusCode();

        // {CUSTOMER} Gets User Cart.
        HttpResponseMessage getUserCartResponseMessageAfterDeletion = await customerAuthenticatedClient.GetAsync("api/users/me/cart");

        // Assert
        using (var _ = new AssertionScope())
        {
            deleteUserCartResponseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);

            getUserCartResponseMessageAfterDeletion.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
    
    [Fact(DisplayName = "DELETE api/users/me/cart should return 404 Not Found when user didn't created the cart")]
    public async Task DeleteUserCart_WhenCartHasUnregisteredProduct_Returns404NotFound()
    {
        // Arrange
        HttpClient adminAuthenticatedClient = await AuthenticateAsAdminAsync();

        // {ADMIN} Creates Customer User
        var dumpedCustomerUser = UserTestData.DumpUser(role: UserRole.Customer, status: UserStatus.Active);
        var createUserRequest = TestUserBuilder.CreateUserRequest(dumpedCustomerUser);
        string preHashPassword = createUserRequest.Password;
        
        HttpResponseMessage createUserHttpResponse = await adminAuthenticatedClient.PostAsJsonAsync("api/users", createUserRequest);
        createUserHttpResponse.EnsureSuccessStatusCode();
        
        // {ADMIN} Creates Three Products
        CreateProductRequest createProductRequestOne = ProductTestData.GetProductRequest();
        HttpResponseMessage createResponseOne = await adminAuthenticatedClient.PostAsJsonAsync("/api/products", createProductRequestOne);
        createResponseOne.EnsureSuccessStatusCode();
        
        CreateProductRequest createProductRequestTwo = ProductTestData.GetProductRequest();
        HttpResponseMessage createResponseTwo = await adminAuthenticatedClient.PostAsJsonAsync("/api/products", createProductRequestTwo);
        createResponseTwo.EnsureSuccessStatusCode();

        CreateProductRequest createProductRequestThree = ProductTestData.GetProductRequest();
        HttpResponseMessage createResponseThree = await adminAuthenticatedClient.PostAsJsonAsync("/api/products", createProductRequestThree);
        createResponseThree.EnsureSuccessStatusCode();

        // Prepare those Created Products to cart addition.
        var productOne = await createResponseOne.Content.ReadFromJsonAsync<CreateProductResponse>();
        var productTwo = await createResponseTwo.Content.ReadFromJsonAsync<CreateProductResponse>();
        var productThree = await createResponseThree.Content.ReadFromJsonAsync<CreateProductResponse>();

        if (!(productOne is not null && productTwo is not null && productThree is not null))
        {
            throw new XunitException("Created products responses were invalid.");
        }
        
        // {CUSTOMER} Act as the created CUSTOMER
        HttpClient customerAuthenticatedClient = await AuthenticateAsCustomerAsync(createUserRequest.Email, preHashPassword);
 
        // {CUSTOMER} Creates User Cart without the created products.
        HttpResponseMessage deleteUserCartResponseMessage = await customerAuthenticatedClient.DeleteAsync("api/users/me/cart");

        // Assert
        using (var _ = new AssertionScope())
        {
            deleteUserCartResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}