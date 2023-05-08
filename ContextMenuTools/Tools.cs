using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace ContextMenuTools;
public class Tools
{
    public static void PackFiles(string path)
    {
        string newFolder = Path.Combine(path, "PackedWithContextMenu");
        Directory.CreateDirectory(newFolder);

        var sourcePath = Directory.GetDirectories(path);
        foreach (string folder in sourcePath.Except(new string[] { newFolder }))
        {
            string subFolder = Path.Combine(newFolder, folder.Split("\\").Last());
            Directory.Move(folder, subFolder);
        }

        foreach (string file in Directory.GetFiles(path))
        {
            File.Move(file, Path.Combine(newFolder, file.Split("\\").Last()));
        }
    }

    public static void Help(string _)
    {
        Console.WriteLine(
                "\n" + 
                " # # # This is help \n\n"+
                "/? - show this menu \n" + 
                "/PackFiles - pack some files into one directory \n" +
                "/Test - Test Arg \n"
            );
    }

    public static bool RegistryContainsKey(string registryPath)
    {
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C reg query \"{registryPath}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return !output.Contains("Error");
    }

    public static void EnableOldMenu(string _)
    {
        string command = "reg add \"HKCU\\Software\\Classes\\CLSID\\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\\InprocServer32\" /f";
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c {command}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = processStartInfo })
        {
            process.Start();
            process.WaitForExit();
        }
        RestartExplorer("");
    }

    public static void DisableOldMenu(string _)
    {
        string command = "reg delete \"HKCU\\Software\\Classes\\CLSID\\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\" /f";
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c {command}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = processStartInfo })
        {
            process.Start();
            process.WaitForExit();
        }
        RestartExplorer("");
    }

    public static void RestartExplorer(string _)
    {
        // Kill the explorer.exe process
        string killCmd = "/c taskkill /f /im explorer.exe";
        Process.Start("cmd.exe", killCmd);

        // Wait for a short period to ensure the process is terminated
        Thread.Sleep(100);

        // Start a new explorer.exe process
        Process.Start("explorer.exe");
    }
}
