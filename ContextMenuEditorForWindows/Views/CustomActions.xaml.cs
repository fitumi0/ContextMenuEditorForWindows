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
using ContextMenuEditorForWindows.CustomControls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ContextMenuEditorForWindows.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomActions : Page
    {
        RegistryKey packItems = Registry.ClassesRoot.OpenSubKey("Directory", true).OpenSubKey("Background", true).OpenSubKey("shell", true);
        public CustomActions()
        {
            this.InitializeComponent();

        }

        private async void AddActionShowPopUp(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog();
            dialog.Content = new PageControl();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Новый элемент Контекстного меню";
            dialog.PrimaryButtonText = "Создать";
            dialog.CloseButtonText = "Отмена";
            //dialog.PrimaryButtonClick = AddButton_Click;
            dialog.DefaultButton = ContentDialogButton.Primary;
            await dialog.ShowAsync();
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
        }

        private void Pass(object sender, RoutedEventArgs e)
        {
            ToggleSwitch ts = (sender as ToggleSwitch);
            //((ts.Parent as StackPanel).Children[1] as TextBlock).Text = Properties.Settings.Default.CopyPath;
        }
        private async void PackToggle(object sender, RoutedEventArgs e)
        {
            string keyName = "PackWithContext";
            ToggleSwitch ts = (sender as ToggleSwitch);
            RegistryKey _rk = packItems.OpenSubKey(keyName, true);
            if (ts != null)
            {
                
                if (_rk == null)
                {
                    RegistryKey packKey = packItems.CreateSubKey(keyName, true);
                    packKey.CreateSubKey("command", true).SetValue("", 
                        // todo : copy tools to some folder. default value or from settings
                        "\"C:\\Applications\\AppData\\Projects\\CSharp\\ContextMenuEditorForWindows\\ContextMenuTools\\bin\\Debug\\net6.0\\ContextMenuTools.exe\" /PackFiles \"%V\"");
                }
                else if (!ts.IsOn)
                {
                    packItems.DeleteSubKeyTree(keyName);
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
            ListViewCustomActionTemplate lv = new ListViewCustomActionTemplate
                (
                    "Pack Items To Folder",
                    packItems.GetSubKeyNames().Contains("PackWithContext") ? packItems.OpenSubKey("PackWithContext", true).GetValue("LegacyDisable") == null : false, // проверка на то, есть ли в реестре уже ключ. если есть (он может быть выключен), проверяем активен ли
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
