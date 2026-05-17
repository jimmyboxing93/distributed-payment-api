# Distributed Payment Processing System
**Modernized .NET 9 Microservices Architecture**

![.NET CI](https://github.com/jimmyboxing93/distributed-payment-api/actions/workflows/dotnet.yml/badge.svg?branch=master)

## 🚀 Project Evolution
This project represents a full-scale modernization of a legacy architecture. I have refactored the codebase from .NET Core 3.1 to **.NET 9**, prioritizing performance, containerization, and modern asynchronous patterns. The solution leverages **Semantic Kernel** to bridge structured business logic with generative AI.

## 🛠️ Tech Stack & Skills
- **Framework:** .NET 9 (Web API & Razor)
- **AI Integration:** Microsoft Semantic Kernel (AI Financial Agent logic and LLM & Vector DB Agnostic)
- **Containerization:** Docker & Docker-Compose (Multi-container orchestration)
- **CI/CD:** GitHub Actions (Automated Build & Test pipelines)
- **Architecture:** Microservices, Repository Pattern, Dependency Injection
- **Database:** SQL Server with EF Core 9 & Qdrant (Vector Store)

## 📂 Project Structure
- `src/PaymentGateway.API`: Core processing engine and Merchant logic.
- `src/Payment.ClientView`: The Consumer/User-facing portal.
- `src/AIFinancialService`: AI-driven service for financial insights.
- `src/SharedData`: Shared models and DTOs to ensure type safety across services.
- `src/PaymentProcessing.Tests`: Comprehensive xUnit and Moq suite.

## 🏗️ Architectural Highlights
- **Stateless IAM & Context Extraction:** Implemented a pure cryptographically signed JWT authentication lifecycle. Downstream AI microservices do not accept user identity strings inside message bodies or request payloads, entirely eliminating ID-tampering and enumeration attacks. Instead, services utilize an injected `IHttpContextAccessor` to safely pull validated identity parameters (`ClaimTypes.NameIdentifier`) directly out of the ambient request pipeline header context.
- **LLM & Vector Agnostic:** The AI layer is built using Semantic Kernel's abstraction layer. By swapping NuGet packages and configuration, the system can transition between **Google Gemini, OpenAI, or Anthropic**, and from **Qdrant to Pinecone or Milvus** without rewriting core business logic.
- **Hybrid AI Grounding (RAG + SQL):** The AI Agent performs "Real-World Grounding" by orchestrating two distinct data streams:
    1. **Unstructured Data:** RAG via Qdrant for banking policies and documentation.
    2. **Structured Data:** Native C# plugins for live SQL account lookups.
- **AI Security & Data Isolation:** Implemented the Interface Segregation Principle (ISP) to create a hard boundary for AI interactions. The AI Agent is injected with a restricted `IBankingReadService`, making it physically impossible for the LLM to execute Delete or Update commands, even if it "hallucinates" a request.
- **Asynchronous Flow:** Fully implemented async/await across the data and service layers to ensure non-blocking I/O.
- **Security:** Implemented custom Middleware for API Key authentication and protection against BOLA (Broken Object Level Authorization).
- **Containerized Environment:** Standardized development using Docker, ensuring seamless transitions between local and cloud environments.
- **Modernized AI Integration:** Utilizing Microsoft Semantic Kernel to bridge the gap between Natural Language Processing and structured C# business logic.

### 🛡️ Security & Reliability (xUnit + Moq)
- **BOLA Protection:** Verified via `ReturnsUnauthorized_WhenUserIsNotOwner` across sensitive operations.
- **Auth Challenge Logic:** Custom middleware overrides to ensure 401 Unauthorized status codes are returned for API consumers, preventing silent HTML redirects.
- **Data Integrity:** Ensured via `Verify(Times.Never)` to confirm no unauthorized database writes occur.
- **Input Validation:** Strict validation for credit card processing and financial data inputs.

## 📈 Roadmap (Active Dev)
- [x] Refactor UI and API to .NET 9
- [x] Reorganize Solution Architecture (`/src` pattern)
- [x] Implement Stateless JWT Authentication & Identity Stores
- [x] Refactor AI Services to extract secure identities via JWT claim context
- [x] Dockerize full environment
- [x] Implement xUnit & Moq for Core Logic
- [x] Implement Interface Segregation for AI Safety (Read-Only Plugin)
- [x] **Integrated RAG (Retrieval-Augmented Generation) for Bank Policies**
- [ ] Integrate Swagger/OpenAPI for RESTful Documentation
- [ ] Integrate AutoMapper for DTO management

```mermaid
graph TD
    User((User / Recruiter)) -->|HTTPS Request + Bearer Header| Gate{JWT Auth Gate}
    
    subgraph "Backend Orchestration (.NET 9)"
        Gate -->|Valid Token Context| MVC[ASP.NET Core 9 Controllers]
        Gate -->|Missing/Invalid| 401[401 Unauthorized Response]
        
        MVC -->|Injected HttpContext| Services[Domain Services & Interfaces]
        
        %% The Isolation Logic
        Services -->|Full CRUD IUserInfo| PaymentAPI[PaymentGateway API]
        Services -->|Stateless Claims Extraction| AI[AI Agent Layer / Semantic Kernel]
        
        AI <-->|Kernel Orchestration| Gemini[Gemini Pro]
    end

    subgraph "Infrastructure (Dockerized)"
        PaymentAPI -->|EF Core 9| DB[(SQL Server 2022)]
        AI -.->|Restricted Read-Only Access| DB
        Env[.env File] -->|Injected Secrets| PaymentAPI
    end

    subgraph "CI/CD & Quality Control"
        Actions[GitHub Actions] -->|Verify| Build[Build & Compile]
        Build -->|Execute| Tests[xUnit / Moq Suite]
        Tests -->|Status| Pass{{"Build: PASSING ✅"}}
        style Pass fill:#d4edda,stroke:#28a745,stroke-width:2px
    end

    %% Validating the code
    Pass -.->|Validates Isolation| AI
    Pass -.->|Validates JWT Auth| Gate
