namespace Browser.Management.TabCommands;

public class ClearResourcesCommand : Command
{
    public ClearResourcesCommand(Browser browser) : base("clear_resources", browser){}

    public override void Execute()
    {
        Browser.resourceManager.ClearCacheByTab(Browser.currentTab.resources);
    }
}