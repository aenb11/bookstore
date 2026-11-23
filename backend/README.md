# Bookstore Backend

A clean, modern RESTful Web API for a bookstore application built with .NET 10,
Entity Framework Core, and PostgreSQL.

---

## Prerequisites

Depending on how you want to run the project, ensure you have the following installed:

- **Option A — Full Docker Setup (Recommended for quick start / frontend integration):**
  - **Docker & Docker Compose** (with Buildx enabled)
  - **Git**
- **Option B — Local Development (Running API directly on host):**
  - **.NET 10 SDK** (can be managed via [mise](https://mise.jdx.dev/))
  - **Docker & Docker Compose** (for PostgreSQL)
  - **Git**

---

## Option A: Run Everything with Docker (Recommended)

This setup builds the .NET 10 API image and starts both PostgreSQL and the Backend API in isolated containers. Database migrations and development sample data seeding are applied automatically on startup.

### 1. Build and Start Services

From the `backend` root directory, run:

```bash
docker compose up -d --build
```

> **Note:** On subsequent runs, if you haven't modified any C# source code, `.csproj`, or `Dockerfile`, you can omit `--build` for near-instant startup:
>
> ```bash
> docker compose up -d
> ```

### 2. Verify Startup & Access API

Monitor the API logs to confirm migrations and startup succeeded:

```bash
docker compose logs -f bookstore_api
```

Once running (defaults to port `5000` on host — check `compose.yaml` if customized):

- **Base URL:** `http://localhost:5000`
- **Interactive API Documentation (Scalar UI):** `http://localhost:5000/scalar/v1`
- **OpenAPI Specification (JSON):** `http://localhost:5000/openapi/v1.json`
- **Uploaded Static Files:** `http://localhost:5000/files/<file-key>`

---

## Option B: Local Development (Host API + Docker DB)

Use this workflow when actively coding, debugging, or generating EF Core migrations locally.

> **Note:** The instructions below use standard Unix shell syntax (Linux/macOS).
> Windows users can execute the same commands in Visual Studio Developer PowerShell.

### 1. Set Up Environment & Restore Dependencies

Navigate to the project root directory:

```bash
cd backend
```

If you use [mise](https://mise.jdx.dev/) for environment management, install the configured tools:

```bash
mise install
```

Restore NuGet packages and local .NET CLI tools (`dotnet-ef`, `csharpier`):

```bash
dotnet restore
dotnet tool restore
```

### 2. Start Only the PostgreSQL Database

Launch only the PostgreSQL container (`postgres`) so it doesn't conflict with your local API process:

```bash
docker compose up -d postgres
```

> **Note:** Ensure that the database credentials and host port specified in `compose.yaml` match `ConnectionStrings:DefaultConnection` in `src/Bookstore.Api/appsettings.json` (or `appsettings.Development.json`).

### 3. Apply Database Migrations

Apply the EF Core migrations to create or update the database schema (development seed data will also run during initialization):

```bash
dotnet ef database update --project src/Bookstore.Data --startup-project src/Bookstore.Api
```

> [!TIP]
> Whenever you modify entities and need to create a new migration:
>
> ```bash
> dotnet ef migrations add <MigrationName> --project src/Bookstore.Data --startup-project src/Bookstore.Api -o Migrations
> ```

### 4. Run the API Locally

Start the Web API server:

```bash
dotnet run --project src/Bookstore.Api --launch-profile https
```

Once running:

- **Base URL:** `https://localhost:<port>` (check the terminal output for the active port)
- **Interactive API Documentation (Scalar UI):** `https://localhost:<port>/scalar/v1`
- **OpenAPI Specification (JSON):** `https://localhost:<port>/openapi/v1.json`

---

## Teardown & Maintenance

Stop all running containers (preserves database volume and uploaded files):

```bash
docker compose down
```

Wipe all database volumes and reset from scratch (re-applies migrations and re-seeds dev data on next startup):

```bash
docker compose down -v
docker compose up -d --build
```

---

## Code Formatting

This project enforces consistent C# formatting via [CSharpier](https://csharpier.com/).

Check code formatting without modifying files:

```bash
dotnet csharpier check .
```

Format all files before committing:

```bash
dotnet csharpier format .
```
