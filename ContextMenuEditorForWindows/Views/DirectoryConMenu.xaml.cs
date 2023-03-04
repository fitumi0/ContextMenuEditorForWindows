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
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public sealed partial class DirMenu : Page
    {
        public DirMenu()
        {
            this.InitializeComponent();
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Classes\Directory\shell", true);
            if (rk != null)
            {
                foreach (string key in rk.GetSubKeyNames())
                {
                    RegistryKeysDir.Items.Add(key);
                }
            }
            
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (RegistryKeysDir.SelectedIndex != -1)
            {
                RegistryKeysDir.Items.RemoveAt(RegistryKeysDir.SelectedIndex);
            }
            //ContentDialog dialog = new ContentDialog();

            //// XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            //dialog.XamlRoot = this.XamlRoot;
            //dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            //dialog.Title = "Any editing";
            //dialog.PrimaryButtonText = "Save";
            //dialog.SecondaryButtonText = "Don't Save";
            //dialog.CloseButtonText = "Cancel";
            //dialog.DefaultButton = ContentDialogButton.Primary;
            //var res = await dialog.ShowAsync();
            //RegistryKeys.Items.Add(res.ToString());
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int n = RegistryKeysDir.Items.Count;
            RegistryKeysDir.Items.Add(n++);
        }
    }
}
