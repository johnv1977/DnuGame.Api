# 🕹 Proyecto Demo – Fase 2: Dashboard Vue + Mejoras Backend

Esta fase completa las tareas pendientes del backend y agrega el **Dashboard Administrativo en Vue 3 + Vuetify** que fue omitido en la Fase 1.

---

## 1. Dashboard Administrativo (Vue 3 + Vuetify) 🆕

### Descripción
Panel de administración corporativo para monitorear el juego, gestionar usuarios y visualizar estadísticas en tiempo real.

### Requerimientos funcionales
- **Autenticación administrativa:**
  - Login con credenciales de administrador contra el backend .NET
  - JWT token para autenticación con el API
  - Logout y manejo de sesión
- **Dashboard principal:**
  - Métricas en tiempo real: jugadores conectados, partidas jugadas, ranking top 10
  - Gráficos de actividad por hora/día
  - Estadísticas de uso del juego
- **Gestión de usuarios:**
  - Lista de usuarios registrados con filtros y paginación
  - Ver detalles de usuario (perfil, estadísticas, historial)
  - Capacidad de deshabilitar/habilitar usuarios
- **Monitor de partidas:**
  - Ver partidas en curso en tiempo real
  - Historial de partidas recientes
  - Conexión a SignalR para actualizaciones live
- **Configuración del sistema:**
  - Ajustar puntajes del juego (Win/Draw/Lose points)
  - Configurar timeouts y reglas
  - Logs del sistema

### Stack tecnológico
- **Vue 3** con Composition API
- **Vuetify 3** para componentes UI corporativos
- **Vue Router** para navegación
- **Pinia** para estado global
- **Axios** para HTTP requests
- **@microsoft/signalr** para conexión en tiempo real
- **Chart.js/Vue-ChartJS** para gráficos
- **Vite** como bundler

### Tareas de implementación
- [ ] Crear proyecto Vue 3 + Vuetify con Vite
- [ ] Configurar router con guards de autenticación
- [ ] Implementar store Pinia para auth y datos globales
- [ ] Crear layout corporativo con sidebar y header
- [ ] Implementar autenticación contra API .NET
- [ ] Crear dashboard con métricas y gráficos
- [ ] Implementar gestión de usuarios (CRUD básico)
- [ ] Conectar a SignalR para updates en tiempo real
- [ ] Crear monitor de partidas en vivo
- [ ] Implementar panel de configuración
- [ ] Agregar logging y manejo de errores
- [ ] Configurar build para producción

### Endpoints del backend requeridos
```
GET /admin/dashboard/metrics     → métricas generales
GET /admin/users                 → lista usuarios con paginación
GET /admin/users/{id}           → detalles de usuario
PUT /admin/users/{id}/status    → habilitar/deshabilitar
GET /admin/matches              → historial de partidas
GET /admin/config               → configuración actual
PUT /admin/config               → actualizar configuración
GET /admin/logs                 → logs del sistema
```

---

## 2. Mejoras Backend (.NET 8) - Tareas Pendientes

### 2.1 Observabilidad y Manejo de Errores
- [ ] **Middleware global de errores:**
  - GlobalExceptionHandlerMiddleware
  - Respuestas ProblemDetails RFC7807 uniformes
  - Logging estructurado de errores
- [ ] **HealthChecks avanzados:**
  - Liveness: `/health/live` (básico)
  - Readiness: `/health/ready` (DB + memoria)
  - Detailed: `/health/detailed` (admin only)
- [ ] **Logging estructurado:**
  - Serilog con sinks (Console, File, Seq opcional)
  - Correlation IDs para trazabilidad
  - Log levels por ambiente
- [ ] **Activity y Tracing:**
  - OpenTelemetry básico
  - Instrumentación de SignalR y EF Core

### 2.2 Pruebas Automatizadas
- [ ] **Proyecto de pruebas xUnit:**
  - `DnuGame.Api.Tests` con estructura modular
- [ ] **Tests unitarios core:**
  - GameRpsService: reglas del juego (rock beats scissors, etc.)
  - TokenService: generación y validación JWT
  - RankingService: ordenamiento y cálculos
