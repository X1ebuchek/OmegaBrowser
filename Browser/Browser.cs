using System.Collections;
using Browser.Management;
using Browser.Management.TabCommands;
using Browser.Networking;
using HtmlAgilityPack;

namespace Browser;

public class Browser //todo tab manager
{

    public ResourceManager resourceManager { get; private set; }
    private List<Command> TabCommands; //todo command manager
    public Tab currentTab { get; private set; }

    public struct BrowserOptions
    {
        public string baseDirectory { get; init; }
    }

    public Browser(BrowserOptions options)
    {

        var baseDirectory = string.IsNullOrEmpty(options.baseDirectory) ? 
                                Directory.GetCurrentDirectory() : 
                                options.baseDirectory;

        var rmPath = Path.Combine(baseDirectory, "resources");
        Directory.CreateDirectory(rmPath);
        resourceManager = new ResourceManager(rmPath);
        
        InitCommands();
    }

    private void InitCommands()
    {
        TabCommands = new List<Command>
        {
            new PrintTreeCommand(this),
            new PrintResourcesCommand(this),
            new DownloadResources(this),
            new ClearResourcesCommand(this)
        };
    }
    
    public void Run()
    {
        
        Console.WriteLine("Enter URL: ");
        var url = Console.ReadLine();

        currentTab = LoadTab(url);
        
        Console.WriteLine("What should I do? I can:");
        foreach (var command in TabCommands)
        {
            Console.WriteLine($"- {command.name}");
        }

        string userInput;
        do
        {
            Console.Write(">>> ");
            userInput = Console.ReadLine();
            
            var commands = TabCommands.Where(command => command.name.Equals(userInput));
            var enumerable = commands as Command[] ?? commands.ToArray();
            if (!enumerable.Any())
            {
                Console.WriteLine("Unknown command. I can:");
                foreach (var command in TabCommands)
                {
                    Console.WriteLine($"- {command.name}");
                }
            }
            else
            {
                foreach (var command in enumerable)
                {
                    command.Execute();
                    break;
                }
            }
        } while (!string.Equals(userInput, "quit", StringComparison.Ordinal));
        
        Console.WriteLine("Shutdown...");

    }

    private Tab? LoadTab(string url)
    {
        try
        {
            var result = resourceManager.GetResource(url, out var file);
            if (!result) throw new Exception();
            
            var doc = new HtmlDocument();
            doc.Load(file);

            var tab = new Tab(url, Util.FillResourcesWithLocation(Util.GetResources(doc), url), doc);

            return tab;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Error on loading page");
        }
    }
}