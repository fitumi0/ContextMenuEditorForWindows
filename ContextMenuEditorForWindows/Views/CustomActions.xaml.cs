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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ContextMenuEditorForWindows.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class CustomActions : Page
    {
        RegistryKey directory = Registry.ClassesRoot.OpenSubKey(CommonResources.registryKeysLocations["Directory"], true);
        public CustomActions()
        {
            this.InitializeComponent();

        }

        private async void AddActionShowPopUp(object sender, RoutedEventArgs e)
        {
            PageControl form = new PageControl();
            var dialog = new ContentDialog();
            dialog.Content = form;
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Новый элемент Контекстного меню";
            dialog.PrimaryButtonText = "Создать";
            
            dialog.IsPrimaryButtonEnabled = false; // validate name and command
            dialog.CloseButtonText = "Отмена";
            dialog.PrimaryButtonClick += delegate 
            {
                var title = (form.FindName("TitleBox") as TextBox).Text;
                var command = (form.FindName("CommandBox") as TextBox).Text;

                var location = CommonResources.registryKeysLocations[
                        (form.FindName("LocationCB") as ComboBox).SelectedItem.ToString()
                    ];
                ListViewCustomActionTemplate lv = new ListViewCustomActionTemplate
                (
                    title,
                    false,
                    false,
                    "",
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
                                RegistryKey menuKey = directory.CreateSubKey(keyName, true);
                                menuKey.SetValue("", title);
                                menuKey.CreateSubKey("command", true).SetValue("",
                                    command);
                            }
                            else if (!ts.IsOn)
                            {
                                directory.DeleteSubKeyTree(keyName);
                            }

                        }
                    })
                
                );
                ListOfCustomActions.Items.Add(lv);
                // save to settings

            };
            dialog.DefaultButton = ContentDialogButton.Primary;
            await dialog.ShowAsync();
        }

        private void AddCustomMenuItem()
        {
        
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
        }

        private async void Pass(object sender, RoutedEventArgs e)
        {
            ToggleSwitch ts = (sender as ToggleSwitch);
            if (ts != null)
            {
                string path = string.Format("{0}AppData", AppDomain.CurrentDomain.BaseDirectory);

                if (!ts.IsOn)
                {
                    Settings set = Settings.Load(CommonResources.settingsPath);
                    
                    var dialog = new ContentDialog();
                    dialog.XamlRoot = this.XamlRoot;
                    dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                    dialog.Title = set.Path.Split("\\").Last();
                    dialog.PrimaryButtonText = "Создать";
                    dialog.CloseButtonText = "Отмена";
                    //dialog.PrimaryButtonClick = AddButton_Click;
                    dialog.DefaultButton = ContentDialogButton.Primary;
                    await dialog.ShowAsync();
                }
                else
                {
                    var settings = new Settings { Path = path };

                    // Save the settings to a JSON file
                    settings.Save(string.Format("{0}\\settings.json", path));
                }
            }
            
            //var weatherForecast = new WeatherForecast
            //{
            //    Date = DateTime.Parse("2019-08-01"),
            //    TemperatureCelsius = 25,
            //    Summary = "Hot"
            //};
            //string dir = "AppData";
            //string fileName = "Settings.json";
            //string jsonString = JsonSerializer.Serialize(weatherForecast);
            //if (!Directory.Exists(dir))
            //{
            //    Directory.CreateDirectory(dir);
            //}
            //File.WriteAllText(string.Format("{0}/{1}/{2}", AppDomain.CurrentDomain.BaseDirectory, dir, fileName), jsonString);
        }
        private async void PackToggle(object sender, RoutedEventArgs e)
        {
            string keyName = "PackWithContext";
            ToggleSwitch ts = (sender as ToggleSwitch);
            RegistryKey _rk = directory.OpenSubKey(keyName, true);
            if (ts != null)
            {
                
                if (ts.IsOn)
                {
                    RegistryKey packKey = directory.CreateSubKey(keyName, true);
                    packKey.CreateSubKey("command", true).SetValue("", 
                        // todo : copy tools to some folder. default value or from settings
                        "\"C:\\Applications\\AppData\\Projects\\CSharp\\ContextMenuEditorForWindows\\ContextMenuTools\\bin\\Debug\\net6.0\\ContextMenuTools.exe\" /PackFiles \"%V\"");
                }
                else if (!ts.IsOn)
                {
                    directory.DeleteSubKeyTree(keyName);
                }

            }
            
            //var dialog = new ContentDialog();
            //dialog.XamlRoot = this.XamlRoot;
            //dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            //dialog.Title = "Новый элемент Контекстного меню";
            //dialog.PrimaryButtonText = "Создать";
            //dialog.CloseButtonText = "Отмена";
            ////dialog.PrimaryButtonClick = AddButton_Click;
            //dialog.DefaultButton = ContentDialogButton.Primary;
            //await dialog.ShowAsync();
        }

        private void ListOfCustomActions_Loaded(object sender, RoutedEventArgs e)
        {
            // add toggle with swap to old context menu if windows 11
            ListViewCustomActionTemplate lv = new ListViewCustomActionTemplate
                (
                    "Pack Items To Folder",
                    directory.GetSubKeyNames().Contains("PackWithContext"), // проверка на то, есть ли в реестре уже ключ. если есть (он может быть выключен), проверяем активен ли
                    false,
                    "Directory Background",
                    true,
                    PackToggle
                );
            ListOfCustomActions.Items.Add(lv);
            ListViewCustomActionTemplate l1v = new ListViewCustomActionTemplate
                (
                    "Veryveryveryveryveryveryveryvery long name",
                    false,
                    true,
                    "Drive",
                    true,
                    Pass
                );
            ListOfCustomActions.Items.Add(l1v);

            // TODO: load and render all saved custom actions

        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            //(((sender as MenuFlyoutItem).Parent) as DropDownButton).Content = (sender as MenuFlyoutItem).Text;
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Удалить элемент @ElementName ? ";
            dialog.PrimaryButtonText = "Удалить";
            dialog.CloseButtonText = "Отмена";
            //dialog.PrimaryButtonClick = AddButton_Click;
            dialog.DefaultButton = ContentDialogButton.Close;
            await dialog.ShowAsync();
        }
    }
       
}
