# API Gateway
.NET Core

- Oncelot

## Requirements
- Docker

## Available Services
All services available

### Auth Service
localhost:8000/swagger

- JWT Authentication
- SQL Server

### Catalog Service
localhost:8001/swagger

- MongoDB

### Basket Service
localhost:8002/swagger

- Redis
- RabbitMQ

### Checkout
localhost:8003/swagger

- RabbitMQ
- SQL Server

### Running
```sh
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```