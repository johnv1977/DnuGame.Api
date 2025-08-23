# 🕹 Proyecto Demo – Piedra, Papel o Tijera (Fullstack)

Este proyecto demostrará el uso de **.NET 8 Web API con SignalR**, **React (PWA con Tailwind)**, **React Native (APK Android)** y **Vue + Vuetify** como panel placeholder.  
Todos los proyectos usarán **GitHub Actions para CI/CD**.

---

## 1. Backend (.NET 8 Web API + SignalR + Vue con Vuetify)

### Requerimientos funcionales
- Implementar un **Hub con SignalR** que permita:
  - Conexión/desconexión de jugadores.
  - Registro de jugada (piedra/papel/tijera).
  - Cálculo del resultado en memoria.
  - Actualización del ranking en tiempo real.
- Implementar un **API REST mínima** con endpoints:
  - `/players` → lista de jugadores y puntajes.
  - `/health` → estado del servicio.
- Implementar almacenamiento en memoria (Diccionario o lista).
- Crear un **frontend placeholder en Vue 3 + Vuetify**:
  - Login de administrador (dummy, sin lógica).
  - Dashboard vacío con layout corporativo.

### Tareas (prompts)
- Crear proyecto .NET 8 Web API con SignalR.
- Implementar Hub con métodos `Join`, `Play`, `GetRanking`.
- Implementar servicio en memoria para gestionar jugadores y puntajes.
- Implementar endpoints REST `/players` y `/health`.
- Configurar CORS para permitir acceso desde apps React/React Native.
- Crear proyecto Vue 3 + Vuetify.
- Configurar layout base en Vuetify (header + sidebar).
- Configurar GitHub Actions para build y deploy en Azure App Service.

---

## 2. Cliente Web (React PWA con Tailwind)

### Requerimientos funcionales
- PWA accesible desde navegador y móvil.
- Pantalla de **login simple** (ingreso de nombre).
- Conexión al Hub de SignalR.
- Pantalla de juego:
  - Botones grandes de Piedra 🪨, Papel 📄, Tijera ✂️.
  - Mostrar jugada propia vs jugada rival.
  - Mostrar resultado (win/lose/empate).
  - Mostrar puntaje acumulado.
- Diseño con **estética arcade** (colores brillantes, tipografía tipo `Press Start 2P`).

### Tareas (prompts)
- Crear proyecto React con Vite + TypeScript.
- Configurar TailwindCSS con fuente arcade (`Press Start 2P`).
- Implementar login por nombre y conexión al Hub.
- Implementar botones de juego y enviar jugada al Hub.
- Implementar visualización de resultados y puntaje.
- Agregar manifest.json y service worker para PWA.
- Configurar GitHub Actions para build y deploy en GitHub Pages.

---

## 3. Cliente Móvil (React Native – Ranking en vivo)

### Requerimientos funcionales
- Conexión al Hub de SignalR.
- Mostrar **ranking global en tiempo real**:
  - Nombre de jugador.
  - Puntaje.
  - Estado de conexión (🟢 en línea / 🔴 desconectado).
- Reordenar lista automáticamente al cambiar puntajes.
- Animación visual al subir/bajar de posición.

### Tareas (prompts)
- Crear proyecto React Native (Expo o bare workflow).
- Instalar cliente de SignalR (`@microsoft/signalr`).
- Conectarse al Hub y recibir actualizaciones de ranking.
- Implementar lista ordenada con puntajes y estado.
- Agregar animación de highlight en cambios de posición.
- Exportar APK para Android.
- Configurar GitHub Actions para build automático del APK.

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

