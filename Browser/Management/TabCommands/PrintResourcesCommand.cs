namespace Browser.Management.TabCommands;

public class PrintResourcesCommand : Command
{
    public PrintResourcesCommand(Browser browser) : base("print_resources", browser){}

    public override void Execute()
    {
        var counter = 0;
        foreach (var res in Browser.currentTab.resources)
        {
            Console.WriteLine($"{counter+1}) {res}");
            counter++;
        }
    }
}