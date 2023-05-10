using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using System.Diagnostics;
using ContextMenuEditorForWindows.Templates;
using Microsoft.UI.Xaml.Shapes;
using Windows.Data.Xml.Dom;

namespace ContextMenuEditorForWindows.Helpers;

public class AppSettings
{
    public string Theme
    {
        get; set;
    }
    public bool HideBuiltInActions
    {
        get; set;
    }
    public List<CustomAction> CustomActions
    {
        get; set;
    }
}
class Settings
{
    private static readonly string filePath = CommonResources.settingsPath;

    public static string Serialize<T>(T data)
    {
        return JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
    }

    public static T Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }

    public static void SaveToFile<T>(T data)
    {
        File.WriteAllText(filePath, Serialize(data));
    }

    public static T LoadFromFile<T>()
    {
        return Deserialize<T>(File.ReadAllText(filePath));
    }
    public static bool SettingFileExists()
    {
        return File.Exists(filePath);
    }
}
