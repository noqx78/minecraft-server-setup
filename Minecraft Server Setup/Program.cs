using System.Runtime.InteropServices;
using System.Net;
using System;
using System.IO;
internal class Program
{

    private static string folderPath; 
    private static void Main(string[] args)
    {
        folderPath = createFolder();  
        loader();
    }

    // ------------------------------------------------------
    static char eula()
    {
        Console.Clear();
        Console.WriteLine("By running this server, you agree to the EULA.");
        Console.WriteLine("You can find the EULA at https://www.minecraft.net/en-us/eula");
        Console.Write("Y = Yes | N No: ");

        char inputChar;
        do
        {
            inputChar = char.ToUpper(Console.ReadKey(true).KeyChar);  
        } while (inputChar != 'Y' && inputChar != 'N');

        if (inputChar == 'Y')
        {
            Console.Clear();
            Console.WriteLine("downloading server.jar");
        }

        return inputChar;
    }

    static void createEula(string folderPath)
    {
        string eulaPath = Path.Combine(folderPath, "eula.txt");
        File.WriteAllText(eulaPath, "eula=true");
        Console.WriteLine($@"Created eula.txt with 'eula=true' at: {eulaPath}");
    }

    // ------------------------------------------------------


    static void loader()
    {
        int listNumber = 1;

        var loaderVar = new loader[]
        {
            new("Vanilla"),
            new("Spigot"),
            new("Paper"),
            new("Fabric"),
        };

        foreach (var loader in loaderVar )
        {
            Console.WriteLine($"({listNumber}) {loader.name}");
            listNumber++;
        }

        Console.Write("\nPlease select a loader: ");
        string input = Console.ReadLine();


        bool parsed = int.TryParse(input, out int selectedNumber);

        if (parsed && selectedNumber >= 1 && selectedNumber <= loaderVar.Length)
        {
           // Console.WriteLine($"\n\ndebug: {loaderVar[selectedNumber - 1].name}");
            selectVersion(loaderVar[selectedNumber - 1].name);
        }
        else
        {
            Console.Clear();
            loader();
        }

    }

    // ------------------------------------------------------

    static void selectVersion(string loaderName)
    {
        Console.Clear();
        int i = 1;
        switch (loaderName)
        {
            case "Spigot":
                i = 1;
                foreach (var version in spigotVersions)
                {
                    Console.WriteLine($"{i} - {version.Name}");
                    i++;
                }

                Console.Write("\nPlease select a version: ");
                string input = Console.ReadLine();
                bool parsed = int.TryParse(input, out int selectedNumber);
                if (parsed && selectedNumber >= 1 && selectedNumber <= spigotVersions.Length)
                {
                    if (eula() == 'Y') {
                        download(spigotVersions[selectedNumber - 1].DownloadUrl, folderPath);
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Console.Clear();
                    selectVersion(loaderName);
                }
                break;

            default:
                Console.WriteLine("Unknown loader");
                break;
        }
    }

    // ------------------------------------------------------

static void download(string url, string folderPath)
{
    string filePath = Path.Combine(folderPath, "server.jar");

    using (WebClient client = new WebClient())
    {
        client.DownloadFile(url, filePath);
    }

    Console.WriteLine($@"download server.jar done / {filePath}");
}

    // ------------------------------------------------------

    static string createFolder()
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string baseFolderName = "server";
        string folderPath = Path.Combine(desktopPath, baseFolderName);
        int counter = 1;

        while (Directory.Exists(folderPath))
        {
            folderPath = Path.Combine(desktopPath, $"{baseFolderName}_{counter}");
            counter++;
        }

        Directory.CreateDirectory(folderPath);
        Console.WriteLine($@"Created folder: {folderPath}");
        createEula(folderPath);
        return folderPath;
    }


    // ------------------------------------------------------

    static string readRam()
    {
        Console.Write("Enter the minimum amount of RAM for the server (in GB): ");
        string input = Console.ReadLine();
        Console.Write("Enter the maximum amount of RAM for the server (in GB): ");
        string input2 = Console.ReadLine();

    }

    // ------------------------------------------------------

    static Spigot[] spigotVersions = new Spigot[]
{
    new Spigot("1.21.5", "https://cdn.getbukkit.org/spigot/spigot-1.21.5.jar")
};

}

public record Vanilla(string Name, string DownloadUrl);
public record Spigot(string Name, string DownloadUrl);
public record Paper(string Name, string DownloadUrl);
public record Fabric(string Name, string DownloadUrl);

public record loader(string name);
