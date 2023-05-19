// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Linq;
using ContextMenuEditorForWindows.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using static ContextMenuEditorForWindows.Helpers.NativeMethods;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ContextMenuEditorForWindows.CustomControls
{
    public sealed partial class PageControl : UserControl
    {
        private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();
        private static readonly string[] ReservedNames = new[]
        {
        "CON", "PRN", "AUX", "NUL",
        "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
        "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
    };

        private bool IsValidFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            if (InvalidFileNameChars.Any(c => fileName.Contains(c)))
            {
                return false;
            }

            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if (ReservedNames.Contains(nameWithoutExtension, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
        public PageControl()
        {
            this.InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var dialog = Root.Parent as ContentDialog;
            // Validate the TextBox content
            AppSettings settings = Settings.LoadFromFile<AppSettings>();
            if (!File.Exists(CommandBox.Text))
            {
                dialog.IsPrimaryButtonEnabled = false;
                CommandBox.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x86, 0x1B, 0x2D));
            }
            else
            {
                CommandBox.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x32, 0x32, 0x32));
            }
            if (!IsValidFileName(TitleBox.Text))
            {
                dialog.IsPrimaryButtonEnabled = false;
                TitleBox.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x86, 0x1B, 0x2D));
            }
            else
            {
                TitleBox.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x32, 0x32, 0x32));
            }
            if (File.Exists(CommandBox.Text) && IsValidFileName(TitleBox.Text))
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
