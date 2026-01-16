using Final.Utils;

namespace Final.Services;

public class UserService
{
    private const string FilePath = "Data/Users.json";
    private UserData userData;

    public UserService()
    {
        userData = JsonHelper.LoadData<UserData>(FilePath);
        InitializeDefaultUsers();
    }

    private void InitializeDefaultUsers()
    {
        if (userData.Users.Count == 0)
        {
            userData.Users.Add(new User
            {
                UserId = 1,
                Username = "admin",
                Password = "admin123",
                FullName = "Hotel Manager",
                Email = "admin@hotel.com",
                Phone = "555000000",
                Balance = 0,
                Role = "Manager",
                RegisterDate = DateTime.Now
            });
            SaveData();
        }
    }

    public bool Register(string username, string password, string fullName, string email, string phone)
    {
        if (userData.Users.Any(u => u.Username == username))
            return false;

        var newUser = new User
        {
            UserId = userData.Users.Count > 0 ? userData.Users.Max(u => u.UserId) + 1 : 1,
            Username = username,
            Password = password,
            FullName = fullName,
            Email = email,
            Phone = phone,
            Balance = 0,
            Role = "Customer",
            RegisterDate = DateTime.Now
        };

        userData.Users.Add(newUser);
        SaveData();
        return true;
    }

    public User GetUserByUsername(string username)
    {
        return userData.Users.FirstOrDefault(u => u.Username == username);
    }

    public void AddBalance(int userId, decimal amount)
    {
        var user = userData.Users.FirstOrDefault(u => u.UserId == userId);
        if (user != null)
        {
            user.Balance += amount;
            SaveData();
        }
    }

    public bool DeductBalance(int userId, decimal amount)
    {
        var user = userData.Users.FirstOrDefault(u => u.UserId == userId);
        if (user != null && user.Balance >= amount)
        {
            user.Balance -= amount;
            SaveData();
            return true;
        }
        return false;
    }

    public List<User> GetAllUsers()
    {
        return userData.Users;
    }

    private void SaveData()
    {
        JsonHelper.SaveData(FilePath, userData);
    }
}