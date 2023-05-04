using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ContextMenuEditorForWindows.Helpers;
class Settings
{
    public string Path { get; set; }
    //public string SomeItem { get; set; }
    public void Save(string filePath)
    {
        var json = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public static Settings Load(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<Settings>(json);
    }
}
