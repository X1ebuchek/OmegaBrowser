using Browser.CSS;
using Browser.Management;
using Browser.Render;
using SkiaSharp;

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
            baseDirectory = WorkingPath,
            viewport = 960
        };

        var browser = new Browser(opts);
        browser.Run();
    }

    static void DoLocal()
    {
        SKRect size = new();
        Layout.paint.MeasureText("m m", ref size);
        Console.WriteLine(size);

    }
}