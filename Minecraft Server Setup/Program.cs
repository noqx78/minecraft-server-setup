using System.Runtime.InteropServices;

internal class Program
{

    private static void Main(string[] args)
    {
        loader();
    }

    static void eula()
    {
        Console.WriteLine("By running this server, you agree to the EULA.");
        Console.WriteLine("You can find the EULA at https://www.minecraft.net/en-us/eula");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

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
            Console.WriteLine($"\n\ndebug: {loaderVar[selectedNumber - 1].name}");
        }
        else
        {
            Console.Clear();
            loader();
        }

    }

}

public record Vanilla(string Name, string DownloadUrl);
public record Spigot(string Name, string DownloadUrl);
public record Paper(string Name, string DownloadUrl);
public record Fabric(string Name, string DownloadUrl);

public record loader(string name);
