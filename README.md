# Currency Conversion API

This is a **.NET Core Web API** application developed as a submission for the Currency Converter API Assessment. It is designed to be **robust, scalable, maintainable**, and aligned with modern software engineering practices.

The API enables conversion between currencies, retrieval of historical and real-time exchange rates, and enforces security and performance measures including JWT authentication, caching, and circuit breaking.

---

## 🧾 Objective

Design and implement a **currency conversion API** using **C# and ASP.NET Core**, focusing on:

- High performance
- Security
- Resilience
- Extensibility
- Observability

---

## 🚀 Features

- Retrieve the latest exchange rates for a base currency (e.g., EUR)
- Convert amounts between currencies (excludes TRY, PLN, THB, and MXN)
- Retrieve historical exchange rates with pagination support
- JWT authentication with role-based access control (RBAC)
- Swagger UI for API exploration and testing

---

## 🛠️ Requirements

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual Studio 2022 (v17.8 or later) or Visual Studio 2023](https://visualstudio.microsoft.com/)
- Internet connection for third-party API calls (Frankfurter API)

---

## 🧑‍💻 Getting Started

1. **Clone the repository**

   ```bash
   git clone https://github.com/nurlab/CurrencyConversion.git
   ```

2. **Open in Visual Studio** and run the application (`F5`)

3. **Access Swagger UI**

   ```
  https://localhost:7261/swagger
   ```

---

## 🔐 Authentication & Authorization

Roles are assigned during signup:
| Role ID | Role Name |
|---------|-----------|
| 1       | Admin     |
| 2       | Manager   |
| 3       | User      |

### How to Use:

- **Signup** via `/Account/sign-up` using username, password, and role.
- **Signin** via `/Account/sign-in` to get a JWT token.
- Use the **Authorize** button in Swagger to input your token in the Value box:
- Once authorized, access APIs based on your role privileges.

---
## 🧪 Sample Users for Testing

Use the following sample accounts to interact with the API. Each account has a predefined role that determines the level of access to the various currency conversion endpoints.

| Username | Password  |
|----------|-----------|
| Admin    | Hello1!@  |
| Manager  | Hello1!@  |
| User     | Hello1!@  | 

### 🔐 Role-Based Access Control (RBAC)

Each role is associated with specific permissions to access certain API endpoints:

- **Admin (Role ID: 1)**: access to the latest exchange rate endpoint:
  - `GET /get-latest-exchange-rate`: Retrieve the latest exchange rates for a specified base currency.

- **Manager (Role ID: 2)**: access to the currency conversion endpoint:
  - `POST /convert`: Convert an amount from one currency to another.

- **User (Role ID: 3)**: accessing historical exchange rate data:
  - `POST /get-rate-history`: Retrieve historical exchange rates for a given date range.

## 📌 Assumptions

- Role numbers (1, 2, 3) are directly input by the user.
- Excluded currencies (TRY, PLN, THB, MXN) will result in a `400 Bad Request`.
- Exchange rate provider is [Frankfurter API](https://www.frankfurter.app/).
- No frontend is included in this submission.
- Swagger is the primary interface for testing.

---

## 🧱 Architecture & Design

- **Caching**: Reduces calls to Frankfurter API.
- **Retry Policies**: Implements exponential backoff.
- **Circuit Breaker**: Gracefully handles API outages.
- **DI & Factory Pattern**: For service extensibility and multiple providers.
- **Security**: JWT-based authentication and rate limiting.
- **RBAC**: Restricts endpoints based on role.
- **Logging & Monitoring**:
  - Logs client IP, client ID, endpoint, status, and duration.
  - Structured logging via Serilog.
  - Supports distributed tracing using OpenTelemetry.

---

## 🧪 Testing

- API tests for verifying endpoint functionality
- Integration tests covering service and data flow
- Service-level tests to ensure business logic correctness
- Test coverage reports available upon request

---

## ☁️ Deployment

- Supports **Dev, Test, and Prod** environments
- Ready for **horizontal scaling**
- Includes **API versioning**

---

# 🚀 Possible Future Enhancements

1. **Microservices Architecture for Scalability**
   - **Description**: Break down the monolithic application into smaller, independently deployable microservices, each focused on a specific domain (e.g., currency conversion, user management, rate fetching). This will allow each service to scale independently based on demand.
   - **Benefit**: Improves scalability and flexibility, enabling you to allocate resources efficiently. It also makes the system more resilient since failures in one service do not affect the others.

2. **Event-Driven Architecture (EDA)**
   - **Description**: Introduce an event-driven approach with event brokers like Kafka or RabbitMQ. For instance, currency conversion requests can trigger events (e.g., for logging, notifications, or updates to external systems).
   - **Benefit**: Enhances the system's scalability and responsiveness. It decouples services, allowing them to process events asynchronously, reducing the risk of bottlenecks.

3. **CQRS (Command Query Responsibility Segregation)**
   - **Description**: Implement CQRS where the system is split into two parts: one handling commands (writes/updates) and another for queries (reads). This could be beneficial for operations that involve frequent read/write operations, such as currency conversions and rate history queries.
   - **Benefit**: Improves performance and scalability, especially for high-read or high-write applications. It also allows for better optimization and more efficient handling of complex queries.

4. **API Gateway**
   - **Description**: Use an API Gateway (e.g., Ocelot in .NET Core) to centralize API routing, load balancing, and rate limiting. This will act as a front-facing layer for client applications, routing requests to the appropriate backend services.
   - **Benefit**: Simplifies the system architecture, provides centralized management of cross-cutting concerns like security, logging, and rate limiting, and reduces the burden on individual microservices.

5. **Multi-Provider Integration**
   - **Description**: Currently, the API relies on a single exchange rate provider (e.g., Frankfurter API). Future versions could integrate multiple providers (e.g., Open Exchange Rates, CurrencyLayer) to provide redundancy, better availability, and potentially more accurate rates.
   - **Benefit**: This would improve the resilience and reliability of the service in case one provider becomes unavailable or has inaccuracies.

6. **Asynchronous Processing and Queuing**
   - **Description**: For more complex operations or bulk currency conversion, introduce asynchronous processing and queuing (e.g., using **Azure Queue Storage** or **RabbitMQ**).
   - **Benefit**: This would allow the API to process requests in the background, improving responsiveness for long-running operations.

7. **Geolocation-Based Currency Detection**
   - **Description**: Add support for automatic currency detection based on the user’s geographical location. This could be done using an IP-to-country mapping service.
   - **Benefit**: Provides a more user-friendly experience by automatically selecting the base currency based on the user's location.

8. **Currency Conversion History and Analytics**
   - **Description**: Store historical conversion data in a database (e.g., **SQL Server** or **NoSQL** like **MongoDB**) and allow users to query historical conversion history. This could be used for analytics or reporting.
   - **Benefit**: Adds value for users who want to track historical trends or perform more advanced analysis on currency conversion data.

9. **Audit Logging for User Actions**
   - **Description**: Implement comprehensive audit logging for all user actions, including login attempts, currency conversions, and other key activities.
   - **Benefit**: This would improve the system's transparency and help in tracking any suspicious activity or troubleshooting issues.

10. **Support for Additional Currency Features**
    - **Description**: Add support for additional features like:
      - **Currency conversion fees**: Allow users to set and apply conversion fees.
      - **Custom exchange rates**: Enable users to define their own exchange rates for specific use cases (e.g., for testing purposes).
    - **Benefit**: Extends the API's functionality to support more advanced financial services, increasing its versatility.

11. **Data Aggregation and Reports**
    - **Description**: Add endpoints to aggregate exchange rate data over a period and generate simple reports (e.g., average rate over a week, volatility of a specific currency).
    - **Benefit**: Offers a more comprehensive tool for businesses needing insights into currency fluctuations.

12. **Multi-Language Support**
    - **Description**: Extend the API to support multiple languages for broader internationalization.
    - **Benefit**: Makes the API accessible to non-English speaking users and enhances user experience across diverse regions.

13. **Integration with Cryptocurrency Data**
    - **Description**: In the future, integrate cryptocurrency exchange rates (e.g., Bitcoin to USD) alongside traditional currencies.
    - **Benefit**: This would expand the service to support digital currencies, aligning with current trends in the financial industry.

14. **Customizable Response Formats**
    - **Description**: Allow clients to specify the format of the API response (e.g., JSON, XML, CSV) to accommodate different use cases.
    - **Benefit**: Provides more flexibility for clients integrating with the API, especially those in different industries or regions that may have specific formatting preferences.

15. **Real-Time Exchange Rate Updates**
    - **Description**: Implement WebSocket or Server-Sent Events (SSE) for real-time updates on currency exchange rates.
    - **Benefit**: Provides users with live rate updates, especially useful for financial traders or businesses that require real-time exchange rate data.

16. **NuGet Package for Client Integration**
    - **Description**: Package the core functionality of the currency conversion API as a **NuGet** package for easy consumption by other developers and systems. The package could include:
      - Client libraries for interacting with the API.
      - Helpers and utilities for performing conversions and managing API requests.
    - **Benefit**: This would make it easier for developers to integrate currency conversion functionality into their applications without needing to directly call the API.

17. **Multi-Tenant Support**
    - **Description**: Implement multi-tenancy to allow different businesses or users to have their own isolated environments, data, and configurations.
    - **Benefit**: Ideal for SaaS offerings where multiple clients use the same API but need separate data partitions.

18. **Advanced User Management (Roles, Permissions, MFA)**
    - **Description**: Implement more advanced user management features, including multi-factor authentication (MFA) and granular role-based access control (RBAC) for API endpoints.
    - **Benefit**: Enhances security by ensuring only authorized users can access sensitive data and operations.

19. **Self-Service Dashboard for Users**
    - **Description**: Create a self-service dashboard where users can track their usage, view transaction history, set preferences (e.g., base currency), and more.
    - **Benefit**: Provides users with greater control and insight into their usage of the service, improving overall satisfaction.

20. **Containerization and CI/CD Pipeline**
    - **Description**: Containerize the application using Docker and integrate with CI/CD pipelines for automated testing, building, and deployment.
    - **Benefit**: Ensures portability and scalability, making the app easy to deploy and manage in cloud environments like AWS, Azure, or Kubernetes.

21. **Customizable Alerts and Notifications**
    - **Description**: Allow users to set price alerts for specific currency conversion thresholds. Users can receive email or SMS notifications when rates reach their desired value.
    - **Benefit**: Adds more user engagement, especially for traders or businesses that need to monitor specific currency thresholds.

22. **Batch Currency Conversion**
    - **Description**: Implement a batch conversion API that can process multiple conversions in one request, improving efficiency for users who need to convert large volumes of currencies at once.
    - **Benefit**: Ideal for financial institutions, enterprises, or applications that require frequent bulk conversions.

23. **Advanced Caching with Content Delivery Networks (CDNs)**
    - **Description**: Enhance the caching mechanism by integrating with a Content Delivery Network (CDN) for faster delivery of exchange rate data globally.
    - **Benefit**: Improves performance, particularly for international users, by reducing latency and enhancing response times.

24. **Rate Shopping and Price Comparison**
    - **Description**: Implement a rate shopping feature that compares exchange rates from multiple providers, helping users get the best rate for their conversions.
    - **Benefit**: Enhances user experience by giving them more flexibility to choose the best rate available at the time of conversion.

25. **Currency Conversion for Non-Standard Units (Gold, Silver)**
    - **Description**: Extend the currency conversion service to handle precious metals like gold and silver, which are commonly traded in different units (e.g., ounces, grams).
    - **Benefit**: Broadens the use cases, attracting users involved in commodities trading or investment.

26. **GraphQL for Flexible Data Fetching**
    - **Description**: Integrate GraphQL as an alternative to REST APIs, providing more flexibility for clients to request only the data they need (e.g., for fetching exchange rates with additional metadata).
    - **Benefit**: Improves efficiency by reducing over-fetching and under-fetching of data, offering clients more control over the data they consume.

---

## 📫 Contact

Please open an issue on the repository for feedback or questions.
