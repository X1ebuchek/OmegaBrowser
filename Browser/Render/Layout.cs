using System.Drawing;
using Browser.DOM;
using Browser.Management;
using HtmlAgilityPack;
using System.Drawing.Text;
using SkiaSharp;

namespace Browser.Render;

public class Layout
{

    private int viewport;
    // public static readonly Font cFont = new("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel);
    public static readonly SKPaint paint = new()
    {
        Color = SKColors.Black,
        TextSize = 14,
        Typeface = SKTypeface.FromFamilyName(
            "Arial", 
            SKFontStyleWeight.Normal, 
            SKFontStyleWidth.Normal, 
            SKFontStyleSlant.Upright)
    };
    public CssHtmlDocument document { get; set; }


    public Layout(int viewport, CssHtmlDocument document)
    {
        this.viewport = viewport;
        this.document = document;
    }

    public List<RenderObject> MakeRenderObjects(HtmlNode node, RenderObject parentObject, RenderObject? sibling = null)
    {
        var list = new List<RenderObject>();

        if (node.Name == "#text")
        {
            var t = node.InnerText.Trim();

            if (string.IsNullOrEmpty(t))
            {
                return list;
            }
            
            list.Add(MakeText(node, parentObject, t, sibling));
            return list;
        }

        if (node.Name == "#comment")
        {
            return list;
        }
        
        document.GetMap()[node].getMap().TryGetValue("display", out var displayType);
        
        switch (displayType)
        {
            case "none":
                return list;
            
            case "list-item":
            case "block":
            { // todo switch image
                var elem = MakeBlock(node, parentObject);
                var localList = new List<RenderObject>();
                elem.Rectangle.bottom = elem.Rectangle.top;

                RenderObject ls = null;

                foreach (var childNode in node.ChildNodes)
                {
                    var ll = MakeRenderObjects(childNode, elem, ls);
                    localList.AddRange(ll);

                    if (childNode.Name == "#text")
                    {
                        foreach (var o in ll)
                        {
                            elem.Rectangle.bottom += o.Rectangle.Height();
                        }
                    }
                    
                    if (ll.Count > 0)
                    {
                        elem.Rectangle.bottom += ll[0].Rectangle.Height();
                        ls = ll[0];
                    }
                    else
                    {
                        ls = null;
                    }
                    
                }
            
                list.Add(elem);
                list.AddRange(localList);
                return list;
            }
            
            case "flex":
            {
                break;
            }
            
            case null:
            case "inline":
            default: // inline
            {
                var elem = MakeInline(node, parentObject, sibling);
                var localList = new List<RenderObject>();
                elem.Rectangle.bottom = elem.Rectangle.top;
                elem.Rectangle.right = elem.Rectangle.left;
                
                RenderObject ls = null;
            
                foreach (var childNode in node.ChildNodes)
                {
                    var ll = MakeRenderObjects(childNode, elem, ls);
                    localList.AddRange(ll);
            
                    if (childNode.Name == "#text")
                    {
                        foreach (var o in ll)
                        {
                            // elem.Rectangle.bottom += o.Rectangle.Height();
                            elem.Rectangle.right += o.Rectangle.Width();
                        }
                    }
                    
                    if (ll.Count > 0)
                    {
                        elem.Rectangle.bottom += ll[0].Rectangle.Height();
                        elem.Rectangle.right += ll[0].Rectangle.Width();
                        ls = ll[0];
                    }
                    else
                    {
                        ls = null;
                    }
                    
                }
            
                list.Add(elem);
                list.AddRange(localList);
                return list;
                
                break;
            }

        }

        return list;
    }

