# Project Setup and Run Instructions

This project uses Docker Compose for local development. Follow the steps below to get the application running.

## Prerequisites

- [Docker](https://www.docker.com/get-started) installed on your machine.
- [Docker Compose](https://docs.docker.com/compose/install/) installed (usually bundled with Docker).

## Running the Project

1. **Clone the Repository**

   ```bash
   git clone <repository-url>
   cd <repository-directory>
   ```

2. **Start the Services**

   Use Docker Compose to build and run all services:

   ```bash
   docker-compose up --build
   ```

   This command will:
   - Build the images for all services.
   - Start containers for the application, databases, and other dependencies.

3. **Access the Application**

   Once the containers are up, you can access the application at:

   - **HTTP Endpoints:** localhost:8080/swagger for Swagger UI and localhost:8080/docs for Scalar UI.


<p align="center">
   <img width="780" alt="image" src="https://github.com/user-attachments/assets/df9086aa-d398-4cfb-b1db-4239816d99ed" />
</p>


   - **Logs, Traces and Metrics (OTEL):** Theres a running Seq instance, available on localhost:5555.


<p align="center">
   <img width="780" alt="image" src="https://github.com/user-attachments/assets/1a4d3db0-adf5-48cc-bace-f0a804f3ea26" />
</p>

---

# Development Checklist

This checklist provides guidance on project structure, API endpoint implementation, and additional requirements for the assessment.

---

## 1. Project Structure & Documentation

### Directory Layout
- **Source Code:**  
  Create a `src/` folder for application source code.
- [**Testing:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/tests/Ambev.DeveloperEvaluation.Functional/WebApiApplicationFactory.cs#L34C21-L34C61)
  Create a `tests/` folder for unit, integration, and end-to-end tests.
- **Documentation:**  
  Maintain a `README.md` that includes:
  - A project overview.
  - Setup instructions.
  - Usage details.

### Version Control
- Use Git with a suitable branching strategy (e.g., Git Flow).
- Write [semantic and descriptive commit messages.](https://github.com/Rennasccenth/Ambev-Evaluation/commits/main/)

---

## 2. [Authentication Endpoints](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Auth/AuthController.cs#L16)

### [Login](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Auth/AuthController.cs#L35)
- **Endpoint:** `POST /api/auth/login`
- **Functionality:**  
  Accepts a JSON payload with `username` and `password`, and returns a JSON response containing an authentication `token`.

### Token Management
- Implement token generation and validation.
- Integrate authentication middleware to secure protected endpoints.

---

## 3. [Products Endpoints](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Products/ProductsController.cs#L28)

### [Retrieving Products](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Products/ProductsController.cs#L47)
- **Endpoint:** `GET /api/products`
- **Features:**
  - Retrieves a list of products.
  - Supports pagination using `_page` (default: 1) and `_size` (default: 10).
  - Allows ordering via `_order` (e.g., "price desc, title asc").
  - Supports filtering by:
    - Specific field values.
    - Partial matches using wildcards (`*`).
    - Range filtering using `_min` and `_max` prefixes.

### [Adding a Product](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Products/ProductsController.cs#L94)
- **Endpoint:** `POST /api/products`
- **Details:**  
  Accepts a request body with:
  - `title`
  - `price`
  - `description`
  - `category`
  - `image`
  - `rating` (including both `rate` and `count`)

### Managing a Specific Product
- [**Retrieve:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Products/ProductsController.cs#L70) `GET /api/products/{productId}` to fetch a product by its ID.
- [**Update:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Products/ProductsController.cs#L119) `PUT /api/products/{productId}` to modify an existing product.
- [**Delete:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Products/ProductsController.cs#L142) `DELETE /api/products/{productId}` to remove a product.

#### Product Categories
- [**List Categories:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Products/ProductsController.cs#L164)
  `GET /api/products/categories` to retrieve all product categories.
- [**Filter by Category:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Products/ProductsController.cs#L179)
  `GET /api/products/category/{category}` to fetch products by category with pagination and ordering.

---

## 4. [Carts Endpoints](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Carts/CartsController.cs#L22)

### Overview
- **List Carts (Currently unimplemented):**  
  `GET /api/carts` should retrieve a list of carts, supporting pagination and ordering.
- [**Create Cart:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Carts/CartsController.cs#L64)
  `POST /api/carts` accepts a JSON body with:
  - `userId`
  - `date` (string or date)
  - `products` (an array of objects with `productId` and `quantity`)

### Cart Details
- [**Retrieve Cart:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Carts/CartsController.cs#L39)
  `GET /api/carts/{cartId}` to fetch a specific cart by its ID.
- [**Update Cart:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Carts/CartsController.cs#L87)
  `PUT /api/carts/{cartId}` to update cart products.
- [**Delete Cart:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Carts/CartsController.cs#L112)
  `DELETE /api/carts/{cartId}` to remove a cart and return a confirmation message.

---

## 5. [Sales Endpoints](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Sales/SalesController.cs#L21)

### Sales Operations
- [**Create Sale (via user checkout):**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Users/UsersController.cs#L305)
  Create a sale from an existing cart, currently this is allowed only for users cart. 
- [**Retrieve Sale:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Sales/SalesController.cs#L41)
  Retrieve sales related to the current user or available to administrators/managers.
- [**Cancel Sale:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Sales/SalesController.cs#L64)
  Allow cancellation of sales for authorized users.
- [**Conclude Sale:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Sales/SalesController.cs#L87)
  Enable administrators or managers to mark sales as concluded.
- **List Sales (Under Development, currently unimplemented):**  
  Support filtering and ordering in the sales listing.

### Sales Record Data Model
Include the following details for each sale:
- [**General Sale Details:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Sales/Common/SaleResponse.cs#L3)
  Sale number, date of sale, customer information, branch/location, total sale amount, and cancellation status.
- [**Item Details:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Sales/Common/SaleProductResponse.cs#L3)
  For each sale item, include the product identifier, quantity, unit price, any calculated discount, and total amount per item.

### [Business Rules](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.Domain/Aggregates/Sales/Services/SalesService.cs#L46)
- No discount for quantities below 4 items.
- Apply a [10% discount](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.Domain/Aggregates/Carts/Strategies/ItemQuantity.cs#L3) for 4 or more identical items.
- Apply a [20% discount](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.Domain/Aggregates/Carts/Strategies/ItemQuantity.cs#L3) for purchases between 10 and 20 identical items.
- Enforce a maximum of [20 identical items per product](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.Domain/Aggregates/Carts/Specifications/ItemQuantityLessThanSpecification.cs#L5).

### [Optional Event Publishing](https://github.com/Rennasccenth/Ambev-Evaluation/tree/main/src/Ambev.DeveloperEvaluation.Domain/Aggregates/Sales/Events)
Log or simulate events for:
- `SaleCreated`
- `SaleModified`
- `SaleCancelled`
- `ItemCancelled`

---

## 6. [Users Endpoints](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Users/UsersController.cs#L40)

### User Management
- [**List Users:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Users/UsersController.cs#L165)
  `GET /api/users` should return a list of users, supporting pagination and ordering (e.g., "username asc, email desc"). User details should include:
  - `id`, `email`, `username`, `password`
  - `name` (with `firstname` and `lastname`)
  - `address` (with `city`, `street`, `number`, `zipcode`, and `geolocation` containing `lat` and `long`)
  - `phone`
  - `status` (e.g., Active, Inactive, Suspended)
  - `role` (e.g., Customer, Manager, Admin)
- [**Add User:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Users/UsersController.cs#L59)
  `POST /api/users` to create a new user.
- [**User Details:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Users/UsersController.cs#L87C11-L87C12)
  `GET /api/users/{userId}` to retrieve details for a specific user.
- [**Update User:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Users/UsersController.cs#L140)
  `PUT /api/users/{userId}` to modify an existing user's details.
- [**Delete User:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Users/UsersController.cs#L115)
  `DELETE /api/users/{userId}` to remove a user and return the details of the deleted user.

### User-Specific Cart Management
For the currently authenticated user:
- [**View Cart:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Users/UsersController.cs#L192)
  `GET /api/users/me/cart` to retrieve the user's cart.
- [**Create Cart:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Users/UsersController.cs#L246)
  `POST /api/users/me/cart` to create a cart.
- [**Delete Cart:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Users/UsersController.cs#L218)
  `DELETE /api/users/me/cart` to remove the user's cart.
- [**Update Cart:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Users/UsersController.cs#L276)
  `PUT /api/users/me/cart` to update the cart.
- [**Checkout:**](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Features/Users/UsersController.cs#L305)
  `POST /api/users/me/cart/checkout` to convert the cart into a sale and return the created Sale object.

---

## 7. General API Features

### [Pagination](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Common/PaginatedResponse.cs#L5) & [Ordering](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.PostgreSQL/Repositories/UserRepository.cs#L96)
Implement query parameters to support:
- Pagination: `_page` and `_size`.
- Multi-field ordering: `_order`.

### [Filtering](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.Common/Results/QueryableExtensions.cs#L24)
Enable filtering capabilities that support:
- Specific field values.
- Partial string matching using wildcards (`*`).
- Range filtering for numeric and date fields using `_min` and `_max` prefixes.

### [Error Handling](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/src/Ambev.DeveloperEvaluation.WebApi/Common/BaseController.cs#L21)
Return appropriate HTTP status codes:
- **2xx** for successful operations.
- **4xx** for client errors.
- **5xx** for server errors.

Standardize error responses with the following JSON structure:
```json
{
	"type": "string",
	"error": "string",
	"detail": "string"
}
```

### [Automated Tests](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/tests/Ambev.DeveloperEvaluation.Functional/WebApiApplicationFactory.cs#L34C21-L34C61)

<p align="center">
   <img width="780" alt="image" src="https://github.com/user-attachments/assets/bb9b5f48-782a-40c8-b474-943a2dd544d7" />
</p>

- Tests the features against a real application, without mock external dependency.
- [Respawns](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/tests/Ambev.DeveloperEvaluation.Functional/WebApiApplicationFactory.cs#L104) databases to ensure data consistency
- Generates [dynamic fake data](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/tests/Ambev.DeveloperEvaluation.Functional/TestData/UserTestData.cs#L14) using [Bogus](https://github.com/Rennasccenth/Ambev-Evaluation/blob/fca4f8ccc61b9573ff9f944bec5044b237144489/tests/Ambev.DeveloperEvaluation.Functional/TestData/ProductsTestData.cs#L36) (a.k.a Faker)

<p align="center">
   <img width="780" alt="image" src="https://github.com/user-attachments/assets/7222604a-c31e-471c-a1f3-c3c716d69788" />
</p>
