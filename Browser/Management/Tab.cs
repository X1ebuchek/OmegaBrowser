using System.Text.RegularExpressions;
using Browser.CSS;
using Browser.DOM;
using Browser.Networking;
using Browser.Render;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Browser.Management;

public class Tab
{
    public string location { get; }
    public HtmlDocument document { get; }
    public CssHtmlDocument cssDocument { get; }
    public Layout layout { get; private set; }
    public List<Resource> resources { get; set; }
    public Resource mainResource { get; }
    public Browser owner { get; }

    private ObjectRenderer _renderer;

    public Tab(Resource mainResource, Browser owner)
    {
        this.mainResource = mainResource;
        this.owner = owner;

        location = mainResource.path;

        document = new HtmlDocument();
        document.Load(mainResource.localPath);

        cssDocument = new CssHtmlDocument(document);

        resources = ResourceUtil.FillResourcesWithLocation(ResourceUtil.GetResources(document), location);
        foreach (var t in resources)
        {
            var resource = t;
            try
            {
                owner.resourceManager.GetResource(ref resource);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Bad resource: {t}");
            }
        }

        HandleCss();
        
        layout = new Layout(owner.Options.viewport, cssDocument, this);
        // foreach (var obj in layout.MakeRenderObjects(
        //              document.DocumentNode.SelectSingleNode("//body"), null)
        //          )
        // {
        //     Console.WriteLine(obj);
        // }
        _renderer = new ObjectRenderer(this, layout.MakeRenderObjects(document.DocumentNode.SelectSingleNode("//body"), null));

    }
    
    public void HandleCss()
    {
        
        var cssNodes = document.DocumentNode.SelectNodes("//link[@rel='stylesheet']|//style");
        if (cssNodes != null)
        {
            foreach (var node in cssNodes)
            {

                CSSGlobalMap map;
                
                if (node.Name == "link")
                {
                    var link = node.Attributes.First(attribute => attribute.Name == "href");
                    if (link == null)
                    {
                        continue;
                    }
                    var fileName = resources.First(x => link.Value.Equals(x.path)).localPath;
                    if (fileName == null)
                    {
                        continue;
                    }
                    map = CssParser.ParseFile(fileName);
                }
                else
                {
                    var cssString = node.InnerText;
                    cssString = cssString.Trim();
                    if (cssString.Equals(""))
                    {
                        continue;
                    }
                    map = CssParser.ParseString(cssString);
                }
                
                foreach (var kvp in map.getMap())
                {
                    var preprocessed = kvp.Key.ToLower().Split(",");
                    var filtered = preprocessed.Where(rule => !rule.Contains(':')).ToList();
                    if (filtered.Count == 0) continue;
                    var selector = string.Join(",", filtered);
                    IList<HtmlNode> selectedNodes;
                    try
                    {
                        selectedNodes = document.DocumentNode.QuerySelectorAll(selector);
                    }
                    catch (Exception e)
                    {
                        //bad css selector
                        continue;
                    }
                    if (selectedNodes == null)
                    {
                        continue;
                    }

                    foreach (var selectedNode in selectedNodes)
                    {
                        foreach (var attrKvp in kvp.Value.getMap())
                        {
                            cssDocument.GetMap()[selectedNode].getMap()[attrKvp.Key.ToLower()] = attrKvp.Value;
                            if (attrKvp.Key.ToLower() == "background-image")
                            {
                                string pattern = @"url\(([^)]+)\)";

                                Regex regex = new Regex(pattern);
                                Match match = regex.Match(attrKvp.Value);

                                if (match.Success)
                                {
                                    string url = match.Groups[1].Value;
                                    Resource res = new Resource(url, Resource.ResourceType.Img);
                                    
                                    try
                                    {
                                        owner.resourceManager.GetResource(ref res);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine($"Bad resource: {url}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("URL not found in the input text.");
                                }
                            }
                        }
                    }
                }
            }
        }
        
        var allNodes = document.DocumentNode.Descendants();
        if (allNodes == null)
        {
            return;
        }

        foreach (var node in allNodes)
        {
            foreach (var attribute in node.Attributes)
            {
                if (!attribute.Name.Equals("style")) continue;
                var map = CssParser.ParseInline(attribute.Value);

                foreach (var kvp in map.getMap())
                {
                    cssDocument.GetMap()[node].getMap()[kvp.Key.ToLower()] = kvp.Value;
                    if (kvp.Key.ToLower() == "background-image")
                    {
                        string pattern = @"url\(([^)]+)\)";

                        Regex regex = new Regex(pattern);
                        Match match = regex.Match(kvp.Value);

                        if (match.Success)
                        {
                            string url = match.Groups[1].Value;
                            Resource res = new Resource(url, Resource.ResourceType.Img);
                                    
                            try
                            {
                                owner.resourceManager.GetResource(ref res);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Bad resource: {url}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("URL not found in the input text.");
                        }
                    }
                }
            }
            
        }
        
    }
}