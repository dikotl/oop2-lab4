using System.IO;
using System.Text.Json;
using ServiceMarketplace.Models;

namespace ServiceMarketplace.Services;

public class DataBaseException(string message) : Exception(message);

public class DataBaseService
{
    // JSON database serialization options, created once and used each time we're saving the database.
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = true,
    };

    public DataBase? Load(DialogService _)
    {
        // TODO: move to DialogService.
        var dialog = new Microsoft.Win32.OpenFileDialog()
        {
            Filter = "JSON DataBase (*.json)|*.json"
        };
        if (dialog.ShowDialog() is true)
        {
            using var stream = dialog.OpenFile();
            using var reader = new StreamReader(stream);
            return Load(json: reader.ReadToEnd());
        }
        return null;
    }

    private DataBase? Load(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<DataBase>(json, _serializerOptions);
        }
        catch (Exception e) when (e is JsonException or NotSupportedException)
        {
            throw new DataBaseException($"JSON deserialization error: {e.Message}");
        }
    }

    public void Save(DataBase db, DialogService _)
    {
        // TODO: move to DialogService.
        var dialog = new Microsoft.Win32.SaveFileDialog()
        {
            Filter = "JSON DataBase (*.json)|*.json"
        };
        if (dialog.ShowDialog() is true)
        {
            using var stream = dialog.OpenFile();
            using var writer = new StreamWriter(stream);
            Save(db, writer);
        }
    }

    public void Save(DataBase db, string filename)
    {
        using var file = File.OpenWrite(filename);
        using var writer = new StreamWriter(file);
        Save(db, writer);
    }

    public void Save(DataBase db, StreamWriter stream)
    {
        var json = JsonSerializer.Serialize(db, _serializerOptions);
        stream.Write(json);
        stream.Flush();
    }
}
