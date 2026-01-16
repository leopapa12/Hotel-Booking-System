using Final.Menus;
using Final.Services;
using Final.Utils;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var userService = new UserService();
        var roomService = new RoomService();
        var reservationService = new ReservationService(roomService);
        var authService = new AuthService(userService);

        var customerMenu = new CustomerMenu(authService, userService, roomService, reservationService);
        var managerMenu = new ManagerMenu(roomService, reservationService);
        var mainMenu = new MainMenu(authService, userService, customerMenu, managerMenu);

        mainMenu.Show();

        Console.WriteLine("\nThank you for using Hotel Booking System! ");
    }
}