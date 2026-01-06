SMT OMS (Order Management System)

SMT OMS is a full-stack Order Management System built with ASP.NET Core, Angular, SQLite, and Docker. It is designed with a clean domain model, containerized deployment, and CI/CD via GitHub Actions.

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

Features & Implementation Notes

Full CRUD APIs implemented for Component, Board, and Orders.
Frontend UI currently implements only:
Order creation
Order deletion
JSON and formatted viewing of orders

Due to time constraints, backend APIs for Board and Components and Order updating are implemented but not fully exposed in the UI.

Entire app is dockerized and can run with a single command:
docker compose up --build -d

CI/CD pipeline applied via GitHub Actions.

Deployment note: Firebase is not suitable for the .NET backend; alternative hosting solutions should be considered for full-stack deployment.

Local Development (Docker)

Prerequisites
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