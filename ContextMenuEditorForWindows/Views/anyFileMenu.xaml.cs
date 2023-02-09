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
    public sealed partial class AnyFileMenu : Page
    {
        private readonly RegistryKey _rkClassRoot = Registry.ClassesRoot.OpenSubKey(@"*\shell", true);

        public AnyFileMenu()
        {
            this.InitializeComponent();
            //RegistryKey rk = Registry.ClassesRoot.OpenSubKey(@"*\shell", true);
            if (_rkClassRoot == null) return;
            foreach (var key in _rkClassRoot.GetSubKeyNames())
            {
                RegistryKeys.Items.Add(key);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (RegistryKeys.SelectedIndex != -1)
            {
                RegistryKeys.Items.RemoveAt(RegistryKeys.SelectedIndex);
            }
            //ContentDialog dialog = new ContentDialog();

            //// XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            //dialog.XamlRoot = this.XamlRoot;
            //dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            //dialog.Title = "Any editing";
            //dialog.PrimaryButtonText = "Save";
            //dialog.SecondaryButtonText = "Don't Save";
            //dialog.CloseButtonText = "Cancel";
            //dialog.DefaultButton = ContentDialogButton.Primary;
            //var res = await dialog.ShowAsync();
            //RegistryKeys.Items.Add(res.ToString());
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _rkClassRoot.CreateSubKey(TitleBox.Text, true, RegistryOptions.None).CreateSubKey("command", true);
            RegistryKeys.Items.Clear();
            foreach (var key in _rkClassRoot.GetSubKeyNames())
            {
                RegistryKeys.Items.Add(key);
            }
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
    }
}