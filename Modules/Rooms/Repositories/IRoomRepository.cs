using DnuGame.Api.Modules.Rooms.Models;

namespace DnuGame.Api.Modules.Rooms.Repositories;

public interface IRoomRepository
{
    Task<IEnumerable<Room>> GetAllAsync(int skip, int take, bool? isOpen);
    Task<Room?> GetByIdAsync(Guid id);
    Task<Room?> GetBySlugAsync(string slug);
    Task<Room> CreateAsync(Room room);
    Task<Room?> UpdateAsync(Room room);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsBySlugAsync(string slug);
    Task<int> GetTotalCountAsync(bool? isOpen);
    Task<int> GetCurrentUserCountAsync(Guid roomId);
}
