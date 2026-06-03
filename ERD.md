# ERD — Maia Fashion E-Commerce Platform

## Auth Service Database

```mermaid
erDiagram
    Users {
        int UserID PK
        string FirstName
        string LastName
        string Email
        bytes PasswordHash
        bytes PasswordSalt
        int RoleID FK
        string RefreshTokenHash
        datetime RefreshTokenExpiry
        bool IsActive
        datetime CreatedAt
        datetime DisabledAt
    }

    Roles {
        int RoleID PK
        string RoleType
    }

    UserRoles {
        int Id PK
        int UserID FK
        int RoleID FK
    }

    Permissions {
        int Id PK
        string Name
        string Description
    }

    RolePermissions {
        int Id PK
        int RoleID FK
        int PermissionId FK
    }

    RefreshTokens {
        int Id PK
        int UserID FK
        string TokenHash
        datetime ExpiresAt
        datetime RevokedAt
        datetime CreatedAt
        bool IsRevoked
    }

    AuditLogs {
        int Id PK
        int UserID FK
        string Action
        string Entity
        string EntityId
        string OldValue
        string NewValue
        string IpAddress
        datetime CreatedAt
    }

    Users ||--o{ UserRoles : "ka role"
    Roles ||--o{ UserRoles : "i caktuar"
    Roles ||--o{ RolePermissions : "ka leje"
    Permissions ||--o{ RolePermissions : "i caktuar"
    Users ||--o{ RefreshTokens : "ka tokens"
    Users ||--o{ AuditLogs : "kryen veprime"
```

---

## WomensSection Database

```mermaid
erDiagram
    CardsWomen {
        int Id PK
        string Title
        string ImageUrl
        decimal Price
        int WomanCategoryId FK
        string Description
        datetime CreatedAt
        datetime UpdatedAt
    }

    WomanCategories {
        int Id PK
        string Name
    }

    Orders {
        int Id PK
        int UserId
        decimal TotalPrice
        datetime CreatedAt
    }

    OrderItems {
        int Id PK
        int OrderId FK
        int ProductId
        int Quantity
        decimal Price
    }

    Carts {
        int Id PK
        int UserId
        datetime CreatedAt
    }

    CartItems {
        int Id PK
        int CartId FK
        int ProductId
        int Quantity
    }

    Wishlists {
        int Id PK
        int UserId
        datetime CreatedAt
    }

    WishlistItems {
        int Id PK
        int WishlistId FK
        int ProductId
    }

    WomanCategories ||--o{ CardsWomen : "ka produkte"
    Orders ||--|{ OrderItems : "permban"
    Carts ||--|{ CartItems : "permban"
    Wishlists ||--|{ WishlistItems : "permban"
```

---

## MenSection Database

```mermaid
erDiagram
    MenCards {
        int Id PK
        string Title
        string ImageUrl
        decimal Price
        int MenCategoryId FK
        string Description
    }

    MenCategories {
        int Id PK
        string Name
    }

    MenCategories ||--o{ MenCards : "ka produkte"
```

---

## KidsSection Database

```mermaid
erDiagram
    KidsCards {
        int Id PK
        string Title
        string ImageUrl
        decimal Price
        int KidsCategoryId FK
        int KidsProductTypeId FK
        string Description
    }

    KidsCategories {
        int Id PK
        string Name
    }

    KidsProductTypes {
        int Id PK
        string Name
    }

    KidsCategories ||--o{ KidsCards : "ka produkte"
    KidsProductTypes ||--o{ KidsCards : "ka produkte"
```

---

## NotificationService Database

```mermaid
erDiagram
    Notifications {
        int Id PK
        string UserId
        string Title
        string Message
        string Type
        bool IsRead
        datetime CreatedAt
    }
```

---

## FileUploadService Database

```mermaid
erDiagram
    FileRecords {
        int Id PK
        string FileName
        string ContentType
        string Url
        string UploadedBy
        string Category
        long SizeInBytes
        datetime UploadedAt
    }
```

---

## SettingsService Database

```mermaid
erDiagram
    Settings {
        int Id PK
        string Key
        string Value
        string Scope
        string OwnerId
        datetime UpdatedAt
    }
```

---

## MongoDB Collections (Maia Service)

```mermaid
erDiagram
    audit_logs {
        ObjectId Id PK
        int UserId
        string Action
        string Entity
        string EntityId
        string IpAddress
        string UserAgent
        datetime Timestamp
        string CreatedBy
    }

    products {
        ObjectId Id PK
        string Name
        string Description
        decimal Price
        string Category
        int Stock
        object Attributes
        array Images
        array Tags
        bool IsActive
        string CreatedBy
        string UpdatedBy
        datetime CreatedAt
        datetime UpdatedAt
    }
```

---

## Tabela e plotë — 26 tabela

| # | Tabela | Shërbimi | Lloji DB |
|---|--------|----------|----------|
| 1 | Users | Auth | SQL |
| 2 | Roles | Auth | SQL |
| 3 | UserRoles | Auth | SQL |
| 4 | Permissions | Auth | SQL |
| 5 | RolePermissions | Auth | SQL |
| 6 | RefreshTokens | Auth | SQL |
| 7 | AuditLogs | Auth | SQL |
| 8 | CardsWomen | WomensSection | SQL |
| 9 | WomanCategories | WomensSection | SQL |
| 10 | Orders | WomensSection | SQL |
| 11 | OrderItems | WomensSection | SQL |
| 12 | Carts | WomensSection | SQL |
| 13 | CartItems | WomensSection | SQL |
| 14 | Wishlists | WomensSection | SQL |
| 15 | WishlistItems | WomensSection | SQL |
| 16 | MenCards | MenSection | SQL |
| 17 | MenCategories | MenSection | SQL |
| 18 | KidsCards | KidsSection | SQL |
| 19 | KidsCategories | KidsSection | SQL |
| 20 | KidsProductTypes | KidsSection | SQL |
| 21 | Notifications | NotificationService | SQL |
| 22 | FileRecords | FileUploadService | SQL |
| 23 | Settings | SettingsService | SQL |
| 24 | audit_logs | Maia | MongoDB |
| 25 | products | Maia | MongoDB |

**Totali: 25 tabela/koleksione** ✅ *(minimum i kërkuar: 24)*
