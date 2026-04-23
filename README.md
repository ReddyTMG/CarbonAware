# Carbon-Aware "Green" API Monitor

## The Story
Cloud computing has a physical footprint. This project addresses the "invisible" carbon cost of software by connecting an ASP.NET Core API to real-time energy grid data. By making carbon intensity visible, we can build systems that prioritize sustainability alongside performance.

## Purpose & Impact
The goal is to shift computational loads to "Green Windows"—times when renewable energy (wind/solar) is dominant on the grid.
- **Environmental Impact:** Helps reduce the carbon footprint of automated tasks like data processing, backups, or AI training.
- **Technical Impact:** Demonstrates professional "Clean Architecture" using Dependency Injection, Middleware, and Background Tasks in .NET.

## How to Use the System
1. **Check Status:** Hit the `/carbon/status` endpoint to get a "Go/No-Go" signal based on live grid data.
2. **View History:** Access `/carbon/history` to see trends over time, allowing for better scheduling of heavy tasks.
3. **Transparent Cost:** Every request to any API endpoint automatically returns an `X-Carbon-Intensity` header, informing the consumer of the current environmental cost.

---

## Prerequisites

Before running this project, ensure you have the following installed:

* **Runtime/SDK:** [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
* **IDE:** [Visual Studio Code](https://code.visualstudio.com/) with the **C# Dev Kit** extension.
* **Global Tools:** The Entity Framework Core CLI tools (required for database management):
```bash
    dotnet tool install --global dotnet-ef
```

---

## Getting Started

### 1. Clone the Repository
```bash
git clone <your-repo-url>
cd CarbonAware
```

### 2. Restore Dependencies
Navigate into the API folder and restore the NuGet packages:
```bash
cd CarbonAware.Api
dotnet restore
```

### 3. Setup the Database
This project uses SQLite for local development. To generate the database file and apply the schema (Migrations), run:
```bash
dotnet ef database update
```

### 4. Run the Application
Start the server:
```bash
dotnet run
```

The API will be available at `http://localhost:5258`.

* **Interactive Documentation:** Explore the endpoints via Scalar UI at `http://localhost:5258/scalar/v1`.

---

## Tech Stack & Core Concepts

* **Backend:** ASP.NET Core 10.0 (Web API)
* **Data Source:** UK National Grid Carbon Intensity API
* **ORM:** Entity Framework Core (SQLite)
* **Patterns:**
  * **Dependency Injection (DI):** Decoupling services from controllers.
  * **Middleware:** Intercepting requests to log environmental data globally.
  * **Background Workers:** Using `BackgroundService` to poll data independently of user requests.