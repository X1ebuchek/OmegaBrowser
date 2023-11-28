using Browser.CSS;
using Browser.Management;
using HtmlAgilityPack;
namespace Browser;

class Program
{
    private static string WorkingPath;
    
    static void Main(string[] args)
    {

        if (args.Length == 0)
        {
            Console.WriteLine("Booting Browser");
            BootBrowser();
        }
        else
        {
            switch (args[0])
            {
                case "debug":
                    DoLocal();
                    break;
            
                case "browser":
                    BootBrowser();
                    break;
            
                default:
                    Console.Error.WriteLine("Please, provide boot options ('debug' or 'browser')");
                    break;
            }
        }
        
    }


    static void BootBrowser()
    {
        WorkingPath = Directory.GetCurrentDirectory();
                
        var opts = new Browser.BrowserOptions
        {
            baseDirectory = WorkingPath
        };
                
        var browser = new Browser(opts);
        browser.Run();
    }
    
    static void DoLocal()
    {
        // WorkingPath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.ToString();
        //
        // var filename = "fortesting.html";
        // var path = Path.Combine(WorkingPath, "html", filename);
        //
        // HtmlDocument doc = new HtmlDocument();
        // doc.Load(path);
        // Util.RecursiveHtmlNodePrint(doc.DocumentNode);
        //
        // var allNodes = doc.DocumentNode.ChildNodes;
        //
        // allNodes.FindFirst("div").AppendChild(HtmlNode.CreateNode("<img src='test.jpg' />"));
        //
        // Util.RecursiveHtmlNodePrint(doc.DocumentNode);
        CssParser.Parse(
            @"C:\Users\vagae\RiderProjects\OmegaBrowser\Browser\bin\Debug\net7.0\resources\se.ifmo.ru__6gu2AbGMyNUXYmwDWFt_Q78EK5ZRFXbBLhA-YyPtfGs");



    }
    
    [Obsolete] static void DoEnter()
    {
        String url = Console.ReadLine();
        
        // Создаем новый объект HtmlDocument
        //HtmlDocument doc = new HtmlDocument();

        // Загружаем HTML в HtmlDocument
        var web = new HtmlWeb();
        var doc = web.Load(url);
        
        Directory.CreateDirectory(WorkingPath + "html");
        
        File.WriteAllText(WorkingPath + "/site.html", doc.ParsedText);

        // Получаем все элементы DOM на странице
        IEnumerable<HtmlNode> allNodes = doc.DocumentNode.ChildNodes;

        foreach (HtmlNode node in allNodes)
        {
            Console.WriteLine($"Parent:  Тег: {node.Name}");
            IEnumerable<HtmlNode> allNodes1 = node.ChildNodes;
            foreach (var node1 in allNodes1)
            {
                Console.WriteLine($"  Child:  Тег: {node1.Name}");
                foreach (var attr in node1.Attributes)
                {
                    Console.WriteLine(attr.Name + " = " + attr.Value);
                    
                }
            }
        }
    }
}

