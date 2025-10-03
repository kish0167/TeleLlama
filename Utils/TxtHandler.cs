namespace TeleLlama.Utils;
using System.Text.Json;

public static class TxtHandler
{
    private static JsonSerializerOptions _options = new JsonSerializerOptions();
    public static void SaveTxt(string path, string content)
    {
        if (!path.EndsWith(".txt"))
        {
            path += ".txt";
        }
        
        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        
        using StreamWriter sw = File.CreateText(path);
        sw.Write(content);
        sw.Close();
    }

    public static string ReadTxt(string path)
    {
        if (!path.EndsWith(".txt"))
        {
            path += ".txt";
        }

        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

        if (!Path.Exists(path))
        {
            return "";
        }
        
        StreamReader r = new StreamReader(path);
        string txt = r.ReadToEnd();
        r.Close();
        return txt;
    }

    public static T? ReadAsObject<T>(string path) where T : class
    {
        string serialised = ReadTxt(path);
        T? obj;
        
        try
        {
            obj = JsonSerializer.Deserialize<T?>(serialised, _options);
        }
        catch (Exception e)
        {
            return null;
        }

        return obj;
    }

    public static void SaveAsObject<T>(T data, string path)
    {
        SaveTxt(path, JsonSerializer.Serialize<T>(data, _options));
    }
}