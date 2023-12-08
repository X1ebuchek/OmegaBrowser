using HtmlAgilityPack;

namespace Browser.DOM;

public class CssHtmlCommentNode : CssHtmlNode
{
    private string _comment;

    internal CssHtmlCommentNode(HtmlDocument ownerdocument, int index)
        : base(HtmlNodeType.Comment, ownerdocument, index)
    {
    }

    /// <summary>Gets or Sets the comment text of the node.</summary>
    public string Comment
    {
        get => this._comment == null ? base.InnerHtml : this._comment;
        set => this._comment = value;
    }

    /// <summary>
    /// Gets or Sets the HTML between the start and end tags of the object. In the case of a text node, it is equals to OuterHtml.
    /// </summary>
    public override string InnerHtml
    {
        get => this._comment == null ? base.InnerHtml : this._comment;
        set => this._comment = value;
    }

    /// <summary>Gets or Sets the object and its content in HTML.</summary>
    public override string OuterHtml => this._comment == null ? base.OuterHtml : "<!--" + this._comment + "-->";
}