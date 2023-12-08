using Browser.Networking;
using HtmlAgilityPack;

namespace Browser.Management;

public class Tab
{
    public string location { get; }
    public HtmlDocument document { get; }
    public List<Resource> resources { get; set; }

    public Tab(string location, HtmlDocument document)
    {
        this.location = location;
        this.document = document;
        resources = new List<Resource>();
    }

    public Tab(string location, List<Resource> resources, HtmlDocument document)
    {
        this.resources = resources;
        this.document = document;
        this.location = location;
    }
}