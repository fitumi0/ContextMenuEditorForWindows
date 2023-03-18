// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ContextMenuEditorForWindows.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class TestPage : Page
{
    private readonly RegistryKey _rkClassRoot = Registry.ClassesRoot.OpenSubKey("CLSID");
    private Dictionary<string, string> CLSIDs = new();
    public TestPage()
    {
        this.InitializeComponent();
        
        if (_rkClassRoot == null) return;

        foreach (var key in _rkClassRoot.GetSubKeyNames())
        {
            RegistryKey tempKey = Registry.ClassesRoot.OpenSubKey("CLSID").OpenSubKey(key);
            CLSIDs.Add(key, tempKey.GetValue("") == null ? "none" : tempKey.GetValue("").ToString());
            TestList.Items.Add(CLSIDs[key]);
        }


    }
}
