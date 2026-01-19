namespace WeatherNow.Drawables;

// https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/bindable-properties?view=net-maui-10.0

// ts is painful
public class MeterDrawable : IDrawable
{
    public double Progress { get; set; } // from 0.0 to 1.0
    public float Thickness { get; set; } = 10;

    public Color BaseColor { get; set; } = Color.FromArgb("#DAD7CD");
    public Color ProgressColor { get; set; } = Color.FromArgb("#FF8A80");

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.Antialias = true;
        canvas.StrokeLineCap = LineCap.Round;
        canvas.StrokeSize = Thickness;

        // how much space the circle can fit in
        float effectiveSize = Math.Min(dirtyRect.Width, dirtyRect.Height);
        float circleRadius = effectiveSize / 2 * 0.8f; // take up 80% of available space

        float containerCenterX = dirtyRect.Width / 2;
        float containerCenterY = dirtyRect.Height / 2;

        // unfilled base color
        canvas.StrokeColor = BaseColor;
        canvas.DrawCircle(containerCenterX, containerCenterY, circleRadius);

        // pain.. just defining how the arc will be drawn OVER the previous circle
        float circleDiameter = circleRadius * 2;
        RectF arcDimensions = new RectF(
            containerCenterX - circleRadius, // calculate left "corner"
            containerCenterY - circleRadius, // calculate top "corner"
            circleDiameter,
            circleDiameter
        );
        
        float sweepAngle = (float)(Progress * 360);

        canvas.StrokeColor = ProgressColor; // filling color
        canvas.DrawArc(arcDimensions, 90, 90 + sweepAngle, false, false);
    }
}