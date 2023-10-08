using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using DOM;
using HtmlAgilityPack;

class Program
{
    static void Main(string[] args)
    {
        string url = Console.ReadLine();

        // Создаем новый объект HtmlDocument
        //HtmlDocument doc = new HtmlDocument();

        // Загружаем HTML в HtmlDocument
        var web = new HtmlWeb();
        var doc = web.Load(url);
        
        File.WriteAllText("C:/Users/vagae/RiderProjects/Browser/Browser/kek.html", doc.ParsedText);

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

