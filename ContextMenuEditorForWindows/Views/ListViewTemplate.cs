using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace ContextMenuEditorForWindows.Views;
public class ListViewItemTemplate
{
    public string Toggle
    {
        get; set;
    }
    public string Text
    {
        get; set;
    }
    // add toggle state
    public ListViewItemTemplate(string toggle, string lvitem)
    {
        Toggle = toggle;
        Text = lvitem;
    }

}
