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
            
            dialog.IsPrimaryButtonEnabled = false; // validate name and command
            dialog.CloseButtonText = "Отмена";
            dialog.PrimaryButtonClick += (ContentDialog sender, ContentDialogButtonClickEventArgs args) =>
            {
                var title = (form.FindName("TitleBox") as TextBox).Text;
                var command = (form.FindName("CommandBox") as TextBox).Text;

                var location = CommonResources.registryKeysLocations[
                        (form.FindName("LocationCB") as ComboBox).SelectedItem.ToString()
                    ];
                RegistryKey keyLocation = Registry.ClassesRoot.OpenSubKey(CommonResources.registryKeysLocations[(form.FindName("LocationCB") as ComboBox).SelectedItem.ToString()], true);

                ListViewCustomActionTemplate lv = new ListViewCustomActionTemplate
                (
                    title,
                    false,
                    false,
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

                CustomAction customAction = new CustomAction(title, command, location);
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

        private void ListOfCustomActions_Loaded(object sender, RoutedEventArgs e)
        {
            ListViewCustomActionTemplate lv = new ListViewCustomActionTemplate
                (
                    "Pack To Folder",
                    keyLocation.GetSubKeyNames().Contains("PackWithContext"),
                    false,
                    false,
                    PackToggle
                );
            ListOfCustomActions.Items.Add(lv);

            AppSettings loadedSettings = Settings.LoadFromFile<AppSettings>();
            foreach ( var item in loadedSettings.CustomActions)
            {
                var title = item.Title;
                // TODO: LOCATIONS AS DICTIONARY
                var location = item.Location;
                var command = item.Command;
                RegistryKey keyLocation = Registry.ClassesRoot.OpenSubKey(CommonResources.registryKeysLocations[/*NOW HERE IS VALUE FROM DICT*/location], true);

                ListViewCustomActionTemplate tmp = new ListViewCustomActionTemplate
                (
                    title,
                    false,
                    false,
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

        }


        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            PageControl form = new PageControl();
            var dialog = new ContentDialog();
            dialog.Content = form;
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = string.Format("Редактировать элемент {0}?", "");
            dialog.PrimaryButtonText = "Изменить";
            dialog.CloseButtonText = "Отмена";
            //dialog.PrimaryButtonClick = AddButton_Click;
            dialog.DefaultButton = ContentDialogButton.Close;
            await dialog.ShowAsync();
        }
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Удалить элемент @ElementName ? ";
            dialog.PrimaryButtonText = "Удалить";
            dialog.CloseButtonText = "Отмена";
            dialog.PrimaryButtonClick += delegate 
            {
                AppSettings appSettings = Settings.LoadFromFile<AppSettings>();
                var deleteItem = (((((sender as Button).Parent as StackPanel).Parent as Grid).Children[0] as StackPanel).Children[2] as TextBlock).Text;
                appSettings.CustomActions.Remove(appSettings.CustomActions.Find(item => item.Title == deleteItem));
                Settings.SaveToFile(appSettings);
                for (int i = ListOfCustomActions.Items.Count - 1; i >= 0; i--)
                {
                    if (ListOfCustomActions.Items[i] is ListViewCustomActionTemplate item && item.Text == deleteItem)
                    {
                        ListOfCustomActions.Items.RemoveAt(i);
                    }
                }
            };
            dialog.DefaultButton = ContentDialogButton.Close;
            await dialog.ShowAsync();
        }
    }
       
}
