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
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;

namespace ContextMenuEditorForWindows.Views
{
    public sealed partial class FileConMenu : Page
    {
        private readonly RegistryKey _rkClassRoot = Registry.ClassesRoot.OpenSubKey("*", true).OpenSubKey("shell", true);

        public FileConMenu()
        {
            this.InitializeComponent();
            //RegistryKey rk = Registry.ClassesRoot.OpenSubKey(@"*\shell", true);
            if (_rkClassRoot == null) return;
            foreach (var key in _rkClassRoot.GetSubKeyNames())
            {
                RegistryKeys.Items.Add(key);
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
            //_rkClassRoot.CreateSubKey(TitleBox.Text, true, RegistryOptions.None).CreateSubKey("command", true);
            //RegistryKeys.Items.Clear();
            //foreach (var key in _rkClassRoot.GetSubKeyNames())
            //{
            //    RegistryKeys.Items.Add(key);
            //}
            int n = RegistryKeys.Items.Count;
            RegistryKeys.Items.Add(n++);
        }

        private void SaveButton_Click(Object sender, RoutedEventArgs e)
        {

        }

        private void RefreshButton_Click(Object sender, RoutedEventArgs e)
        {
            RegistryKeys.Items.Clear();
            foreach (var key in _rkClassRoot.GetSubKeyNames())
            {
                RegistryKeys.Items.Add(key);
            }
        }

        private void CommandBar_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void RegistryKeys_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // fill right panel fields 
            TitleBox.Text = RegistryKeys.SelectedItem.ToString();
            // open sub key "command" from registry with be able to edit it
            var registryData = "null";
            //CommandBox.Text = registryData;
        }
    }
}