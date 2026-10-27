# Bookstore Backend

A clean, modern RESTful Web API for a bookstore application built with .NET 10,
Entity Framework Core, and PostgreSQL.

---

## Prerequisites

Ensure you have the following installed on your machine:

- **.NET 10 SDK** (can be managed via [mise](https://mise.jdx.dev/))
- **Docker & Docker Compose** (for PostgreSQL)
- **Git**

---

## Getting Started

> **Note:** The instructions below use standard Unix shell syntax (Linux/macOS).
> Windows users can execute the same commands in Visual Studio Developer PowerShell.

### 1. Set Up Environment & Restore Dependencies

Navigate to the project root directory:

```bash
cd backend
```

If you use [mise](https://mise.jdx.dev/) for environment management, install the
configured tools:

```bash
mise install
```

Restore NuGet packages and local .NET CLI tools (`dotnet-ef`):

```bash
dotnet restore
dotnet tool restore
```

---

### 2. Start the Database (PostgreSQL)

Launch the PostgreSQL container in the background:

```bash
docker compose up -d
```

> **Note:** Ensure that credentials and ports specified in `compose.yaml` match
> the connection string in `src/Bookstore.Api/appsettings.json`.

---

### 3. Apply Database Migrations

Apply the EF Core migrations to create or update the database schema:

```bash
dotnet ef database update --project src/Bookstore.Data --startup-project src/Bookstore.Api
```

> [!TIP]
> Whenever you modify entities and need to create a new migration:
>
> ```bash
> dotnet ef migrations add <MigrationName> \
> --project src/Bookstore.Data \
> --startup-project src/Bookstore.Api \
> -o Migrations
> ```

---

### 4. Run the API

Start the Web API server:

```bash
dotnet run --project src/Bookstore.Api --launch-profile https
```

Once running:

- **Base URL:** `https://localhost:<port>` (check the terminal output for the
  active port)
- **Interactive API Documentation (Scalar UI):** `https://localhost:<port>/scalar/v1`

---

## Teardown & Maintenance

Stop the database container:

```bash
docker compose down
```

Wipe all database volumes and reset from scratch:

```bash
docker compose down -v
docker compose up -d
dotnet ef database update --project src/Bookstore.Data --startup-project src/Bookstore.Api
```
