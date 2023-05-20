# ContextMenuEditorForWindows
Simple context menu (Right-click) editor for Win 10\11. 03.02.2023

TODO: Windows 10 icons not displaying correctly. Change the icon format, or replace it with SVG

## Custom actions
You have the ability to add your own (custom) actions to the context menu, for example, some program, as well as attach an icon to it.
In the editing tab of custom actions, you can see the icon and name. 
TODO: add some information, for example, to which context menu the action belongs[1]. as well as sorting

[1] now it can be viewed/changed by clicking on the button with a pencil

![Custom Actions Preview](https://github.com/fitumi0/ContextMenuEditorForWindows/blob/master/Screenshots/Custom%20Actions.png "Custom Actions Preview")

## Add actions

![Add action](https://github.com/fitumi0/ContextMenuEditorForWindows/blob/master/Screenshots/New%20Element.png)

## Settings
For Windows 11: Ability to switch to the "Old" context menu. I like the term "classic".
For lower versions this item is not available.

The ability to enable / disable custom actions supplied with the application (now there are only 2 of them, attention is not focused on them. just let them be)

TODO: Add drive and desktop page (which most people don't need, but could also potentially have different processing logic)
![Settings](https://github.com/fitumi0/ContextMenuEditorForWindows/blob/master/Screenshots/Settings.png "Settings")

Settings are stored in JSON format. For the example above, the settings are as follows:
```JSON
{
  "HideBuiltInActions": true,
  "CustomActions": [
    {
      "Title": "Open Context Menu Editor",
      "Command": "C:\\Applications\\AppData\\Projects\\CSharp\\ContextMenuEditorForWindowsLatest\\ContextMenuEditorForWindows\\bin\\x64\\Debug\\net6.0-windows10.0.19041.0\\ContextMenuEditorForWindows.exe",
      "Icon": "C:\\Applications\\AppData\\Projects\\CSharp\\ContextMenuEditorForWindowsLatest\\ContextMenuEditorForWindows\\Assets\\EditMenuW.ico",
      "Location": "Directory Background"
    },
    {
      "Title": "Open Photoshop",
      "Command": "C:\\Applications\\Adobe Photoshop 2022\\Photoshop.exe",
      "Icon": "",
      "Location": "Directory Background"
    },
    {
      "Title": "Open Telegram",
      "Command": "C:\\Applications\\Telegram Desktop\\Telegram.exe",
      "Icon": "",
      "Location": "Desktop"
    }
  ]
}
```


