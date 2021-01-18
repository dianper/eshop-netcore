# API Gateway
.NET Core

### Auth Service
localhost:8000/swagger

- SQL Server

### Catalog Service
localhost:8001/swagger

- MongoDB

### Basket Service
localhost:8002/swagger

- Redis

### Running
```sh
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```