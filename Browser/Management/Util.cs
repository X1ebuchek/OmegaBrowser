using System.Security.Cryptography;
using System.Text;
using Browser.Networking;
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

}