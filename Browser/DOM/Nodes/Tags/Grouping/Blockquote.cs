namespace DOM.Nodes;

public class Blockquote : Node
 {
     public Blockquote() : base("blockquote", new LinkedList<Node>()){}
 
     string cite { get; set; }
 }
