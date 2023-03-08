// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

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
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ContextMenuEditorForWindows.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class _DirectoryBackgroundShellEx : Page
    {
        public _DirectoryBackgroundShellEx()
        {
            this.InitializeComponent();
            
            RegistryKey rk = Registry.ClassesRoot.OpenSubKey("Directory", true).OpenSubKey("Background", true).OpenSubKey("shellex");
            if (rk != null)
            {
                foreach (string key in rk.GetSubKeyNames())
                {
                    // todo: recursion get all child nodes for every key and build treeview
                    TreeViewNode rootNode = getAllSubNodes(rk.OpenSubKey(key));
                    RegistryKeys.RootNodes.Add(rootNode);
                }
            }

        }

        private TreeViewNode getAllSubNodes(RegistryKey rk)
        {
            TreeViewNode rootNode = new TreeViewNode() { Content = rk.ToString().Split(@"\").Last() };
            string[] names = rk.GetSubKeyNames();
            foreach (string name in names)
            {
                rootNode.Children.Add(getAllSubNodes(rk.OpenSubKey(name)));
            }
            return rootNode;
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void RegistryKeys_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (RegistryKeys.SelectedItem != null)
            {
                TitleBox.Text = RegistryKeys.SelectedNode.Content.ToString();
            }
        }
    }
}
