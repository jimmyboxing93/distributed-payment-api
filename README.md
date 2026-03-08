# Distributed Payment Processing System
**Modernized .NET 9 Microservices Architecture**

[![.NET Core CI](https://github.com/jimmyboxing93/distributed-payment-api/actions/workflows/dotnet.yml/badge.svg)](https://github.com/jimmyboxing93/distributed-payment-api/actions)

## 🚀 Project Evolution
This project represents a full-scale modernization of a legacy Service-Oriented Architecture (SOA). I have refactored the codebase from .NET Core 3.1/5.0 to **.NET 9**, prioritizing performance, containerization, and modern asynchronous patterns.

## 🛠️ Tech Stack & Skills
- **Framework:** .NET 9 (MVC & Web API)
- **Containerization:** Docker & Docker-Compose (Multi-container orchestration)
- **CI/CD:** GitHub Actions (Automated Build & Test pipelines)
- **Architecture:** Microservices, Repository Pattern, Dependency Injection
- **Communication:** RESTful API with HttpClient Factory & Typed Clients

## 🏗️ Architectural Highlights
- **Microservices-Ready:** Decoupled the monolithic logic into a dedicated Payment API and a Consumer UI.
- **Asynchronous Flow:** Fully implemented async/await across the data and service layers to ensure non-blocking I/O.
- **Security:** Implemented custom Middleware for API Key authentication and secure environment variable management.
- **Containerized Environment:** Standardized development and deployment using Docker, ensuring it works on anyones machine and translates to a cloud enviornment.

## 📈 Roadmap (Active Dev)
- [x] Refactor UI and API to .NET 9
- [x] Dockerize full environment
- [ ] Implement xUnit & Moq for Core Logic
- [ ] Add AutoMapper for DTO management
- [ ] Build Java 17 Microservice equivalent (Polyglot Engineering)
