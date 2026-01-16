public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public decimal Balance { get; set; }
    public string Role { get; set; } 
    public DateTime RegisterDate { get; set; }
}

public class UserData
{
    public List<User> Users { get; set; } = new List<User>();
}