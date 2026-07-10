# LAB 3 Architecture Report - gRPC and Microservices LMS

## 1. System Overview

This project refactors the Learning Management System from a single API into a microservices-based architecture. The system is divided into independent ASP.NET Core Web API services and uses YARP as an API Gateway. Service-to-service communication is implemented with gRPC for the required Course Service to Student Service lookup flow.

The main runtime components are:

- API Gateway: public entry point for client requests.
- Identity Service: authentication, authorization data, JWT generation, and refresh tokens.
- Student Service: student management and student information.
- Course Service: course management and enrollment management.
- Identity Database: SQL Server database for users and refresh tokens.
- Student Database: SQL Server database for students.
- Course Database: SQL Server database for courses, semesters, and enrollments.

All services are deployed together through Docker Compose. The gateway exposes a single client-facing HTTP endpoint, then routes requests to the correct internal service.

## 2. Service Decomposition

### Identity Service

The Identity Service owns authentication and token operations. It exposes APIs for login and refresh token generation. It validates user credentials, creates JWT access tokens, and stores hashed refresh tokens in the Identity database.

Main responsibilities:

- Login API.
- JWT access token generation.
- Refresh token generation and validation.
- User and role storage.
- Authentication-related persistence.

Relevant routes:

- `POST /api/v1/auth/login`
- `POST /api/v1/auth/refresh-token`

### Student Service

The Student Service owns student data. It exposes REST APIs for student CRUD operations and also hosts the gRPC server used by Course Service. The service does not depend on Course Service or Course database tables.

Main responsibilities:

- Student management.
- Student information lookup.
- gRPC server implementation for student lookup.

Relevant routes:

- `GET /api/v1/students`
- `GET /api/v1/students/{id}`
- `POST /api/v1/students`
- `PUT /api/v1/students/{id}`
- `DELETE /api/v1/students/{id}`

gRPC endpoint:

- `StudentLookup.GetStudent`

### Course Service

The Course Service owns course, semester, and enrollment data. It exposes REST APIs for course and enrollment workflows. When it needs student information, it does not access the Student database directly. Instead, it calls Student Service through a strongly typed gRPC client.

Main responsibilities:

- Course management.
- Semester relationship for courses.
- Enrollment management.
- Student existence validation through gRPC before enrollment.
- Course student summary retrieval through gRPC.

Relevant routes:

- `GET /api/v1/courses`
- `GET /api/v1/courses/{id}`
- `GET /api/v1/courses/{courseId}/students`
- `POST /api/v1/courses`
- `PUT /api/v1/courses/{id}`
- `DELETE /api/v1/courses/{id}`
- `GET /api/v1/enrollments`
- `POST /api/v1/enrollments`

## 3. Database Design

Each service has its own database container and its own connection string. Direct access to another service's database is avoided.

### Identity Database

Database name: `LMS_IDENTITY_DB`

Tables:

- `Users`
- `RefreshTokens`

The Identity database stores only authentication-related data. It does not store students, courses, semesters, subjects, or enrollments.

### Student Database

Database name: `LMS_STUDENT_DB`

Tables:

- `Students`

The Student database stores student profile information such as full name, email, and date of birth. Enrollment and course data are not stored here.

### Course Database

Database name: `LMS_COURSE_DB`

Tables:

- `Courses`
- `Semesters`
- `Enrollments`

The Course database stores enrollment rows with a `StudentId` value. This value is used as a foreign identifier across service boundaries, but it is not enforced as a database foreign key to the Student database. Student details are resolved through gRPC when needed.

## 4. API Gateway Configuration

The API Gateway uses YARP Reverse Proxy. It is the public entry point for clients and forwards requests to the correct service.

Route examples:

- `/api/auth/*` and `/api/v1/auth/*` -> Identity Service
- `/api/v1/students/*` -> Student Service
- `/api/v1/courses/*` -> Course Service
- `/api/v1/enrollments/*` -> Course Service

The gateway validates JWT tokens before forwarding protected requests. Student, course, and enrollment routes require the `authenticated` authorization policy. Authentication routes are left open so clients can log in and refresh tokens.

In Docker Compose, the gateway routes to internal container addresses:

- `http://identity-service:8080/`
- `http://student-service:8080/`
- `http://course-service:8080/`

## 5. gRPC Communication Flow

The required gRPC scenario is Course Service retrieving student information from Student Service.

Protocol Buffers file:

- `Protos/student.proto`

Defined gRPC service:

```proto
service StudentLookup {
  rpc GetStudent (StudentRequest) returns (StudentReply);
}
```

### Enrollment Flow

The enrollment business flow is:

1. Client sends `POST /api/v1/enrollments` to the API Gateway.
2. API Gateway validates the JWT token.
3. API Gateway forwards the request to Course Service.
4. Course Service receives `studentId` and `courseId`.
5. Course Service calls `StudentLookup.GetStudent` on Student Service through gRPC.
6. Student Service checks its own Student database.
7. Student Service returns whether the student exists and includes summary data.
8. Course Service creates the enrollment only if the student exists and the course exists.
9. Course Service returns the created enrollment response.

This prevents Course Service from reading the Student database directly while still allowing enrollment validation.

### Course Students Flow

The `GET /api/v1/courses/{courseId}/students` endpoint reads enrolled student IDs from the Course database, then resolves each student summary through the Student Service gRPC endpoint. This keeps detailed student information owned by Student Service.

## 6. Authentication and Authorization

JWT authentication is used across the services. Identity Service issues the JWT. API Gateway and internal services validate the token using the configured issuer, audience, and signing key.

Protected APIs use `[Authorize]`. Role-based authorization is also implemented, for example admin-only course deletion with `[Authorize(Roles = "Admin")]`.

Supported authentication scenarios:

- Login returns an access token and refresh token.
- Refresh token endpoint returns a new token pair.
- Protected endpoints reject missing or invalid tokens with HTTP 401.
- Role-protected endpoints require the correct user role.

## 7. Docker Deployment

Docker Compose defines the complete system with the minimum required containers:

- `api-gateway`
- `identity-service`
- `student-service`
- `course-service`
- `identity-db`
- `student-db`
- `course-db`

Each service has its own Dockerfile. SQL Server containers are configured with health checks. Application services wait for their database dependencies before starting. Course Service also depends on Student Service because it uses Student Service for gRPC lookup.

External ports:

- API Gateway: `8080`
- Identity Service: `8081`
- Student REST API: `8082`
- Student gRPC: `9082`
- Course Service: `8083`
- Identity DB: `14331`
- Student DB: `14332`
- Course DB: `14333`

## 8. Logging and Swagger

Serilog is configured in the services and gateway. Request logging records:

- HTTP method.
- Request path.
- Response status code.
- Execution time.

Swagger/OpenAPI is enabled for the services. Swagger includes JWT bearer authentication support so protected endpoints can be tested by entering a token in the Swagger UI authorization dialog.

## 9. Testing

The included Postman collection demonstrates the required scenarios:

- Login generates a JWT token.
- Protected API access succeeds with a valid JWT.
- Unauthorized request returns HTTP 401.
- Course enrollment succeeds after Student Service validates the student through gRPC.
- Invalid student enrollment fails.
- Course student lookup returns student information through Course Service to Student Service gRPC communication.
- Refresh token endpoint can generate a new token pair.

The solution also builds successfully with:

```bash
dotnet build Lab1.slnx
```

## 10. Notes

The project implements the core Lab 3 microservices and gRPC requirements. Bonus items such as RabbitMQ, Redis cache, OpenTelemetry distributed tracing, and Polly circuit breaker are not part of the current implementation.
