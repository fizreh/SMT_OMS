## Project Background
For details on the project evolution and backend development challenges, see [PROJECT_HISTORY.md](./Project_History.md).

SMT OMS (Order Management System)

SMT OMS is a full-stack Order Management System built with ASP.NET Core, Angular, SQLite, and Docker, designed with a clean domain model, containerized deployment, and CI/CD via GitHub Actions.

Tech Stack

Backend
ASP.NET Core Web API
Entity Framework Core
SQLite (persisted via Docker volume)
Serilog logging

Frontend
Angular (standalone components)
Angular Material
Nginx (production serving & API reverse proxy)

Infrastructure
Docker & Docker Compose
GitHub Actions (CI/CD)
Docker Hub (image registry)


Local Development (Docker)

*Prerequisites
Docker & Docker Compose
Node.js (optional, for local Angular dev)
.NET SDK (optional, for local API dev)

Run the Application
docker compose up -d --build

Access
Frontend: http://localhost:4200
Backend API: http://localhost:7121
Swagger: http://localhost:7121/swagger

Data Persistence
SQLite database is stored in a Docker volume:

volumes:
  - smt-data:/app/data

  CI/CD Pipeline
Triggered on pushes or PRs to main.

Pipeline Steps
Checkout source code
Inject environment secrets
Build backend Docker image
Build frontend Docker image
Push images to Docker Hub

Docker Images
smt_oms:backend
smt_oms:frontend