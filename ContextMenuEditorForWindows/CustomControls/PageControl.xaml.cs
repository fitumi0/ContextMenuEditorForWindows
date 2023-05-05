// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using ContextMenuEditorForWindows.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ContextMenuEditorForWindows.CustomControls
{
    public sealed partial class PageControl : UserControl
    {
        public PageControl()
        {
            this.InitializeComponent();
        }

        //private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        //{
        //    LocationButton.Content = (sender as MenuFlyoutItem).Text;
        //}

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Get the TextBox and ContentDialog
            var dialog = Root.Parent as ContentDialog;
            // Validate the TextBox content
            if (string.IsNullOrEmpty(TitleBox.Text) || string.IsNullOrEmpty(CommandBox.Text))
            {
                // Disable the primary button if the TextBox is empty
                dialog.IsPrimaryButtonEnabled = false;
            }
            else
            {
                // Enable the primary button if the TextBox is not empty
                dialog.IsPrimaryButtonEnabled = true;
            }
        }

        private void LocationCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dialog = Root.Parent as ContentDialog;
            //dialog.PrimaryButtonClick;
            // change Primary Button action
        }

        private async void PickComandFile(object sender, RoutedEventArgs e)
        {
            // TODO: FIX
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();

        }
    }
}
