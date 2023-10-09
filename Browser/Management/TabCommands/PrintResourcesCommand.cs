namespace Browser.Management.TabCommands;

public class PrintResourcesCommand : Command
{
    public PrintResourcesCommand(Browser browser) : base("print_resources", browser){}

    public override void Execute()
    {
        for (var i = 0; i < Browser.currentTab.resources.Count; i++)
        {
            Console.WriteLine($"{i+1}) {Browser.currentTab.resources[i]}");
        }
    }
}