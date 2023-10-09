using System.Collections;
using Browser.Management;
using Browser.Management.TabCommands;
using Browser.Networking;
using HtmlAgilityPack;

namespace Browser;

public class Browser //todo tab manager
{

    private ResourceManager _resourceManager;
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
        _resourceManager = new ResourceManager(rmPath);
        
        InitCommands();
    }

    private void InitCommands()
    {
        TabCommands = new List<Command>
        {
            new PrintTreeCommand(this),
            new PrintResourcesCommand(this)
        };
    }
    
    public void Run()
    {
        
        Console.WriteLine("Enter URL: ");
        var url = Console.ReadLine();

        currentTab = LoadTab(url);

        var userInput = "";
        
        Console.WriteLine("What should I do? I can:");
        foreach (var command in TabCommands)
        {
            Console.WriteLine($"- {command.name}");
        }
        
        while (!string.Equals(userInput, "quit", StringComparison.Ordinal))
        {
            
            Console.Write(">>> ");
            userInput = Console.ReadLine();

            foreach (var command in TabCommands.Where(command => command.name.Equals(userInput)))
            {
                command.Execute();
                break;
            }
            
        }
        
        Console.WriteLine("Shutdown...");

    }

    private Tab? LoadTab(string url)
    {
        try
        {
            var file = _resourceManager.GetResource(url);
            var doc = new HtmlDocument();
            doc.Load(file);

            var tab = new Tab(url, Util.GetResources(doc), doc);

            return tab;
        }
        catch (Exception e)
        {
            throw new Exception("Error on loading page");
        }
    }
}