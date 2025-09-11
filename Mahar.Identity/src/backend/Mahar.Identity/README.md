# Mahar.Identity Project

## Overview
Mahar.Identity is a .NET 9.0 project that provides identity management functionalities, including user registration, authentication, and token management. It is built on top of ASP.NET Core Identity and utilizes Entity Framework Core for data access.

## Project Structure
The project follows a modular structure, with the following key components:

- **Controllers**: Contains API controllers for handling authentication-related requests.
- **Services**: Defines and implements services for identity management.
- **Data**: Contains the database context and migration files for managing the identity database schema.
- **Models**: Defines the user model and other related entities.
- **DTOs**: Contains Data Transfer Objects for user data.
- **Extensions**: Provides extension methods for configuring services and settings.
- **Configuration**: Holds configuration classes for identity settings.
- **Mapping**: Contains mapping profiles for converting between entities and DTOs.

## Setup Instructions
1. **Clone the Repository**: Clone the repository containing the Mahar.Identity project.
2. **Install Dependencies**: Navigate to the project directory and run the following command to restore the necessary packages:
   ```
   dotnet restore
   ```
3. **Database Configuration**: Update the `appsettings.json` file with your database connection string and JWT settings.
4. **Migrations**: Run the following command to create the initial database migration:
   ```
   dotnet ef migrations add InitialCreate
   ```
5. **Update Database**: Apply the migration to the database using:
   ```
   dotnet ef database update
   ```
6. **Run the Application**: Start the application with the following command:
   ```
   dotnet run
   ```

## Usage
- **Register User**: Send a POST request to `/api/account/register` with user details to create a new account.
- **Login User**: Send a POST request to `/api/account/login` with credentials to authenticate and receive a JWT.
- **Refresh Token**: Use the refresh token endpoint to obtain a new access token.

## Contributing
Contributions are welcome! Please submit a pull request or open an issue for any enhancements or bug fixes.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.