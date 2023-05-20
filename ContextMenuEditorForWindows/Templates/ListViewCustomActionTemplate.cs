using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;

namespace ContextMenuEditorForWindows.Templates;
public class ListViewCustomActionTemplate
{
    public string Text
    {
        get; set;
    }
    public string SwitchIsOn
    {
        get; set;
    }
    public string Icon
    {
        get; set;
    }
    public string EditButtonVisible
    {
        get; set;
    }
    public string DeleteButtonVisible
    {
        get; set;
    }
    public RoutedEventHandler ToggledFunc
    {
        get; set;
    }

    public ListViewCustomActionTemplate(string lvitem, bool switchIsOn, string icon, bool editBtnVisible, bool delBtnVisible, RoutedEventHandler toggledFunc)
    {
        Text = lvitem;
        SwitchIsOn = switchIsOn ? "True" : "False";
        Icon = icon == "" ? Path.Combine(Environment.CurrentDirectory, "Assets", "None.png") : icon;
        EditButtonVisible = editBtnVisible ? "True" : "False";
        DeleteButtonVisible = delBtnVisible ? "True" : "False";
        ToggledFunc = toggledFunc;
    }

}
