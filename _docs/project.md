# 🕹 Proyecto Demo – Piedra, Papel o Tijera (Fullstack)

Este proyecto demostrará el uso de **.NET 8 Web API con SignalR**, **React (PWA con Tailwind)**, **React Native (APK Android)** y **Vue + Vuetify** como panel placeholder.  
Todos los proyectos usarán **GitHub Actions para CI/CD**.

---

## 1. Backend (.NET 8 Web API + SignalR) ✅ COMPLETADO

### Requerimientos funcionales implementados
- ✅ **Autenticación completa con usuario/contraseña:**
  - Registro de usuarios con ASP.NET Core Identity
  - Login con generación de JWT (60 min expiración)
  - Endpoint `/auth/me` para perfil autenticado
  - Base de datos SQLite con Entity Framework Core
- ✅ **Hub SignalR autenticado** (`/hubs/rps`) que permite:
  - `Join()` → conexión/desconexión de jugadores
  - `Play(move)` → registro de jugada (rock/paper/scissors)
  - `GetRanking()` → ranking actualizado
  - Emparejamiento automático 1v1 con cola concurrente
  - Cálculo del resultado en memoria (Win +3, Draw +1, Lose +0)
  - Eventos en tiempo real: `PlayerJoined`, `PlayerLeft`, `RoundResult`, `RankingUpdated`
- ✅ **API REST** con endpoints:
  - `/auth/register` → registro de usuario
  - `/auth/login` → autenticación y JWT
  - `/auth/me` → perfil del usuario autenticado
  - `/players` → lista de jugadores y puntajes (autenticado)
  - `/health` → estado del servicio (público)
- ✅ **Almacenamiento híbrido:**
  - SQLite para usuarios y autenticación (persistente)
  - Memoria para estado de juego y ranking (thread-safe)
- ✅ **Arquitectura modular:**
  - `Modules/` → Auth, Players, GameRps, Ranking
  - `Infrastructure/` → Auth (Identity + JWT), Persistence (EF Core)
  - `Common/` → Abstractions, DTOs, Utils
- ✅ **Seguridad y CORS:**
  - Hub protegido con `[Authorize]`
  - JWT via query para WebSockets
  - CORS configurado para React/React Native

### Estado técnico
- **Puerto:** http://localhost:5215
- **Base de datos:** SQLite (`app.db`)
- **Swagger:** Disponible en Development
- **Compilación:** ✅ Exitosa

### Tareas pendientes (ver [Fase 2](./project_fase_2.md))
- ⏳ **Observabilidad:** Middleware de errores, HealthChecks avanzados, logging estructurado
- ⏳ **Pruebas:** Tests unitarios (GameRpsService), integración (Auth endpoints), SignalR Hub
- ⏳ **Documentación:** Swagger mejorado, colección Postman, README técnico
- ⏳ **Admin endpoints:** Dashboard metrics, user management, game config, system logs
- ⏳ **CI/CD:** GitHub Actions pipeline, Docker, deploy Azure

### Tareas completadas
- ✅ Crear proyecto .NET 8 Web API con SignalR
- ✅ Implementar autenticación con ASP.NET Core Identity + JWT
- ✅ Implementar Hub con métodos `Join`, `Play`, `GetRanking`
- ✅ Implementar servicio en memoria thread-safe para jugadores y puntajes
- ✅ Implementar lógica completa del juego RPS con emparejamiento automático
- ✅ Implementar endpoints REST `/auth/*`, `/players` y `/health`
- ✅ Configurar CORS para permitir acceso desde apps React/React Native
- ✅ Configurar base de datos SQLite con migraciones EF Core

### Tareas pendientes (Fase 2)
- ⏳ **Dashboard Vue:** Panel administrativo con autenticación y métricas ([Ver Fase 2](./project_fase_2.md))
- ⏳ **Backend mejoras:** Observabilidad, pruebas, documentación, endpoints admin
- ⏳ **DevOps:** CI/CD pipelines, Docker, deploy automático

---

## 2. Dashboard Administrativo (Vue 3 + Vuetify) 🆕

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



## 3. Cliente Web (React PWA con Tailwind) 🚧 PENDIENTE

### Requerimientos funcionales
- PWA accesible desde navegador y móvil.
- Pantalla de **autenticación** (registro/login con usuario y contraseña).
- Conexión al Hub de SignalR con JWT.
- Pantalla de juego:
  - Botones grandes de Piedra 🪨, Papel 📄, Tijera ✂️.
  - Mostrar jugada propia vs jugada rival.
  - Mostrar resultado (win/lose/empate).
  - Mostrar puntaje acumulado.
- Diseño con **estética arcade** (colores brillantes, tipografía tipo `Press Start 2P`).

### Tareas pendientes
- Crear proyecto React con Vite + TypeScript.
- Configurar TailwindCSS con fuente arcade (`Press Start 2P`).
- Implementar autenticación (registro/login) con JWT.
- Implementar conexión al Hub con token Bearer.
- Implementar botones de juego y enviar jugada al Hub.
- Implementar visualización de resultados y puntaje.
- Agregar manifest.json y service worker para PWA.
- Configurar GitHub Actions para build y deploy en GitHub Pages.

---

## 4. Cliente Móvil (React Native – Ranking en vivo) 🚧 PENDIENTE

### Requerimientos funcionales
- Conexión al Hub de SignalR con autenticación JWT.
- Mostrar **ranking global en tiempo real**:
  - Nombre de jugador.
  - Puntaje.
  - Estado de conexión (🟢 en línea / 🔴 desconectado).
- Reordenar lista automáticamente al cambiar puntajes.
- Animación visual al subir/bajar de posición.

### Tareas pendientes
- Crear proyecto React Native (Expo o bare workflow).
- Implementar autenticación con el backend.
- Instalar cliente de SignalR (`@microsoft/signalr`).
- Conectarse al Hub con JWT y recibir actualizaciones de ranking.
- Implementar lista ordenada con puntajes y estado.
- Agregar animación de highlight en cambios de posición.
- Exportar APK para Android.
- Configurar GitHub Actions para build automático del APK.

---

## 🎯 Resumen del Estado Actual

### ✅ Fase 1 Completada - Backend Core (100%)
- **API funcional:** http://localhost:5215
- **Autenticación:** Usuario/contraseña con JWT
- **SignalR Hub:** `/hubs/rps` con juego completo RPS
- **Base de datos:** SQLite con Identity
- **Arquitectura:** Modular y escalable
- **CORS:** Configurado para clientes web/móvil

### 🚧 Pendiente - Clientes y Mejoras
- **Vue Dashboard:** Panel administrativo
- **React PWA:** Cliente web con estética arcade
- **React Native:** App móvil con ranking en vivo
- **Backend mejoras:** Observabilidad, pruebas, documentación
- **CI/CD:** GitHub Actions para todos los proyectos

### 📚 Documentación Técnica
- **Swagger:** Disponible en Development mode
- **Endpoints:** `/auth/*`, `/players`, `/health`, `/hubs/rps`
- **Base de datos:** SQLite con migraciones EF Core automáticas
- **Siguiente fase:** [project_fase_2.md](./project_fase_2.md)

---

## 4. Branding / Estética

### Requerimientos
- Colores y logo de la organización en header o splash screen.
- Mensajes dinámicos y ligeros:
  - “👑 Nuevo líder: {jugador}”
  - “🔥 {jugador} imparable con {victorias} seguidas”
- Consistencia visual:
  - Web → arcade colorido con Tailwind.
  - Móvil → sobrio con acentos de color.
  - Dashboard Vue → look corporativo con Vuetify.

