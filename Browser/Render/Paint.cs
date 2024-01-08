using System.Text.RegularExpressions;
using Browser.Management;
using Browser.Networking;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace Browser.Render;

public class Paint
{
    private static VScrollBar verticalScrollBar;
    private static int textSize = (int)Layout.paint.TextSize;

    private static SKBitmap bufferBitmap;
    private static SKCanvas bufferCanvas;

    private static readonly Size browserSize = new(980, 500);
    
    public static void paint(Tab tab, List<RenderObject> list)
    {
        var form = new Form
        {
            ClientSize = browserSize,
            Text = "OmegaBrowser",
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false
        };
        
        var skiaPanel = new SKControl();
        skiaPanel.Height = list[0].Rectangle.Height();
        skiaPanel.Width = browserSize.Width - 20;
        skiaPanel.PaintSurface += (sender, e) =>
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear(SKColor.Parse("#ffffff"));
            if (bufferBitmap == null)
            {
                // Инициализируем буферный холст и его канвас
                bufferBitmap = new SKBitmap(skiaPanel.Width, skiaPanel.Height);
                bufferCanvas = new SKCanvas(bufferBitmap);
                // Отрисовываем все объекты на буфере
                DrawAllObjects(tab, bufferCanvas, list);
            }

            // Определяем видимую область
            var visibleRect = new SKRectI(0, verticalScrollBar.Value, 
                skiaPanel.Width, browserSize.Height+verticalScrollBar.Value);
            
            var defaultRect = new SKRectI(0, 0, 
                skiaPanel.Width, browserSize.Height);
    
            // Отображаем только видимую область из буфера
            canvas.DrawBitmap(bufferBitmap, visibleRect, defaultRect);
            
        };
        
        verticalScrollBar = new VScrollBar();
        verticalScrollBar.Dock = DockStyle.Right;
        verticalScrollBar.Width = 20;
        verticalScrollBar.Maximum = list[0].Rectangle.Height() - (browserSize.Height - 100);
        verticalScrollBar.SmallChange = 50; 
        verticalScrollBar.LargeChange = 100;
        verticalScrollBar.Scroll += (sender, e) =>
        {
            skiaPanel.Invalidate();
        };
        
        form.Controls.Add(skiaPanel);
        form.Controls.Add(verticalScrollBar);
        Application.Run(form);
        
        
    }
    
    private static void DrawAllObjects(Tab tab, SKCanvas canvas, List<RenderObject> list)
    {
        foreach (var obj in list)
        {
            // Console.WriteLine(obj.Rectangle.left + " " + obj.Rectangle.top);
            if (obj.GetType() == typeof(TextObject))
            {
                Rect rect = obj.Rectangle;
                string text = ((TextObject)obj).Text;
                    
                obj.Map.getMap().TryGetValue("text-decoration", out var underline);
                var under = !string.IsNullOrEmpty(underline) && underline.Equals("underline");
                    
                SKColor sColor;
                obj.Map.getMap().TryGetValue("color", out var color);
                try
                {
                    sColor = SKColor.Parse(color.ToUpper());
                }
                catch (Exception e)
                {
                    sColor = SKColors.Black;
                }

                drawText(canvas, sColor, rect, text, textSize, under);
                    
            }
            else if (obj.GetType() == typeof(ImageObject))
            {
                Console.WriteLine("lolololoollollollplloolooloooolo");
                Rect rect = obj.Rectangle;
                //Resource res = new Resource(((ImageObject)obj).LocalPath, Resource.ResourceType.Img);
                try
                {
                    //tab.owner.resourceManager.GetResource(ref res);
                    if (((ImageObject)obj).LocalPath != null) drawImage(canvas, rect.left, rect.top, ((ImageObject)obj).LocalPath);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Bad resource: {((ImageObject)obj).LocalPath}");
                }
            }
            else
            {
                Rect rect = obj.Rectangle;
                obj.Map.getMap().TryGetValue("background-color", out var backColor);
                    
                obj.Map.getMap().TryGetValue("background-image", out var backImg);
                    
                var r = new Random();
                int A = r.Next(1000, 5000);
                string hexValue1 = A.ToString("X");

                string pattern = @"url\(([^)]+)\)";

                Regex regex = new Regex(pattern);
                if (backImg != null)
                {
                    Match match = regex.Match(backImg);

                    if (match.Success)
                    {
                        string url = match.Groups[1].Value;
                        Resource res = new Resource(url, Resource.ResourceType.Img);
                                    
                        //Console.WriteLine("Extracted URL: " + url);
                            
                        try
                        {
                            tab.owner.resourceManager.GetResource(ref res);
                            if (res.localPath != null) drawImage(canvas, rect.left, rect.top, res.localPath);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"Bad resource: {url}");
                        }
                    }
                }

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
                else
                {
                    drawDefaultRect(canvas, new SKColor(0,0,0,0),rect.left, rect.top, rect.Width(), rect.Height());
                    Console.WriteLine(hexValue1);
                }
                    
            }
        }
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
    

    public static void drawText(SKCanvas canvas, SKColor color, Rect rect, string text, float textSize, bool under)
    {
        var p = Layout.paint;
        p.Color = color;
        canvas.DrawText(text,rect.left,rect.bottom, p);

        if (under)
        {
            float lineY = rect.bottom + 2.0f;

            canvas.DrawLine(rect.left, lineY, rect.right, lineY, p);
        }
    }

    public static void drawImage(SKCanvas canvas, float x, float y, string path)
    {
        SKBitmap bitmap = SKBitmap.Decode(path);
        canvas.DrawBitmap(bitmap, new SKPoint(x,y));
    }
}