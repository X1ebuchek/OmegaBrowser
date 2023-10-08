using System;
using System.Collections;

namespace DOM.Nodes;

public class Title : Node
{
    public Title() : base("title", new LinkedList<Node>()){}

    string text { get; set; }
}