- [ ] **Tests de integración:**
  - Auth endpoints con WebApplicationFactory
  - Players endpoints protegidos
  - Base de datos en memoria para tests
- [ ] **Tests de SignalR Hub:**
  - Hub connection y authentication
  - Game flow completo (Join → Play → Result)
  - TestServer con SignalR client

### 2.3 Documentación y Herramientas
- [ ] **Swagger mejorado:**
  - Grouping por tags/módulos
  - Ejemplos de requests/responses
  - Documentación de errores comunes
- [ ] **Colección Postman completa:**
  - Pre-request script para JWT automático
  - Carpetas por módulo (Auth, Players, Game, Admin)
  - Variables de ambiente (dev, staging, prod)
  - Tests automáticos en requests
- [ ] **README técnico:**
  - Guía de instalación y desarrollo
  - Arquitectura y decisiones técnicas
  - Endpoints y contratos
  - Diagramas de flujo del juego

### 2.4 Endpoints Administrativos
- [ ] **Módulo Admin:** (`/admin/*` - solo administradores)
  - Dashboard metrics endpoint
  - User management CRUD
  - Game configuration management
  - System logs endpoint
- [ ] **Autorización por roles:**
  - Role "Admin" en Identity
  - Policy-based authorization
  - Middleware de validación de roles

---

## 3. CI/CD y DevOps

### 3.1 GitHub Actions - Backend
- [ ] **Pipeline .NET:**
  - Triggers: push a main, PRs
  - Jobs: restore → build → test → analyze
  - Code coverage reports
  - Security scanning (CodeQL)
- [ ] **Docker containerización:**
  - Dockerfile multi-stage
  - Docker Compose para desarrollo
  - Registry push (opcional)
- [ ] **Deploy automático:**
  - Azure App Service (dev/staging)
  - Variable secrets management
  - Health check post-deploy

### 3.2 GitHub Actions - Vue Dashboard
- [ ] **Pipeline Vue:**
  - Build con Vite
  - Linting (ESLint + Prettier)
  - Unit tests (Vitest)
  - Type checking (TypeScript)
- [ ] **Deploy:**
  - GitHub Pages o Azure Static Web Apps
  - Environment-specific configs

---

## 4. Configuración y Ambiente

### 4.1 Variables de Entorno
```bash
# Desarrollo
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__Default=Data Source=app.db
JWT__Key=dev-secret-key
JWT__Issuer=DnuGame
JWT__Audience=DnuGameClients

# Producción
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__Default={{AZURE_SQL_CONNECTION}}
JWT__Key={{SECURE_JWT_KEY}}
LOGGING__LEVEL=Warning
```

### 4.2 Configuración Avanzada
- [ ] **Multiple environments:**
  - appsettings.Development.json
  - appsettings.Staging.json
  - appsettings.Production.json
- [ ] **Feature flags:** (opcional)
  - Azure App Configuration
  - Toggle features sin deployment

---

## 5. Métricas de Completitud

### Backend Fase 2
- [ ] Observabilidad (4 tareas)
- [ ] Pruebas (4 tipos de tests)
- [ ] Documentación (3 entregables)
- [ ] Admin endpoints (4 módulos)

### Vue Dashboard
- [ ] Setup y configuración (4 tareas)
- [ ] Autenticación (3 features)
- [ ] Dashboards (4 pantallas)
- [ ] Deploy y CI (2 pipelines)

### Total estimado: **~25-30 tareas** para completar Fase 2

---

## 6. Prioridades de Implementación

### Sprint 1 (Backend Core)
1. Middleware de errores global
2. HealthChecks avanzados
3. Tests unitarios básicos
4. Endpoints admin básicos

### Sprint 2 (Vue Dashboard)
1. Setup Vue + Vuetify proyecto
2. Autenticación contra backend
3. Layout y navegación
4. Dashboard principal con métricas

### Sprint 3 (Integración)
1. SignalR en Vue para updates live
2. Gestión de usuarios completa
3. Tests de integración
4. Documentación Swagger/Postman

### Sprint 4 (DevOps)
1. CI/CD pipelines
2. Docker y deploy
3. Monitoring y logs
4. Security y performance tuning
