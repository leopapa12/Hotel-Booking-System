using Final.Models;
using Final.Utils;

namespace Final.Services;

public class RoomService
{
    private const string FilePath = "Data/Rooms.json";
    private RoomData roomData;

    public RoomService()
    {
        roomData = JsonHelper.LoadData<RoomData>(FilePath);
        InitializeDefaultRooms();
    }

    private void InitializeDefaultRooms()
    {
        if (roomData.Rooms.Count == 0)
        {
            roomData.Rooms.AddRange(new List<Room>
            {
                new Room { RoomId = 101, RoomNumber = "101", Type = "Single", PricePerNight = 80, Capacity = 1, Floor = 1, Amenities = new List<string> { "WiFi", "TV", "AC" }, IsAvailable = true },
                new Room { RoomId = 102, RoomNumber = "102", Type = "Single", PricePerNight = 80, Capacity = 1, Floor = 1, Amenities = new List<string> { "WiFi", "TV", "AC" }, IsAvailable = true },
                new Room { RoomId = 201, RoomNumber = "201", Type = "Double", PricePerNight = 120, Capacity = 2, Floor = 2, Amenities = new List<string> { "WiFi", "TV", "AC", "Minibar" }, IsAvailable = true },
                new Room { RoomId = 301, RoomNumber = "301", Type = "Suite", PricePerNight = 200, Capacity = 4, Floor = 3, Amenities = new List<string> { "WiFi", "TV", "AC", "Minibar", "Jacuzzi" }, IsAvailable = true }
            });
            SaveData();
        }
    }

    public List<Room> GetAvailableRooms()
    {
        return roomData.Rooms.Where(r => r.IsAvailable).ToList();
    }

    public List<Room> SearchRoomsByType(string type)
    {
        return roomData.Rooms.Where(r => r.IsAvailable && r.Type.ToLower() == type.ToLower()).ToList();
    }

    public Room GetRoomById(int roomId)
    {
        return roomData.Rooms.FirstOrDefault(r => r.RoomId == roomId);
    }

    public bool AddRoom(Room room)
    {
        if (roomData.Rooms.Any(r => r.RoomId == room.RoomId))
            return false;

        roomData.Rooms.Add(room);
        SaveData();
        return true;
    }

    public bool UpdateRoomPrice(int roomId, decimal newPrice)
    {
        var room = roomData.Rooms.FirstOrDefault(r => r.RoomId == roomId);
        if (room != null)
        {
            room.PricePerNight = newPrice;
            SaveData();
            return true;
        }
        return false;
    }

    public void SetRoomAvailability(int roomId, bool isAvailable)
    {
        var room = roomData.Rooms.FirstOrDefault(r => r.RoomId == roomId);
        if (room != null)
        {
            room.IsAvailable = isAvailable;
            SaveData();
        }
    }

    private void SaveData()
    {
        JsonHelper.SaveData(FilePath, roomData);
    }
}
