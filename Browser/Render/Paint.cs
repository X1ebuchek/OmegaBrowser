using Browser.Management;
using Topten.RichTextKit;

namespace Browser.Render;

using System;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
// using System.Windows.Forms;

public class Paint
{
    private static VScrollBar verticalScrollBar;
    private static int textSize = (int)Layout.paint.TextSize;
    public static void paint(Tab tab, List<RenderObject> list)
    {
        var form = new Form
        {
            ClientSize = new System.Drawing.Size(980, 500),
            Text = "OmegaBrowser",
            FormBorderStyle = FormBorderStyle.FixedDialog
        };
        
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
                else
                {
                    Rect rect = obj.Rectangle;
                    obj.Map.getMap().TryGetValue("background-color", out var backColor);
                    obj.Map.getMap().TryGetValue("Color", out var color);
                    
                    var r = new Random();
                    int A = r.Next(1000, 5000);
                    string hexValue1 = A.ToString("X");

                    if (!string.IsNullOrEmpty(backColor))
                    {
                        try
                        {
                            SKColor sColor = SKColor.Parse(backColor.ToUpper());
                            drawDefaultRect(canvas, sColor,rect.left, rect.top, rect.Width(), rect.Height());
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                        }
                        
                        //Console.WriteLine(backColor + " " + SKColor.Parse(backColor));
                    }
                    else if (!string.IsNullOrEmpty(color))
                    {
                        try
                        {
                            drawDefaultRect(canvas, SKColor.Parse(color.ToUpper()),rect.left, rect.top, rect.Width(), rect.Height());
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                        }
                    }
                    else
                    {
                        drawDefaultRect(canvas, new SKColor(0,0,0,0),rect.left, rect.top, rect.Width(), rect.Height());
                        Console.WriteLine(hexValue1);
                    }
                    
                }
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
        var p = Layout.paint;
        canvas.DrawText(text,rect.left,rect.bottom, p);
    }

    public static void drawImage(SKCanvas canvas, float x, float y, string path)
    {
        SKBitmap bitmap = SKBitmap.Decode(path);
        canvas.DrawBitmap(bitmap, new SKPoint(x,y));
    }
}