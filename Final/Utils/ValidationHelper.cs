namespace Final.Utils;

public static class ValidationHelper
{
    public static bool IsValidEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }

    public static bool IsValidPhone(string phone)
    {
        return phone.Length >= 9 && phone.All(char.IsDigit);
    }

    public static bool IsValidDate(string date, out DateTime result)
    {
        return DateTime.TryParse(date, out result);
    }
}
