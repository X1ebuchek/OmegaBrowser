using System.Collections;
using Browser.CSS;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Browser.DOM;

public class CssHtmlDocument
{
    private readonly HtmlNodeEqualityComparer _comparer = new(); 
    private Dictionary<HtmlNode, CSSAttrMap> _cssMap;

    public CssHtmlDocument(HtmlDocument document)
    {
        _cssMap = new Dictionary<HtmlNode, CSSAttrMap>(_comparer);
        foreach (var node in document.DocumentNode.Descendants())
        {
            _cssMap[node] = FillDefaultCssAttributes(node.Name);
        }
        _cssMap[document.DocumentNode] = FillDefaultCssAttributes(document.DocumentNode.Name);
    }

    public Dictionary<HtmlNode, CSSAttrMap> GetMap()
    {
        return _cssMap;
    }
    
    private static CSSAttrMap FillDefaultCssAttributes(string tagName)
    {
        var attrMap = new CSSAttrMap();

        switch (tagName)
        {
            case "a:link":
                attrMap.getMap().Add("color", "(internal value)");
                attrMap.getMap().Add("text-decoration", "underline");
                attrMap.getMap().Add("cursor", "auto");
                break;
            case "a:visited":
                attrMap.getMap().Add("color", "(internal value)");
                attrMap.getMap().Add("text-decoration", "underline");
                attrMap.getMap().Add("cursor", "auto");
                break;
            case "a:link:active":
                attrMap.getMap().Add("color", "(internal value)");
                break;
            case "a:visited:active":
                attrMap.getMap().Add("color", "(internal value)");
                break;
            case "address":
                attrMap.getMap().Add("display", "block");
                break;
            case "area":
                attrMap.getMap().Add("display", "none");
                break;
            case "article":
                attrMap.getMap().Add("display", "block");
                break;
            case "aside":
                attrMap.getMap().Add("display", "block");
                break;
            case "audio":
                break;
            case "b":
                attrMap.getMap().Add("font-weight", "bold");
                break;
            case "base":
                break;
            case "bdi":
                break;
            case "bdo":
                attrMap.getMap().Add("unicode-bidi", "bidi-override");
                break;
            case "blockquote":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("margin-top", "1em");
                attrMap.getMap().Add("margin-bottom", "1em");
                attrMap.getMap().Add("margin-left", "40px");
                attrMap.getMap().Add("margin-right", "40px");
                break;
            case "body":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("margin", "8px");
                break;
            case "body:focus":
                attrMap.getMap().Add("outline", "none");
                break;
            case "br":
                break;
            case "button":
                attrMap.getMap().Add("display", "inline-block");
                break;
            case "canvas":
                break;
            case "caption":
                attrMap.getMap().Add("display", "table-caption");
                attrMap.getMap().Add("text-align", "center");
                break;
            case "cite":
                attrMap.getMap().Add("font-style", "italic");
                break;
            case "code":
                attrMap.getMap().Add("font-family", "monospace");
                break;
            case "col":
                attrMap.getMap().Add("display", "table-column");
                break;
            case "colgroup":
                attrMap.getMap().Add("display", "table-column-group");
                break;
            case "datalist":
                attrMap.getMap().Add("display", "none");
                break;
            case "dd":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("margin-left", "40px");
                break;
            case "del":
                attrMap.getMap().Add("text-decoration", "line-through");
                break;
            case "details":
                attrMap.getMap().Add("display", "block");
                break;
            case "dfn":
                attrMap.getMap().Add("font-style", "italic");
                break;
            case "dialog":
                break;
            case "div":
                attrMap.getMap().Add("display", "block");
                break;
            case "dl":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("margin-top", "1em");
                attrMap.getMap().Add("margin-bottom", "1em");
                attrMap.getMap().Add("margin-left", "0");
                attrMap.getMap().Add("margin-right", "0");
                break;
            case "dt":
                attrMap.getMap().Add("display", "block");
                break;
            case "em":
                attrMap.getMap().Add("font-style", "italic");
                break;
            case "embed:focus":
                attrMap.getMap().Add("outline", "none");
                break;
            case "fieldset":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("margin-left", "2px");
                attrMap.getMap().Add("margin-right", "2px");
                attrMap.getMap().Add("padding-top", "0.35em");
                attrMap.getMap().Add("padding-bottom", "0.625em");
                attrMap.getMap().Add("padding-left", "0.75em");
                attrMap.getMap().Add("padding-right", "0.75em");
                attrMap.getMap().Add("border", "2px groove (internal value)");
                break;
            case "figcaption":
                attrMap.getMap().Add("display", "block");
                break;
            case "figure":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("margin-top", "1em");
                attrMap.getMap().Add("margin-bottom", "1em");
                attrMap.getMap().Add("margin-left", "40px");
                attrMap.getMap().Add("margin-right", "40px");
                break;
            case "footer":
                attrMap.getMap().Add("display", "block");
                break;
            case "form":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("margin-top", "0em");
                break;
            case "h1":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("font-size", "2em");
                attrMap.getMap().Add("margin-top", "0.67em");
                attrMap.getMap().Add("margin-bottom", "0.67em");
                attrMap.getMap().Add("margin-left", "0");
                attrMap.getMap().Add("margin-right", "0");
                attrMap.getMap().Add("font-weight", "bold");
                break;
            case "h2":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("font-size", "1.5em");
                attrMap.getMap().Add("margin-top", "0.83em");
                attrMap.getMap().Add("margin-bottom", "0.83em");
                attrMap.getMap().Add("margin-left", "0");
                attrMap.getMap().Add("margin-right", "0");
                attrMap.getMap().Add("font-weight", "bold");
                break;
            case "h3":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("font-size", "1.17em");
                attrMap.getMap().Add("margin-top", "1em");
                attrMap.getMap().Add("margin-bottom", "1em");
                attrMap.getMap().Add("margin-left", "0");
                attrMap.getMap().Add("margin-right", "0");
                attrMap.getMap().Add("font-weight", "bold");
                break;
            case "h4":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("margin-top", "1.33em");
                attrMap.getMap().Add("margin-bottom", "1.33em");
                attrMap.getMap().Add("margin-left", "0");
                attrMap.getMap().Add("margin-right", "0");
                attrMap.getMap().Add("font-weight", "bold");
                break;
            case "h5":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("font-size", ".83em");
                attrMap.getMap().Add("margin-top", "1.67em");
                attrMap.getMap().Add("margin-bottom", "1.67em");
                attrMap.getMap().Add("margin-left", "0");
                attrMap.getMap().Add("margin-right", "0");
                attrMap.getMap().Add("font-weight", "bold");
                break;
            case "h6":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("font-size", ".67em");
                attrMap.getMap().Add("margin-top", "2.33em");
                attrMap.getMap().Add("margin-bottom", "2.33em");
                attrMap.getMap().Add("margin-left", "0");
                attrMap.getMap().Add("margin-right", "0");
                attrMap.getMap().Add("font-weight", "bold");
                break;
            case "head":
                attrMap.getMap().Add("display", "none");
                break;
            case "header":
                attrMap.getMap().Add("display", "block");
                break;
            case "hr":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("margin-top", "0.5em");
                attrMap.getMap().Add("margin-bottom", "0.5em");
                attrMap.getMap().Add("margin-left", "auto");
                attrMap.getMap().Add("margin-right", "auto");
                attrMap.getMap().Add("border-style", "inset");
                attrMap.getMap().Add("border-width", "1px");
                break;
            case "html":
                attrMap.getMap().Add("display", "block");
                break;
            case "html:focus":
                attrMap.getMap().Add("outline", "none");
                break;
            case "i":
                attrMap.getMap().Add("font-style", "italic");
                break;
            case "iframe:focus":
                attrMap.getMap().Add("outline", "none");
                break;
            case "iframe[seamless]":
                attrMap.getMap().Add("display", "block");
                break;
            case "img":
                attrMap.getMap().Add("display", "inline-block");
                break;
            case "input":
                attrMap.getMap().Add("display", "inline-block");
                break;
            case "ins":
                attrMap.getMap().Add("text-decoration", "underline");
                break;
            case "kbd":
                attrMap.getMap().Add("font-family", "monospace");
                break;
            case "label":
                attrMap.getMap().Add("cursor", "default");
                break;
            case "legend":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("padding-left", "2px");
                attrMap.getMap().Add("padding-right", "2px");
                attrMap.getMap().Add("border", "none");
                break;
            case "li":
                attrMap.getMap().Add("display", "list-item");
                break;
            case "link":
                attrMap.getMap().Add("display", "none");
                break;
            case "main":
                break;
            case "map":
                attrMap.getMap().Add("display", "inline");
                break;
            case "mark":
                attrMap.getMap().Add("background-color", "yellow");
                attrMap.getMap().Add("color", "black");
                break;
            case "menu":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("list-style-type", "disc");
                attrMap.getMap().Add("margin-top", "1em");
                attrMap.getMap().Add("margin-bottom", "1em");
                attrMap.getMap().Add("margin-left", "0");
                attrMap.getMap().Add("margin-right", "0");
                attrMap.getMap().Add("padding-left", "40px");
                break;
            case "menuitem":
                break;
            case "meta":
                break;
            case "meter":
                break;
            case "nav":
                attrMap.getMap().Add("display", "block");
                break;
            case "noscript":
                break;
            case "object:focus":
                attrMap.getMap().Add("outline", "none");
                break;
            case "ol":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("list-style-type", "decimal");
                attrMap.getMap().Add("margin-top", "1em");
                attrMap.getMap().Add("margin-bottom", "1em");
                attrMap.getMap().Add("margin-left", "0");
                attrMap.getMap().Add("margin-right", "0");
                attrMap.getMap().Add("padding-left", "40px");
                break;
            case "optgroup":
                break;
            case "option":
                break;
            case "output":
                attrMap.getMap().Add("display", "inline");
                break;
            case "p":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("margin-top", "1em");
                attrMap.getMap().Add("margin-bottom", "1em");
                attrMap.getMap().Add("margin-left", "0");
                attrMap.getMap().Add("margin-right", "0");
                break;
            case "param":
                attrMap.getMap().Add("display", "none");
                break;
            case "picture":
                break;
            case "pre":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("font-family", "monospace");
                attrMap.getMap().Add("white-space", "pre");
                attrMap.getMap().Add("margin", "1em 0");
                break;
            case "progress":
                break;
            case "q":
                attrMap.getMap().Add("display", "inline");
                break;
            case "q::before":
                attrMap.getMap().Add("content", "open-quote");
                break;
            case "q::after":
                attrMap.getMap().Add("content", "close-quote");
                break;
            case "rp":
                break;
            case "rt":
                attrMap.getMap().Add("line-height", "normal");
                break;
            case "ruby":
                break;
            case "s":
                attrMap.getMap().Add("text-decoration", "line-through");
                break;
            case "samp":
                attrMap.getMap().Add("font-family", "monospace");
                break;
            case "script":
                attrMap.getMap().Add("display", "none");
                break;
            case "section":
                attrMap.getMap().Add("display", "block");
                break;
            case "select":
                attrMap.getMap().Add("display", "inline-block");
                break;
            case "small":
                attrMap.getMap().Add("font-size", "smaller");
                break;
            case "strike":
                attrMap.getMap().Add("text-decoration", "line-through");
                break;
            case "strong":
                attrMap.getMap().Add("font-weight", "bold");
                break;
            case "style":
                attrMap.getMap().Add("display", "none");
                break;
            case "sub":
                attrMap.getMap().Add("vertical-align", "sub");
                attrMap.getMap().Add("font-size", "smaller");
                break;
            case "summary":
                attrMap.getMap().Add("display", "block");
                break;
            case "sup":
                attrMap.getMap().Add("vertical-align", "super");
                attrMap.getMap().Add("font-size", "smaller");
                break;
            case "table":
                attrMap.getMap().Add("display", "table");
                attrMap.getMap().Add("border-collapse", "separate");
                attrMap.getMap().Add("border-spacing", "2px");
                attrMap.getMap().Add("border-color", "gray");
                break;
            case "tbody":
                attrMap.getMap().Add("display", "table-row-group");
                attrMap.getMap().Add("vertical-align", "middle");
                attrMap.getMap().Add("border-color", "inherit");
                break;
            case "td":
                attrMap.getMap().Add("display", "table-cell");
                attrMap.getMap().Add("vertical-align", "inherit");
                break;
            case "template":
                break;
            case "textarea":
                attrMap.getMap().Add("display", "inline-block");
                break;
            case "tfoot":
                attrMap.getMap().Add("display", "table-footer-group");
                attrMap.getMap().Add("vertical-align", "middle");
                attrMap.getMap().Add("border-color", "inherit");
                break;
            case "th":
                attrMap.getMap().Add("display", "table-cell");
                attrMap.getMap().Add("vertical-align", "inherit");
                attrMap.getMap().Add("font-weight", "bold");
                attrMap.getMap().Add("text-align", "center");
                break;
            case "thead":
                attrMap.getMap().Add("display", "table-header-group");
                attrMap.getMap().Add("vertical-align", "middle");
                attrMap.getMap().Add("border-color", "inherit");
                break;
            case "time":
                break;
            case "title":
                attrMap.getMap().Add("display", "none");
                break;
            case "tr":
                attrMap.getMap().Add("display", "table-row");
                attrMap.getMap().Add("vertical-align", "inherit");
                attrMap.getMap().Add("border-color", "inherit");
                break;
            case "track":
                break;
            case "u":
                attrMap.getMap().Add("text-decoration", "underline");
                break;
            case "ul":
                attrMap.getMap().Add("display", "block");
                attrMap.getMap().Add("list-style-type", "disc");
                attrMap.getMap().Add("margin-top", "1em");
                attrMap.getMap().Add("margin-bottom", "1 em");
                attrMap.getMap().Add("margin-left", "0");
                attrMap.getMap().Add("margin-right", "0");
                attrMap.getMap().Add("padding-left", "40px");
                break;
            case "var":
                attrMap.getMap().Add("font-style", "italic");
                break;
            case "video":
                break;
            case "wbr":
                break;
            default: 
                attrMap.getMap().Add("display", "inline");
                break;
        }

        return attrMap;
    }
}

internal class HtmlNodeEqualityComparer : IEqualityComparer<HtmlNode>
{
    public bool Equals(HtmlNode x, HtmlNode y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.XPath == y.XPath;
    }

    public int GetHashCode(HtmlNode obj)
    {
        return obj.XPath.GetHashCode();
    }
}