using System.Drawing;
using Browser.DOM;
using Browser.Management;
using HtmlAgilityPack;
using System.Drawing.Text;

namespace Browser.Render;

public class Layout
{

    private int viewport;
    public CssHtmlDocument document { get; set; }


    public Layout(int viewport, CssHtmlDocument document)
    {
        this.viewport = viewport;
        this.document = document;
    }

    public List<RenderObject> MakeRenderObjects(HtmlNode node, RenderObject parentObject)
    {
        var list = new List<RenderObject>();

        document.GetMap()[node].getMap().TryGetValue("display", out var displayType);
        
        
        if (displayType is null or "none")
        {
            if (node.Name == "#text")
            {
                list.Add(MakeText(node, parentObject, node.InnerText));
            }
            return list;
        }

        if (displayType == "block" || true) // todo switch image
        {
            var elem = MakeBlock(node, parentObject);
            var localList = new List<RenderObject> { elem };
            var h = 0;

            foreach (var childNode in node.ChildNodes)
            {
                var ll = MakeRenderObjects(childNode, elem);
                localList.AddRange(ll);
                try
                {
                    h += ll[0].Rectangle.Height();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }
            
            list.AddRange(localList);
        }

        return list;
    }

    private RenderObject MakeBlock(HtmlNode node, RenderObject parentObject)
    {
        var elem = new RenderObject
        {
            Map = document.GetMap()[node]
        };

        var rect = new Rect();
        var parentWidth = parentObject == null ? 0 : parentObject.Rectangle.Width();

        int marginLeft, marginRight, marginTop, marginBottom;
        marginLeft = marginRight = marginTop = marginBottom = 0;
        
        elem.Map.getMap().TryGetValue("margin", out var margin);
        if (!string.IsNullOrEmpty(margin))
        {
            var marginArray = margin.Split(" ");

            switch (marginArray.Length)
            {
                case 1:
                    var value = marginArray[0];
                    if (!value.EndsWith("%"))
                    {
                        marginLeft = marginRight = marginTop = marginBottom = ParseValue(value);
                    }
                    else
                    {
                        marginLeft = marginRight = marginTop = marginBottom = parentWidth / 100 * int.Parse(value[..^1]);
                    }
                    break;

                case 2:
                    var value1 = marginArray[0];
                    var value2 = marginArray[1];
                    
                    if (!value1.EndsWith("%"))
                    {
                        marginTop = marginBottom = ParseValue(value1);
                    }
                    else
                    {
                        marginTop = marginBottom = parentWidth / 100 * int.Parse(value1[..^1]);
                    }
                    
                    if (!value2.EndsWith("%"))
                    {
                        marginLeft = marginRight = ParseValue(value2);
                    }
                    else
                    {
                        marginLeft = marginRight = parentWidth / 100 * int.Parse(value2[..^1]);
                    }
                    
                    break;
                case 3:
                    var value_1 = marginArray[0];
                    var value_2 = marginArray[1];
                    var value_3 = marginArray[2];
                    
                    if (!value_1.EndsWith("%"))
                    {
                        marginTop = ParseValue(value_1);
                    }
                    else
                    {
                        marginTop = parentWidth / 100 * int.Parse(value_1[..^1]);
                    }
                    
                    if (!value_2.EndsWith("%"))
                    {
                        marginLeft = marginRight = ParseValue(value_2);
                    }
                    else
                    {
                        marginLeft = marginRight = parentWidth / 100 * int.Parse(value_2[..^1]);
                    }
                    
                    if (!value_3.EndsWith("%"))
                    {
                        marginBottom = ParseValue(value_3);
                    }
                    else
                    {
                        marginBottom = parentWidth / 100 * int.Parse(value_3[..^1]);
                    }
                    break;
                case 4:
                    var value_11 = marginArray[0];
                    var value_12 = marginArray[1];
                    var value_13 = marginArray[2];
                    var value_14 = marginArray[3];
                    
                    if (!value_11.EndsWith("%"))
                    {
                        marginTop = ParseValue(value_11);
                    }
                    else
                    {
                        marginTop = parentWidth / 100 * int.Parse(value_11[..^1]);
                    }
                    
                    if (!value_12.EndsWith("%"))
                    {
                        marginRight = ParseValue(value_12);
                    }
                    else
                    {
                        marginRight = parentWidth / 100 * int.Parse(value_12[..^1]);
                    }
                    
                    if (!value_13.EndsWith("%"))
                    {
                        marginBottom = ParseValue(value_13);
                    }
                    else
                    {
                        marginBottom = parentWidth / 100 * int.Parse(value_13[..^1]);
                    }
                    
                    if (!value_14.EndsWith("%"))
                    {
                        marginLeft = ParseValue(value_14);
                    }
                    else
                    {
                        marginLeft = parentWidth / 100 * int.Parse(value_14[..^1]);
                    }
                    
                    break;
                default:
                    break;
            }
        }
        
        int paddingLeft, paddingRight, paddingTop, paddingBottom;
        paddingLeft = paddingRight = paddingTop = paddingBottom = 0;

        document.GetMap()[node.ParentNode].getMap().TryGetValue("padding", out var padding);

        if (!string.IsNullOrEmpty(padding))
        {
            var paddingArray = padding.Split(" ");

            switch (paddingArray.Length)
            {
                case 1:
                    var value = paddingArray[0];
                    if (!value.EndsWith("%"))
                    {
                        paddingLeft = paddingRight = paddingTop = paddingBottom = ParseValue(value);
                    }
                    else
                    {
                        paddingLeft = paddingRight = paddingTop = paddingBottom = parentWidth / 100 * int.Parse(value[..^1]);
                    }
                    break;

                case 2:
                    var value1 = paddingArray[0];
                    var value2 = paddingArray[1];
                    
                    if (!value1.EndsWith("%"))
                    {
                        paddingTop = paddingBottom = ParseValue(value1);
                    }
                    else
                    {
                        paddingTop = paddingBottom = parentWidth / 100 * int.Parse(value1[..^1]);
                    }
                    
                    if (!value2.EndsWith("%"))
                    {
                        paddingLeft = paddingRight = ParseValue(value2);
                    }
                    else
                    {
                        paddingLeft = paddingRight = parentWidth / 100 * int.Parse(value2[..^1]);
                    }
                    
                    break;
                case 3:
                    var value_1 = paddingArray[0];
                    var value_2 = paddingArray[1];
                    var value_3 = paddingArray[2];
                    
                    if (!value_1.EndsWith("%"))
                    {
                        paddingTop = ParseValue(value_1);
                    }
                    else
                    {
                        paddingTop = parentWidth / 100 * int.Parse(value_1[..^1]);
                    }
                    
                    if (!value_2.EndsWith("%"))
                    {
                        paddingLeft = paddingRight = ParseValue(value_2);
                    }
                    else
                    {
                        paddingLeft = paddingRight = parentWidth / 100 * int.Parse(value_2[..^1]);
                    }
                    
                    if (!value_3.EndsWith("%"))
                    {
                        paddingBottom = ParseValue(value_3);
                    }
                    else
                    {
                        paddingBottom = parentWidth / 100 * int.Parse(value_3[..^1]);
                    }
                    break;
                case 4:
                    var value_11 = paddingArray[0];
                    var value_12 = paddingArray[1];
                    var value_13 = paddingArray[2];
                    var value_14 = paddingArray[3];
                    
                    if (!value_11.EndsWith("%"))
                    {
                        paddingTop = ParseValue(value_11);
                    }
                    else
                    {
                        paddingTop = parentWidth / 100 * int.Parse(value_11[..^1]);
                    }
                    
                    if (!value_12.EndsWith("%"))
                    {
                        paddingRight = ParseValue(value_12);
                    }
                    else
                    {
                        paddingRight = parentWidth / 100 * int.Parse(value_12[..^1]);
                    }
                    
                    if (!value_13.EndsWith("%"))
                    {
                        paddingBottom = ParseValue(value_13);
                    }
                    else
                    {
                        paddingBottom = parentWidth / 100 * int.Parse(value_13[..^1]);
                    }
                    
                    if (!value_14.EndsWith("%"))
                    {
                        paddingLeft = ParseValue(value_14);
                    }
                    else
                    {
                        paddingLeft = parentWidth / 100 * int.Parse(value_14[..^1]);
                    }
                    
                    break;
                default:
                    break;
            }
        }
        
        elem.Map.getMap().TryGetValue("padding-right", out var padding_right);
        if (!string.IsNullOrEmpty(padding_right))
        {
            if (!padding_right.EndsWith("%"))
            {
                marginRight = ParseValue(padding_right);
            }
            else
            {
                marginRight = parentWidth / 100 * int.Parse(padding_right[..^1]);
            }
        }
        
        elem.Map.getMap().TryGetValue("padding-top", out var padding_top);
        if (!string.IsNullOrEmpty(padding_top))
        {
            if (!padding_top.EndsWith("%"))
            {
                marginTop = ParseValue(padding_top);
            }
            else
            {
                marginTop = parentWidth / 100 * int.Parse(padding_top[..^1]);
            }
        }
        
        elem.Map.getMap().TryGetValue("padding-left", out var padding_left);
        if (!string.IsNullOrEmpty(padding_left))
        {
            if (!padding_left.EndsWith("%"))
            {
                marginLeft = ParseValue(padding_left);
            }
            else
            {
                marginLeft = parentWidth / 100 * int.Parse(padding_left[..^1]);
            }
        }
        
        elem.Map.getMap().TryGetValue("padding-bottom", out var padding_bottom);
        if (!string.IsNullOrEmpty(padding_bottom))
        {
            if (!padding_bottom.EndsWith("%"))
            {
                marginBottom = ParseValue(padding_bottom);
            }
            else
            {
                marginBottom = parentWidth / 100 * int.Parse(padding_bottom[..^1]);
            }
        }

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
            rect.top = parentObject.Rectangle.top + marginTop + paddingTop;
        }

        elem.Rectangle = rect;
        return elem;
    }

    private RenderObject MakeText(HtmlNode node, RenderObject parentObject, string text)
    {
        var elem = new TextObject(text) // todo add padding too
        {
            Map = document.GetMap()[node],
        };

        var rect = new Rect();
        
        // todo check parent
        rect.left = parentObject.Rectangle.left;
        rect.right = parentObject.Rectangle.right + text.Length * 3;
        rect.top = parentObject.Rectangle.top + 14;
        
        elem.Rectangle = rect;
        return elem;
    }

    private int ParseValue(string text)
    {

        if (text.Equals("0"))
        {
            return 0;
        }

        var symbols = text[^2..];

        return symbols switch
        {
            "px" => int.Parse(text[..^2]),
            "vw" => viewport / 100 * int.Parse(text[..^2]),
            _ => 0
        };
    }
}