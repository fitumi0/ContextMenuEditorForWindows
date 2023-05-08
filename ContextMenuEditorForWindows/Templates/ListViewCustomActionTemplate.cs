﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ContextMenuEditorForWindows.Templates;
public class ListViewCustomActionTemplate
{
    public string Text
    {
        get; set;
    }
    public string SwitchIsOn
    {
        get; set;
    }
    public string DropDownVisible
    {
        get; set;
    }
    //public string DropDownName
    //{
    //    get; set;
    //}
    public string DefaultMenu
    {
        get; set;
    }
    public string EditButtonVisible
    {
        get; set;
    }
    public string DeleteButtonVisible
    {
        get; set;
    }
    public RoutedEventHandler ToggledFunc
    {
        get; set;
    }

    public ListViewCustomActionTemplate(string lvitem, bool switchIsOn, bool editBtnVisible, bool delBtnVisible, RoutedEventHandler toggledFunc)
    {
        Text = lvitem;
        SwitchIsOn = switchIsOn ? "True" : "False";
        EditButtonVisible = editBtnVisible ? "True" : "False";
        DeleteButtonVisible = delBtnVisible ? "True" : "False";
        ToggledFunc = toggledFunc;
    }

}
