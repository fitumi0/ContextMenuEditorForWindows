// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ContextMenuEditorForWindows.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DirectoryBackgroundConMenu : Page
    {
        private List<string> tabs = new List<string>();
            public DirectoryBackgroundConMenu()
            {
                this.InitializeComponent();
                RegistryKey rk = Registry.ClassesRoot.OpenSubKey("Directory", true).OpenSubKey("Background", true);
                if (rk != null)
                {
                    foreach (string key in rk.GetSubKeyNames())
                    {
                        tabs.Add(key);
                    }
            }
        }       

        private void TabViewNav_OnLoaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < tabs.Count; i++)
            {
                TabViewItem newItem = new TabViewItem();
                newItem.Header = $"{tabs[i]}";
                newItem.IsClosable = false;
                Frame frame = new Frame();
                switch (i){
                    case 0:
                        {
                            frame.Navigate(typeof(_DirectoryBackgroundShell));
                            break;
                        }
                    case 1:
                        {
                            frame.Navigate(typeof(_DirectoryBackgroundShellEx));
                            break;
                        }
                }
                newItem.Content = frame;
                newItem.IconSource = new SymbolIconSource() { Symbol = Symbol.Document };
                (sender as TabView).TabItems.Add(newItem);
            }
        }
    }
}
