// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.
using ContextMenuEditorForWindows.CustomControls;
using ContextMenuEditorForWindows.Helpers;
using ContextMenuEditorForWindows.Templates;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

#pragma warning disable IDE0007 // disable warnings "use var instead explict type". wth c# become like js?
#pragma warning disable IDE0090
#pragma warning disable IDE0044

namespace ContextMenuEditorForWindows.Views
{
    public sealed partial class FileConMenu : Page
    {
        private List<RegistryKey> rkeys = new()
        {
            Registry.ClassesRoot.OpenSubKey("*", true).OpenSubKey("shell", true),
            //Registry.ClassesRoot.OpenSubKey("*", true).OpenSubKey("shellex", true).OpenSubKey("ContextMenuHandlers", true)
        };

        //RegistrySecurity rs = new RegistrySecurity(); // it is right string for this code
        string currentUser = Environment.UserDomainName + "\\" + Environment.UserName;

        private Dictionary<string, string> namePaths = new Dictionary<string, string>();
        
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
                AppSettings loadedSettings = Settings.LoadFromFile<AppSettings>();
                foreach (var key in rk.GetSubKeyNames())
                {
                    if (!loadedSettings.CustomActions.Any(item => CommonResources.GetHash(item.Title) == key))
                    {
                        addItem(key, rk);
                    }
                }
            }
        }

        private async void addItem(string key, RegistryKey root)
        {
            object value = root.OpenSubKey(key).GetValue("");
            object muiverb = root.OpenSubKey(key).GetValue("MUIVerb");

            bool isEnable = !root.OpenSubKey(key).GetValue("LegacyDisable", false).Equals("");

            if (!CommonResources.hiddenKeys.Contains(key.ToLower()))
            {
                if (value != null)
                {
                    Match m = Regex.Match(value.ToString(), CommonResources.regPattern, RegexOptions.IgnoreCase);
                    if (m.Success)
                    {
                        {
                            RegistryKey _rk = CommonResources.CLSID.OpenSubKey(value.ToString());
                            ListViewItemTemplate lv = new ListViewItemTemplate
                                (
                                    _rk.GetHashCode().ToString(),
                                    _rk.GetValue("").ToString(),
                                    isEnable
                                );
                            namePaths.Add(_rk.ToString(), root.OpenSubKey(key).ToString());
                            RegistryKeys.Items.Add(lv);
                        }
                    }


                    else if (value.ToString().Contains(".dll"))
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
                    else if (!value.ToString().Contains(".exe"))
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
                if (muiverb != null)
                {
                    ListViewItemTemplate lv = new ListViewItemTemplate
                    (
                        muiverb.GetHashCode().ToString(),
                        muiverb.ToString(),
                        isEnable
                    );
                    namePaths.Add(muiverb.ToString(), root.OpenSubKey(key).ToString());
                    RegistryKeys.Items.Add(lv);
                }
            }
        }

        //private void ClosePopupClicked(object sender, RoutedEventArgs e)
        //{
        //    // if the Popup is open, then close it 
        //    if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        //}

        //// Handles the Click event on the Button on the page and opens the Popup. 
        //private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        //{
        //    // open the Popup if it isn't open already 
        //    if (!StandardPopup.IsOpen) { StandardPopup.IsOpen = true; }
        //}

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

            try
            {
                // Refactor this shyte (set name to TextBlock and pass text)
                string key = namePaths[
                    ((ts.Parent as StackPanel).Children[1] as TextBlock).Text
                ].Replace(@"HKEY_CLASSES_ROOT\", "").Replace(@"\", "\\");
                RegistryKey _rk = Registry.ClassesRoot.OpenSubKey(key, true);

                if (ts != null)
                {
                    if (ts.IsOn) { _rk.DeleteValue(disableValue); }
                    else
                    {

                        //rs.AddAccessRule(new RegistryAccessRule(currentUser, RegistryRights.WriteKey | RegistryRights.ReadKey | RegistryRights.Delete | RegistryRights.FullControl, AccessControlType.Allow));

                        //RegistrySecurity tempRS = new RegistrySecurity();
                        //tempRS = _rk.GetAccessControl(AccessControlSections.All);
                        //_rk.SetAccessControl(rs);
                        _rk.SetValue(disableValue, "", RegistryValueKind.String);
                        //_rk.SetAccessControl(tempRS);
                    }
                }
            }
            catch { }
            

        }
    }
}