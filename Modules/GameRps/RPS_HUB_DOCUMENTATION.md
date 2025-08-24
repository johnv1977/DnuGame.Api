# Rock-Paper-Scissors SignalR Hub

## Overview

The `RpsHub` provides real-time game functionality for the Rock-Paper-Scissors game using SignalR.

## Connection Details

- **Hub URL**: `/hubs/rps`
- **Authentication**: Required (Bearer token)
- **Protocol**: WebSocket/SignalR

## Client Methods (Server → Client)

These are methods that can be called from the server to connected clients:

### PlayerJoined
```typescript
connection.on("PlayerJoined", (player: Player) => {
    // Handle player joining
});
```

### PlayerLeft
```typescript
connection.on("PlayerLeft", (playerId: string) => {
    // Handle player leaving
});
```

### MoveRegistered
```typescript
connection.on("MoveRegistered", (data: { move: string, timestamp: string }) => {
    // Handle move confirmation
});
```

### RankingUpdated
```typescript
connection.on("RankingUpdated", (ranking: RankingEntry[]) => {
    // Handle ranking updates
});
```

### GameResult
```typescript
connection.on("GameResult", (result: GameResult) => {
    // Handle game result
});
```

## Server Methods (Client → Server)

These are methods that can be called from the client to the server:

### Join()
```typescript
await connection.invoke("Join");
```
- **Description**: Join the game hub and register as an active player
- **Authentication**: Required
- **Side Effects**: 
  - Registers player in the system
  - Updates player online status
  - Sends `PlayerJoined` event to all clients
  - Sends current ranking to joining client

### Play(move: string)
```typescript
await connection.invoke("Play", "rock"); // or "paper" or "scissors"
```
- **Parameters**:
  - `move`: Valid moves are "rock", "paper", "scissors" (case insensitive)
- **Description**: Submit a move for the current game round
- **Authentication**: Required
- **Side Effects**: 
  - Queues player for matchmaking
  - Sends `MoveRegistered` confirmation to caller

### GetRanking()
```typescript
await connection.invoke("GetRanking");
```
- **Description**: Request current player ranking
- **Authentication**: Required
- **Side Effects**: 
  - Sends `RankingUpdated` event to caller

## Connection Lifecycle

### OnConnectedAsync
Automatically called when a client connects to the hub.

### OnDisconnectedAsync
Automatically called when a client disconnects:
- Updates player offline status
- Sends `PlayerLeft` event to remaining clients

## Error Handling

All hub methods can throw `HubException` with descriptive error messages:

- **"User not authenticated"**: When the user is not properly authenticated
- **"User not found"**: When the authenticated user cannot be found in the system
- **"Invalid move"**: When an invalid move is submitted to Play()
- **"Failed to register move"**: When the game service cannot process the move

## Example Client Connection (TypeScript)

```typescript
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

// Create connection
const connection = new HubConnectionBuilder()
    .withUrl("/hubs/rps", {
        accessTokenFactory: () => localStorage.getItem("authToken")
    })
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Information)
    .build();

// Set up event handlers
connection.on("PlayerJoined", (player) => {
    console.log("Player joined:", player);
});

connection.on("RankingUpdated", (ranking) => {
    console.log("Ranking updated:", ranking);
});

connection.on("MoveRegistered", (data) => {
    console.log("Move registered:", data);
});

// Start connection and join
await connection.start();
await connection.invoke("Join");

// Play a move
await connection.invoke("Play", "rock");
```

## API Endpoint for Hub Information

You can also get this information programmatically via the REST API:

```
GET /game/rps/hub/info
```

This endpoint returns detailed information about the hub's methods, events, and parameters in JSON format.
