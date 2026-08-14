using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetworkDiagram.Controls
{
    internal sealed class TrafficGraphControl : Control
    {
        private const int MaxPoints = 160;
        private const int GridLineCount = 4;
        private const double ScaleHeadroom = 1.12D;
        private const double MinScaleMax = 1024D;
        private static readonly TimeSpan AnimationDuration = TimeSpan.FromMilliseconds(220);
        private readonly Queue<int> mSentValues = new Queue<int>();
        private readonly Queue<int> mReceivedValues = new Queue<int>();
        private double mPreviousScaleMax = MinScaleMax;
        private double mScaleMax = MinScaleMax;
        private DateTime mAnimationStartedUtc;
        private bool mAnimating;

        public static readonly DependencyProperty SentBrushProperty =
            DependencyProperty.Register("SentBrush", typeof(Brush), typeof(TrafficGraphControl),
                new FrameworkPropertyMetadata(Brushes.Plum, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ReceivedBrushProperty =
            DependencyProperty.Register("ReceivedBrush", typeof(Brush), typeof(TrafficGraphControl),
                new FrameworkPropertyMetadata(Brushes.PaleTurquoise, FrameworkPropertyMetadataOptions.AffectsRender));

        static TrafficGraphControl()
        {
            FocusableProperty.OverrideMetadata(typeof(TrafficGraphControl), new FrameworkPropertyMetadata(false));
            BackgroundProperty.OverrideMetadata(
                typeof(TrafficGraphControl),
                new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));
        }

        public Brush SentBrush
        {
            get { return (Brush)GetValue(SentBrushProperty); }
            set { SetValue(SentBrushProperty, value); }
        }

        public Brush ReceivedBrush
        {
            get { return (Brush)GetValue(ReceivedBrushProperty); }
            set { SetValue(ReceivedBrushProperty, value); }
        }

        public int MaxValue
        {
            get
            {
                int max = 1;
                foreach (int value in mSentValues) {
                    max = Math.Max(max, value);
                }

                foreach (int value in mReceivedValues) {
                    max = Math.Max(max, value);
                }

                return max;
            }
        }

        public void AddValues(int sent, int received)
        {
            mPreviousScaleMax = GetRenderedScaleMax(GetAnimationProgress());
            Enqueue(mSentValues, sent);
            Enqueue(mReceivedValues, received);
            UpdateScaleMax();
            StartAnimation();
            InvalidateVisual();
        }

        public void Clear()
        {
            StopAnimation();
            mScaleMax = MinScaleMax;
            mPreviousScaleMax = MinScaleMax;
            mSentValues.Clear();
            mReceivedValues.Clear();
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Rect bounds = new Rect(0, 0, ActualWidth, ActualHeight);
            if (bounds.Width <= 1 || bounds.Height <= 1) {
                return;
            }

            drawingContext.DrawRectangle(Background, null, bounds);
            drawingContext.PushClip(new RectangleGeometry(bounds));

            Brush gridBrush = TryFindResource("BorderBrush") as Brush ?? Brushes.Gainsboro;
            Brush labelBrush = TryFindResource("MutedTextBrush") as Brush ?? Brushes.Gray;
            Pen gridPen = new Pen(gridBrush, 1);
            double progress = GetAnimationProgress();
            List<double> sentValues = ToDoubleList(mSentValues);
            List<double> receivedValues = ToDoubleList(mReceivedValues);
            double scaleMax = GetRenderedScaleMax(progress);
            double scrollOffset = mAnimating ? (1D - EaseOutCubic(progress)) * GetXStep(bounds) : 0D;

            DrawGrid(drawingContext, bounds, gridPen, labelBrush, scaleMax);
            DrawSeries(drawingContext, bounds, receivedValues, scaleMax, scrollOffset, ReceivedBrush);
            DrawSeries(drawingContext, bounds, sentValues, scaleMax, scrollOffset, SentBrush);
            drawingContext.Pop();
        }

        private static void Enqueue(Queue<int> values, int value)
        {
            values.Enqueue(Math.Max(0, value));
            while (values.Count > MaxPoints) {
                values.Dequeue();
            }
        }

        private void DrawGrid(DrawingContext drawingContext, Rect bounds, Pen pen, Brush labelBrush, double scaleMax)
        {
            double stepX = bounds.Width / GridLineCount;
            double stepY = bounds.Height / GridLineCount;

            for (int index = 1; index < GridLineCount; index++) {
                double x = Math.Round(index * stepX) + 0.5;
                drawingContext.DrawLine(pen, new Point(x, 0), new Point(x, bounds.Height));

                double y = Math.Round(index * stepY) + 0.5;
                drawingContext.DrawLine(pen, new Point(0, y), new Point(bounds.Width, y));
                double value = scaleMax * (GridLineCount - index) / GridLineCount;
                DrawGridLabel(drawingContext, bounds, y, FormatRate(value), labelBrush);
            }
        }

        private void DrawSeries(DrawingContext drawingContext, Rect bounds, IList<double> points, double max, double scrollOffset, Brush brush)
        {
            if (points.Count < 2) {
                return;
            }

            double xStep = GetXStep(bounds);
            int offset = MaxPoints - points.Count;
            List<Point> renderPoints = new List<Point>(points.Count);

            for (int index = 0; index < points.Count; index++) {
                double x = (offset + index) * xStep + scrollOffset;
                double ratio = Math.Min(1.0, points[index] / max);
                double y = bounds.Height - (ratio * (bounds.Height - 4)) - 2;
                renderPoints.Add(new Point(x, y));
            }

            StreamGeometry geometry = new StreamGeometry();
            using (StreamGeometryContext context = geometry.Open())
            {
                context.BeginFigure(renderPoints[0], false, false);
                for (int index = 1; index < renderPoints.Count; index++) {
                    Point previous = renderPoints[index - 1];
                    Point current = renderPoints[index];
                    double midX = (previous.X + current.X) / 2D;
                    context.BezierTo(
                        new Point(midX, previous.Y),
                        new Point(midX, current.Y),
                        current,
                        true,
                        false);
                }
            }

            geometry.Freeze();
            Pen pen = new Pen(brush, 2);
            pen.StartLineCap = PenLineCap.Round;
            pen.EndLineCap = PenLineCap.Round;
            pen.LineJoin = PenLineJoin.Round;
            pen.Freeze();
            drawingContext.DrawGeometry(null, pen, geometry);
        }

        private void UpdateScaleMax()
        {
            double desired = CreateNiceScale(Math.Max(MinScaleMax, MaxValue * ScaleHeadroom));
            if (desired > mScaleMax) {
                mScaleMax = desired;
                return;
            }

            if (desired < mScaleMax * 0.5D) {
                mScaleMax = Math.Max(desired, CreateNiceScale(mScaleMax * 0.97D));
            }
        }

        private void StartAnimation()
        {
            mAnimationStartedUtc = DateTime.UtcNow;
            if (mAnimating) {
                return;
            }

            mAnimating = true;
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void StopAnimation()
        {
            if (!mAnimating) {
                return;
            }

            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            mAnimating = false;
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (GetAnimationProgress() >= 1D) {
                StopAnimation();
                mPreviousScaleMax = mScaleMax;
            }

            InvalidateVisual();
        }

        private double GetAnimationProgress()
        {
            if (!mAnimating) {
                return 1D;
            }

            double elapsed = (DateTime.UtcNow - mAnimationStartedUtc).TotalMilliseconds;
            return Math.Max(0D, Math.Min(1D, elapsed / AnimationDuration.TotalMilliseconds));
        }

        private double GetRenderedScaleMax(double progress)
        {
            if (!mAnimating) {
                return mScaleMax;
            }

            double eased = EaseOutCubic(progress);
            return mPreviousScaleMax + ((mScaleMax - mPreviousScaleMax) * eased);
        }

        private static double GetXStep(Rect bounds)
        {
            return bounds.Width / (MaxPoints - 1);
        }

        private static List<double> ToDoubleList(Queue<int> values)
        {
            List<double> result = new List<double>(values.Count);
            foreach (int value in values) {
                result.Add(value);
            }

            return result;
        }

        private void DrawGridLabel(DrawingContext drawingContext, Rect bounds, double y, string text, Brush labelBrush)
        {
            FormattedText formattedText = new FormattedText(
                text,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface("Segoe UI"),
                10D,
                labelBrush,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);
            formattedText.MaxTextWidth = Math.Max(1D, bounds.Width - 12D);
            drawingContext.DrawText(formattedText, new Point(6D, Math.Max(2D, y - formattedText.Height - 2D)));
        }

        private static double CreateNiceScale(double value)
        {
            if (value <= MinScaleMax) {
                return MinScaleMax;
            }

            double exponent = Math.Floor(Math.Log10(value));
            double magnitude = Math.Pow(10D, exponent);
            double normalized = value / magnitude;
            double nice;

            if (normalized <= 1D) {
                nice = 1D;
            } else if (normalized <= 2D) {
                nice = 2D;
            } else if (normalized <= 5D) {
                nice = 5D;
            } else {
                nice = 10D;
            }

            return nice * magnitude;
        }

        private static string FormatRate(double bytesPerSecond)
        {
            string[] units = { "B/s", "KB/s", "MB/s", "GB/s", "TB/s" };
            double value = Math.Max(0D, bytesPerSecond);
            int unitIndex = 0;
            while (value >= 1024D && unitIndex < units.Length - 1) {
                value /= 1024D;
                unitIndex++;
            }

            return string.Format(CultureInfo.CurrentCulture, "{0:N0} {1}", value, units[unitIndex]);
        }

        private static double EaseOutCubic(double value)
        {
            double inverse = 1D - value;
            return 1D - (inverse * inverse * inverse);
        }
    }
}
