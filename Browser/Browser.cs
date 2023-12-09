using Browser.Management;
using Browser.Management.TabCommands;
using Browser.Networking;

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
            new PrintCssTreeCommand(this),
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

    private Tab LoadTab(string url)
    {
        try
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                url = "https://" + url;
            }
            var res = new Resource(url, Resource.ResourceType.Html);
            var result = resourceManager.GetResource(ref res);
            if (!result) throw new Exception();

            return new Tab(res, this);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Error on loading page");
        }
    }
}