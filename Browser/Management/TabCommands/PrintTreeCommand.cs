namespace Browser.Management.TabCommands;

public class PrintTreeCommand : Command
{
    public PrintTreeCommand(Browser browser) : base("print_tree", browser){}

    public override void Execute()
    {
        Util.RecursiveHtmlNodePrint(Browser.currentTab.document.DocumentNode);
    }

}