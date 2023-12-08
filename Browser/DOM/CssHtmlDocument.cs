using HtmlAgilityPack;

namespace Browser.DOM;

public class CssHtmlDocument : HtmlDocument
{
    internal HtmlNode CreateNode(HtmlNodeType type) => this.CreateNode(type, -1);

    internal HtmlNode CreateNode(HtmlNodeType type, int index)
    {
        if (type == HtmlNodeType.Comment)
            return (HtmlNode) new CssHtmlCommentNode(this, index);
        return type == HtmlNodeType.Text ? (HtmlNode) new CssHtmlTextNode(this, index) : new CssHtmlNode(type, this, index);
    }
}