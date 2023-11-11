using HtmlAgilityPack;

namespace Browser.Management;

public class Tab
{
    public string location { get; }
    public HtmlDocument document { get; }
    public HashSet<string> resources { get; }

    public Tab(string location, HtmlDocument document)
    {
        this.location = location;
        this.document = document;
        resources = new HashSet<string>();
    }

    public Tab(string location, HashSet<string> resources, HtmlDocument document)
    {
        this.resources = resources;
        this.document = document;
        this.location = location;
    }

    public void AddResource(string res)
    {
        resources.Add(res);
    }
}