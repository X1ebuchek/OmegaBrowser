using Browser.CSS;

namespace Browser.Render;

public class Rect
{
    public int left { get; set; }
    public int top { get; set; }
    public int right { get; set;  }
    public int bottom { get; set; }

    public int Width()
    {
        return right - left;
    }

    public int Height()
    {
        return bottom - top;
    }
}

public class RenderObject // todo image object
{
    public Rect Rectangle { get; set; }
    public CSSAttrMap Map { get; set; }
}

public class TextObject : RenderObject
{
    public string Text { get; set; }

    public TextObject(string text)
    {
        Text = text;
    }
}