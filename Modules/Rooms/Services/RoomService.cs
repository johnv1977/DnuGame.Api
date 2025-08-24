using DnuGame.Api.Modules.Rooms.DTOs;
using DnuGame.Api.Modules.Rooms.Exceptions;
using DnuGame.Api.Modules.Rooms.Models;
using DnuGame.Api.Modules.Rooms.Repositories;

namespace DnuGame.Api.Modules.Rooms.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<PaginatedResult<RoomListDto>> GetRoomsAsync(int page, int pageSize, bool? isOpen)
    {
        var skip = (page - 1) * pageSize;
        var rooms = await _roomRepository.GetAllAsync(skip, pageSize, isOpen);
        var totalCount = await _roomRepository.GetTotalCountAsync(isOpen);
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var roomDtos = new List<RoomListDto>();
        foreach (var room in rooms)
        {
            var currentUserCount = await _roomRepository.GetCurrentUserCountAsync(room.Id);
            roomDtos.Add(new RoomListDto(
                room.Id,
                room.Slug,
                room.Name,
                room.Color,
                room.Icon,
                room.UserLimit,
                room.IsOpen,
                currentUserCount
            ));
        }

        return new PaginatedResult<RoomListDto>(roomDtos, page, pageSize, totalCount, totalPages);
    }

    public async Task<RoomResponseDto?> GetRoomBySlugAsync(string slug)
    {
        var room = await _roomRepository.GetBySlugAsync(slug);
        if (room == null)
            return null;

        var currentUserCount = await _roomRepository.GetCurrentUserCountAsync(room.Id);
        return new RoomResponseDto(
            room.Id,
            room.Slug,
            room.Name,
            room.Color,
            room.Icon,
            room.UserLimit,
            room.IsOpen,
            currentUserCount,
            room.CreatedAt,
            room.UpdatedAt
        );
    }

    public async Task<RoomResponseDto> CreateRoomAsync(CreateRoomDto createRoomDto)
    {
        // Validar que el slug sea único
        if (await _roomRepository.ExistsBySlugAsync(createRoomDto.Slug))
        {
            throw new RoomSlugAlreadyExistsException(createRoomDto.Slug);
        }

        var room = new Room
        {
            Slug = createRoomDto.Slug,
            Name = createRoomDto.Name,
            Color = createRoomDto.Color,
            Icon = createRoomDto.Icon,
            UserLimit = createRoomDto.UserLimit,
            IsOpen = createRoomDto.IsOpen
        };

        var createdRoom = await _roomRepository.CreateAsync(room);
        var currentUserCount = await _roomRepository.GetCurrentUserCountAsync(createdRoom.Id);

        return new RoomResponseDto(
            createdRoom.Id,
            createdRoom.Slug,
            createdRoom.Name,
            createdRoom.Color,
            createdRoom.Icon,
            createdRoom.UserLimit,
            createdRoom.IsOpen,
            currentUserCount,
            createdRoom.CreatedAt,
            createdRoom.UpdatedAt
        );
    }

    public async Task<RoomResponseDto?> UpdateRoomAsync(string slug, UpdateRoomDto updateRoomDto)
    {
        var existingRoom = await _roomRepository.GetBySlugAsync(slug);
        if (existingRoom == null)
            return null;

        // Actualizar solo los campos proporcionados
        if (!string.IsNullOrEmpty(updateRoomDto.Name))
            existingRoom.Name = updateRoomDto.Name;

        if (!string.IsNullOrEmpty(updateRoomDto.Color))
            existingRoom.Color = updateRoomDto.Color;

        if (!string.IsNullOrEmpty(updateRoomDto.Icon))
            existingRoom.Icon = updateRoomDto.Icon;

        if (updateRoomDto.UserLimit.HasValue)
            existingRoom.UserLimit = updateRoomDto.UserLimit.Value;

        if (updateRoomDto.IsOpen.HasValue)
            existingRoom.IsOpen = updateRoomDto.IsOpen.Value;

        var updatedRoom = await _roomRepository.UpdateAsync(existingRoom);
        if (updatedRoom == null)
            return null;

        var currentUserCount = await _roomRepository.GetCurrentUserCountAsync(updatedRoom.Id);
        return new RoomResponseDto(
            updatedRoom.Id,
            updatedRoom.Slug,
            updatedRoom.Name,
            updatedRoom.Color,
            updatedRoom.Icon,
            updatedRoom.UserLimit,
            updatedRoom.IsOpen,
            currentUserCount,
            updatedRoom.CreatedAt,
            updatedRoom.UpdatedAt
        );
    }

    public async Task<bool> DeleteRoomAsync(Guid id)
    {
        return await _roomRepository.DeleteAsync(id);
    }

    public async Task<bool> RoomExistsBySlugAsync(string slug)
    {
        return await _roomRepository.ExistsBySlugAsync(slug);
    }
}
