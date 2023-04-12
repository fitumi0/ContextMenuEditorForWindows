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
        
        private async void PackToggle(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Новый элемент Контекстного меню";
            dialog.PrimaryButtonText = "Создать";
            dialog.CloseButtonText = "Отмена";
            //dialog.PrimaryButtonClick = AddButton_Click;
            dialog.DefaultButton = ContentDialogButton.Primary;
            await dialog.ShowAsync();
        }

        private void ListOfCustomActions_Loaded(object sender, RoutedEventArgs e)
        {
            ListViewCustomActionTemplate lv = new ListViewCustomActionTemplate
                (
                    "",
                    "Pack Items To Folder",
                    false,
                    true,
                    PackToggle
                );
            ListOfCustomActions.Items.Add(lv);
        }
    }
       
}
