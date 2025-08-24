using DnuGame.Api.Infrastructure.Persistence;
using DnuGame.Api.Modules.Rooms.Models;
using Microsoft.EntityFrameworkCore;

namespace DnuGame.Api.Modules.Rooms.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationDbContext _context;

    public RoomRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Room>> GetAllAsync(int skip, int take, bool? isOpen)
    {
        var query = _context.Rooms.AsQueryable();

        if (isOpen.HasValue)
        {
            query = query.Where(r => r.IsOpen == isOpen.Value);
        }

        return await query
            .OrderBy(r => r.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<Room?> GetByIdAsync(Guid id)
    {
        return await _context.Rooms.FindAsync(id);
    }

    public async Task<Room?> GetBySlugAsync(string slug)
    {
        return await _context.Rooms.FirstOrDefaultAsync(r => r.Slug == slug);
    }

    public async Task<Room> CreateAsync(Room room)
    {
        room.Id = Guid.NewGuid();
        room.CreatedAt = DateTime.UtcNow;
        room.UpdatedAt = DateTime.UtcNow;

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task<Room?> UpdateAsync(Room room)
    {
        var existingRoom = await GetByIdAsync(room.Id);
        if (existingRoom == null)
            return null;

        existingRoom.Name = room.Name;
        existingRoom.Color = room.Color;
        existingRoom.Icon = room.Icon;
        existingRoom.UserLimit = room.UserLimit;
        existingRoom.IsOpen = room.IsOpen;
        existingRoom.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingRoom;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var room = await GetByIdAsync(id);
        if (room == null)
            return false;

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsBySlugAsync(string slug)
    {
        return await _context.Rooms.AnyAsync(r => r.Slug == slug);
    }

    public async Task<int> GetTotalCountAsync(bool? isOpen)
    {
        var query = _context.Rooms.AsQueryable();

        if (isOpen.HasValue)
        {
            query = query.Where(r => r.IsOpen == isOpen.Value);
        }

        return await query.CountAsync();
    }

    public async Task<int> GetCurrentUserCountAsync(Guid roomId)
    {
        // TODO: Implementar cuando tengamos el módulo de usuarios en salas
        // Por ahora retornamos 0
        return 0;
    }
}
