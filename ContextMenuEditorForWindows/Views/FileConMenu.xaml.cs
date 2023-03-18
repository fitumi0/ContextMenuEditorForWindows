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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;

namespace ContextMenuEditorForWindows.Views
{
    public sealed partial class FileConMenu : Page
    {
        private readonly RegistryKey _rkClassRoot = Registry.ClassesRoot.OpenSubKey("Directory", true).OpenSubKey("Background", true).OpenSubKey("shell", true);
        // add to hiddenKeys some keys which contains string parameter "legacydisable"
        private readonly string[] hiddenKeys = {
            "removeproperties",
            "explore",
            "open", 
            "opennewprocess",
            "opennewwindow", 
            "find", 
            "updateencryptionsettings", 
            "cmd"
        };
        public FileConMenu()
        {
            this.InitializeComponent();
            //RegistryKey rk = Registry.ClassesRoot.OpenSubKey(@"*\shell", true);
            if (_rkClassRoot == null) return;
            foreach (var key in _rkClassRoot.GetSubKeyNames())
            {
                {
                    var value = _rkClassRoot.OpenSubKey(key).GetValue("");
                    // добавить проверку на каскадное меню
                    addItem(value, key);
                }
            }
        }

        private void addItem(object value, string key)
        {
            if (value != null && value.ToString().Contains(".dll"))
            {
                IntPtr handle = NativeMethods.LoadLibrary(value.ToString().Split(",")[0].Replace("@", ""));
                StringBuilder sb = new StringBuilder(255);
                NativeMethods.LoadString(handle, (uint)Math.Abs(Int32.Parse(value.ToString().Split(",").Last())), sb, sb.Capacity + 1);
                NativeMethods.FreeLibrary(handle);
                string enchancedString = sb.ToString().Split(",")[0].Replace("&", "");
                ListViewItemTemplate lv = new ListViewItemTemplate(enchancedString.GetHashCode().ToString(), enchancedString);
                RegistryKeys.Items.Add(lv);
            }
            else if (value != null && !value.ToString().Contains(".exe"))
            {
                string enchancedString = _rkClassRoot.OpenSubKey(key).GetValue("").ToString().Replace("&", "");
                ListViewItemTemplate lv = new ListViewItemTemplate(enchancedString.GetHashCode().ToString(), enchancedString);
                RegistryKeys.Items.Add(lv);
            }
            else if (!hiddenKeys.Contains(key.ToLower()))
            {
                ListViewItemTemplate lv = new ListViewItemTemplate(key.GetHashCode().ToString(), key);

                RegistryKeys.Items.Add(lv);
            }
        }

        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (RegistryKeys.SelectedItem.ToString() == "removeproperties" ||
                RegistryKeys.SelectedItem.ToString() == "UpdateEncryptionSettingsWork")
            {
                ContentDialog dialog = new ContentDialog();

                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                dialog.XamlRoot = this.XamlRoot;
                dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                dialog.Title = "Access denied!";
                dialog.PrimaryButtonText = "Ok";
                dialog.DefaultButton = ContentDialogButton.Primary;
                await dialog.ShowAsync();
            }
            else if (RegistryKeys.SelectedIndex != -1)
            {
                RegistryKeys.Items.RemoveAt(RegistryKeys.SelectedIndex);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(Object sender, RoutedEventArgs e)
        {

        }

        private void RefreshButton_Click(Object sender, RoutedEventArgs e)
        {
            RegistryKeys.Items.Clear();
            foreach (var key in _rkClassRoot.GetSubKeyNames())
            {
                {
                    var value = _rkClassRoot.OpenSubKey(key).GetValue("");
                    addItem(value, key);
                }
            }
        }

        private void CommandBar_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void RegistryKeys_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // fill right panel fields 
            //TitleBox.Text = RegistryKeys.SelectedItem.ToString();
            // open sub key "command" from registry with be able to edit it
            var registryData = "null";
            //CommandBox.Text = registryData;
        }

        private async void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch ts = (sender as ToggleSwitch);
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = ((ts.Parent as StackPanel).Children[1] as TextBlock).Text ;
            dialog.PrimaryButtonText = "Ok";
            dialog.DefaultButton = ContentDialogButton.Primary;
            await dialog.ShowAsync();
        }
    }
}