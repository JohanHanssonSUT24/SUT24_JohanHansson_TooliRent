TooliRent API
This is a backend solution for managing tool rentals, at a fictive company called TooliRent, built with ASP.NET Core. The API handles users, tools, categories, statistics and bookings. This documentation provides an overview of the project's architecture, technologies, and how to use its various endpoints.

Architecture and Design
The API follows a Clean Architecture approach to enforce separation of concerns, making it more modular, testable, and easy to maintain.

Domain: Contains the business logic and entities (such as Tool, Booking, User).

Application: Holds the application logic, DTOs (Data Transfer Objects), and the service layer. This layer is independent of the infrastructure and database.

Infrastructure: Manages database communication via Entity Framework Core and repositories.

Web API: The exposed API layer that handles HTTP requests.

Technologies Used
Language: C#

Framework: .NET 8, ASP.NET Core Web API

Database: SQL Server via Entity Framework Core


Tools:

AutoMapper: For object-to-object mapping between entities and DTOs.

FluentValidation: For defining and executing validation rules for incoming data.

Swagger/OpenAPI: For interactive API documentation.

Prerequisites
To run the project locally, you must have the following installed:

.NET SDK 8.0 or later

A local SQL Server instance or a connection string to an existing database.

How to Run the Project
Clone this repository.

Navigate to the project folder in your terminal.

Update the database connection string in appsettings.json.

Run the command dotnet restore to download all dependencies.

Run the command dotnet ef database update to create the database and run all migrations.

Run the command dotnet run to start the API.

The API will start at https://localhost:7000 (or another port depending on your configuration). The Swagger documentation is available at /swagger.

API Authentication
The API uses a token-based authentication mechanism (JWT). To access protected endpoints, you must include a valid JWT in the Authorization header as follows: Bearer <your_token>.

API Endpoints
Here is a complete list of all available endpoints, grouped by resource.

-Bookings-
Method

Endpoint

Description

Requirements

GET

/api/Bookings

Retrieves all bookings.

Admin

GET

/api/Bookings/{id}

Retrieves a specific booking.

Admin, Member

POST

/api/Bookings

Creates a new booking.

Admin, Member

PUT

/api/Bookings/{id}

Updates a booking.

Admin, Member

GET

/api/Bookings/mybookings

Retrieves all bookings for the logged-in user.

Admin, Member

DELETE

/api/Bookings/{id}

Cancels a booking.

Admin, Member

POST

/api/Bookings/{id}/pickup

Marks a booking as picked up.

Admin, Member

POST

/api/Bookings/{id}/return

Marks a booking as returned.

Admin, Member

GET

/api/Bookings/overdue

Retrieves all overdue bookings.

Admin

POST

/api/Bookings/{id}/markoverdue

Marks a booking as overdue.

Admin

POST

/api/Bookings/{id}/approved

Approves a booking.

Admin

-Tools-
Method

Endpoint

Description

Requirements

GET

/api/Tools

Retrieves all tools.

None

GET

/api/Tools/{id}

Retrieves a specific tool.

None

POST

/api/Tools

Creates a new tool.

Admin

PUT

/api/Tools/{id}

Updates an existing tool.

Admin

DELETE

/api/Tools/{id}

Deletes a tool.

Admin

-Users-
Method

Endpoint

Description

Requirements

GET

/api/Users

Retrieves all users.

Admin

GET

/api/Users/{id}

Retrieves a specific user.

Admin

POST

/api/Users/register

Registers a new user.

None

POST

/api/Users/login

Logs in a user and returns a JWT.

None

PUT

/api/Users/{id}

Updates a user's information.

Admin, Member

DELETE

/api/Users/{id}

Deletes a user.

Admin

POST

/api/Users/{id}/activate

Activates a user account.

Admin

POST

/api/Users/{id}/deactivate

Deactivates a user account.

Admin

-Tool Categories-
Method

Endpoint

Description

Requirements

GET

/api/ToolCategories

Retrieves all tool categories.

None

POST

/api/ToolCategories

Creates a new category.

Admin

PUT

/api/ToolCategories/{id}

Updates a category.

Admin

DELETE

/api/ToolCategories/{id}

Deletes a category.

Admin

-Statistics-
Method

Endpoint

Description

Requirements

GET

/api/Statistics

Retrieves overall statistics.

Admin

GET

/api/Statistics/category/{categoryName}

Retrieves tools by category name.

Admin

GET

/api/Statistics/tool/{toolId}

Retrieves statistics for a specific tool.

Admin