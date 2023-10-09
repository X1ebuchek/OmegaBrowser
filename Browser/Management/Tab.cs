using HtmlAgilityPack;

namespace Browser.Management;

public class Tab
{
    public string location { get; }
    public HtmlDocument document { get; }
    public List<string> resources { get; }

    public Tab(string location, HtmlDocument document)
    {
        this.location = location;
        this.document = document;
        resources = new List<string>();
    }

    public Tab(string location, List<string> resources, HtmlDocument document)
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