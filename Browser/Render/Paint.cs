using Topten.RichTextKit;

namespace Browser.Render;

using System;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
// using System.Windows.Forms;

public class Paint
{
    private static VScrollBar verticalScrollBar;
    private static int textSize = 14;
    public static void paint(List<RenderObject> list)
    {
        var form = new Form
        {
            ClientSize = new System.Drawing.Size(980, 500),
            Text = "OmegaBrowser",
            FormBorderStyle = FormBorderStyle.FixedDialog
        };

        SKRect tsize = new();
        Layout.paint.MeasureText("Abc", ref tsize);
        textSize = (int)tsize.Height;
        var skiaPanel = new SKControl();
        skiaPanel.Height = list[0].Rectangle.Height();
        skiaPanel.Width = 960;
        skiaPanel.PaintSurface += (sender, e) =>
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear(SKColor.Parse("#ffffff"));
            
            foreach (var obj in list)
            {
                // Console.WriteLine(obj.Rectangle.left + " " + obj.Rectangle.top);
                if (obj.GetType() == typeof(TextObject))
                {
                    Rect rect = obj.Rectangle;
                    string text = ((TextObject)obj).Text;
                    drawText(canvas, SKColors.Black, rect, text, textSize);
                }
                // else
                // {
                //     Rect rect = obj.Rectangle;
                //     var r = new Random();
                //     int A = r.Next(1000, 5000);
                //     string hexValue1 = A.ToString("X");
                //     drawDefaultRect(canvas, SKColor.Parse("#" + hexValue1), rect.left, rect.top, rect.Width(), rect.Height());
                // }
            }
            
            
        };
        
        verticalScrollBar = new VScrollBar();
        verticalScrollBar.Dock = DockStyle.Right;
        verticalScrollBar.Maximum = list[0].Rectangle.Height() - 400;
        verticalScrollBar.SmallChange = 50; 
        verticalScrollBar.LargeChange = 100;
        verticalScrollBar.Scroll += (sender, e) =>
        {
            skiaPanel.Top = -verticalScrollBar.Value;
        };
        
        form.Controls.Add(skiaPanel);
        form.Controls.Add(verticalScrollBar);
        Application.Run(form);
        
        
    }
    
     public static void drawDefaultRect(SKCanvas canvas, SKColor color, float x, float y, float w, float h)
    {
        SKPaint p = new() { Color = color };
        canvas.DrawRect(x, y, w, h, p);
    }
    public static void drawDefaultRectWithBorder(SKCanvas canvas, SKColor color, float x, float y, float w, float h, SKColor borderColor, float borderWidth)
    {
        SKPaint p = new() { Color = color };
        canvas.DrawRect(x, y, w, h, p);
        
        SKPaint borderPaint = new SKPaint
        {
            Color = borderColor,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = borderWidth
        };
        canvas.DrawRect(x,y,w,h,borderPaint);
    }
    

    public static void drawRoundRect(SKCanvas canvas, SKColor color, float x, float y, float w, float h,
        float cornerRadius)
    {
        SKPaint p = new() { Color = color };
        SKRect rect = new SKRect(x, y, x+w, y+h);
        canvas.DrawRoundRect(rect, cornerRadius, cornerRadius, p);
    }
    public static void drawRoundRectWithBorder(SKCanvas canvas, SKColor color, float x, float y, float w, float h,
        float cornerRadius, SKColor borderColor, float borderWidth)
    {
        SKPaint p = new() { Color = color };
        SKRect rect = new SKRect(x, y, x+w, y+h);
        canvas.DrawRoundRect(rect, cornerRadius, cornerRadius, p);
        
        SKPaint borderPaint = new SKPaint
        {
            Color = borderColor,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = borderWidth
        };
        canvas.DrawRoundRect(rect, cornerRadius, cornerRadius, borderPaint);
    }
    

    public static void drawText(SKCanvas canvas, SKColor color, Rect rect, string text, float textSize)
    {
        text = text.Replace("&nbsp;", " ")
            .Replace("&gt;", ">")
            .Replace("&lt;", "<");
        
        var p = Layout.paint;


        // var rs = new RichString()
        //     .Alignment(TextAlignment.Center)
        //     .FontFamily("Arial")
        //     // .MarginBottom(1).MarginLeft(1).MarginTop(1).MarginRight(1)
        //     .Add(text, fontSize: textSize, fontWeight: 400, fontItalic: false);
        //
        // rs.Paint(canvas, new SKPoint(rect.left, rect.top));
        // rs.MaxWidth = rect.Width();
        // rs.MaxHeight = rect.Height();

        var k = 0;
        foreach (var s in text.Split("\n"))
        {
            var d = s;
            for (int i = 0; i < d.Length; i++)
            {
                if (char.IsWhiteSpace(d[i]))
                {
                    d = d.Replace(d[i].ToString(), " ");
                }
            }
        
            while (d.Contains("  "))
            {
                d = d.Replace("  ", " ");
            }
            
            canvas.DrawText(d.Trim(),rect.left,rect.bottom+ k*textSize, p);
            k++;
        }

    }

    public static void drawImage(SKCanvas canvas, float x, float y, string path)
    {
        SKBitmap bitmap = SKBitmap.Decode(path);
        canvas.DrawBitmap(bitmap, new SKPoint(x,y));
    }
}