    private RenderObject MakeInline(HtmlNode node, RenderObject parentObject, RenderObject? sibling)
    {
        var elem = new RenderObject
        {
            Map = document.GetMap()[node],
            ObjectType = RenderObjectType.Inline
        };

        var rect = new Rect();
        var parentWidth = parentObject == null ? 0 : parentObject.Rectangle.Width();
        
        // todo parent margin-top, ...
        CssMath.GetMargin(elem.Map.getMap(), parentWidth, viewport, 
            out var marginLeft, out var marginRight, out var marginTop, out var marginBottom);
        
        CssMath.GetPadding(document.GetMap()[node.ParentNode].getMap(), parentWidth, viewport,
            out var paddingLeft, out var paddingRight, out var paddingTop, out var paddingBottom);

        if (parentObject == null)
        {
            rect.left = 0 + marginLeft + paddingLeft;
            // rect.right = viewport - marginRight - paddingRight;
            rect.top = 0 + marginTop + marginTop;
        }
        else
        {
            if (sibling != null)
            {
                rect.left = sibling.Rectangle.right + marginLeft;
                // rect.right = parentObject.Rectangle.right - marginRight - paddingRight;
                rect.top = sibling.Rectangle.top + marginTop;
            }
            else
            {
                rect.left = parentObject.Rectangle.left + marginLeft + paddingLeft;
                // rect.right = parentObject.Rectangle.right - marginRight - paddingRight;
                rect.top = parentObject.Rectangle.bottom + marginTop + paddingTop;
            }
        }

        elem.Rectangle = rect;
        return elem;
    }

    private RenderObject MakeBlock(HtmlNode node, RenderObject parentObject)
    {
        var elem = new RenderObject
        {
            Map = document.GetMap()[node],
            ObjectType = RenderObjectType.Block
        };

        var rect = new Rect();
        var parentWidth = parentObject == null ? 0 : parentObject.Rectangle.Width();
        
        // todo parent margin-top, ...
        CssMath.GetMargin(elem.Map.getMap(), parentWidth, viewport, 
            out var marginLeft, out var marginRight, out var marginTop, out var marginBottom);
        
        CssMath.GetPadding(document.GetMap()[node.ParentNode].getMap(), parentWidth, viewport,
            out var paddingLeft, out var paddingRight, out var paddingTop, out var paddingBottom);

        if (parentObject == null)
        {
            rect.left = 0 + marginLeft + paddingLeft;
            rect.right = viewport - marginRight - paddingRight;
            rect.top = 0 + marginTop + marginTop;
        }
        else
        {
            rect.left = parentObject.Rectangle.left + marginLeft + paddingLeft;
            rect.right = parentObject.Rectangle.right - marginRight - paddingRight;
            rect.top = parentObject.Rectangle.bottom + marginTop + paddingTop;
        }

        elem.Rectangle = rect;
        return elem;
    }

    private TextObject MakeText(HtmlNode node, RenderObject parentObject, string text, RenderObject? sibling=null)
    {
        var parentWidth = parentObject.Rectangle.Width();
        
        CssMath.GetPadding(document.GetMap()[node.ParentNode].getMap(), parentWidth, viewport,
            out var paddingLeft, out var paddingRight, out var paddingTop, out var paddingBottom);

        var neededWidth = parentWidth - paddingLeft - paddingRight;
        // var size = TextRenderer.MeasureText(text, cFont);
        
        SKRect size = new();
        paint.MeasureText(text, ref size);

        if (size.Width > neededWidth && parentObject.Rectangle.Width() > 0)
        {
            text = CssMath.SplitTextLines(text, neededWidth, paint);
            // size = TextRenderer.MeasureText(text, cFont);
            paint.MeasureText(text, ref size);
        }
            
        var elem = new TextObject(text)
        {
            Map = document.GetMap()[node],
            ObjectType = RenderObjectType.Inline
        };

        var rect = new Rect();
        
        if (sibling != null)
        {
            rect.left = sibling.Rectangle.right + paddingLeft;
            rect.right = rect.left + (int)size.Width + paddingRight;
            rect.top = sibling.Rectangle.top + paddingTop;
            rect.bottom = rect.top + (int)size.Height;
        }
        else
        {
            rect.left = parentObject.Rectangle.left + paddingLeft;
            rect.right = rect.left + (int)size.Width + paddingRight;
            rect.top = parentObject.Rectangle.bottom + paddingTop;
            rect.bottom = rect.top + (int)size.Height;
        }
        
        elem.Rectangle = rect;
        return elem;
        // }
        
        
        
    }

}