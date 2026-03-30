using System;
using System.IO;
using System.Text.Json;
using FastShot.Models;

namespace FastShot.Services;

public class ConfigService
{
    private readonly string _path;
    public Config Config { get; private set; } = new();

    public ConfigService()
    {
        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FastShot");
        Directory.CreateDirectory(dir);
        _path = Path.Combine(dir, "settings.json");
        Load();
    }

    public void Load()
    {
        if (!File.Exists(_path)) { Save(); return; }
        try
        {
            var json = File.ReadAllText(_path);
            Config = JsonSerializer.Deserialize<Config>(json) ?? new Config();
        }
        catch { Config = new Config(); }
    }

    public void Save()
    {
        var json = JsonSerializer.Serialize(Config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_path, json);
    }
}
