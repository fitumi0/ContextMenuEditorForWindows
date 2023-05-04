// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using ContextMenuEditorForWindows.Helpers;
using ContextMenuEditorForWindows.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using Windows.ApplicationModel.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ContextMenuEditorForWindows;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
/// 
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

        appWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 860, Height = 600 });
        var OS = Environment.OSVersion.Version.Build >= 22000 ? 11 : Environment.OSVersion.Version.Major;
        var requreAdmin = (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
             .IsInRole(WindowsBuiltInRole.Administrator) ? "" : "(Required Admin Rights)";
        
        Title = string.Format("Context Menu Editor v0.2 for Windows {0} {1}", OS.ToString(), requreAdmin);
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


        // check folder from settings cantains moved tools file
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
