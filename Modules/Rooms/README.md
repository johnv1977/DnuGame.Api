# Módulo de Salas (Rooms)

## Descripción
El módulo de Salas permite a los usuarios gestionar salas de juego donde pueden reunirse para jugar. Incluye operaciones CRUD completas para la administración de salas.

## Funcionalidades Implementadas

### ✅ Entidades
- **Room**: Entidad principal con propiedades Id, Slug, Name, Color, Icon, UserLimit, IsOpen, CreatedAt, UpdatedAt

### ✅ DTOs con Validaciones FluentValidation
- **CreateRoomDto**: Para crear nuevas salas con validaciones automáticas
- **UpdateRoomDto**: Para actualizar salas existentes
- **RoomResponseDto**: Respuesta completa de sala con información de usuario actual
- **RoomListDto**: Respuesta simplificada para listados
- **PaginatedResult<T>**: Para respuestas paginadas

### ✅ Servicios
- **IRoomService** / **RoomService**: Lógica de negocio
- **IRoomRepository** / **RoomRepository**: Acceso a datos

### ✅ API Endpoints

#### Públicos (Requiere autenticación)
- `GET /api/rooms` - Obtener lista paginada de salas
- `GET /api/rooms/{slug}` - Obtener sala por slug

#### Administrativos (Requiere rol Admin)
- `POST /api/rooms` - Crear nueva sala
- `PUT /api/rooms/{slug}` - Actualizar sala existente
- `DELETE /api/rooms/{id}` - Eliminar sala

### ✅ Configuración de Base de Datos
- **RoomConfiguration**: Configuración de Entity Framework
- **Migración**: Tabla Rooms creada con índices únicos

### ✅ Excepciones Personalizadas
- **RoomNotFoundException**: Cuando una sala no se encuentra
- **RoomSlugAlreadyExistsException**: Cuando el slug ya existe
- **RoomCapacityExceededException**: Cuando se excede la capacidad

### ✅ Validaciones
- **FluentValidation**: Validación avanzada con validadores dedicados
- **Validación asíncrona de Slug único**: Verifica unicidad en base de datos
- **Slug único**: Validado a nivel de base de datos y servicio
- **Formato hex de color**: Validado con regex
- **Límites de usuarios**: Entre 2 y 50
- **Mensajes de error personalizados**: Respuestas claras y específicas

### ✅ Autorización
- **AdminOnly**: Política para operaciones administrativas
- **Documentación Swagger**: Completa con tipos de request/response

## Estructura de Archivos

```
Modules/Rooms/
├── Models/
│   └── Room.cs
├── DTOs/
│   └── RoomDtos.cs
├── Services/
│   ├── IRoomService.cs
│   └── RoomService.cs
├── Repositories/
│   ├── IRoomRepository.cs
│   └── RoomRepository.cs
├── Validators/
│   ├── CreateRoomDtoValidator.cs
│   └── UpdateRoomDtoValidator.cs
├── Exceptions/
│   └── RoomExceptions.cs
├── Configuration/
│   └── RoomConfiguration.cs
├── RoomsModule.cs
└── README.md
```

## Uso de la API

### Crear una Sala (Admin requerido)
```http
POST /api/rooms
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Sala Principal",
  "slug": "sala-principal",
  "color": "#FF5722",
  "icon": "gamepad",
  "userLimit": 10,
  "isOpen": true
}
```

### Obtener Lista de Salas
```http
GET /api/rooms?page=1&pageSize=10&isOpen=true
```

### Obtener Sala por Slug
```http
GET /api/rooms/sala-principal
```

### Actualizar Sala (Admin requerido)
```http
PUT /api/rooms/sala-principal
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Sala Principal Actualizada",
  "userLimit": 15
}
```

### Eliminar Sala (Admin requerido)
```http
DELETE /api/rooms/{id}
Authorization: Bearer {token}
```

## Validaciones Implementadas

- **Name**: Requerido, máximo 100 caracteres
- **Slug**: Requerido, máximo 50 caracteres, solo alfanuméricos y guiones, único
- **Color**: Requerido, formato hex válido (#RRGGBB)
- **Icon**: Requerido, máximo 100 caracteres
- **UserLimit**: Requerido, entre 2 y 50

## Códigos de Respuesta HTTP

- **200 OK**: Operación exitosa
- **201 Created**: Sala creada exitosamente
- **204 No Content**: Sala eliminada exitosamente
- **400 Bad Request**: Datos de entrada inválidos
- **401 Unauthorized**: No autenticado
- **403 Forbidden**: Sin permisos de administrador
- **404 Not Found**: Sala no encontrada
- **409 Conflict**: Slug ya existe

## Consideraciones Técnicas

- **Paginación**: Implementada en el listado de salas
- **Índices de Base de Datos**: Slug único, IsOpen para filtrado
- **FluentValidation**: Validación avanzada con validadores personalizados
- **Validación Asíncrona**: Verificación de unicidad de slug en tiempo real
- **Manejo de Errores**: Excepciones personalizadas con respuestas HTTP apropiadas
- **Documentación Swagger**: Tipos de request/response explícitos

## TODO / Mejoras Futuras

- [ ] Implementar conteo real de usuarios activos en salas
- [ ] Agregar filtros adicionales (por nombre, color, etc.)
- [ ] Implementar soft delete en lugar de eliminación física
- [ ] Agregar cacheo para consultas frecuentes
- [ ] Implementar audit trail para cambios en salas

## Historial de Refactorizaciones

### ✅ Refactorización FluentValidation (Agosto 2025)
- **Migración de DataAnnotations a FluentValidation**: Se refactorizó el sistema de validaciones para usar FluentValidation en lugar de DataAnnotations
- **Validadores Dedicados**: Se crearon `CreateRoomDtoValidator` y `UpdateRoomDtoValidator` con validaciones específicas
- **Validación Asíncrona**: Implementada verificación asíncrona de unicidad de slug
- **Mensajes de Error Mejorados**: Personalización completa de mensajes de validación
- **Integración con Minimal APIs**: Configuración completa del sistema de validación en endpoints
