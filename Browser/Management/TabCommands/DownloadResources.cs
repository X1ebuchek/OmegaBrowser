using Browser.Networking;

namespace Browser.Management.TabCommands;

public class DownloadResources : Command
{
    public DownloadResources(Browser browser) : base("download_resources", browser) {}
    public override void Execute()
    {
        foreach (var t in Browser.currentTab.resources)
        {
            var resource = t;
            var res = Browser.resourceManager.GetResource(ref resource);
            Console.WriteLine(res ? $"[Done]: {resource.path} -> {resource.localPath}" : $"[Error]: {resource.path}");
        }
    }
}