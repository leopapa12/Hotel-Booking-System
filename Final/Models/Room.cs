namespace Final.Models;

public class Room
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; }
    public string Type { get; set; } 
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; }
    public int Floor { get; set; }
    public List<string> Amenities { get; set; } = new List<string>();
    public bool IsAvailable { get; set; }
}

public class RoomData
{
    public List<Room> Rooms { get; set; } = new List<Room>();
}
