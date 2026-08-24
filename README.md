# VitaStays — Proyecto BDA

Aplicación web para la gestión y análisis de un complejo de cabañas, desarrollada con **.NET 9**, **ASP.NET Core Web API**, **Blazor WebAssembly**, **Entity Framework Core** y **SQL Server**.

La solución permite administrar reservas y cancelaciones, consultar clientes y cabañas, definir objetivos de ocupación y analizar el nivel de ocupación de cada cabaña de forma anual, mensual y diaria.

El proyecto utiliza una arquitectura dividida en capas y una API REST protegida mediante autenticación **JWT**.

## Funcionalidades principales

- Registro e inicio de sesión de usuarios.
- Autenticación mediante **JSON Web Tokens (JWT)**.
- Roles de usuario `Admin` y `Employee`.
- Contraseñas almacenadas mediante **BCrypt**.
- Consulta de cabañas.
- Consulta de clientes.
- Gestión de reservas.
- Actualización del estado de reservas.
- Registro y consulta de cancelaciones.
- Motivos de cancelación.
- Definición de objetivos de ocupación:
  - generales;
  - anuales por cabaña;
  - mensuales por cabaña.
- Cálculo de ocupación:
  - anual;
  - mensual;
  - diaria.
- Comparación entre ocupación real y objetivos definidos.
- Indicador tipo **semáforo** para visualizar el cumplimiento de las metas.
- Interfaz web responsive desarrollada con **MudBlazor**.
- Documentación y prueba de endpoints mediante **Swagger**.

## Arquitectura

La solución se encuentra dividida en seis proyectos:

```text
Proyecto_BDA/
│
├── Domain/          # Entidades, DTOs, enums e interfaces de repositorios
├── Application/     # Servicios de aplicación y generación de tokens JWT
├── Business/        # Lógica de negocio, servicios y mapeos
├── Infraestructure/ # Persistencia, DbContext, repositorios y migraciones
├── Api/             # API REST ASP.NET Core
├── Web/             # Frontend Blazor WebAssembly
└── Proyecto_BDA_1.sln
```

### Domain

Representa el núcleo del dominio y contiene:

- entidades;
- DTOs de entrada y salida;
- enumeraciones;
- contratos de repositorios.

Entidades principales:

- `User`
- `Cliente`
- `Cabaña`
- `Reserva`
- `Cancelacion`
- `Objetivo`

### Application

Contiene servicios vinculados a la aplicación, principalmente la generación de tokens JWT a través de `TokenService`.

### Business

Implementa las reglas de negocio del sistema.

Incluye servicios para:

- autenticación;
- cabañas;
- clientes;
- reservas;
- cancelaciones;
- objetivos;
- ocupación.

También utiliza **AutoMapper** para transformar entidades y DTOs.

### Infraestructure

Contiene la implementación de persistencia:

- `AppDbContext`;
- repositorios;
- Entity Framework Core;
- migraciones;
- conexión con SQL Server.

### Api

Expone los servicios mediante una **ASP.NET Core Web API**.

Controladores disponibles:

```text
/api/auth
/api/cabañas
/api/clientes
/api/reservas
/api/cancelaciones
/api/objetivos
/api/ocupacion
```

La API incluye Swagger para visualizar y probar los endpoints durante el desarrollo.

### Web

Frontend desarrollado con **Blazor WebAssembly** y **MudBlazor**.

Incluye pantallas para:

- login y registro;
- cabañas;
- clientes;
- reservas;
- cancelaciones;
- objetivos;
- métricas de ocupación.

El proyecto también contiene `manifest.webmanifest` y service workers, permitiendo una estructura compatible con una **Progressive Web App (PWA)**.

## Tecnologías utilizadas

| Tecnología | Uso |
|---|---|
| C# | Lenguaje principal |
| .NET 9 | Plataforma de desarrollo |
| ASP.NET Core Web API | Backend REST |
| Blazor WebAssembly | Frontend |
| MudBlazor | Componentes de interfaz |
| Entity Framework Core | ORM |
| SQL Server | Base de datos |
| JWT Bearer | Autenticación |
| BCrypt.Net | Hash seguro de contraseñas |
| AutoMapper | Mapeo entre entidades y DTOs |
| Swagger / OpenAPI | Documentación y pruebas de la API |

## Modelo de autenticación

El proceso de autenticación funciona de la siguiente manera:

```text
Usuario
   │
   ▼
Login
   │
   ▼
API /api/auth/login
   │
   ▼
Validación BCrypt
   │
   ▼
Generación JWT
   │
   ▼
Blazor WebAssembly
   │
   ▼
Peticiones autenticadas a la API
```

