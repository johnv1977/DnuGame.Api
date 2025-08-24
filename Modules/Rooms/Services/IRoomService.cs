using DnuGame.Api.Modules.Rooms.DTOs;

namespace DnuGame.Api.Modules.Rooms.Services;

public interface IRoomService
{
    Task<PaginatedResult<RoomListDto>> GetRoomsAsync(int page, int pageSize, bool? isOpen);
    Task<RoomResponseDto?> GetRoomBySlugAsync(string slug);
    Task<RoomResponseDto> CreateRoomAsync(CreateRoomDto createRoomDto);
    Task<RoomResponseDto?> UpdateRoomAsync(string slug, UpdateRoomDto updateRoomDto);
    Task<bool> DeleteRoomAsync(Guid id);
    Task<bool> RoomExistsBySlugAsync(string slug);
}
