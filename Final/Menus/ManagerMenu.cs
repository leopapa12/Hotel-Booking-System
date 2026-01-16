using Final.Models;
using Final.Services;
using Final.Utils;

namespace Final.Menus;

public class ManagerMenu
{
    private readonly RoomService roomService;
    private readonly ReservationService reservationService;

    public ManagerMenu(RoomService roomService, ReservationService reservationService)
    {
        this.roomService = roomService;
        this.reservationService = reservationService;
    }

    public void Show()
    {
        while (true)
        {
            ConsoleHelper.ShowHeader(" MANAGER PANEL");
            Console.WriteLine("1.  Add Room");
            Console.WriteLine("2.  Update Room Price");
            Console.WriteLine("3.  View All Reservations");
            Console.WriteLine("4.  Logout");
            Console.WriteLine("═══════════════════════════════════════");
            Console.Write("Choose option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddRoom();
                    break;
                case "2":
                    UpdateRoomPrice();
                    break;
                case "3":
                    ViewAllReservations();
                    break;
                case "4":
                    return;
                default:
                    ConsoleHelper.ShowError("Invalid option!");
                    ConsoleHelper.Pause();
                    break;
            }
        }
    }

    private void AddRoom()
    {
        ConsoleHelper.ShowHeader(" ADD ROOM");
        Console.Write("Room ID: ");
        if (!int.TryParse(Console.ReadLine(), out int roomId))
        {
            ConsoleHelper.ShowError("Invalid ID!");
            ConsoleHelper.Pause();
            return;
        }

        Console.Write("Room Number: ");
        string roomNumber = Console.ReadLine();
        Console.Write("Type (Single/Double/Suite): ");
        string type = Console.ReadLine();
        Console.Write("Price per night: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            ConsoleHelper.ShowError("Invalid price!");
            ConsoleHelper.Pause();
            return;
        }

        var room = new Room
        {
            RoomId = roomId,
            RoomNumber = roomNumber,
            Type = type,
            PricePerNight = price,
            Capacity = type == "Single" ? 1 : type == "Double" ? 2 : 4,
            Floor = int.Parse(roomNumber[0].ToString()),
            Amenities = new List<string> { "WiFi", "TV", "AC" },
            IsAvailable = true
        };

        if (roomService.AddRoom(room))
        {
            ConsoleHelper.ShowSuccess("Room added successfully!");
        }
        else
        {
            ConsoleHelper.ShowError("Room ID already exists!");
        }
        ConsoleHelper.Pause();
    }

    private void UpdateRoomPrice()
    {
        ConsoleHelper.ShowHeader(" UPDATE ROOM PRICE");
        Console.Write("Room ID: ");
        if (!int.TryParse(Console.ReadLine(), out int roomId))
        {
            ConsoleHelper.ShowError("Invalid ID!");
            ConsoleHelper.Pause();
            return;
        }

        Console.Write("New price: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal newPrice))
        {
            ConsoleHelper.ShowError("Invalid price!");
            ConsoleHelper.Pause();
            return;
        }

        if (roomService.UpdateRoomPrice(roomId, newPrice))
        {
            ConsoleHelper.ShowSuccess("Price updated successfully!");
        }
        else
        {
            ConsoleHelper.ShowError("Room not found!");
        }
        ConsoleHelper.Pause();
    }

    private void ViewAllReservations()
    {
        ConsoleHelper.ShowHeader(" ALL RESERVATIONS");
        var reservations = reservationService.GetAllReservations();

        if (reservations.Count == 0)
        {
            ConsoleHelper.ShowInfo("No reservations found.");
        }
        else
        {
            foreach (var res in reservations)
            {
                var room = roomService.GetRoomById(res.RoomId);
                Console.WriteLine($"\n Reservation #{res.ReservationId}");
                Console.WriteLine($"   User ID: {res.UserId}");
                Console.WriteLine($"   Room: {room?.RoomNumber} ({room?.Type})");
                Console.WriteLine($"   Check-in: {res.CheckInDate:yyyy-MM-dd}");
                Console.WriteLine($"   Check-out: {res.CheckOutDate:yyyy-MM-dd}");
                Console.WriteLine($"   Total: {res.TotalPrice}₾");
                Console.WriteLine($"   Status: {res.Status}");
                Console.WriteLine($"   Paid: {(res.IsPaid ? "Yes" : "No")}");
                Console.WriteLine($"   Booked: {res.BookingDate:yyyy-MM-dd}");
            }
        }
        ConsoleHelper.Pause();
    }
}