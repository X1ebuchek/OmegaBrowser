using Browser.DOM;
using Browser.Networking;
using HtmlAgilityPack;

namespace Browser.Management;

public class Tab
{
    public string location { get; }
    public HtmlDocument document { get; }
    public List<Resource> resources { get; set; }
    public Resource mainResource { get; }
    public Browser owner { get; }

    public Tab(Resource mainResource, Browser owner)
    {
        this.mainResource = mainResource;
        this.owner = owner;

        location = mainResource.path;

        document = new CssHtmlDocument();
        document.Load(mainResource.localPath);

        resources = ResourceUtil.FillResourcesWithLocation(ResourceUtil.GetResources(document), location);
        foreach (var t in resources)
        {
            var resource = t;
            owner.resourceManager.GetResource(ref resource);
        }
        
    }
}