# Computer Store API

A .NET 9 Web API project with JWT authentication, MySQL integration using Entity Framework Core, and a clean architecture design.

## Features

- ASP.NET Core 9 Web API
- MySQL database with Entity Framework Core and Pomelo
- JWT Authentication using ASP.NET Core Identity
- Clean Architecture project structure
- Role-based authorization with three roles: user, admin, super_admin
- Complete user management system
- API documentation with Swagger/OpenAPI

## Prerequisites

- .NET 9 SDK
- MySQL Server
- Entity Framework Core tools

## Project Structure

```
ComputerAPI/
├── Controllers/          # API controllers
├── Data/                 # Database context and configurations
├── DTOs/                 # Data Transfer Objects
├── Helpers/              # Helper classes like JWT configuration
├── Interfaces/           # Service interfaces
├── Models/               # Entity models
├── Services/             # Business logic implementations
├── Program.cs            # Application configuration and startup
└── appsettings.json      # Application settings
```

## Database Setup

1. Install MySQL Server if you haven't already
2. Update the connection string in `appsettings.json` with your MySQL credentials:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=computer_store;User=YOUR_USERNAME;Password=YOUR_PASSWORD;Port=3306;CharSet=utf8mb4;"
}
```

3. Make sure to set a strong JWT secret key in appsettings.json:

```json
"JWT": {
  "Secret": "your_super_secure_key_with_at_least_32_characters",
  "Issuer": "computer-api",
  "Audience": "computer-client",
  "ExpirationMinutes": 60
}
```

## Getting Started

1. Clone the repository
2. Navigate to the project directory:
   ```
   cd ComputerAPI
   ```
3. Restore dependencies:
   ```
   dotnet restore
   ```
4. Create and apply database migrations:
   ```
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

   Or use the provided migration script:
   ```
   chmod +x migrate.sh
   ./migrate.sh
   ```
5. Run the application:
   ```
   dotnet run
   ```
6. Access the Swagger documentation at:
   ```
   https://localhost:5001/swagger
   ```

## API Endpoints

### Authentication

- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login and receive JWT token
- `GET /api/auth/profile` - Get current user profile (requires authentication)

### User Management

- `GET /api/user` - Get all users (requires admin role)
- `GET /api/user/{id}` - Get user by ID (requires admin role or own account)
- `PUT /api/user/{id}/roles` - Update user role (requires super_admin role)
- `DELETE /api/user/{id}` - Delete a user (requires super_admin role)

## Default Users

Upon first run, the application creates a default super_admin user:

- Email: admin@example.com
- Password: Admin@123456

## Database Schema

The database uses snake_case naming convention for fields and includes the following tables:

- `users` - User information with extended properties
- `roles` - Application roles
- `user_roles` - Mapping between users and roles
- `user_claims` - User claims for authorization
- `user_logins` - External login information
- `user_tokens` - User tokens
- `role_claims` - Role-specific claims

## Contributing

1. Create a feature branch
2. Commit your changes
3. Push to the branch
4. Create a new Pull Request

## License

This project is licensed under the MIT License.
