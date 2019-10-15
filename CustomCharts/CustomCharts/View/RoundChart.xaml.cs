using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CustomCharts.View
{
#if Release
    [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    public partial class RoundChart : Frame
    {
        private const float startAngle = 135;
        private const float backgroundSweepAngle = 270;

        private float dataSweepAngle = 1;

        public static readonly BindableProperty MaxValueProperty =
            BindableProperty.Create(nameof(MaxValue), typeof(float), typeof(RoundChart),
                defaultValue: 100f, propertyChanged: RefreshChart);

        public static readonly BindableProperty CurrentValueProperty =
            BindableProperty.Create(nameof(MaxValue), typeof(float), typeof(RoundChart),
                defaultValue: 0f, propertyChanged: RefreshChart);

        public static readonly BindableProperty ForegroundColorProperty =
            BindableProperty.Create(nameof(ForegroundColor), typeof(Color), typeof(RoundChart),
                defaultValue: Color.FromRgb(138, 107, 174), propertyChanged: RefreshChart);

        public float MaxValue
        {
            get => (float)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public float CurrentValue
        {
            get => (float)GetValue(CurrentValueProperty);
            set => SetValue(CurrentValueProperty, value);
        }

        public Color ForegroundColor
        {
            get => (Color)GetValue(ForegroundColorProperty);
            set => SetValue(ForegroundColorProperty, value);
        }

        public RoundChart()
        {
            InitializeComponent();
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;
            var diferenceX = info.Width > info.Height ? (info.Width - info.Height) / 2 : 0;
            var diferenceY = info.Height > info.Width ? (info.Height - info.Width) / 2 : 0;
            var strokeWidth = (4 * (info.Width - diferenceX)) / 100;

            var rect = new SKRect(
                5 + strokeWidth + diferenceX,
                5 + strokeWidth + diferenceY,
                info.Width - (5 + strokeWidth + diferenceX),
                info.Height - (5 + strokeWidth + diferenceY));

            canvas.Clear();

            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.LightGray.ToSKColor(),
                StrokeWidth = strokeWidth,
                IsAntialias = true,
                IsStroke = true
            };

            using (SKPath backgroundPath = new SKPath())
            {
                backgroundPath.AddArc(rect, startAngle, backgroundSweepAngle);
                canvas.DrawPath(backgroundPath, paint);
            }

            paint.StrokeWidth = (8 * (info.Width - diferenceX)) / 100;
            paint.Color = ForegroundColor.ToSKColor();

            using (SKPath dataPath = new SKPath())
            {
                dataPath.AddArc(rect, startAngle, dataSweepAngle);
                canvas.DrawPath(dataPath, paint);
            }
        }

        private static void RefreshChart(BindableObject bindable, object oldValue, object newValue)
        {
            if(bindable is RoundChart roundChart)
            {
                roundChart.RefreshChartValues(roundChart.CurrentValue, roundChart.MaxValue);
                roundChart.canvasView.InvalidateSurface();
                roundChart.valueLabel.Text = $"{roundChart.CurrentValue}/{roundChart.MaxValue}";
            }
        }

        private void RefreshChartValues(float currentValue, float maxValue)
        {
            if (currentValue >= maxValue)
                dataSweepAngle = backgroundSweepAngle;
            if (currentValue <= 0)
                dataSweepAngle = 1;

            var valuePercent = (currentValue * 100f) / maxValue;
            dataSweepAngle = (valuePercent * backgroundSweepAngle) / 100f;
        }

        public async Task AnimateAsync(int totalMs = 500)
        {
            var animTime = totalMs < 10 ? 500 : totalMs;
            var currentMs = 0;
            while (currentMs <= animTime)
            {
                var valueStep = (currentMs * CurrentValue) / animTime;
                RefreshChartValues(valueStep, MaxValue);
                canvasView.InvalidateSurface();
                valueLabel.Text = $"{Math.Truncate(valueStep)}/{MaxValue}";
                currentMs += 10;
                await Task.Delay(10);
            }
        }
    }
}