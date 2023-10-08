using Browser.Networking;
using HtmlAgilityPack;

namespace Browser;

public class Browser
{

    private ResourceManager rm;

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
        rm = new ResourceManager(rmPath);
    }
    
    public void Run()
    {
        
        Console.WriteLine("Enter URL: ");
        var url = Console.ReadLine();

        try
        {
            var file = rm.GetResource(url);
            var doc = new HtmlDocument();
            doc.Load(file);
        
            Util.RecursiveHtmlNodePrint(doc.DocumentNode);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
        }

    }
}