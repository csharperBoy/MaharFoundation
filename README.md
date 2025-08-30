# MaharFoundation

A modular monolith foundation framework built with ASP.NET Core 9, React 18, and Clean Architecture principles.

## Architecture
- Backend: ASP.NET Core 9 with Modular Monolith pattern
- Frontend: React 18 with TypeScript
- Database: PostgreSQL with Entity Framework Core
- Authentication: To be determined (options: ASP.NET Core Identity, Duende IdentityServer, or other)

## Getting Started
1. Clone repository
2. Run `dotnet restore`
3. Set up database connection string
4. Run `dotnet run` for backend
5. Run `npm start` for frontend

## Project Structure
src/
├── backend/ # .NET Core modules
│   ├── core/ # Core abstractions
│   ├── common/ # Cross-cutting services
│   └── api/ # Main API project
└── frontend/ # React application
    ├── public/
    └── src/
