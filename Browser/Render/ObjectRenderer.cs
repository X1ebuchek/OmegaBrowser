using System.Text.RegularExpressions;
using Browser.Management;
using Browser.Networking;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace Browser.Render;

public class ObjectRenderer
{
    private VScrollBar verticalScrollBar;

    private SKBitmap bufferBitmap;
    private SKCanvas bufferCanvas;

    private readonly Size browserSize = new(980, 500);
    
    public ObjectRenderer(Tab tab, List<RenderObject> list)
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
                this.DrawAllObjects(tab, list);
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
    
    private void DrawAllObjects(Tab tab, List<RenderObject> list)
    {
        foreach (var obj in list)
        {
            obj.DoRender(this);
        }
    }
    
    public void drawDefaultRect( SKColor color, float x, float y, float w, float h)
    {
        SKPaint p = new() { Color = color };
        bufferCanvas.DrawRect(x, y, w, h, p);
    }
    public void drawDefaultRectWithBorder( SKColor color, float x, float y, float w, float h, SKColor borderColor, float borderWidth)
    {
        SKPaint p = new() { Color = color };
        bufferCanvas.DrawRect(x, y, w, h, p);
        
        SKPaint borderPaint = new SKPaint
        {
            Color = borderColor,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = borderWidth
        };
        bufferCanvas.DrawRect(x,y,w,h,borderPaint);
    }
    

    public void drawRoundRect(SKColor color, float x, float y, float w, float h, float cornerRadius)
    {
        SKPaint p = new() { Color = color };
        SKRect rect = new SKRect(x, y, x+w, y+h);
        bufferCanvas.DrawRoundRect(rect, cornerRadius, cornerRadius, p);
    }
    public void drawRoundRectWithBorder(SKColor color, float x, float y, float w, float h,
        float cornerRadius, SKColor borderColor, float borderWidth)
    {
        SKPaint p = new() { Color = color };
        SKRect rect = new SKRect(x, y, x+w, y+h);
        bufferCanvas.DrawRoundRect(rect, cornerRadius, cornerRadius, p);
        
        SKPaint borderPaint = new SKPaint
        {
            Color = borderColor,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = borderWidth
        };
        bufferCanvas.DrawRoundRect(rect, cornerRadius, cornerRadius, borderPaint);
    }
    

    public void drawText(SKColor color, Rect rect, string text, float textSize, bool under)
    {
        var p = new SKPaint()
        {
            Color = SKColors.Black,
            TextSize = textSize,
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("Times New Roman")
        };;
        p.Color = color;
        bufferCanvas.DrawText(text,rect.left,rect.bottom, p);

        if (under)
        {
            float lineY = rect.bottom + 2.0f;

            bufferCanvas.DrawLine(rect.left, lineY, rect.right, lineY, p);
        }
    }

    public void drawImage(float x, float y, string path)
    {
        SKBitmap bitmap = SKBitmap.Decode(path);
        bufferCanvas.DrawBitmap(bitmap, new SKPoint(x,y));
    }
}