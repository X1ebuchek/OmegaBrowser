using System.Collections;
namespace DOM;

public abstract class Node
{
    public string TagName { get; set; }
    public IEnumerable<Node> Children { get; set; }

    public Node(string tagName, IEnumerable<Node> children)
    {
        TagName = tagName;
        Children = children;
    }

}