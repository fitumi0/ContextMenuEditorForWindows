// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Claims;
using System.Security.AccessControl;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ContextMenuEditorForWindows.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DirectoryBackgroundConMenu : Page
    {
        RegistrySecurity rs = new RegistrySecurity(); // it is right string for this code
        string currentUserStr = Environment.UserDomainName + "\\" + Environment.UserName;

        private List<RegistryKey> rkeys = new()
        {
            Registry.ClassesRoot.OpenSubKey("Directory", true).OpenSubKey("Background", true).OpenSubKey("shell", true),
            //Registry.ClassesRoot.OpenSubKey("Directory", true).OpenSubKey("Background", true).OpenSubKey("shellex", true).OpenSubKey("ContextMenuHandlers", true)
        };


        private Dictionary<string, string> namePaths = new Dictionary<string, string>();
        
        public DirectoryBackgroundConMenu()
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
                        addItem(key, rk);
                    }
                }
            }
        }

        private void addItem(string key, RegistryKey root)
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
                string key = namePaths[
                    ((ts.Parent as StackPanel).Children[1] as TextBlock).Text
                ].Replace(@"HKEY_CLASSES_ROOT\", "").Replace(@"\", "\\");
                RegistryKey _rk = Registry.ClassesRoot.OpenSubKey(key, true);

                if (ts != null)
                {
                    //RegistrySecurity tempRS = new RegistrySecurity();
                    //tempRS = _rk.GetAccessControl(AccessControlSections.All);
                    //rs.AddAccessRule(new RegistryAccessRule(currentUserStr, RegistryRights.WriteKey | RegistryRights.ReadKey | RegistryRights.Delete | RegistryRights.FullControl, AccessControlType.Allow));

                    if (ts.IsOn)
                    {
                        //_rk.SetAccessControl(rs);
                        _rk.DeleteValue(disableValue);
                        //_rk.SetAccessControl(tempRS);
                    }
                    else
                    {
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
