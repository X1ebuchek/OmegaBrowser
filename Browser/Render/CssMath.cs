using SkiaSharp;

namespace Browser.Render;

public class CssMath
{

    public static string SplitTextLines(string text, int width, SKPaint paint, bool firstTimeStop = false)
    {
        var outString = "";
        var cacheString = "";

        SKRect size = new();
        paint.MeasureText(text, ref size);
        
        var counter = 0;
        while (counter < text.Length)
        {
            var c = text[counter];
            paint.MeasureText(cacheString + c, ref size);
            if (size.Width > width) //TextRenderer.MeasureText(text, font)
            {
                if (firstTimeStop)
                {
                    return outString;
                }
                outString += "\n";
                cacheString = c.ToString();
            }
            else
            {
                cacheString += c;
            }

            outString += c;
            counter++;
        }

        return outString;
    }
    
    public static int ParseValue(string text, int viewport)
    {

        if (text.Equals("0") || text.EndsWith("rem"))
        {
            return 0;
        }

        if (text.StartsWith("."))
        {
            text = "0" + text;
        }

        var symbols = text[^2..];

        return symbols switch
        {
            "px" => int.Parse(text[..^2]),
            "vw" => viewport * int.Parse(text[..^2]) / 100 ,
            "em" => (int)(16 * float.Parse(text[..^2].Replace(".", ","))),
            _ => 0
        };
    }

    public static void GetFontSize(string fs, int viewport, out int size)
    {
        var value = fs.Split(" ")[0];
        if (!value.EndsWith("%"))
        {
           size = ParseValue(value, viewport);
        }
        else
        {
            size = 16 * int.Parse(value[..^1]) / 100;
        }

    }
    
