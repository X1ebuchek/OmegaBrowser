namespace Browser.Management.TabCommands;

public class Command
{
    protected Command(string name, Browser browser)
    {
        Browser = browser;
        this.name = name;
    }

    public string name {  get; }
    protected readonly Browser Browser;

    public virtual void Execute()
    {
        throw new NotImplementedException();
    }
}