El token incluye información del usuario y su rol, permitiendo aplicar autorización en la API y en la interfaz.

## Análisis de ocupación

Una de las funcionalidades principales del proyecto es el cálculo de ocupación de las cabañas.

El sistema puede obtener:

### Ocupación anual

Calcula el porcentaje de ocupación de una cabaña durante cada año disponible.

### Ocupación mensual

Calcula las noches reservadas y disponibles de cada mes y compara el resultado con la meta configurada.

### Ocupación diaria

Permite determinar para cada día de un mes si la cabaña se encuentra:

```text
Ocupada
```

o

```text
Desocupada
```

### Semáforo de objetivos

El proyecto utiliza un indicador para comparar la ocupación obtenida con la meta:

- **Verde:** la ocupación alcanza o supera el objetivo.
- **Naranja:** la ocupación se encuentra cerca del objetivo.
- **Rojo:** la ocupación se encuentra por debajo del objetivo.

## Requisitos

Para ejecutar el proyecto se necesita:

- **.NET 9 SDK**.
- **Visual Studio 2022** o un editor compatible con .NET.
- **SQL Server** o SQL Server Express.
- SQL Server Management Studio, opcional.
- Navegador web moderno.

## Configuración de la API

La API utiliza dos valores de configuración importantes:

```text
ConnectionStrings:DefaultConnection
Jwt:Key
```

Por seguridad, las credenciales reales y las claves JWT **no deberían almacenarse en un repositorio público**.

Una opción recomendada para desarrollo es utilizar **User Secrets**.

Desde la carpeta raíz del proyecto:

```bash
cd Api

dotnet user-secrets init

dotnet user-secrets set "ConnectionStrings:DefaultConnection" "TU_CONNECTION_STRING"

dotnet user-secrets set "Jwt:Key" "UNA_CLAVE_SECRETA_LARGA_Y_SEGURA"
```

Luego volver a la raíz:

```bash
cd ..
```

También pueden utilizarse variables de entorno compatibles con la configuración de ASP.NET Core.

## Base de datos

Las migraciones se encuentran en:

```text
Infraestructure/Migrations
```

Para aplicar las migraciones desde la raíz de la solución:

```bash
dotnet ef database update --project Infraestructure --startup-project Api
```

Si no se dispone de la herramienta `dotnet-ef`, puede instalarse con:

```bash
dotnet tool install --global dotnet-ef
```

## Ejecución

La aplicación necesita ejecutar **la API y el frontend Blazor al mismo tiempo**.

### 1. Restaurar dependencias

Desde la raíz:

```bash
dotnet restore Proyecto_BDA_1.sln
```

### 2. Ejecutar la API

En una terminal:

```bash
dotnet run --project Api/Api.csproj --launch-profile https
```

La configuración actual utiliza:

```text
https://localhost:7123
```

Swagger estará disponible durante el entorno de desarrollo desde la interfaz de la API.

### 3. Ejecutar el frontend

En otra terminal:

```bash
dotnet run --project Web/Web.csproj --launch-profile https
```

El perfil HTTPS del frontend utiliza:

```text
https://localhost:7162
```

El frontend está configurado actualmente para consumir la API en:

```text
https://localhost:7123
```

## Compilación

Para compilar toda la solución:

```bash
dotnet build Proyecto_BDA_1.sln
```

## Seguridad

El proyecto implementa:

- hash de contraseñas con **BCrypt**;
- autenticación mediante JWT;
- autorización mediante roles;
- endpoints protegidos con `[Authorize]`;
- separación entre entidades internas y DTOs;
- validaciones de negocio en la capa de servicios.

### Importante antes de publicar el repositorio

Revisar `Api/appsettings.json` y asegurarse de que no contenga:

- contraseñas de SQL Server;
- credenciales privadas;
- claves JWT reales;
- tokens o API keys.

En caso de que una clave secreta haya sido publicada previamente, se recomienda **rotarla y generar una nueva**, ya que eliminarla de un commit posterior no la elimina automáticamente del historial de Git.

## Estado del proyecto

Proyecto académico orientado a aplicar conceptos de:

- bases de datos avanzadas;
- APIs REST;
- arquitectura por capas;
- separación de responsabilidades;
- Entity Framework Core;
- autenticación y autorización;
- desarrollo frontend con Blazor;
- análisis de datos de ocupación.

## Repositorio

GitHub:

```text
https://github.com/Ignaco1/Proyecto_BDA
```
