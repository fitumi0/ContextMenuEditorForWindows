using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace ContextMenuEditorForWindows.Views;
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
            "cmd",//отключаемо
            "powershell",//отключаемо
            "wsl"//отключаемо
        };

    public static readonly string regPattern = @"^\{[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}\}$";

    public static RegistryKey CLSID = Registry.ClassesRoot.OpenSubKey("CLSID", true);

    public static readonly Dictionary<byte, string> registryKeysLocations = new () 
    {
        { 0, @"HKEY_CLASSES_ROOT\*\shell" }, // File
        { 1, @"HKEY_CLASSES_ROOT\Directory\shell" }, // Directory
        { 2, @"HKEY_CLASSES_ROOT\Directory\Background\shell" }, // Directory Background
        { 3, @"HKEY_CLASSES_ROOT\DesktopBackground\Shell" }, // Desktop
        { 4, @"HKEY_CLASSES_ROOT\Drive\shell" }, // Drive
    };
}
