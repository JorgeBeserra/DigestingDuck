using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using IqOptionApiDotNet.Models;

namespace DigestingDuck.Desktop;
public sealed class CandleChart : Control
{
    private CandleInfo[] values = [];
    public void SetCandles(IEnumerable<CandleInfo> candles)
    {
        values = candles.OrderBy(x => x.From).TakeLast(60).ToArray();
        InvalidateVisual();
    }
    public override void Render(DrawingContext context)
    {
        base.Render(context);
        context.DrawRectangle(new SolidColorBrush(Color.Parse("#1C2332")), null, new Rect(Bounds.Size), 12, 12);
        if (values.Length == 0)
        {
            var text = new FormattedText("Selecione um ativo e consulte as velas.", CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, new Typeface("Arial"), 14, Brushes.LightGray);
            context.DrawText(text, new Point(24, 24));
            return;
        }
        var min = values.Min(x => x.Min); var max = values.Max(x => x.Max);
        var range = Math.Max(max - min, 0.000001);
        double Y(double v) => 24 + (max - v) / range * Math.Max(1, Bounds.Height - 48);
        var step = Math.Max(1, (Bounds.Width - 90) / values.Length);
        for (var grid = 0; grid <= 4; grid++)
        {
            var value = min + range * grid / 4;
            var y = Y(value);
            context.DrawLine(new Pen(new SolidColorBrush(Color.Parse("#30394C")), 1), new Point(15,y), new Point(Bounds.Width - 70,y));
            context.DrawText(new FormattedText(value.ToString("N5"), CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Arial"), 11, Brushes.LightGray), new Point(Bounds.Width - 66, y - 8));
        }
        for (var i = 0; i < values.Length; i++)
        {
            var candle = values[i]; var x = 20 + step * (i + 0.5);
            var brush = new SolidColorBrush(Color.Parse(candle.Close >= candle.Open ? "#7DDAB0" : "#F27B87"));
            context.DrawLine(new Pen(brush,1), new Point(x,Y(candle.Max)),new Point(x,Y(candle.Min)));
            var top = Math.Min(Y(candle.Open),Y(candle.Close));
            context.DrawRectangle(brush,null,new Rect(x-step*0.3,top,Math.Max(1,step*0.6),Math.Max(1,Math.Abs(Y(candle.Close)-Y(candle.Open)))));
        }
    }
}
