namespace WeatherNow.Controls;

public partial class ProgressMeter : ContentView
{
    public static readonly BindableProperty ProgressProperty =
    BindableProperty.Create(nameof(Progress), typeof(double), typeof(ProgressMeter), 0.0,
        propertyChanged: (bindable, oldValue, newValue) => {
            if (bindable is not ProgressMeter control) return;

            control.MeterDrawable.Progress = Math.Clamp((double) newValue, 0.0, 1.0);
            control.MeterGraphicsView.Invalidate();
        });

    public static readonly BindableProperty ThicknessProperty =
    BindableProperty.Create(nameof(Progress), typeof(float), typeof(ProgressMeter), 10f,
        propertyChanged: (bindable, oldValue, newValue) => {
            if (bindable is not ProgressMeter control) return;

            control.MeterDrawable.Thickness = (float)newValue;
            control.MeterGraphicsView.Invalidate();
        });

    public static readonly BindableProperty BaseColorProperty =
    BindableProperty.Create(nameof(BaseColor), typeof(Color), typeof(ProgressMeter), Color.FromArgb("#DAD7CD"),
        propertyChanged: (bindable, oldValue, newValue) => {
            if (bindable is not ProgressMeter control) return;

            control.MeterDrawable.BaseColor = (Color)newValue;
            control.MeterGraphicsView.Invalidate();
        });

    public static readonly BindableProperty ProgressColorProperty =
    BindableProperty.Create(nameof(ProgressColor), typeof(Color), typeof(ProgressMeter), Color.FromArgb("#FF8A80"),
        propertyChanged: (bindable, oldValue, newValue) => {
            if (bindable is not ProgressMeter control) return;

            control.MeterDrawable.ProgressColor = (Color)newValue;
            control.MeterGraphicsView.Invalidate();
        });


    public double Progress
    {
        get => (double)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public float Thickness
    {
        get => (float)GetValue(ThicknessProperty);
        set => SetValue(ThicknessProperty, value);
    }

    public Color BaseColor
    {
        get => (Color)GetValue(BaseColorProperty);
        set => SetValue(BaseColorProperty, value);
    }

    public Color ProgressColor
    {
        get => (Color)GetValue(ProgressColorProperty);
        set => SetValue(ProgressColorProperty, value);
    }

    public ProgressMeter()
	{
		InitializeComponent();
	}
}