using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using Ambev.DeveloperEvaluation.Functional.Features.Users;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.CreateUserCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.SellCartItems;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Sdk;

namespace Ambev.DeveloperEvaluation.Functional.Features.Sales;

public sealed class SalesTests : BaseTest
{
    public SalesTests(DeveloperEvaluationWebApplicationFactory webApplicationFactory) : base(webApplicationFactory) { }
    
    [Fact(DisplayName = "POST api/users/me/cart/checkout should return 422 Unprocessable Entity when the User Cart is Empty.")]
    public async Task SellCartItems_WhenCartIsEmpty_Returns433UnprocessableEntity()
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
            Products = []
        };

        // {CUSTOMER} Creates User Cart without the created products.
        HttpResponseMessage createUserCartResponseMessage = await customerAuthenticatedClient.PostAsJsonAsync("api/users/me/cart", userCartRequestSummary);
        createUserCartResponseMessage.EnsureSuccessStatusCode();

        // {CUSTOMER} Gets User Cart.
        HttpResponseMessage getUserCartResponseMessage = await customerAuthenticatedClient.GetAsync(createUserCartResponseMessage.Headers.Location);
        getUserCartResponseMessage.EnsureSuccessStatusCode();

        // {CUSTOMER} Buys all Cart Products, in this case, None!
        HttpResponseMessage buyCartItemsResponseMessage = await customerAuthenticatedClient.PostAsJsonAsync("api/users/me/cart/checkout", 
            new SellCartItemsRequestBody
            {
                BranchName = "Some Branch from Belém"
            });
        
        // Assert
        using (var _ = new AssertionScope())
        {
            buyCartItemsResponseMessage.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }
    }
    
    [Fact(DisplayName = "POST api/users/me/cart/checkout should create a Sale when the User Cart has Items and return 204 Created")]
    public async Task ShouldCreateEntireCartProductsSale()
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

        // {CUSTOMER} Buys all Cart Products!
        HttpResponseMessage buyCartItemsResponseMessage = await customerAuthenticatedClient.PostAsJsonAsync("api/users/me/cart/checkout", 
            new SellCartItemsRequestBody
            {
                BranchName = "Some Branch from Belém"
            });
        buyCartItemsResponseMessage.EnsureSuccessStatusCode();

        
        // Assert
        using (var _ = new AssertionScope())
        {
            var createdSaleResponse = await buyCartItemsResponseMessage.Content.ReadFromJsonAsync<SaleResponse>();
            if (createdSaleResponse is not null)
            {
                createdSaleResponse.Should().NotBeNull();
                createdSaleResponse.Should().BeOfType<SaleResponse>();
                createdSaleResponse.Products.Should().HaveCount(3);
                createdSaleResponse.Products.Should().ContainSingle(x => x.Id == productOne.Id && x.Quantity == 1);
                createdSaleResponse.Products.Should().ContainSingle(x => x.Id == productTwo.Id && x.Quantity == 4);
                createdSaleResponse.Products.Should().ContainSingle(x => x.Id == productThree.Id && x.Quantity == 9);
            }
        }
    }
    
    [Fact(DisplayName = "GET api/sales/{saleId} should get Sale when the User creates one and return 200 OK")]
    public async Task GetSales_WhenSaleExists_Returns200OK()
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

        // {CUSTOMER} Buys all Cart Products!
        HttpResponseMessage buyCartItemsResponseMessage = await customerAuthenticatedClient.PostAsJsonAsync("api/users/me/cart/checkout", 
            new SellCartItemsRequestBody
            {
                BranchName = "Some Branch from Belém"
            });
        buyCartItemsResponseMessage.EnsureSuccessStatusCode();
        var createdSaleResponse = await buyCartItemsResponseMessage.Content.ReadFromJsonAsync<SaleResponse>();

        // {CUSTOMER} Gets the created Sale
        if (createdSaleResponse is null) throw new XunitException("Created Sale was null.");
        HttpResponseMessage getSaleResponse = await customerAuthenticatedClient.GetAsync($"api/sales/{createdSaleResponse.Id}");

        // Assert
        using (var _ = new AssertionScope())
        {
            getSaleResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getSaleResponseContent = await getSaleResponse.Content.ReadFromJsonAsync<SaleResponse>();
            
            getSaleResponseContent.Should().NotBeNull();
            getSaleResponseContent.Should().BeOfType<SaleResponse>();
        }
    }
    
    [Fact(DisplayName = "POST api/sales/{saleId}/cancel when the sale exists, should cancel the sale and return 200 OK")]
    public async Task CancelSale_WhenSaleExists_Returns200OK()
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

        // {CUSTOMER} Buys all Cart Products!
        HttpResponseMessage buyCartItemsResponseMessage = await customerAuthenticatedClient.PostAsJsonAsync("api/users/me/cart/checkout", 
            new SellCartItemsRequestBody
            {
                BranchName = "Some Branch from Belém"
            });
        buyCartItemsResponseMessage.EnsureSuccessStatusCode();
        var createdSaleResponse = await buyCartItemsResponseMessage.Content.ReadFromJsonAsync<SaleResponse>();

        // {CUSTOMER} Gets the created Sale
        if (createdSaleResponse is null) throw new XunitException("Created Sale was null.");
        HttpResponseMessage getSaleResponse = await customerAuthenticatedClient.GetAsync($"api/sales/{createdSaleResponse.Id}");
        getSaleResponse.EnsureSuccessStatusCode();

        // {CUSTOMER} Cancel the recently created Sale
        HttpResponseMessage cancelSaleResponse = await customerAuthenticatedClient.PostAsJsonAsync($"api/sales/{createdSaleResponse.Id}/cancel", new { });
        cancelSaleResponse.EnsureSuccessStatusCode();

        // Assert
        using (var _ = new AssertionScope())
        {
            cancelSaleResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var deletedSaleResponse = await cancelSaleResponse.Content.ReadFromJsonAsync<SaleResponse>();
            if (deletedSaleResponse is null) throw new XunitException("Deleted Sale was null.");

            deletedSaleResponse.CanceledDate.Should().NotBeNull();
            deletedSaleResponse.Canceled.Should().Be(true);
        }
    }
    
    [Fact(DisplayName = "POST api/sales/{saleId}/conclude when the sale exists but you are a customer, returns 403 Forbidden")]
    public async Task ConcludeSale_WhenSaleExistsAsCustomerUser_Return403Forbidden()
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

        // {CUSTOMER} Buys all Cart Products!
        HttpResponseMessage buyCartItemsResponseMessage = await customerAuthenticatedClient.PostAsJsonAsync("api/users/me/cart/checkout", 
            new SellCartItemsRequestBody
            {
                BranchName = "Some Branch from Belém"
            });
        buyCartItemsResponseMessage.EnsureSuccessStatusCode();
        var createdSaleResponse = await buyCartItemsResponseMessage.Content.ReadFromJsonAsync<SaleResponse>();

        // {CUSTOMER} Gets the created Sale
        if (createdSaleResponse is null) throw new XunitException("Created Sale was null.");
        HttpResponseMessage getSaleResponse = await customerAuthenticatedClient.GetAsync($"api/sales/{createdSaleResponse.Id}");
        getSaleResponse.EnsureSuccessStatusCode();

        // {CUSTOMER} Conclude the recently created Sale
        HttpResponseMessage concludedSaleResponse = await customerAuthenticatedClient.PostAsJsonAsync($"api/sales/{createdSaleResponse.Id}/conclude", new { });

        // Assert
        using (var _ = new AssertionScope())
        {
            concludedSaleResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
    
    [Fact(DisplayName = "POST api/sales/{saleId}/conclude when the sale exists, should conclude the Sale and return 200 OK")]
    public async Task ConcludeSale_WhenSaleExists_Return200OK()
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

        // {CUSTOMER} Buys all Cart Products!
        HttpResponseMessage buyCartItemsResponseMessage = await customerAuthenticatedClient.PostAsJsonAsync("api/users/me/cart/checkout", 
            new SellCartItemsRequestBody
            {
                BranchName = "Some Branch from Belém"
            });
        buyCartItemsResponseMessage.EnsureSuccessStatusCode();
        var createdSaleResponse = await buyCartItemsResponseMessage.Content.ReadFromJsonAsync<SaleResponse>();

        // {CUSTOMER} Gets the created Sale
        if (createdSaleResponse is null) throw new XunitException("Created Sale was null.");
        HttpResponseMessage getSaleResponse = await customerAuthenticatedClient.GetAsync($"api/sales/{createdSaleResponse.Id}");
        getSaleResponse.EnsureSuccessStatusCode();

        // {ADMIN} Conclude the recently created Sale
        HttpResponseMessage concludedSaleResponse = await adminAuthenticatedClient.PostAsJsonAsync($"api/sales/{createdSaleResponse.Id}/conclude", new { });
        concludedSaleResponse.EnsureSuccessStatusCode();

        // Assert
        using (var _ = new AssertionScope())
        {
            concludedSaleResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var concludedSale = await concludedSaleResponse.Content.ReadFromJsonAsync<SaleResponse>();
            if (concludedSale is null) throw new XunitException("Concluded Sale was null.");

            concludedSale.Terminated.Should().BeTrue();
            concludedSale.TerminationDate.Should().NotBeNull();
        }
    }
}