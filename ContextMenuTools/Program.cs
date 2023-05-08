using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using ContextMenuTools;



class Program
{
    static void Main(string[] args)
    {
        string path = Directory.Exists(args[args.Length - 1]) && args.Length > 1? args[args.Length - 1] : ""; 
        string[] newArgs = new string[args.Length];
        
        if (Directory.Exists(path) && path != "")
        {
            Array.Copy(args, newArgs, args.Length - 1);
        }
        else
        {
            Array.Copy(args, newArgs, args.Length);
        }   

        Dictionary<string, Delegate> validArgs = new Dictionary<string, Delegate>();
        validArgs["/PackFiles"] = Tools.PackFiles;
        validArgs["/EnableOldCM"] = Tools.EnableOldMenu;
        validArgs["/DisableOldCM"] = Tools.DisableOldMenu;
        validArgs["/RestartExplorer"] = Tools.RestartExplorer;
        validArgs["/RGK"] = Tools.RegistryContainsKey;
        validArgs["/?"] = Tools.Help;

        foreach (string arg in newArgs)
        {
            if ((arg.First() != '/' || arg.Length < 2) && arg != path) {
                Console.WriteLine("Неверный аргумент \"{0}\"", arg);
                Console.WriteLine(path == arg);
                return;
            }
            else
            {
                try
                {
                    validArgs[arg].DynamicInvoke(path);
                    return;
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("Неверный аргумент \"{0}\"", arg);
                    return;
                }
            }            
        }
    }
}