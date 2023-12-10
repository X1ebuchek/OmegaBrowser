using Browser.DOM;
using HtmlAgilityPack;

namespace Browser.Management;

public class Util
{
    public static void RecursiveHtmlNodePrint(HtmlNode node, string indentation = "")
    {
        Console.WriteLine($"{indentation}{node.Name}");
        foreach (var nodeAttribute in node.Attributes)
        {
            Console.WriteLine($"{indentation} -{nodeAttribute.Name}:{nodeAttribute.Value}");
        }

        foreach (var child in node.ChildNodes)
        {
            RecursiveHtmlNodePrint(child, indentation + "  ");
        }
    }
    
    public static void RecursiveCssHtmlNodePrint(HtmlNode node, CssHtmlDocument document, string indentation = "")
    {
        Console.WriteLine($"{indentation}{node.Name}");
        foreach (var nodeAttribute in document.GetMap()[node].getMap())
        {
            Console.WriteLine($"{indentation} -{nodeAttribute.Key}:{nodeAttribute.Value}");
        }

        foreach (var child in node.ChildNodes)
        {
            RecursiveCssHtmlNodePrint(child, document, indentation + "  ");
        }
    }

}