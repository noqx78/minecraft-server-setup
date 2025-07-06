using System.Runtime.InteropServices;

internal class Program
{

    private static void Main(string[] args)
    {

        Console.Title = "Minecraft Server Setup";
        loader();
    }

    // ------------------------------------------------------
    static char eula()
    {
        Console.WriteLine("By running this server, you agree to the EULA.");
        Console.WriteLine("You can find the EULA at https://www.minecraft.net/en-us/eula");
        char inputChar;
        do
        {
            Console.Write("Y = Yes | N No: ");
            inputChar = char.ToUpper(Console.ReadKey(true).KeyChar);  
        } while (inputChar != 'Y' && inputChar != 'N');

        return inputChar;
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
                        // logic to download
                    } else
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

    static void download(string url)
    {

    }

    // ------------------------------------------------------

    static Spigot[] spigotVersions = new Spigot[]
{
    new Spigot("1.21.5", "https://getbukkit.org/get/cNW08KHVlCEwof2IkXbxXIKeDPbfgMBU")
};

}

public record Vanilla(string Name, string DownloadUrl);
public record Spigot(string Name, string DownloadUrl);
public record Paper(string Name, string DownloadUrl);
public record Fabric(string Name, string DownloadUrl);

public record loader(string name);
