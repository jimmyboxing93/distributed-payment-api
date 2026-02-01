![Build Status](https://github.com/jimmyboxing93/distributed-payment-api/actions/workflows/dotnet.yml/badge.svg)

# Distributed Payment Processing API
**Status: Modernization in Progress**
*Currently refactoring to include xUnit Testing, AutoMapper, and Java 17 Microservice equivalents.*

## Overview
A Service-Oriented Architecture (SOA) system built to handle credit card processing. This project demonstrates a decoupled design where a frontend MVC application communicates with a secured backend REST API.

## Tech Stack
- **Backend:** C# | .NET Core | Web API
- **Data:** Entity Framework Core | SQL Server (Repository Pattern)
- **Frontend:** ASP.NET MVC | HttpClient (SOA Integration)
- **Security:** API Key Middleware | ASP.NET Identity
- ** DevOps:** GitHub Actions (CI/CD) | .NET 9 SDK

## Key Architectural Patterns
- **Repository Pattern:** Abstracted data access layer using interfaces (`IUserInfo`) to ensure the business logic is independent of the database provider.
- **Dependency Injection:** Utilized constructor injection to maintain loose coupling across the system.
- **Distributed Services:** Developed a separate API layer to allow for independent scaling of the frontend and backend.
