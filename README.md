# Carbon-Aware "Green" API Monitor

## The Story
Cloud computing has a physical footprint. This project addresses the "invisible" carbon cost of software by connecting an ASP.NET Core API to real-time energy grid data.

## Purpose & Impact
The goal is to shift computational loads to "Green Windows"—times when renewable energy (wind/solar) is high. 
- **Impact:** Reduces the carbon intensity of background tasks like data processing or backups.
- **Technical Focus:** Demonstrates clean architecture using Dependency Injection, Middleware, and the Options Pattern in .NET.

## How the User Uses the System
1. **Check Status:** A user hits `/carbon/status` to receive a "Go/No-Go" signal based on current grid intensity (gCO2/kWh).
2. **Transparent Monitoring:** Every request to the API automatically returns an `X-Carbon-Intensity` header, informing the consumer of the environmental cost of that specific call.

## Tech Stack
- **Backend:** ASP.NET Core 10.0
- **Data Source:** UK National Grid Carbon Intensity API
- **UI Documentation:** Scalar (OpenAPI)