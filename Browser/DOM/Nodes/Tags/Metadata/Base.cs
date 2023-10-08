namespace DOM.Nodes;

public class Base : Node
{
    public Base() : base("base", new LinkedList<Node>()){}

    string href { get; set; }

    string target { get; set; }
}