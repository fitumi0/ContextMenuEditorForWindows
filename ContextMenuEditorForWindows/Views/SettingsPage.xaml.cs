// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using ContextMenuEditorForWindows.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUICommunity;
using ContextMenuTools;
using Windows.Devices.Geolocation;


namespace ContextMenuEditorForWindows.Views
{
    public sealed partial class SettingsPage : Page
    {
        string directory = "\"C:\\Applications\\AppData\\Projects\\CSharp\\ContextMenuEditorForWindowsLatest\\ContextMenuTools\\bin\\Debug\\net6.0\\ContextMenuTools.exe\"";
        string registryPath = "Software\\Classes\\CLSID";

        public SettingsPage()
        {
            this.InitializeComponent();
            var OS = Environment.OSVersion.Version.Build >= 22000 ? 11 : Environment.OSVersion.Version.Major;
            if (OS == 11)
            {
                SettingsCard settingsCard = new SettingsCard();
                settingsCard.Header = "Classic context menu";
                settingsCard.Description = "Switch New Windows 11 menu to classic.";

                settingsCard.Content = new ToggleSwitch
                {
                    OnContent = "Classic",
                    OffContent = "New",

                };
                (settingsCard.Content as ToggleSwitch).Toggled += toggled;
                // Fix Toggle
                (settingsCard.Content as ToggleSwitch).IsOn = Registry.CurrentUser.OpenSubKey(registryPath).GetSubKeyNames().Contains("{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}");
                SettingsItems.Children.Insert(1, settingsCard);
            }
        }

        private void toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch ts = (sender as ToggleSwitch);
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = string.Format(!ts.IsOn ? "/C {0} /DisableOldCM" : "/C {0} /EnableOldCM", directory),
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
            };
            Process P = new Process { StartInfo = startInfo };
            P.Start();
            P.WaitForExit();
        }

    }
}