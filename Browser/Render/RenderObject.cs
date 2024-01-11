using Browser.CSS;
using Browser.Networking;
using SkiaSharp;

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

public enum RenderObjectType {
    Block,
    Inline,
    Flex,
    InlineBlock
}

public abstract class RenderObjectBackground
{
    public abstract void DoRender(RenderObject obj, ObjectRenderer renderer);
}

public class RenderObjectDefaultBackground : RenderObjectBackground
{
    public override void DoRender(RenderObject obj, ObjectRenderer renderer)
    {
        Rect rect = obj.Rectangle;
        renderer.drawDefaultRect(new SKColor(0,0,0,0),rect.left, rect.top, rect.Width(), rect.Height());
    }
}
public class RenderObjectImageBackground(Resource image) : RenderObjectBackground
{
    public Resource Image { get; } = image;

    public override void DoRender(RenderObject obj, ObjectRenderer renderer)
    {
        Rect rect = obj.Rectangle;
        if (this.Image.localPath != null) renderer.drawImage(rect.left, rect.top, this.Image.localPath);
    }
}

public class RenderObjectSolidColorBackground(SKColor backColor) : RenderObjectBackground
{
    public SKColor BackColor { get; } = backColor;

    public override void DoRender(RenderObject obj, ObjectRenderer renderer)
    {
        Rect rect = obj.Rectangle;
        renderer.drawDefaultRect(BackColor,rect.left, rect.top, rect.Width(), rect.Height());
    }
}

public class RenderObject // todo image object
{
    public Rect Rectangle { get; set; }
    public CSSAttrMap Map { get; set; }
    public RenderObjectType ObjectType { get; set; }

    public List<RenderObjectBackground> BackgroundObjects { get; set; }

    public virtual void DoRender(ObjectRenderer renderer)
    {
        this.BackgroundObjects.ForEach(b => b.DoRender(this, renderer));
    }
}

public class TextObject : RenderObject
{
    public string Text { get; set; }

    public int textSize { set; get; }

    public TextObject(string text, int textSize)
    {
        Text = text;
        this.textSize = textSize;
    }

    public override void DoRender(ObjectRenderer renderer)
    {
        Rect rect = this.Rectangle;
        string text = this.Text;
                    
        this.Map.getMap().TryGetValue("text-decoration", out var underline);
        var under = !string.IsNullOrEmpty(underline) && underline.Equals("underline");
                    
        SKColor sColor;
        this.Map.getMap().TryGetValue("color", out var color);
        try
        {
            sColor = SKColor.Parse(color.ToUpper());
        }
        catch (Exception e)
        {
            sColor = SKColors.Black;
        }

        renderer.drawText(sColor, rect, text, textSize, under);
    }
}

public class ImageObject : RenderObject
{
    public string LocalPath { get; set; }

    public ImageObject(string localPath)
    {
        LocalPath = localPath;
    }

    public override void DoRender(ObjectRenderer renderer)
    {
        Rect rect = this.Rectangle;
        try
        {
            if (this.LocalPath != null) 
                renderer.drawImage( rect.left, rect.top, this.LocalPath);
        }
        catch (Exception)
        {
            Console.WriteLine($"Bad resource: {this.LocalPath}");
        }
    }
}