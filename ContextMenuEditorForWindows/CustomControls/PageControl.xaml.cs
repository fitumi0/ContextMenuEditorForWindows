// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using static ContextMenuEditorForWindows.Helpers.NativeMethods;

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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var dialog = Root.Parent as ContentDialog;
            // Validate the TextBox content
            if (string.IsNullOrEmpty(TitleBox.Text) || string.IsNullOrEmpty(CommandBox.Text))
            {
                dialog.IsPrimaryButtonEnabled = false;
            }
            else
            {
                dialog.IsPrimaryButtonEnabled = true;
            }
        }

        private void PickComandFile(object sender, RoutedEventArgs e)
        {
            CommandBox.Text = OpenFileDialog("*");
        }

        private void PickIcon(object sender, RoutedEventArgs e)
        {
            IconBox.Text = OpenFileDialog("ico");
        }
    }
}
