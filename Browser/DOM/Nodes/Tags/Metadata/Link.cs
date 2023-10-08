namespace DOM.Nodes;

using System;

public class Link : Node
{
    public Link() : base("title", new LinkedList<Node>()){}

    string href { get; set; }

    string? crossOrigin { get; set; }

    string rel { get; set; }

    string _as { get; set; }

    DOMTokenList relList { get; }

    string media { get; set; }

    string integrity { get; set; }

    string hreflang { get; set; }

    string type { get; set; }

    DOMTokenList sizes { get; }

    string imageSrcset { get; set; }

    string imageSizes { get; set; }

    string referrerPolicy { get; set; }

    DOMTokenList blocking { get; }

    bool disabled { get; set; }

    string fetchPriority { get; set; }
}