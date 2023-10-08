using System.Security.Cryptography;
using System.Text;
using HtmlAgilityPack;

namespace Browser;

public class Util
{
    public static void RecursiveHtmlNodePrint(HtmlNode node, String indentation = "")
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
    
    public static string ComputeHash(string str)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(str));
        return Convert.ToBase64String(hash)
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }
}