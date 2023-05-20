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
using ContextMenuEditorForWindows.Helpers;


namespace ContextMenuEditorForWindows.Views
{
    public sealed partial class SettingsPage : Page
    {
        string directory = string.Format("\"{0}ContextMenuTools.exe\"", AppDomain.CurrentDomain.BaseDirectory);
        static string registryPath = "Software\\Classes\\CLSID";
        static int OS = Environment.OSVersion.Version.Build >= 22000 ? 11 : Environment.OSVersion.Version.Major;

        public bool OldCMIsEnabled = OS == 11;
        // for correct work, build only for your system bit depth.
        // this will not work for x64 system when building x86 and vice versa
        public bool ToggleIsOn = Registry.CurrentUser.OpenSubKey(registryPath).GetSubKeyNames().Contains("{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}");
        private bool _pageLoaded = false;
        public bool BuiltInIsOn = Settings.LoadFromFile<AppSettings>().HideBuiltInActions;


        public SettingsPage()
        {
            this.InitializeComponent();
            
        }

        private void toggled(object sender, RoutedEventArgs e)
        {
            // eliminates unnecessary command invocation when loading
            if (!_pageLoaded)
            {
                return;
            }
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _pageLoaded = true;
        }

        private void DisableBuiltInActions(object sender, RoutedEventArgs e)
        {
            if (!_pageLoaded)
            {
                return;
            }
            AppSettings settings = Settings.LoadFromFile<AppSettings>();
            settings.HideBuiltInActions = !settings.HideBuiltInActions;
            Settings.SaveToFile(settings);
        }

        private void ShowExtraTabs(object sender, RoutedEventArgs e)
        {

        }
    }
}