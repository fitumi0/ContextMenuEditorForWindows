using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenuTools;
class Tools
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
}
