// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#pragma warning disable IDE0007 // disable warnings "use var instead explict type". wth c# become like js?
#pragma warning disable IDE0090
#pragma warning disable IDE0044

namespace ContextMenuEditorForWindows.Views
{
    public sealed partial class FileConMenu : Page
    {
        private static readonly string pattern = @"^\{[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}\}$";

        private List<RegistryKey> rkeys = new()
        {
            Registry.ClassesRoot.OpenSubKey("*", true).OpenSubKey("shell", true),
            Registry.ClassesRoot.OpenSubKey("*", true).OpenSubKey("shellex", true).OpenSubKey("ContextMenuHandlers", true)
        };


        private Dictionary<string, string> namePaths = new Dictionary<string, string>();
        // ������ ��������� �������� ����� �������� ����, ��� ����� ��������� ��� ������ ������ �������
        private readonly string[] hiddenKeys = {
            "removeproperties",
            "explore",
            "open", 
            "opennewprocess",
            "opennewwindow", 
            "find", 
            "updateencryptionsettings",
            "updateencryptionsettingswork",
            "cmd",//����������
            "powershell",//����������
            "wsl"//����������
        };
        public FileConMenu()
        {
            this.InitializeComponent();
            foreach (RegistryKey rk in rkeys)
            {
                parseKey(rk);
            }

        }

        private void parseKey(RegistryKey rk)
        {
            if (rk != null)
            {
                foreach (var key in rk.GetSubKeyNames())
                {
                    {
                        var value = rk.OpenSubKey(key).GetValue("");
                        addItem(value, key, rk);
                    }
                }
            }
        }

        private void addItem(object value, string key, RegistryKey root)
        {
            if (value != null) {
                Match m = Regex.Match(value.ToString(), pattern, RegexOptions.IgnoreCase);
                if (m.Success) return;

                bool isEnable = !root.OpenSubKey(key).GetValue("LegacyDisable", false).Equals("");
                if (value.ToString().Contains(".dll") && !hiddenKeys.Contains(key.ToLower()))
                {
                    string path = value.ToString().Split(",")[0];

                    IntPtr handle = NativeMethods.LoadLibrary(path.Replace("@", ""));
                    StringBuilder sb = new StringBuilder(255);
                    NativeMethods.LoadString
                        (
                            handle,
                            (uint)Math.Abs(Int32.Parse(value.ToString().Split(",").Last())),
                            sb,
                            sb.Capacity + 1
                        );
                    NativeMethods.FreeLibrary(handle);

                    string enchancedString = sb.ToString().Split(",")[0].Replace("&", "");
                    ListViewItemTemplate lv = new ListViewItemTemplate
                        (
                            enchancedString.GetHashCode().ToString(),
                            enchancedString,
                            isEnable
                        );

                    namePaths.Add(enchancedString, root.OpenSubKey(key).ToString());
                    RegistryKeys.Items.Add(lv);
                }
                else if (!value.ToString().Contains(".exe") && !hiddenKeys.Contains(key.ToLower()))
                {

                    string enchancedString = value.ToString().Replace("&", "");
                    ListViewItemTemplate lv = new ListViewItemTemplate
                        (
                            enchancedString.GetHashCode().ToString(),
                            enchancedString,
                            isEnable
                        );
                    namePaths.Add(enchancedString, root.OpenSubKey(key).ToString());
                    RegistryKeys.Items.Add(lv);
                }
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
            namePaths.Clear();
            foreach (RegistryKey rk in rkeys)
            {
                parseKey(rk);
            }
        }

        private void CommandBar_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void RegistryKeys_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            string disableValue = "LegacyDisable";
            ToggleSwitch ts = (sender as ToggleSwitch);
            ContentDialog dialog = new ContentDialog();

            string key = namePaths[
                    ((ts.Parent as StackPanel).Children[1] as TextBlock).Text
                ].Replace(@"HKEY_CLASSES_ROOT\", "").Replace(@"\", "\\");
            RegistryKey _rk = Registry.ClassesRoot.OpenSubKey(key, true);

            if (ts != null)
            {
                if (ts.IsOn) { _rk.DeleteValue(disableValue); }
                else { _rk.SetValue(disableValue, "", RegistryValueKind.String); }
            }



            //XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            //dialog.XamlRoot = this.XamlRoot;
            //dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            //dialog.Title = key.ToString();
            //dialog.PrimaryButtonText = "Ok";
            //dialog.DefaultButton = ContentDialogButton.Primary;
            //await dialog.ShowAsync();
        }
    }
}