using System.Security.Cryptography;
using System.Text;
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
    
    public static string ComputeHash(string str)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(str));
        return Convert.ToBase64String(hash)
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    public static List<string> GetResources(HtmlDocument document)
    {
        var root = document.DocumentNode;
        var result = (
            from script in root.SelectNodes("//script") 
            from attribute in script.Attributes 
            where attribute.Name == "src" select attribute.Value
            ).ToList();
        
        result.AddRange(
            from style in root.SelectNodes("//link[@rel='stylesheet']")
            from styleAttribute in style.Attributes 
            where styleAttribute.Name == "href" select styleAttribute.Value
            );

        return result;
    }
}