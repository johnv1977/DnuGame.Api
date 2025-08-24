namespace DnuGame.Api.Modules.Rooms.Exceptions;

public class RoomNotFoundException : Exception
{
    public RoomNotFoundException(string slug) 
        : base($"Room with slug '{slug}' was not found.")
    {
    }

    public RoomNotFoundException(Guid id) 
        : base($"Room with ID '{id}' was not found.")
    {
    }
}

public class RoomSlugAlreadyExistsException : Exception
{
    public RoomSlugAlreadyExistsException(string slug) 
        : base($"Room with slug '{slug}' already exists.")
    {
    }
}

public class RoomCapacityExceededException : Exception
{
    public RoomCapacityExceededException(string roomName, int currentCount, int limit) 
        : base($"Room '{roomName}' has reached its capacity limit. Current: {currentCount}, Limit: {limit}")
    {
    }
}
