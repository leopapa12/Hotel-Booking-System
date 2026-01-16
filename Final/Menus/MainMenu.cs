using Final.Models;
using Final.Services;
using Final.Utils;

namespace Final.Menus;

public class MainMenu
{
    private readonly AuthService authService;
    private readonly UserService userService;
    private readonly CustomerMenu customerMenu;
    private readonly ManagerMenu managerMenu;

    public MainMenu(AuthService authService, UserService userService, CustomerMenu customerMenu, ManagerMenu managerMenu)
    {
        this.authService = authService;
        this.userService = userService;
        this.customerMenu = customerMenu;
        this.managerMenu = managerMenu;
    }

    public void Show()
    {
        while (true)
        {
            ConsoleHelper.ShowHeader(" HOTEL BOOKING SYSTEM");
            Console.WriteLine("1.  Login");
            Console.WriteLine("2.  Register");
            Console.WriteLine("3.  Exit");
            Console.WriteLine("═══════════════════════════════════════");
            Console.Write("Choose option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    Register();
                    break;
                case "3":
                    return;
                default:
                    ConsoleHelper.ShowError("Invalid option!");
                    ConsoleHelper.Pause();
                    break;
            }
        }
    }

    private void Login()
    {
        ConsoleHelper.ShowHeader(" LOGIN");
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();

        if (authService.Login(username, password))
        {
            ConsoleHelper.ShowSuccess($"Welcome, {authService.CurrentUser.FullName}!");
            ConsoleHelper.Pause();

            if (authService.IsManager())
            {
                managerMenu.Show();
            }
            else
            {
                customerMenu.Show();
            }

            authService.Logout();
        }
        else
        {
            ConsoleHelper.ShowError("Invalid credentials!");
            ConsoleHelper.Pause();
        }
    }

    private void Register()
    {
        ConsoleHelper.ShowHeader(" REGISTER");
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();
        Console.Write("Full Name: ");
        string fullName = Console.ReadLine();
        Console.Write("Email: ");
        string email = Console.ReadLine();
        Console.Write("Phone: ");
        string phone = Console.ReadLine();

        if (userService.Register(username, password, fullName, email, phone))
        {
            ConsoleHelper.ShowSuccess("Registration successful!");
        }
        else
        {
            ConsoleHelper.ShowError("Username already exists!");
        }
        ConsoleHelper.Pause();
    }
}