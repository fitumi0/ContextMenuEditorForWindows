// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.AccessControl;
using System.Text.Json;
using ContextMenuEditorForWindows.CustomControls;
using ContextMenuEditorForWindows.Templates;
using ContextMenuEditorForWindows.Helpers;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using Windows.Data.Xml.Dom;
using Windows.ApplicationModel.DataTransfer;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ContextMenuEditorForWindows.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class CustomActions : Page
    {
        // TODO: RENAME
        RegistryKey keyLocation = Registry.ClassesRoot.OpenSubKey(CommonResources.registryKeysLocations["Directory Background"], true);

        public CustomActions()
        {
            this.InitializeComponent();
            AddCustomElements();
        }

        private async void AddActionShowPopUp(object sender, RoutedEventArgs e)
        {
            AppSettings settings = new AppSettings { CustomActions = new() };

            PageControl form = new PageControl();
            var dialog = new ContentDialog();
            dialog.Content = form;
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Новый элемент Контекстного меню";
            dialog.PrimaryButtonText = "Создать";
            
            dialog.IsPrimaryButtonEnabled = false;
            dialog.CloseButtonText = "Отмена";
            dialog.PrimaryButtonClick += (ContentDialog sender, ContentDialogButtonClickEventArgs args) =>
            {
                var title = (form.FindName("TitleBox") as TextBox).Text;
                var command = (form.FindName("CommandBox") as TextBox).Text;

                var location = CommonResources.registryKeysLocations[
                        (form.FindName("LocationCB") as ComboBox).SelectedItem.ToString()
                    ];
                var icon = (form.FindName("IconBox") as TextBox).Text;
                RegistryKey keyLocation = Registry.ClassesRoot.OpenSubKey(CommonResources.registryKeysLocations[(form.FindName("LocationCB") as ComboBox).SelectedItem.ToString()], true);

                ListViewCustomActionTemplate lv = new ListViewCustomActionTemplate
                (
                    title,
                    false,
                    icon,
                    true,
                    true,

                    new RoutedEventHandler((sender, e) =>
                    {
                        string keyName = CommonResources.GetHash(title);
                        ToggleSwitch ts = (sender as ToggleSwitch);
                        RegistryKey _rk = Registry.ClassesRoot.OpenSubKey(location, true).OpenSubKey(keyName, true);
                        if (ts != null)
                        {

                            if (_rk == null)
                            {
                                RegistryKey menuKey = keyLocation.CreateSubKey(keyName, true);
                                menuKey.SetValue("", title);
                                menuKey.SetValue("Icon", icon);
                                menuKey.CreateSubKey("command", true).SetValue("",
                                    string.Format("\"{0}\"",command));
                            }
                            else if (!ts.IsOn)
                            {
                                keyLocation.DeleteSubKeyTree(keyName);
                            }

                        }
                    })

                );
                ListOfCustomActions.Items.Add(lv);
                NothingPlaceholder.Visibility = Visibility.Collapsed;
                CustomAction customAction = new CustomAction(title, command, icon, (form.FindName("LocationCB") as ComboBox).SelectedItem.ToString());
                if (Settings.SettingFileExists())
                {
                    AppSettings loadedSettings = Settings.LoadFromFile<AppSettings>();
                    loadedSettings.CustomActions.Add(customAction);
                    Settings.SaveToFile(loadedSettings);
                }
                else
                {
                    Settings.SaveToFile(new AppSettings { CustomActions = new() { customAction } });
                }
                

            };
            dialog.DefaultButton = ContentDialogButton.Primary;
            await dialog.ShowAsync();
        }

        private void ClearClipboard(object sender, RoutedEventArgs e)
        {
            string keyName = "ClearClip";
            ToggleSwitch ts = (sender as ToggleSwitch);
            RegistryKey _rk = keyLocation.OpenSubKey(keyName, true);
            if (ts != null)
            {
                if (ts.IsOn)
                {
                    RegistryKey packKey = keyLocation.CreateSubKey(keyName, true);
                    packKey.SetValue("", keyName);
                    packKey.CreateSubKey("command", true).SetValue("",
                        string.Format("\"{0}ContextMenuTools.exe\" /ClearClip", AppDomain.CurrentDomain.BaseDirectory));
                }
                else if (!ts.IsOn)
                {
                    keyLocation.DeleteSubKeyTree(keyName);
                }

            }
        }

        private void PackToggle(object sender, RoutedEventArgs e)
        {
            string keyName = "PackWithContext";
            ToggleSwitch ts = (sender as ToggleSwitch);
            RegistryKey _rk = keyLocation.OpenSubKey(keyName, true);
            if (ts != null)
            {
                if (ts.IsOn)
                {
                    RegistryKey packKey = keyLocation.CreateSubKey(keyName, true);
                    packKey.SetValue("", keyName);
                    packKey.CreateSubKey("command", true).SetValue("",
                        string.Format("\"{0}ContextMenuTools.exe\" /PackFiles \"%V\"", AppDomain.CurrentDomain.BaseDirectory));
                }
                else if (!ts.IsOn)
                {
                    keyLocation.DeleteSubKeyTree(keyName);
                }

            }
        }

        private void AddCustomElements()
        {
            AppSettings loadedSettings = Settings.LoadFromFile<AppSettings>();
            if (!loadedSettings.HideBuiltInActions)
            {
                ListViewCustomActionTemplate lv = new ListViewCustomActionTemplate
                (
                    "Pack To Folder",
                    keyLocation.GetSubKeyNames().Contains("PackWithContext"),
                    "",
                    false,
                    false,
                    PackToggle
                );
                ListOfCustomActions.Items.Add(lv);
                ListViewCustomActionTemplate clipboard = new ListViewCustomActionTemplate
                (
                    "Clear Clipboard",
                    keyLocation.GetSubKeyNames().Contains(""),
                    "",
                    false,
                    false,
                    ClearClipboard
                );
                ListOfCustomActions.Items.Add(clipboard);
            }

            foreach ( var item in loadedSettings.CustomActions)
            {
                var title = item.Title;
                var location = item.Location;
                var command = item.Command;
                var icon = item.Icon;
                RegistryKey keyLocation = Registry.ClassesRoot.OpenSubKey(CommonResources.registryKeysLocations[location], true);
                ListViewCustomActionTemplate tmp = new ListViewCustomActionTemplate
                (
                    title,
                    keyLocation.OpenSubKey(CommonResources.GetHash(title)) != null,
                    icon,
                    true,
                    true,

                    new RoutedEventHandler((sender, e) =>
                    {
                        string keyName = CommonResources.GetHash(title);
                        ToggleSwitch ts = (sender as ToggleSwitch);
                        RegistryKey _rk = Registry.ClassesRoot.OpenSubKey(CommonResources.registryKeysLocations[location], true).OpenSubKey(keyName, true);
                        if (ts != null)
                        {
                            if (_rk == null)
                            {
                                RegistryKey menuKey = keyLocation.CreateSubKey(keyName, true);
                                menuKey.SetValue("", title);
                                menuKey.SetValue("Icon", icon);
                                menuKey.CreateSubKey("command", true).SetValue("",
                                    string.Format("\"{0}\"", command));
                            }
                            else if (!ts.IsOn)
                            {
                                keyLocation.DeleteSubKeyTree(keyName);
                            }

                        }
                    })

                );
                ListOfCustomActions.Items.Add(tmp);
            }
            if (ListOfCustomActions.Items.Count == 0)
            {
                NothingPlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                NothingPlaceholder.Visibility = Visibility.Collapsed;
            }
        }


        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            AppSettings appSettings = Settings.LoadFromFile<AppSettings>();
            var editItem = (((((sender as Button).Parent as StackPanel).Parent as Grid).Children[0] as StackPanel).Children[2] as TextBlock).Text;

            PageControl form = new PageControl();
            (form.FindName("TitleBox") as TextBox).Text = appSettings.CustomActions.Find(item => item.Title == editItem).Title;
            (form.FindName("CommandBox") as TextBox).Text = appSettings.CustomActions.Find(item => item.Title == editItem).Command;
            (form.FindName("IconBox") as TextBox).Text = appSettings.CustomActions.Find(item => item.Title == editItem).Icon;
            (form.FindName("LocationCB") as ComboBox).SelectedItem = appSettings.CustomActions.Find(item => item.Title == editItem).Location;
            var dialog = new ContentDialog();
            dialog.Content = form;
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = string.Format("Редактирование элемента {0}", editItem);
            dialog.PrimaryButtonText = "Изменить";
            dialog.CloseButtonText = "Отмена";
            dialog.PrimaryButtonClick += delegate 
            {

                var title = (form.FindName("TitleBox") as TextBox).Text;
                var command = (form.FindName("CommandBox") as TextBox).Text;

                var location = (form.FindName("LocationCB") as ComboBox).SelectedItem.ToString();
                var icon = (form.FindName("IconBox") as TextBox).Text;
                CustomAction item = new CustomAction(title, command, icon, location);
                int index = appSettings.CustomActions.FindIndex(item => item.Title == editItem);
                appSettings.CustomActions[index] = item;
                Settings.SaveToFile(appSettings);

                ListOfCustomActions.Items.Clear();
                AddCustomElements();
            };
            dialog.DefaultButton = ContentDialogButton.Close;
            await dialog.ShowAsync();
        }
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var deleteItem = (((((sender as Button).Parent as StackPanel).Parent as Grid).Children[0] as StackPanel).Children[2] as TextBlock).Text;

            var dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = string.Format("Удалить элемент {0}? ", deleteItem);
            dialog.PrimaryButtonText = "Удалить";
            dialog.CloseButtonText = "Отмена";
            dialog.PrimaryButtonClick += delegate 
            {
                AppSettings appSettings = Settings.LoadFromFile<AppSettings>();

                string keyName = CommonResources.GetHash(deleteItem);
                RegistryKey keyLocation = Registry.ClassesRoot.OpenSubKey(CommonResources.registryKeysLocations[appSettings.CustomActions.Find(item => item.Title == deleteItem).Location], true);
                keyLocation.DeleteSubKeyTree(keyName);

                appSettings.CustomActions.Remove(appSettings.CustomActions.Find(item => item.Title == deleteItem));
                Settings.SaveToFile(appSettings);
                



                for (int i = ListOfCustomActions.Items.Count - 1; i >= 0; i--)
                {
                    if (ListOfCustomActions.Items[i] is ListViewCustomActionTemplate item && item.Text == deleteItem)
                    {
                        ListOfCustomActions.Items.RemoveAt(i);
                    }
                }
                if (ListOfCustomActions.Items.Count == 0)
                {
                    NothingPlaceholder.Visibility = Visibility.Visible;
                }
            };
            dialog.DefaultButton = ContentDialogButton.Close;
            await dialog.ShowAsync();
        }
    }
       
}
