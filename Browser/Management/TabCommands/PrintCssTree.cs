using Browser.DOM;

namespace Browser.Management.TabCommands;

public class PrintCssTreeCommand : Command
{
    public PrintCssTreeCommand(Browser browser) : base("print_css_tree", browser){}

    public override void Execute()
    {
        Browser.currentTab.HandleCss();
        Util.RecursiveCssHtmlNodePrint((CssHtmlNode)Browser.currentTab.document.DocumentNode);
    }

}