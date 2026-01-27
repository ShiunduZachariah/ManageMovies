# ManageMovies API

A RESTful API for managing a movie collection, built with .NET 10, Entity Framework Core, and PostgreSQL.

## 🚀 Tech Stack

*   **.NET 10** (ASP.NET Core Web API)
*   **Entity Framework Core** (ORM)
*   **PostgreSQL** (Database)
*   **Docker & Docker Compose** (Containerization)
*   **Scalar** (API Documentation)

## 📋 Prerequisites

*   [.NET 10 SDK](https://dotnet.microsoft.com/download)
*   [Docker Desktop](https://www.docker.com/products/docker-desktop)

## 🛠️ Getting Started

### 1. Start the Database

Use Docker Compose to spin up the PostgreSQL container.

```bash
docker-compose up -d
```

This will start a PostgreSQL instance on port `5432` with the credentials defined in `docker-compose.yaml`.

### 2. Apply Migrations

Update the database schema to match your models.

```bash
cd ManageMovies
dotnet ef database update
```

### 3. Run the Application

```bash
dotnet run
```

The API will be available at `https://localhost:7043` (or similar, check your launch logs).

## 📖 API Documentation

Once the application is running, you can access the interactive API documentation (Scalar) at:

*   **URL:** `/scalar/v1` (e.g., `https://localhost:7043/scalar/v1`)

### Endpoints

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/movies` | Create a new movie |
| `GET` | `/api/movies` | Get all movies |
| `GET` | `/api/movies/{id}` | Get a movie by ID |
| `PUT` | `/api/movies/{id}` | Update a movie |
| `DELETE` | `/api/movies/{id}` | Delete a movie |

## ⚙️ Configuration

Configuration is handled in `appsettings.json`.

**Database Connection String:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=ManageMovie;Username=admin;Password=TestPassword2025;"
}
```

## 🧪 Testing

You can use the `ManageMovies.http` file included in the project to test endpoints directly from your IDE (VS Code, Rider, or Visual Studio).
