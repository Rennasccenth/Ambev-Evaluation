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

   - **HTTP Endpoints:** localhost:8080/swagger for the old and good one or localhost:8080/docs for ScalarUI

4. **Stopping the Services**

   To stop and remove the containers, run:

   ```bash
   docker-compose down
   ```

### What was developed?
- [x] User management endpoints, CRUD + GetAll built with the requested dynamic filtering and sorting. -Note: I've added the properties in the Request object just to make it explicit the accepted properties, but the filter will emerge from parsing the query string. Once user was created, we emit a UserRegisteredEvent, which is just logged by his respective handler.

- [x] Product management endpoints, some of them are protected, like create the product. But theres no validation over who can add a product, like the role of the user, (RBAC), but it could be so easily added by getting the user role stored in the jwt token, just like I did in the IUserContext implementation. On GetProducts endpoint, we also accepts the dynamic filter and sorting.

- [x] Auth Endpoint for token generation.

- [x] Carts Creation and retrieval. Theres a interesting security validation here, you can't access the cart that you don't own. Which means that to operate over a cart, you need to be the owner of if (We can check from the UserId stored in your token, just like we could do for any interesting information related to the logged user. This feature can alse be used on inner layers due abstraction.)

- [x] Sales creation and retrieval. You can create a sale, which basically means "convert all products stored in my cart to a sale." by doing this, we validate a few things like if all products in the creating sale exists, verify the requested business rules, the ownership of the cart and them after all this we apply any discount and create the sale. Once created, the domain events of if are dispatched.

- [x] Test suit. Theres a robust test suit for the project, that runs a mirror application. Basically we run the app withot expose any port, so we can execute any request while access internal components of the running app (like all services registered in the DI!).

- [x] Database Respawning. To ensure every test runs isolated from others, we reset the databases (mongo and psql for now) to a known state every time a test runs.  
- [x] Handle Sale events. Even that for now not all them are being fired. (Cancelation and termination are also related to Inventory, which I didn't manage to implement)
- [x] Added Open Telemetry support. Currently we have a initial setup for OTEL. The Aspire dashboard can be acessed at localhost:18888, assuming you ran the docker compose.

<img width="299" alt="image" src="https://github.com/user-attachments/assets/2cf2f991-b975-4b6d-877c-bbeece741cef" />


## Additional Notes

- **Environment Variables:**  
  You can customize configurations using environment variables. Check the `docker-compose.yml` file for default settings and adjust as needed.
  
- **Database Migrations:**  
  In any case a migration was needed, you can perform it by creating a EFBundle. There is a updated Bundle in the PostgreSQL project and there is no need for mongo or redis.

Enjoy exploring the application!


## Overview
This section provides a high-level overview of the project and the various skills and competencies it aims to assess for developer candidates. 

See [Overview](/docs/overview.md)

## Tech Stack
This section lists the key technologies used in the project, including the backend, testing, frontend, and database components. 

See [Tech Stack](/docs/tech-stack.md)

## Frameworks
This section outlines the frameworks and libraries that are leveraged in the project to enhance development productivity and maintainability. 

See [Frameworks](/docs/frameworks.md)

<!-- 
## API Structure
This section includes links to the detailed documentation for the different API resources:
- [API General](./docs/general-api.md)
- [Products API](/.doc/products-api.md)
- [Carts API](/.doc/carts-api.md)
- [Users API](/.doc/users-api.md)
- [Auth API](/.doc/auth-api.md)
-->

## Project Structure
This section describes the overall structure and organization of the project files and directories. 

See [Project Structure](/docs/project-structure.md)
