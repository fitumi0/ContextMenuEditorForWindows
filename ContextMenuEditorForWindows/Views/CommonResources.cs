using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace ContextMenuEditorForWindows.Views;
internal class CommonResources
{
    public static readonly string[] hiddenKeys = {
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


}
