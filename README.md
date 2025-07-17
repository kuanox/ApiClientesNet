# ApiClientesJava

# User API con Autenticación Segura

## 🚀 Tecnologías
- **Backend**: .NET 8.0, ASP.NET Core, Entity Framework Core 8.0
- **Base de datos**: SQL Server 2022 (LocalDB para desarrollo)
- **Autenticación**: JWT + Identity Framework
- **Documentación**: Swagger UI

## 🔧 Instalación rápida
```bash
# 1. Clonar repositorio
git clone https://github.com/tu-usuario/user-api.git
cd user-api

# 2. Configurar BD (LocalDB)
sqllocaldb create MSSQLLocalDB
sqllocaldb start MSSQLLocalDB
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "CREATE DATABASE UserApi_Dev"

# 3. Restaurar paquetes y migraciones
dotnet restore
dotnet ef database update
```bash

## Endpoints principales

POST /api/auth/register
Content-Type: application/json
{
  "email": "usuario@ejemplo.com",
  "password": "P@ssw0rd123"
}

POST /api/auth/login
Content-Type: application/json
{
  "email": "usuario@ejemplo.com",
  "password": "P@ssw0rd123"
}

GET /api/users
Authorization: Bearer {token_jwt}

## Uso local

1. Iniciar API:

dotnet run

2. Acceder a Swagger:

https://localhost:7206/swagger


## Configuración

Editar appsettings.Development.json:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=UserApi_Dev;Trusted_Connection=True;"
  },
  "JwtSettings": {
    "Secret": "ClaveSecretaParaDesarrollo123",
    "ExpiryDays": 7
  }
}
  
## Características clave

- Hash seguro de contraseñas con PBKDF2

- Tokens JWT con expiración configurable

- Validación automática de modelos

- Sistema de roles integrado
	

