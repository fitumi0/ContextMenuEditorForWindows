using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ContextMenuEditorForWindows.Views;
public class ListViewCustomActionTemplate
{
    public string Toggle
    {
        get; set;
    }
    public string Text
    {
        get; set;
    }
    public string SwitchIsOn
    {
        get; set;
    }
    public string ButtonVisible
    {
        get; set;
    }
    public RoutedEventHandler ToggledFunc
    {
        get; set;
    }

    public ListViewCustomActionTemplate(string toggle, string lvitem, bool switchIsOn, bool btnVisible, RoutedEventHandler toggledFunc)
    {
        Toggle = toggle;
        Text = lvitem;
        SwitchIsOn = switchIsOn ? "True" : "False";
        ButtonVisible = btnVisible ? "Visible" : "Collapsed";
        ToggledFunc = toggledFunc;
    }

}
