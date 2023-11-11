namespace Browser.Management.TabCommands;

public class DownloadResources : Command
{
    public DownloadResources(Browser browser) : base("download_resources", browser) {}
    public override void Execute()
    {
        foreach (var resource in Browser.currentTab.resources)
        {
            var res = Browser.resourceManager.GetResource(resource, out var unused);
            Console.WriteLine(res ? $"[Done]: {resource} -> {unused}" : $"[Error]: {resource}");
        }
    }
}