# Maia — Fashion E-Commerce Platform

Full-stack e-commerce aplikacion i ndërtuar me arkitekturë mikroshërbimesh.  
**Stack:** .NET 10 · React + Vite · SQL Server · MongoDB Atlas · Redis · SignalR

---

## Mikroshërbimet

| Shërbimi | Port | Swagger | Përshkrimi |
|----------|------|---------|------------|
| **Maia** (main API) | `5293` | http://localhost:5293/swagger | Gateway kryesor, MongoDB, Redis |
| **Auth** | `5000` | http://localhost:5000/swagger | JWT, Refresh Tokens, Roles, Permissions |
| **WomensSection** | `5182` | http://localhost:5182/swagger | Produkte, Cart, Orders, Wishlist |
| **MenSection** | `5018` | http://localhost:5018/swagger | Produkte burra, Kategori |
| **KidsSection** | `5062` | http://localhost:5062/swagger | Produkte fëmijë, Kategori, Lloje |
| **NotificationService** | `5151` | http://localhost:5151/swagger | Email, SignalR njoftimet |
| **FileUploadService** | `5270` | http://localhost:5270/swagger | Azure Blob Storage |
| **OrderService** | `5200` | http://localhost:5200/swagger | Cart, Orders, Stripe Payment |
| **SettingsService** | `5187` | http://localhost:5187/swagger | Konfigurime globale/user |

---

## Kërkesat paraprake

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) ose SQL Server
- [MongoDB Atlas](https://www.mongodb.com/cloud/atlas) account (falas)
- [Redis](https://redis.io) (lokal ose cloud)

---

## Instalimi dhe konfigurimi

### 1. Klono repository-in

```bash
git clone https://github.com/OrgesaBerisha/Maia.git
cd Maia
```

### 2. Konfiguro sekretet — krijo `.env` te `Maia/Maia/`

```env
MONGODB_CONNECTION_STRING="mongodb+srv://user:password@cluster.mongodb.net/?retryWrites=true&w=majority"
MONGODB_DATABASE_NAME="MaiaDB"
REDIS_CONNECTION_STRING="localhost:6379"
```

### 3. Krijo databazat SQL

Për secilën mikroshërbim që ka databazë SQL, ekzekuto brenda folderit të projektit:

```bash
# Maia
cd Maia/Maia && dotnet ef database update

# Auth
cd Auth/Auth && dotnet ef database update

# WomensSection
cd WomensSection/WomensSection && dotnet ef database update

# MenSection
cd MenSection && dotnet ef database update

# KidsSection
cd KidsSection && dotnet ef database update

# NotificationService
cd NotificationService && dotnet ef database update

# FileUploadService
cd FileUploadService && dotnet ef database update

# SettingsService
cd SettingsService && dotnet ef database update
```

### 4. Instalo varësitë e frontendeve

```bash
# WomensSection frontend
cd WomensSection/WomensSection/frontend && npm install

# Maia frontend (nëse ka)
cd Maia/frontend && npm install
```

---

## Ekzekutimi

Hap terminal të veçantë për secilin shërbim:

```bash
# Terminal 1
cd Auth/Auth && dotnet run --launch-profile http

# Terminal 2
cd WomensSection/WomensSection && dotnet run --launch-profile http

# Terminal 3
cd MenSection && dotnet run --launch-profile http

# Terminal 4
cd KidsSection && dotnet run --launch-profile http

# Terminal 5
cd OrderService && dotnet run --launch-profile http

# Terminal 6
cd NotificationService && dotnet run --launch-profile http
```

### Frontend

```bash
cd Maia/frontend && npm run dev
```

Frontend hapet te: `http://localhost:5173` (ose portin që Vite zgjedh)

---

## Llogaritë e paracaktuara

| Roli | Email | Fjalëkalimi |
|------|-------|-------------|
| Admin | admin@admin.com | Admin123! |
| Sales Manager | sales@manager.com | Sales123! |
| Women Manager | women@manager.com | Women123! |
| Men Manager | men@manager.com | Men123! |
| Kids Manager | kids@manager.com | Kids123! |

---

## Pagesa me Stripe (Test Mode)

Konfiguro çelësat Stripe te `OrderService/appsettings.json` (nuk commit-ohet):

```json
"Stripe": {
  "SecretKey": "sk_test_YOUR_KEY",
  "PublishableKey": "pk_test_YOUR_KEY"
}
```

**Kartë testimi:**

| Fusha | Vlera |
|-------|-------|
| Numri i kartës | 4242 4242 4242 4242 |
| Expiry | 12/28 |
| CVV | 123 |
| Postal code | 10001 |

---

## Struktura e projektit

```
Maia/
├── Auth/                   # Autentifikim, autorizim, refresh tokens
├── Maia/                   # API kryesor, MongoDB (audit_logs), Redis cache
├── WomensSection/          # Produkte grash, Cart, Orders, Wishlist
├── MenSection/             # Produkte burra, kategori
├── KidsSection/            # Produkte fëmijë, kategori, lloje
├── NotificationService/    # Njoftime email + SignalR real-time
├── FileUploadService/      # Ngarkimi i imazheve (Azure Blob)
└── SettingsService/        # Konfigurime globale dhe per-user
```

---

## Databaza

| Lloji | Teknologjia | Përdorimi |
|-------|------------|-----------|
| SQL Server | Entity Framework Core | Users, Roles, Products, Orders, Cart |
| MongoDB Atlas | MongoDB.Driver | audit_logs (LOGIN, REGISTER, LOGIN_FAILED) |
| Redis | StackExchange.Redis | Cache produktesh (TTL 10 min) |

---

## Eksporti i të dhënave

Çdo mikroshërbim mbështet eksport në **CSV, Excel dhe JSON**:

| Shërbimi | Endpoint | Të dhënat |
|----------|----------|-----------|
| MenSection | `GET /api/export/men-products/{csv\|excel\|json}` | Produkte burra |
| KidsSection | `GET /api/export/kids-products/{csv\|excel\|json}` | Produkte fëmijë |
| WomensSection | `GET /api/export/women-products/{csv\|excel\|json}` | Produkte grash |
| WomensSection | `GET /api/export/orders/{csv\|excel\|json}` | Porositë |
| Auth | `GET /api/export/users/{csv\|excel\|json}` | Përdoruesit *(Admin only)* |

---

## Real-Time (SignalR)

NotificationService ofron njoftime në kohë reale:

- **Hub URL:** `http://localhost:5151/hubs/notifications`
- **Event:** `ReceiveNotification`
- Çdo user lidhet me grupin `user-{userId}`

---

## Siguria

- JWT Access Token (30 min) + Refresh Token (7 ditë) — HttpOnly cookies
- Password hashing me BCrypt
- Role-based authorization: `Admin`, `SalesManager`, `WomenManager`, `MenManager`, `Customer`
- Secrets në `.env` — kurrë në kod
- CORS i konfiguruar vetëm për `localhost:5173`

---

## Ekipi

| Anëtar | GitHub |
|--------|--------|
| Orgesa Berisha | [@OrgesaBerisha](https://github.com/OrgesaBerisha) |
| Anesa Mecinaj | [@AnesaaM](https://github.com/AnesaaM) |
| Festa Thaqi | [@festathaqi](https://github.com/festathaqi) |
