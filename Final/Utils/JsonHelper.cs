using System.Text.Json;

namespace Final.Utils;

public static class JsonHelper
{
    public static T LoadData<T>(string filePath) where T : new()
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return new T();
            }

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json) ?? new T();
        }
        catch
        {
            return new T();
        }
    }

    public static void SaveData<T>(string filePath, T data)
    {
        string directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(filePath, json);
    }
}