using Final.Models;
using Final.Utils;

namespace Final.Services;

public class AuthService
{
    private readonly UserService userService;
    public User CurrentUser { get; private set; }

    public AuthService(UserService userService)
    {
        this.userService = userService;
    }

    public bool Login(string username, string password)
    {
        var user = userService.GetUserByUsername(username);
        if (user != null && user.Password == password)
        {
            CurrentUser = user;
            return true;
        }
        return false;
    }

    public void Logout()
    {
        CurrentUser = null;
    }

    public bool IsLoggedIn()
    {
        return CurrentUser != null;
    }

    public bool IsManager()
    {
        return CurrentUser?.Role == "Manager";
    }
}