using Final.Services;
using Final.Utils;

namespace Final.Menus;

public class CustomerMenu
{
    private readonly AuthService authService;
    private readonly UserService userService;
    private readonly RoomService roomService;
    private readonly ReservationService reservationService;

    public CustomerMenu(AuthService authService, UserService userService, RoomService roomService, ReservationService reservationService)
    {
        this.authService = authService;
        this.userService = userService;
        this.roomService = roomService;
        this.reservationService = reservationService;
    }

    public void Show()
    {
        while (true)
        {
            var user = authService.CurrentUser;
            ConsoleHelper.ShowHeader($"Welcome, {user.FullName}!  Balance: {user.Balance:F2}₾");
            Console.WriteLine("1.  View Available Rooms");
            Console.WriteLine("2.  Search Rooms by Type");
            Console.WriteLine("3.  Book Room");
            Console.WriteLine("4.  My Reservations");
            Console.WriteLine("5.  Cancel Reservation");
            Console.WriteLine("6.  Make Payment");
            Console.WriteLine("7.  Add Balance");
            Console.WriteLine("8.  Logout");
            Console.WriteLine("═══════════════════════════════════════");
            Console.Write("Choose option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewAvailableRooms();
                    break;
                case "2":
                    SearchRoomsByType();
                    break;
                case "3":
                    BookRoom();
                    break;
                case "4":
                    ViewMyReservations();
                    break;
                case "5":
                    CancelReservation();
                    break;
                case "6":
                    MakePayment();
                    break;
                case "7":
                    AddBalance();
                    break;
                case "8":
                    return;
                default:
                    ConsoleHelper.ShowError("Invalid option!");
                    ConsoleHelper.Pause();
                    break;
            }
        }
    }

    private void ViewAvailableRooms()
    {
        ConsoleHelper.ShowHeader("  AVAILABLE ROOMS");
        var rooms = roomService.GetAvailableRooms();

        if (rooms.Count == 0)
        {
            ConsoleHelper.ShowInfo("No rooms available.");
        }
        else
        {
            foreach (var room in rooms)
            {
                Console.WriteLine($"\n Room {room.RoomNumber} (ID: {room.RoomId})");
                Console.WriteLine($"   Type: {room.Type} | Capacity: {room.Capacity} | Floor: {room.Floor}");
                Console.WriteLine($"   Price: {room.PricePerNight}₾/night");
                Console.WriteLine($"   Amenities: {string.Join(", ", room.Amenities)}");
            }
        }
        ConsoleHelper.Pause();
    }

    private void SearchRoomsByType()
    {
        ConsoleHelper.ShowHeader(" SEARCH ROOMS");
        Console.Write("Enter room type (Single/Double/Suite): ");
        string type = Console.ReadLine();

        var rooms = roomService.SearchRoomsByType(type);

        if (rooms.Count == 0)
        {
            ConsoleHelper.ShowInfo($"No {type} rooms available.");
        }
        else
        {
            foreach (var room in rooms)
            {
                Console.WriteLine($"\n Room {room.RoomNumber} - {room.PricePerNight}₾/night");
            }
        }
        ConsoleHelper.Pause();
    }

    private void BookRoom()
    {
        ConsoleHelper.ShowHeader(" BOOK ROOM");
        Console.Write("Room ID: ");
        if (!int.TryParse(Console.ReadLine(), out int roomId))
        {
            ConsoleHelper.ShowError("Invalid room ID!");
            ConsoleHelper.Pause();
            return;
        }

        Console.Write("Check-in date (yyyy-mm-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime checkIn))
        {
            ConsoleHelper.ShowError("Invalid date!");
            ConsoleHelper.Pause();
            return;
        }

        Console.Write("Check-out date (yyyy-mm-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime checkOut))
        {
            ConsoleHelper.ShowError("Invalid date!");
            ConsoleHelper.Pause();
            return;
        }

        if (reservationService.BookRoom(authService.CurrentUser.UserId, roomId, checkIn, checkOut))
        {
            ConsoleHelper.ShowSuccess("Room booked successfully!");
        }
        else
        {
            ConsoleHelper.ShowError("Booking failed!");
        }
        ConsoleHelper.Pause();
    }

    private void ViewMyReservations()
    {
        ConsoleHelper.ShowHeader(" MY RESERVATIONS");
        var reservations = reservationService.GetUserReservations(authService.CurrentUser.UserId);

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
                Console.WriteLine($"   Room: {room?.RoomNumber} ({room?.Type})");
                Console.WriteLine($"   Check-in: {res.CheckInDate:yyyy-MM-dd}");
                Console.WriteLine($"   Check-out: {res.CheckOutDate:yyyy-MM-dd}");
                Console.WriteLine($"   Total: {res.TotalPrice}₾");
                Console.WriteLine($"   Status: {res.Status}");
                Console.WriteLine($"   Paid: {(res.IsPaid ? "Yes" : "No")}");
            }
        }
        ConsoleHelper.Pause();
    }

    private void CancelReservation()
    {
        ConsoleHelper.ShowHeader(" CANCEL RESERVATION");
        Console.Write("Reservation ID: ");
        if (!int.TryParse(Console.ReadLine(), out int reservationId))
        {
            ConsoleHelper.ShowError("Invalid ID!");
            ConsoleHelper.Pause();
            return;
        }

        if (reservationService.CancelReservation(reservationId, authService.CurrentUser.UserId))
        {
            ConsoleHelper.ShowSuccess("Reservation cancelled!");
        }
        else
        {
            ConsoleHelper.ShowError("Cancellation failed!");
        }
        ConsoleHelper.Pause();
    }

    private void MakePayment()
    {
        ConsoleHelper.ShowHeader(" MAKE PAYMENT");
        Console.Write("Reservation ID: ");
        if (!int.TryParse(Console.ReadLine(), out int reservationId))
        {
            ConsoleHelper.ShowError("Invalid ID!");
            ConsoleHelper.Pause();
            return;
        }

        var reservations = reservationService.GetUserReservations(authService.CurrentUser.UserId);
        var reservation = reservations.FirstOrDefault(r => r.ReservationId == reservationId);

        if (reservation == null)
        {
            ConsoleHelper.ShowError("Reservation not found!");
            ConsoleHelper.Pause();
            return;
        }

        if (userService.DeductBalance(authService.CurrentUser.UserId, reservation.TotalPrice))
        {
            reservationService.MakePayment(reservationId, authService.CurrentUser.UserId);
            ConsoleHelper.ShowSuccess($"Payment of {reservation.TotalPrice}₾ successful!");
        }
        else
        {
            ConsoleHelper.ShowError("Insufficient balance!");
        }
        ConsoleHelper.Pause();
    }

    private void AddBalance()
    {
        ConsoleHelper.ShowHeader(" ADD BALANCE");
        Console.Write("Amount: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
        {
            userService.AddBalance(authService.CurrentUser.UserId, amount);
            ConsoleHelper.ShowSuccess($"Added {amount}₾ to your balance!");
        }
        else
        {
            ConsoleHelper.ShowError("Invalid amount!");
        }
        ConsoleHelper.Pause();
    }
}
