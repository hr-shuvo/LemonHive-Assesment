# Lemon Hive - E-Commerce Full-Stack App

## This is a simple full-stack e-commerce project built with .NET 8.

- **Frontend:** Razor Pages
- **Backend:** ASP.NET Core Web API
- **Databases:** **PostgreSQL** (for products), **MongoDB** (for cart)
- **Features:** product list, search, cart, pagination, discounts by date
- Used Docker for databases

## Run the Database with Docker

1. Make sure **Docker** is running.
2. In the project folder, run this command to start database:
   ```bash
   docker-compose up -d
3. Or update connection strings in `appsettings.json` to use your local databases.

## Run the Application
1. Open the solution in Visual Studio.
2. Restore NuGet packages.
3. Run the project.

## Project Structure
- **Frontend**: Razor Pages frontend.
- **Backend**: ASP.NET Core Web API backend.
- **Databases**: PostgreSQL for products and MongoDB for cart.
- **Docker**: Used for running databases.

## Features
- Product listing with search functionality.
- Cart management using MongoDB.
- Pagination for product lists.
- Discounts based on date ranges.
- TODO: Responsive design for mobile devices.
- TODO: Add user authentication and authorization.


