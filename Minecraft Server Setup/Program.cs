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

        foreach (var loader in loaderVar)
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
                    Console.WriteLine($"{i:00} - {version.Name}");
                    i++;
                }

                Console.Write("\nPlease select a version: ");
                string input = Console.ReadLine();
                bool parsed = int.TryParse(input, out int selectedNumber);
                if (parsed && selectedNumber >= 1 && selectedNumber <= spigotVersions.Length)
                {
                    if (eula() == 'Y') {

                        download(spigotVersions[selectedNumber - 1].DownloadUrl, folderPath);
                        string ramArgs = ram();
                        createBat(folderPath, ramArgs);
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

            case "Vanilla":
                i = 1;
                foreach (var version in vanillaVersions)
                {
                    Console.WriteLine($"{i:00} - {version.Name}");
                    i++;
                }

                Console.Write("\nPlease select a version: ");
                input = Console.ReadLine();
                parsed = int.TryParse(input, out selectedNumber);
                if (parsed && selectedNumber >= 1 && selectedNumber <= vanillaVersions.Length)
                {
                    if (eula() == 'Y')
                    {
                        download(vanillaVersions[selectedNumber - 1].DownloadUrl, folderPath);
                        string ramArgs = ram();
                        createBat(folderPath, ramArgs);
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

            case "Paper":
                i = 1;
                foreach (var version in paperVersions)
                {
                    Console.WriteLine($"{i:00} - {version.Name}");
                    i++;
                }

                Console.Write("\nPlease select a version: ");
                input = Console.ReadLine();
                parsed = int.TryParse(input, out selectedNumber);
                if (parsed && selectedNumber >= 1 && selectedNumber <= paperVersions.Length)
                {
                    if (eula() == 'Y')
                    {
                        download(paperVersions[selectedNumber - 1].DownloadUrl, folderPath);
                        string ramArgs = ram();
                        createBat(folderPath, ramArgs);
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

    static string ram()
    {
        Console.Clear();
        string inputMin;
        string inputMax;
        int minRam = 0;
        int maxRam = 0;
        bool validInput;

        do
        {
            Console.Write("Enter the minimum amount of RAM for the server (in GB): ");
            inputMin = Console.ReadLine();

            Console.Write("Enter the maximum amount of RAM for the server (in GB): ");
            inputMax = Console.ReadLine();

            validInput = int.TryParse(inputMin, out minRam) &&
                         int.TryParse(inputMax, out maxRam) &&
                         minRam > 0 &&
                         maxRam >= minRam;

            if (!validInput)
            {
                Console.Clear();
                Console.WriteLine("Invalid input.");
                Console.WriteLine("Please enter whole numbers (like 2, 4, 8) and make sure min RAM ≤ max RAM.\n");
            }

        } while (!validInput);

        Console.Clear();
        Console.WriteLine($"RAM configuration: -Xms{minRam}G -Xmx{maxRam}G");
        return $"-Xms{minRam}G -Xmx{maxRam}G";
    }

    // ------------------------------------------------------


    static void createBat(string folderPath, string ramArgs)
    {
        string batPath = Path.Combine(folderPath, "start.bat");

        string content = $@"@echo off
:loop
java {ramArgs} -jar server.jar nogui
echo Server crashed or stopped. Restarting in 5 seconds...
timeout /t 5
goto loop";

        File.WriteAllText(batPath, content);
        Console.WriteLine($@"Created start.bat at: {batPath}");
    }


    // ------------------------------------------------------


    static Spigot[] spigotVersions = new Spigot[]
    {
    new Spigot("1.21.5", "https://cdn.getbukkit.org/spigot/spigot-1.21.5.jar"),
    new Spigot("1.21.4", "https://cdn.getbukkit.org/spigot/spigot-1.21.4.jar"),
    new Spigot("1.21.3", "https://cdn.getbukkit.org/spigot/spigot-1.21.3.jar"),
    new Spigot("1.21.1", "https://cdn.getbukkit.org/spigot/spigot-1.21.1.jar"),
    new Spigot("1.20.6", "https://cdn.getbukkit.org/spigot/spigot-1.20.6.jar"),
    new Spigot("1.20.4", "https://cdn.getbukkit.org/spigot/spigot-1.20.4.jar"),
    new Spigot("1.20.2", "https://cdn.getbukkit.org/spigot/spigot-1.20.2.jar"),
    new Spigot("1.20.1", "https://cdn.getbukkit.org/spigot/spigot-1.20.1.jar"),
    new Spigot("1.19.4", "https://cdn.getbukkit.org/spigot/spigot-1.19.4.jar"),
    new Spigot("1.19.3", "https://cdn.getbukkit.org/spigot/spigot-1.19.3.jar"),
    new Spigot("1.19.2", "https://cdn.getbukkit.org/spigot/spigot-1.19.2.jar"),
    new Spigot("1.19.1", "https://cdn.getbukkit.org/spigot/spigot-1.19.1.jar"),
    new Spigot("1.19",   "https://cdn.getbukkit.org/spigot/spigot-1.19.jar"),
    new Spigot("1.18.2", "https://cdn.getbukkit.org/spigot/spigot-1.18.2.jar"),
    new Spigot("1.18.1", "https://cdn.getbukkit.org/spigot/spigot-1.18.1.jar"),
    new Spigot("1.18",   "https://cdn.getbukkit.org/spigot/spigot-1.18.jar"),
    new Spigot("1.16.5", "https://cdn.getbukkit.org/spigot/spigot-1.16.5.jar"),
    new Spigot("1.16.4", "https://cdn.getbukkit.org/spigot/spigot-1.16.4.jar"),
    new Spigot("1.16.3", "https://cdn.getbukkit.org/spigot/spigot-1.16.3.jar"),
    new Spigot("1.16.2", "https://cdn.getbukkit.org/spigot/spigot-1.16.2.jar"),
    new Spigot("1.16.1", "https://cdn.getbukkit.org/spigot/spigot-1.16.1.jar"),
    new Spigot("1.15.2", "https://cdn.getbukkit.org/spigot/spigot-1.15.2.jar"),
    new Spigot("1.15.1", "https://cdn.getbukkit.org/spigot/spigot-1.15.1.jar"),
    new Spigot("1.15",   "https://cdn.getbukkit.org/spigot/spigot-1.15.jar"),
    new Spigot("1.14.4", "https://cdn.getbukkit.org/spigot/spigot-1.14.4.jar"),
    new Spigot("1.14.3", "https://cdn.getbukkit.org/spigot/spigot-1.14.3.jar"),
    new Spigot("1.14.2", "https://cdn.getbukkit.org/spigot/spigot-1.14.2.jar"),
    new Spigot("1.14.1", "https://cdn.getbukkit.org/spigot/spigot-1.14.1.jar"),
    new Spigot("1.14",   "https://cdn.getbukkit.org/spigot/spigot-1.14.jar"),
    new Spigot("1.13.2", "https://cdn.getbukkit.org/spigot/spigot-1.13.2.jar"),
    new Spigot("1.13.1", "https://cdn.getbukkit.org/spigot/spigot-1.13.1.jar"),
    new Spigot("1.13",   "https://cdn.getbukkit.org/spigot/spigot-1.13.jar"),
    new Spigot("1.12.2", "https://cdn.getbukkit.org/spigot/spigot-1.12.2.jar"),
    new Spigot("1.12.1", "https://cdn.getbukkit.org/spigot/spigot-1.12.1.jar"),
    new Spigot("1.12",   "https://cdn.getbukkit.org/spigot/spigot-1.12.jar"),
    new Spigot("1.11.2", "https://cdn.getbukkit.org/spigot/spigot-1.11.2.jar"),
    new Spigot("1.11",   "https://cdn.getbukkit.org/spigot/spigot-1.11.jar"),
    new Spigot("1.10.2", "https://cdn.getbukkit.org/spigot/spigot-1.10.2.jar"),
    new Spigot("1.9.4",  "https://cdn.getbukkit.org/spigot/spigot-1.9.4.jar"),
    new Spigot("1.9.2",  "https://cdn.getbukkit.org/spigot/spigot-1.9.2.jar"),
    new Spigot("1.9",    "https://cdn.getbukkit.org/spigot/spigot-1.9.jar"),
    new Spigot("1.8.8",  "https://cdn.getbukkit.org/spigot/spigot-1.8.8.jar"),
    new Spigot("1.8.3",  "https://cdn.getbukkit.org/spigot/spigot-1.8.3.jar"),
    new Spigot("1.8",    "https://cdn.getbukkit.org/spigot/spigot-1.8.jar"),
    };


    static Vanilla[] vanillaVersions = new Vanilla[]
    {
    new Vanilla("1.21.7", "https://piston-data.mojang.com/v1/objects/05e4b48fbc01f0385adb74bcff9751d34552486c/server.jar"),
    new Vanilla("1.21.4", "https://piston-data.mojang.com/v1/objects/4707d00eb834b446575d89a61a11b5d548d8c001/server.jar"),
    new Vanilla("1.21.1", "https://piston-data.mojang.com/v1/objects/59353fb40c36d304f2035d51e7d6e6baa98dc05c/server.jar"),
    new Vanilla("1.19.4", "https://piston-data.mojang.com/v1/objects/8f3112a1049751cc472ec13e397eade5336ca7ae/server.jar"),
    new Vanilla("1.16.5", "https://piston-data.mojang.com/v1/objects/1b557e7b033b583cd9f66746b7a9ab1ec1673ced/server.jar"),
    new Vanilla("1.12.2", "https://piston-data.mojang.com/v1/objects/886945bfb2b978778c3a0288fd7fab09d315b25f/server.jar"),
    new Vanilla("1.8.9", "https://piston-data.mojang.com/v1/objects/b58b2ceb36e01bcd8dbf49c8fb66c55a9f0676cd/server.jar"),
    };

    static Paper[] paperVersions = new Paper[]
    {
    new Paper("1.21.7", "https://api.papermc.io/v2/projects/paper/versions/1.21.7/builds/17/downloads/paper-1.21.7-17.jar"),
    new Paper("1.21.6", "https://api.papermc.io/v2/projects/paper/versions/1.21.6/builds/48/downloads/paper-1.21.6-48.jar"),
    new Paper("1.21.4", "https://api.papermc.io/v2/projects/paper/versions/1.21.4/builds/232/downloads/paper-1.21.4-232.jar"),
    new Paper("1.20.4", "https://api.papermc.io/v2/projects/paper/versions/1.20.4/builds/540/downloads/paper-1.20.4-540.jar"),
    new Paper("1.19.4", "https://api.papermc.io/v2/projects/paper/versions/1.19.4/builds/550/downloads/paper-1.19.4-550.jar"),
    new Paper("1.18.2", "https://api.papermc.io/v2/projects/paper/versions/1.18.2/builds/385/downloads/paper-1.18.2-385.jar"),
    new Paper("1.16.5", "https://api.papermc.io/v2/projects/paper/versions/1.16.5/builds/794/downloads/paper-1.16.5-794.jar"),
    new Paper("1.12.2", "https://api.papermc.io/v2/projects/paper/versions/1.12.2/builds/1620/downloads/paper-1.12.2-1620.jar"),
    };

}

public record Vanilla(string Name, string DownloadUrl);
public record Spigot(string Name, string DownloadUrl);
public record Paper(string Name, string DownloadUrl);
public record Fabric(string Name, string DownloadUrl);

public record loader(string name);
