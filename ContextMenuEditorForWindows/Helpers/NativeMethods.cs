using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.UI.Xaml.Controls;

namespace ContextMenuEditorForWindows.Helpers;
static class NativeMethods
{
    const int SW_HIDE = 0;
    const int SW_SHOW = 5;

    [DllImport("kernel32.dll")]
    public static extern IntPtr LoadLibrary(string dllToLoad);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

    [DllImport("kernel32.dll")]
    public static extern bool FreeLibrary(IntPtr hModule);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int LoadString(IntPtr hInstance, uint uID, StringBuilder lpBuffer, int nBufferMax);

    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("comdlg32.dll", CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName(ref OPENFILENAME lpofn);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct OPENFILENAME
    {
        public int lStructSize;
        public IntPtr hwndOwner;
        public IntPtr hInstance;
        public string lpstrFilter;
        public string lpstrCustomFilter;
        public int nMaxCustFilter;
        public int nFilterIndex;
        public string lpstrFile;
        public int nMaxFile;
        public string lpstrFileTitle;
        public int nMaxFileTitle;
        public string lpstrInitialDir;
        public string lpstrTitle;
        public int Flags;
        public short nFileOffset;
        public short nFileExtension;
        public string lpstrDefExt;
        public IntPtr lCustData;
        public IntPtr lpfnHook;
        public string lpTemplateName;
        public IntPtr pvReserved;
        public int dwReserved;
        public int FlagsEx;
    }

    private static Dictionary<string, string> FILETYPES = new ()
    {
        { "txt", "Text Files (*.txt)\0*.txt\0All Files (*.*)\0*.*\0" },
        { "*", "All Files (*.*)\0*.*\0"},
        { "ico", "Icons (*.ico)\0*.ico\0"},
        { "exe", "Executable Files (*.exe)\0*.exe\0"},

    };

    public static string OpenFileDialog(string fileType)
    {
        var ofn = new OPENFILENAME();
        ofn.lStructSize = Marshal.SizeOf(ofn);
        ofn.lpstrFilter = FILETYPES[fileType];
        ofn.lpstrFile = new string('\0', 260);
        ofn.nMaxFile = ofn.lpstrFile.Length;
        ofn.lpstrFileTitle = new string('\0', 260);
        ofn.nMaxFileTitle = ofn.lpstrFileTitle.Length;
        ofn.lpstrInitialDir = "C:\\";
        ofn.lpstrTitle = "Open File";
        ofn.Flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

        if (GetOpenFileName(ref ofn))
        {
            // The user selected a file, do something with it
            return ofn.lpstrFile;
        }
        return "";
    }
}
