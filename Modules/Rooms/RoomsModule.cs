using DnuGame.Api.Common.DTO;
using DnuGame.Api.Modules.Rooms.DTOs;
using DnuGame.Api.Modules.Rooms.Exceptions;
using DnuGame.Api.Modules.Rooms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace DnuGame.Api.Modules.Rooms;

public static class RoomsModule
{
    public static IEndpointRouteBuilder MapRoomsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/rooms").WithTags("Rooms");

        // GET /api/rooms - Obtener lista de salas
        group.MapGet("/", GetRoomsAsync)
            .WithName("GetRooms")
            .WithOpenApi()
            .Produces<PaginatedResult<RoomListDto>>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        // GET /api/rooms/{slug} - Obtener sala por slug
        group.MapGet("/{slug}", GetRoomBySlugAsync)
            .WithName("GetRoomBySlug")
            .WithOpenApi()
            .Produces<RoomResponseDto>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        // POST /api/rooms - Crear nueva sala
        group.MapPost("/", CreateRoomAsync)
            .RequireAuthorization("AdminOnly")
            .WithName("CreateRoom")
            .WithOpenApi()
            .Accepts<CreateRoomDto>("application/json")
            .Produces<RoomResponseDto>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status401Unauthorized)
            .Produces<ErrorResponse>(StatusCodes.Status403Forbidden)
            .Produces<ErrorResponse>(StatusCodes.Status409Conflict);

        // PUT /api/rooms/{slug} - Actualizar sala existente
        group.MapPut("/{slug}", UpdateRoomAsync)
            .RequireAuthorization("AdminOnly")
            .WithName("UpdateRoom")
            .WithOpenApi()
            .Accepts<UpdateRoomDto>("application/json")
            .Produces<RoomResponseDto>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status401Unauthorized)
            .Produces<ErrorResponse>(StatusCodes.Status403Forbidden)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        // DELETE /api/rooms/{id} - Eliminar sala
        group.MapDelete("/{id:guid}", DeleteRoomAsync)
            .RequireAuthorization("AdminOnly")
            .WithName("DeleteRoom")
            .WithOpenApi()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorResponse>(StatusCodes.Status401Unauthorized)
            .Produces<ErrorResponse>(StatusCodes.Status403Forbidden)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<IResult> GetRoomsAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? isOpen = null,
        IRoomService roomService = null!)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var result = await roomService.GetRoomsAsync(page, pageSize, isOpen);
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new ErrorResponse(
                "get_rooms_failed",
                "Failed to Get Rooms",
                ex.Message,
                400
            ));
        }
    }

    private static async Task<IResult> GetRoomBySlugAsync(
        string slug,
        IRoomService roomService)
    {
        try
        {
            var room = await roomService.GetRoomBySlugAsync(slug);
            if (room == null)
            {
                return Results.NotFound(new ErrorResponse(
                    "room_not_found",
                    "Room Not Found",
                    $"Room with slug '{slug}' was not found.",
                    404
                ));
            }

            return Results.Ok(room);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new ErrorResponse(
                "get_room_failed",
                "Failed to Get Room",
                ex.Message,
                400
            ));
        }
    }

    private static async Task<IResult> CreateRoomAsync(
        [FromBody] CreateRoomDto request,
        IRoomService roomService,
        IValidator<CreateRoomDto> validator)
    {
        try
        {
            // Validar con FluentValidation
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return Results.BadRequest(new ErrorResponse(
                    "validation_failed",
                    "Validation Failed",
                    string.Join("; ", errors),
                    400
                ));
            }

            var room = await roomService.CreateRoomAsync(request);
            return Results.Created($"/api/rooms/{room.Slug}", room);
        }
        catch (RoomSlugAlreadyExistsException ex)
        {
            return Results.Conflict(new ErrorResponse(
                "room_slug_exists",
                "Room Slug Already Exists",
                ex.Message,
                409
            ));
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new ErrorResponse(
                "create_room_failed",
                "Failed to Create Room",
                ex.Message,
                400
            ));
        }
    }

    private static async Task<IResult> UpdateRoomAsync(
        string slug,
        [FromBody] UpdateRoomDto request,
        IRoomService roomService,
        IValidator<UpdateRoomDto> validator)
    {
        try
        {
            // Validar con FluentValidation
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return Results.BadRequest(new ErrorResponse(
                    "validation_failed",
                    "Validation Failed",
                    string.Join("; ", errors),
                    400
                ));
            }

            var room = await roomService.UpdateRoomAsync(slug, request);
            if (room == null)
            {
                return Results.NotFound(new ErrorResponse(
                    "room_not_found",
                    "Room Not Found",
                    $"Room with slug '{slug}' was not found.",
                    404
                ));
            }

            return Results.Ok(room);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new ErrorResponse(
                "update_room_failed",
                "Failed to Update Room",
                ex.Message,
                400
            ));
        }
    }

    private static async Task<IResult> DeleteRoomAsync(
        Guid id,
        IRoomService roomService)
    {
        try
        {
            var deleted = await roomService.DeleteRoomAsync(id);
            if (!deleted)
            {
                return Results.NotFound(new ErrorResponse(
                    "room_not_found",
                    "Room Not Found",
                    $"Room with ID '{id}' was not found.",
                    404
                ));
            }

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new ErrorResponse(
                "delete_room_failed",
                "Failed to Delete Room",
                ex.Message,
                400
            ));
        }
    }
}
