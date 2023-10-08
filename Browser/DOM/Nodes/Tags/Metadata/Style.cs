namespace DOM.Nodes;
using System;

public class Style : Node
{
    public Style() : base("style", new LinkedList<Node>()){}

    bool disabled { get; set; }

    string media { get; set; }

    DOMTokenList blocking { get; }
}