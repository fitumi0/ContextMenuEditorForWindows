// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using ContextMenuEditorForWindows.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ContextMenuEditorForWindows;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

        appWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 540, Height = 800 });
        var OS = Environment.OSVersion.Version.Build >= 22000 ? 11 : Environment.OSVersion.Version.Major;
        Title = "Context Menu Editor v0.2 for Windows " + OS.ToString();
        this.InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        
        NavView.SelectedItem = NavView.MenuItems.OfType<NavigationViewItem>().First();
        // change to welcome page (which can open from top menu later)
        ContentFrame.Navigate(
                   typeof(FileConMenu),
                   null,
                   new DrillInNavigationTransitionInfo()
                   );


    }


    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        FrameNavigationOptions navOptions = new FrameNavigationOptions
        {
            TransitionInfoOverride = new DrillInNavigationTransitionInfo()
        };
        if (sender.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
        {
            navOptions.IsNavigationStackEnabled = false;
        }
        
        if (args.IsSettingsInvoked)
        {
            ContentFrame.NavigateToType(typeof(SettingsPage), null, navOptions);
        }
        else if (args.InvokedItemContainer != null && (args.InvokedItemContainer.Tag != null))
        {
            Type newPage = Type.GetType(args.InvokedItemContainer.Tag.ToString()!);
            ContentFrame.Navigate(
                   newPage,
                   null,
                   args.RecommendedNavigationTransitionInfo
                   );
            ContentFrame.NavigateToType(newPage, null, navOptions);
        }

        
    }

    private void NavView_Loaded(object sender, RoutedEventArgs e)
    {

    }
}
