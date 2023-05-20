using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace ContextMenuEditorForWindows.Helpers;
internal class CommonResources
{
    public static readonly string[] hiddenKeys =
        {
            "removeproperties",
            "explore",
            "open",
            "opennewprocess",
            "opennewwindow",
            "find",
            "updateencryptionsettings",
            "updateencryptionsettingswork",
            "cmd",
            "powershell",
            "wsl"
        };

    public static string settingsPath = string.Format("{0}AppData\\settings.json", AppDomain.CurrentDomain.BaseDirectory);

    public static readonly string regPattern = @"^\{[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}\}$";

    public static RegistryKey CLSID = Registry.ClassesRoot.OpenSubKey("CLSID", true);

    public static readonly Dictionary<string, string> registryKeysLocations = new()
    {
        { "File", @"*\shell" }, 
        { "Directory", @"Directory\shell" }, 
        { "Directory Background", @"Directory\Background\shell" }, 
        { "Desktop", @"DesktopBackground\Shell" },
        { "Drive", @"Drive\shell" },
    };
    public static string GetHash(string input)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                builder.Append(Convert.ToChar(bytes[i] % 26 + 65)); // A-Z
            }
            for (int i = 6; i < 10; i++)
            {
                int value = bytes[i] % 36;
                if (value >= 10) value += 7; // exclude ASCII 58-64
                builder.Append(Convert.ToChar(value + 48)); // 0-9, A-Z
            }
            return builder.ToString();
        }
    }
}
