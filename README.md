# Carbon-Aware "Green" API Monitor

## Background

Cloud computing has a physical footprint. This project addresses the "invisible" carbon cost of software by connecting an ASP.NET Core API to real-time energy grid data. By making carbon intensity visible, we can build systems that prioritize sustainability alongside performance.

---

## Purpose & Impact

The goal is to shift computational loads to **"Green Windows"** — times when renewable energy (wind/solar) is dominant on the grid.

- **Environmental Impact:** Reduces the carbon footprint of automated tasks like data processing, backups, or AI training.
- **Technical Impact:** Demonstrates an enterprise-grade stack featuring real-time data streaming, defensive security, and automated background processing.

---

## How the System Works

1. **Automated Monitoring:** A .NET Background Service polls the UK National Grid API every 30 seconds, analyzing and categorizing the current carbon intensity.
2. **Real-Time Visualization:** Using SignalR (WebSockets), the React frontend updates instantly as new data is collected — no manual refresh required.
3. **Defensive API Design:** Built-in Rate Limiting protects against API abuse, while Global Error Handling ensures professional JSON responses even during server-side failures.

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

## Tech Stack

### Backend (.NET 10)

- **SignalR:** Real-time bi-directional communication between server and UI.
- **EF Core + SQLite:** Persistent relational storage for historical trend analysis.
- **Middleware Pipeline:** Global Exception Handling, CORS Policy Management, and Fixed-Window Rate Limiting.
- **User Secrets:** Secure configuration management (Zero-Footprint in Git).

### Frontend (React + Vite)

- **Tailwind CSS v4:** Modern, "CSS-first" styling for a high-performance UI.
- **SignalR Client:** Live data synchronization with built-in duplicate prevention logic.
- **Responsive Design:** Optimized for both desktop and mobile viewing.

---

## Getting Started

### 1. Clone & Setup

```bash
git clone <your-repo-url>
cd CarbonAware
```

### 2. Launch the Backend

```bash
cd CarbonAware.Api
dotnet user-secrets init
dotnet user-secrets set "CarbonApi:BaseUrl" "https://api.carbonintensity.org.uk/"
dotnet ef database update
dotnet run
```

API available at: `http://localhost:5258`

### 3. Launch the Frontend

```bash
cd ../CarbonAware.UI
npm install
npm run dev
```

Dashboard available at: `http://localhost:5173`

---

## Security & Robustness Features

- **CORS Policy:** Strict origin validation to permit only trusted frontend consumers.
- **Rate Limiting:** Fixed-window policy (e.g., 5 requests per 10s) to prevent DoS attacks and resource exhaustion.
- **Idempotent UI:** Frontend logic filters incoming SignalR events to prevent duplicate rendering during network race conditions.
- **X-Carbon Headers:** Every API response includes custom headers (`X-Carbon-Intensity`) to promote developer awareness.