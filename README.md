## Prerequisites

- [Docker Desktop for Windows](https://www.docker.com/products/docker-desktop/) (includes Docker Compose)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (only for local development without Docker)
- [Git for Windows](https://git-scm.com/download/win)

> All commands below are for **PowerShell** or **Command Prompt (CMD)**. Docker Desktop must be running before executing any `docker` command.

---

## Running with Docker (recommended)

**1. Clone the repository**

```powershell
git clone <repository-url>
cd InventoryAPI
```

**2. Create the environment file**

```powershell
copy .env.example .env
```

Then open `.env` with Notepad or any editor and set your values:

```
DB_SA_PASSWORD=YourStrong!Passw0rd
JWT_KEY=super-secret-jwt-key-minimum-32-characters-long
JWT_ISSUER=inventory-api
JWT_AUDIENCE=inventory-api-clients
AUTH_CLIENT_ID=inventory-client
AUTH_CLIENT_SECRET=inventory-secret-2024
```

**3. Build and start the application**

```powershell
docker-compose up --build
```

Swagger UI: `https://localhost:7266/swagger/index.html`.

To stop the containers:

```powershell
docker-compose down
```

---

## Running locally (without Docker)

**1. Start only the database container**

```powershell
docker-compose up db db-init
```

**2. Set environment variables**

In **PowerShell**:

```powershell
$env:ConnectionStrings__Default = "Server=localhost,1433;Database=InventoryDB;User Id=sa;Password=Ad1nB@seDat0$;TrustServerCertificate=True;"
$env:Jwt__Key                   = "MiClaveSuperSecretaMuyLarga123456789"
$env:Jwt__Issuer                = "InventoryAPI"
$env:Jwt__Audience              = "InventoryAPIUsers"
$env:Auth__ClientId             = "root"
$env:Auth__ClientSecret         = "toor"
```

In **Command Prompt (CMD)**:

```cmd
set ConnectionStrings__Default=Server=localhost,1433;Database=InventoryDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;
set Jwt__Key=super-secret-jwt-key-minimum-32-characters-long
set Jwt__Issuer=inventory-api
set Jwt__Audience=inventory-api-clients
set Auth__ClientId=inventory-client
set Auth__ClientSecret=inventory-secret-2024
```

**3. Run the API**

```powershell
cd src\InventoryAPI.API
dotnet run
```

---

## Running unit tests

```powershell
dotnet test tests\InventoryAPI.Tests\InventoryAPI.Tests.csproj
```

To see detailed output per test:

```powershell
dotnet test tests\InventoryAPI.Tests\InventoryAPI.Tests.csproj --logger "console;verbosity=detailed"
```

---

## Authentication

The API uses JWT Bearer tokens via OAuth2 `client_credentials` flow.

**Get a token** (using PowerShell):

```powershell
$body = '{ "clientId": "inventory-client", "clientSecret": "inventory-secret-2024" }'
Invoke-RestMethod -Uri http://localhost:5232/api/auth/token -Method Post -Body $body -ContentType "application/json"
```

Or using **curl** (available in PowerShell 5+ and CMD on Windows 10+):

```powershell
curl -X POST http://localhost:5232/api/auth/token `
     -H "Content-Type: application/json" `
     -d "{ \"clientId\": \"inventory-client\", \"clientSecret\": \"inventory-secret-2024\" }"
```

Response:

```json
{
  "access_token": "<jwt>",
  "token_type": "Bearer"
}
```

Use the token in all subsequent requests:

```
Authorization: Bearer <access_token>
```

> Tip: You can also use the **Swagger UI** at `http://localhost:7266/swagger` — click **Authorize**, paste `Bearer <token>`, and all requests will include the header automatically.

---

## API Endpoints

All endpoints require `Authorization: Bearer <token>`.

### Products

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/products` | List all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create a product |
| PUT | `/api/products/{id}` | Update a product |
| DELETE | `/api/products/{id}` | Delete a product |

**Create product body:**

```json
{
  "name": "Laptop Pro",
  "description": "High-performance laptop",
  "price": 1299.99,
  "stock": 50,
  "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

### Categories

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/categories` | List all categories |
| GET | `/api/categories/{id}` | Get category by ID |
| POST | `/api/categories` | Create a category |
| PUT | `/api/categories/{id}` | Update a category |
| DELETE | `/api/categories/{id}` | Delete a category |

**Create category body:**

```json
{
  "name": "Electronics",
  "description": "Electronic devices and accessories"
}
```

### Inventory Movements

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/inventory/movements` | Register stock in/out movement |

**Register movement body:**

```json
{
  "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "quantity": 10,
  "movementType": 1,
  "reason": "Purchase order #1234"
}
```

`movementType`: `1` = In (stock entry), `2` = Out (stock exit)

---

## Swagger

Interactive API documentation available at `http://localhost:7266/swagger` when the application is running. Includes a Bearer token input field for authenticated requests.

---

## Troubleshooting on Windows

**Docker Desktop not starting containers**
Make sure Docker Desktop is fully running (system tray icon is stable, not animating) before running `docker-compose up`.

**Port 1433 already in use**
A local SQL Server instance may be using port 1433. Stop it via Services (`services.msc`) or change the port mapping in `docker-compose.yml` to `"1434:1433"` and update the connection string accordingly.

**`docker-compose` not recognized**
Newer versions of Docker Desktop use `docker compose` (without hyphen). Try:

```powershell
docker compose up --build
```

**Line ending issues with init.sql**
If the database init script fails, convert `scripts\init.sql` to Windows line endings using Notepad++ (Edit → EOL Conversion → Windows) or run:

```powershell
(Get-Content scripts\init.sql) | Set-Content scripts\init.sql
```