    public static void GetMargin(Dictionary<string, string> map, int parentWidth, int viewport,
        out int marginLeft, out int marginRight, out int marginTop, out int marginBottom)
    {
        marginLeft = marginRight = marginTop = marginBottom = 0;
        
        map.TryGetValue("margin", out var margin);
        if (!string.IsNullOrEmpty(margin))
        {
            var marginArray = margin.Split(" ");

            switch (marginArray.Length)
            {
                case 1:
                    var value = marginArray[0];
                    if (!value.EndsWith("%"))
                    {
                        marginLeft = marginRight = marginTop = marginBottom = ParseValue(value, viewport);
                    }
                    else
                    {
                        marginLeft = marginRight =
                            marginTop = marginBottom = parentWidth / 100 * int.Parse(value[..^1]);
                    }

                    break;

                case 2:
                    var value1 = marginArray[0];
                    var value2 = marginArray[1];

                    if (!value1.EndsWith("%"))
                    {
                        marginTop = marginBottom = ParseValue(value1, viewport);
                    }
                    else
                    {
                        marginTop = marginBottom = parentWidth / 100 * int.Parse(value1[..^1]);
                    }

                    if (!value2.EndsWith("%"))
                    {
                        marginLeft = marginRight = ParseValue(value2, viewport);
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
                        marginTop = ParseValue(value_1, viewport);
                    }
                    else
                    {
                        marginTop = parentWidth / 100 * int.Parse(value_1[..^1]);
                    }

                    if (!value_2.EndsWith("%"))
                    {
                        marginLeft = marginRight = ParseValue(value_2, viewport);
                    }
                    else
                    {
                        marginLeft = marginRight = parentWidth / 100 * int.Parse(value_2[..^1]);
                    }

                    if (!value_3.EndsWith("%"))
                    {
                        marginBottom = ParseValue(value_3, viewport);
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
                        marginTop = ParseValue(value_11, viewport);
                    }
                    else
                    {
                        marginTop = parentWidth / 100 * int.Parse(value_11[..^1]);
                    }

                    if (!value_12.EndsWith("%"))
                    {
                        marginRight = ParseValue(value_12, viewport);
                    }
                    else
                    {
                        marginRight = parentWidth / 100 * int.Parse(value_12[..^1]);
                    }

                    if (!value_13.EndsWith("%"))
                    {
                        marginBottom = ParseValue(value_13, viewport);
                    }
                    else
                    {
                        marginBottom = parentWidth / 100 * int.Parse(value_13[..^1]);
                    }

                    if (!value_14.EndsWith("%"))
                    {
                        marginLeft = ParseValue(value_14, viewport);
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

        map.TryGetValue("margin-right", out var margin_right);
        if (!string.IsNullOrEmpty(margin_right))
        {
            if (!margin_right.EndsWith("%"))
            {
                marginRight = ParseValue(margin_right, viewport);
            }
            else
            {
                marginRight = parentWidth / 100 * int.Parse(margin_right[..^1]);
            }
        }
        
        map.TryGetValue("margin-top", out var margin_top);
        if (!string.IsNullOrEmpty(margin_top))
        {
            if (!margin_top.EndsWith("%"))
            {
                marginTop = ParseValue(margin_top, viewport);
            }
            else
            {
                marginTop = parentWidth / 100 * int.Parse(margin_top[..^1]);
            }
        }
        
        map.TryGetValue("margin-left", out var margin_left);
        if (!string.IsNullOrEmpty(margin_left))
        {
            if (!margin_left.EndsWith("%"))
            {
                marginLeft = ParseValue(margin_left, viewport);
            }
            else
            {
                marginLeft = parentWidth / 100 * int.Parse(margin_left[..^1]);
            }
        }
        
        map.TryGetValue("margin-bottom", out var margin_bottom);
        if (!string.IsNullOrEmpty(margin_bottom))
        {
            if (!margin_bottom.EndsWith("%"))
            {
                marginBottom = ParseValue(margin_bottom, viewport);
            }
            else
            {
                marginBottom = parentWidth / 100 * int.Parse(margin_bottom[..^1]);
            }
        }
    }

    
    
    public static void GetPadding(Dictionary<string, string> map, int parentWidth, int viewport,
        out int paddingLeft, out int paddingRight, out int paddingTop, out int paddingBottom)
    {
        
        paddingLeft = paddingRight = paddingTop = paddingBottom = 0;

        map.TryGetValue("padding", out var padding);
        if (string.IsNullOrEmpty(padding)) return;
        var paddingArray = padding.Split(" ");

        switch (paddingArray.Length)
        {
            case 1:
                var value = paddingArray[0];
                if (!value.EndsWith("%"))
                {
                    paddingLeft = paddingRight = paddingTop = paddingBottom = ParseValue(value, viewport);
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
                    paddingTop = paddingBottom = ParseValue(value1, viewport);
                }
                else
                {
                    paddingTop = paddingBottom = parentWidth / 100 * int.Parse(value1[..^1]);
                }
                    
                if (!value2.EndsWith("%"))
                {
                    paddingLeft = paddingRight = ParseValue(value2, viewport);
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
                    paddingTop = ParseValue(value_1, viewport);
                }
                else
                {
                    paddingTop = parentWidth / 100 * int.Parse(value_1[..^1]);
                }
                    
                if (!value_2.EndsWith("%"))
                {
                    paddingLeft = paddingRight = ParseValue(value_2, viewport);
                }
                else
                {
                    paddingLeft = paddingRight = parentWidth / 100 * int.Parse(value_2[..^1]);
                }
                    
                if (!value_3.EndsWith("%"))
                {
                    paddingBottom = ParseValue(value_3, viewport);
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
                    paddingTop = ParseValue(value_11, viewport);
                }
                else
                {
                    paddingTop = parentWidth / 100 * int.Parse(value_11[..^1]);
                }
                    
                if (!value_12.EndsWith("%"))
                {
                    paddingRight = ParseValue(value_12, viewport);
                }
                else
                {
                    paddingRight = parentWidth / 100 * int.Parse(value_12[..^1]);
                }
                    
                if (!value_13.EndsWith("%"))
                {
                    paddingBottom = ParseValue(value_13, viewport);
                }
                else
                {
                    paddingBottom = parentWidth / 100 * int.Parse(value_13[..^1]);
                }
                    
                if (!value_14.EndsWith("%"))
                {
                    paddingLeft = ParseValue(value_14, viewport);
                }
                else
                {
                    paddingLeft = parentWidth / 100 * int.Parse(value_14[..^1]);
                }
                    
                break;
            default:
                break;
        }
        
        map.TryGetValue("padding-right", out var padding_right);
        if (!string.IsNullOrEmpty(padding_right))
        {
            if (!padding_right.EndsWith("%"))
            {
                paddingRight = ParseValue(padding_right, viewport);
            }
            else
            {
                paddingRight = parentWidth / 100 * int.Parse(padding_right[..^1]);
            }
        }
        
        map.TryGetValue("padding-top", out var padding_top);
        if (!string.IsNullOrEmpty(padding_top))
        {
            if (!padding_top.EndsWith("%"))
            {
                paddingTop = ParseValue(padding_top, viewport);
            }
            else
            {
                paddingTop = parentWidth / 100 * int.Parse(padding_top[..^1]);
            }
        }
        
        map.TryGetValue("padding-left", out var padding_left);
        if (!string.IsNullOrEmpty(padding_left))
        {
            if (!padding_left.EndsWith("%"))
            {
                paddingLeft = ParseValue(padding_left, viewport);
            }
            else
            {
                paddingLeft = parentWidth / 100 * int.Parse(padding_left[..^1]);
            }
        }
        
        map.TryGetValue("padding-bottom", out var padding_bottom);
        if (!string.IsNullOrEmpty(padding_bottom))
        {
            if (!padding_bottom.EndsWith("%"))
            {
                paddingBottom = ParseValue(padding_bottom, viewport);
            }
            else
            {
                paddingBottom = parentWidth / 100 * int.Parse(padding_bottom[..^1]);
            }
        }
    }
}