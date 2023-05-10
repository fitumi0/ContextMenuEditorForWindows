using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;

namespace ContextMenuEditorForWindows.Templates;
public class CustomAction
{
    public string Title { get; set; }
    public string Command { get; set; }
    public string Location { get; set; }

    public CustomAction(string title, string command, string location)
    {
        Title = title;
        Command = command;
        Location = location;
    }
}
