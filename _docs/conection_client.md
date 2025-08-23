# Conexión a los Servicios en Tiempo Real de DnuGame API (TypeScript)

Este documento proporciona la información necesaria para conectar tu proyecto cliente en TypeScript con los servicios en tiempo real de la API DnuGame, utilizando SignalR.

## Endpoint de SignalR Hub

- **URL:** `ws://localhost:5215/hubs/rps` (Desarrollo) o `https://[your-domain]/hubs/rps` (Producción)

## Autenticación

El hub de SignalR requiere autenticación mediante un token JWT (JSON Web Token). Debes enviar el token como un parámetro de consulta (`access_token`) en la URL del WebSocket.

### Proceso de Autenticación

1. **Obtener el Token JWT:** Después de que el usuario inicie sesión correctamente en tu aplicación cliente, obtén el token JWT del servicio de autenticación de la API.
2. **Construir la URL del Hub:** Agrega el token JWT como un parámetro de consulta a la URL del hub de SignalR:

   ```
   http://localhost:5215/hubs/rps?access_token=[YOUR_JWT_TOKEN]
   ```

   Reemplaza `[YOUR_JWT_TOKEN]` con el token JWT real.

## Ejemplo de Conexión en TypeScript

A continuación, se muestra un ejemplo de cómo conectarse al hub de SignalR desde una aplicación cliente TypeScript utilizando la librería `@microsoft/signalr`.

### Prerrequisitos

Asegúrate de tener instalada la librería `@microsoft/signalr` en tu proyecto:

```bash
npm install @microsoft/signalr
```

o

```bash
yarn add @microsoft/signalr
```

### Código de Ejemplo

```typescript
import * as signalR from "@microsoft/signalr";

// 1. Obtén el token JWT (reemplaza con tu lógica de obtención de token)
const token = "YOUR_JWT_TOKEN";

// 2. Construye la URL del hub con el token JWT
const hubUrl = `http://localhost:5215/hubs/rps?access_token=${token}`;

// 3. Crea la conexión de SignalR
const connection = new signalR.HubConnectionBuilder()
  .withUrl(hubUrl)
  .configureLogging(signalR.LogLevel.Information)
  .build();

// Función para iniciar la conexión
async function start() {
  try {
    await connection.start();
    console.log("SignalR Connected.");
  } catch (err) {
    console.log(err);
    setTimeout(start, 5000); // Intenta reconectar cada 5 segundos
  }
}

// Reconexión automática en caso de desconexión
connection.onclose(async () => {
  console.log("SignalR Disconnected.");
  await start();
});

// 4. Define los métodos que el hub puede invocar en el cliente
connection.on("PlayerJoined", (playerId: string) => {
  console.log(`Player ${playerId} joined`);
});

connection.on("PlayerLeft", (playerId: string) => {
    console.log(`Player ${playerId} left`);
});

connection.on("RoundResult", (result: any) => {
    console.log("Round Result:", result);
});

connection.on("RankingUpdated", (ranking: any) => {
    console.log("Ranking Updated:", ranking);
});

// 5. Inicia la conexión
start();

// 6. Llama a los métodos del hub (ejemplo)
connection.invoke("Join")
  .catch(err => console.error("Error al invocar Join:", err));

// Ejemplo de como jugar
// connection.invoke("Play", "ROCK").catch(err => console.error("Error al invocar Play:", err));
```

### Explicación del Código

- **Importación:** Importa la librería `signalR` de `@microsoft/signalr`.
- **Obtención del Token:** Reemplaza `"YOUR_JWT_TOKEN"` con la lógica real para obtener el token JWT después de que el usuario inicie sesión.
- **Construcción de la URL:** Construye la URL del hub de SignalR, agregando el token JWT como parámetro de consulta.
- **Creación de la Conexión:** Crea una instancia de `HubConnection` utilizando `HubConnectionBuilder`, configurando la URL y el nivel de logging.
- **Función `start()`:** Intenta iniciar la conexión al hub. Si falla, intenta reconectar cada 5 segundos.
- **Reconexión Automática:** Configura el evento `onclose` para que intente reconectar automáticamente cuando la conexión se cierre.
- **Definición de Métodos del Hub:** Define los métodos que el hub puede invocar en el cliente (por ejemplo, `PlayerJoined`, `PlayerLeft`, `RoundResult`, `RankingUpdated`).  Asegúrate de que los tipos de datos coincidan con los que envía el servidor.
- **Inicio de la Conexión:** Llama a la función `start()` para iniciar la conexión.
- **Llamada a Métodos del Hub:** Llama a los métodos del hub utilizando `connection.invoke()`.  Maneja los errores que puedan ocurrir durante la invocación.

## Métodos del Hub (Servidor)

Los siguientes métodos están disponibles en el hub del lado del servidor:

- `Join()`: Permite al cliente unirse al juego.
- `Play(choice: string)`: Permite al cliente jugar una ronda, enviando su elección (por ejemplo, "ROCK", "PAPER", "SCISSORS").
- `GetRanking()`: Permite al cliente obtener el ranking actual de jugadores.

## Métodos Invocados por el Servidor (Cliente)

El servidor puede invocar los siguientes métodos en el cliente:

- `PlayerJoined(playerId: string)`: Notifica al cliente que un nuevo jugador se ha unido al juego.
- `PlayerLeft(playerId: string)`: Notifica al cliente que un jugador ha abandonado el juego.
- `RoundResult(result: any)`: Envía el resultado de una ronda al cliente.  El formato exacto de `result` dependerá de la implementación del servidor.
- `RankingUpdated(ranking: any)`: Envía el ranking actualizado de jugadores al cliente. El formato exacto de `ranking` dependerá de la implementación del servidor.

## Notas Adicionales

- Asegúrate de manejar los errores de conexión y las desconexiones inesperadas.
- Considera implementar un mecanismo de reintento más sofisticado para la reconexión.
- Adapta el código de ejemplo a la estructura y necesidades específicas de tu proyecto.
- Verifica la documentación de `@microsoft/signalr` para obtener información más detallada sobre la configuración y el uso de la librería.