namespace DOM.Nodes;

using System;

public class Meta : Node
{
    public Meta() : base("meta", new LinkedList<Node>()){}

    string name { get; set; }

    string httpEquiv { get; set; }

    string content { get; set; }

    string media { get; set; }
}