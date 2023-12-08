using HtmlAgilityPack;

namespace Browser.DOM;

public class CssHtmlTextNode : CssHtmlNode
{
    private string _text;

    internal CssHtmlTextNode(HtmlDocument ownerdocument, int index)
        : base(HtmlNodeType.Text, ownerdocument, index)
    {
    }

    /// <summary>
    /// Gets or Sets the HTML between the start and end tags of the object. In the case of a text node, it is equals to OuterHtml.
    /// </summary>
    public override string InnerHtml
    {
        get => this.OuterHtml;
        set => this._text = value;
    }

    /// <summary>Gets or Sets the object and its content in HTML.</summary>
    public override string OuterHtml => this._text == null ? base.OuterHtml : this._text;

    /// <summary>Gets or Sets the text of the node.</summary>
    public string Text
    {
        get => this._text == null ? base.OuterHtml : this._text;
        set
        {
            this._text = value;
            this.SetChanged();
        }
    }
}