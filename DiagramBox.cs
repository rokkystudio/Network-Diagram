using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetworkDiagram
{
    public class DiagramData : List<DiagramPoint>
    {
        public Color LineColor { get; private set; } = Color.DarkGray;

        public void SetLineColor(Color color)
        {
            LineColor = color;
        }
    }

    public class DiagramPoint
    {
        public int time { get; set; }
        public int value { get; set; }

        public DiagramPoint(int value)
        {
            this.value = value;
            time = 0;
        }
    }

    public class DiagramScaling
    {
        private bool mAutoScale = true;
        private int mMinScaleY = 102400;
        private int mScaleYCurrent;
        private int mScaleYStart;
        private int mScaleYFinish;
        private int mScaleYStep;
        private int mScaleYTime = 1000;

        public DiagramScaling()
        {
            mScaleYCurrent = mMinScaleY;
            mScaleYFinish = mMinScaleY;
        }

        public void SetAutoScale(bool enabled)
        {
            mAutoScale = enabled;
            SetScaleY(mScaleYFinish);
        }

        public int GetScaleY()
        {
            return mScaleYCurrent;
        }

        public void SetScaleY(int scale)
        {
            if (scale < mMinScaleY) {
                scale = mMinScaleY;
            }

            if (mScaleYCurrent > mMinScaleY * 4 && scale == mMinScaleY) {
                mScaleYCurrent = scale;
                mScaleYFinish = scale;
                return;
            }

            if (mScaleYCurrent == mMinScaleY && mScaleYFinish == mMinScaleY) {
                mScaleYCurrent = scale;
                mScaleYFinish = scale;
                return;
            }

            if (scale == mScaleYFinish) {
                return;
            }

            if (mAutoScale) {
                mScaleYStart = mScaleYCurrent;
                mScaleYFinish = scale;
                mScaleYStep = 0;
            } else {
                mScaleYCurrent = scale;
                mScaleYFinish = scale;
            }
        }

        public void PushTime(int time)
        {
            if (mScaleYCurrent == mScaleYFinish) {
                return;
            }

            mScaleYStep += time;
            if (mScaleYStep > mScaleYTime) {
                mScaleYStep = mScaleYTime;
                mScaleYCurrent = mScaleYFinish;
                return;
            }

            mScaleYCurrent = mScaleYStart + (mScaleYFinish - mScaleYStart) * mScaleYStep / mScaleYTime;
        }
    }

    public partial class DiagramBox : UserControl
    {
        private readonly List<DiagramData> mDiagramList = new List<DiagramData>();
        private readonly DiagramScaling mScaling = new DiagramScaling();
        private readonly Plot mPlot = new Plot(1, 1);

        private Bitmap mPlotBitmap;
        private int mMaxValue;
        private int mMaxTime = 10000;
        private Color mBackgroundColor = Color.White;
        private Color mBorderColor = Color.Gainsboro;
        private Color mGridColor = Color.FromArgb(32, 0, 0, 0);

        public DiagramBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            UpdatePlotStyle();
        }

        public void Stop()
        {
            AnimTimer.Stop();
        }

        public void SetTheme(ThemePalette palette)
        {
            mBackgroundColor = palette.SurfaceColor;
            mBorderColor = palette.BorderColor;
            mGridColor = Color.FromArgb(50, palette.BorderColor);
            RedrawPlot();
            Invalidate();
        }

        public void SetColor(int index, Color color)
        {
            InitDiagramData(index);
            DiagramData data = mDiagramList[index];
            data.SetLineColor(color);
            RedrawPlot();
            Invalidate();
        }

        public void AddValue(int index, int value)
        {
            InitDiagramData(index);
            DiagramData data = mDiagramList[index];
            data.Insert(0, new DiagramPoint(value));
            if (value > mMaxValue) {
                mMaxValue = value;
            }

            RedrawPlot();
            Invalidate();
        }

        public int GetMaxValue()
        {
            return mMaxValue;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (mPlotBitmap == null && Width > 0 && Height > 0) {
                RedrawPlot();
            }

            e.Graphics.Clear(mBackgroundColor);

            if (mPlotBitmap != null) {
                e.Graphics.DrawImageUnscaled(mPlotBitmap, 0, 0);
            }

            using (Pen pen = new Pen(mBorderColor)) {
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            RedrawPlot();
        }

        private void UpdatePlotStyle()
        {
            mPlot.Style(
                figureBackground: mBackgroundColor,
                dataBackground: mBackgroundColor,
                grid: mGridColor,
                tick: mBorderColor,
                axisLabel: mBorderColor,
                titleLabel: mBorderColor,
                dataBackgroundImage: null,
                figureBackgroundImage: null);
            mPlot.Frameless(true);
            mPlot.Grid(true, mGridColor, LineStyle.Solid, false);
        }

        private void RedrawPlot()
        {
            if (Width <= 0 || Height <= 0) {
                return;
            }

            mPlot.Clear();
            UpdatePlotStyle();

            for (int index = 0; index < mDiagramList.Count; index++) {
                DiagramData data = mDiagramList[index];
                if (data.Count == 0 || data.All(point => point.value == 0)) {
                    continue;
                }

                double[] xs = new double[data.Count];
                double[] ys = new double[data.Count];
                int targetIndex = 0;

                for (int sourceIndex = data.Count - 1; sourceIndex >= 0; sourceIndex--) {
                    xs[targetIndex] = -data[sourceIndex].time / 1000d;
                    ys[targetIndex] = data[sourceIndex].value;
                    targetIndex++;
                }

                mPlot.AddScatter(xs, ys, color: data.LineColor, lineWidth: 2f, markerSize: 0f);
            }

            mPlot.SetAxisLimits(
                xMin: -mMaxTime / 1000d,
                xMax: 0,
                yMin: 0,
                yMax: Math.Max(1, mScaling.GetScaleY()));

            Bitmap bitmap = mPlot.Render(Width, Height, lowQuality: false, scale: 1);
            Bitmap previousBitmap = mPlotBitmap;
            mPlotBitmap = bitmap;
            previousBitmap?.Dispose();
        }

        private void InitDiagramData(int index)
        {
            int add = index - mDiagramList.Count + 1;
            if (add > 0) {
                for (int i = 0; i < add; i++) {
                    mDiagramList.Add(new DiagramData());
                }
            }
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            mMaxValue = 0;

            foreach (DiagramData data in mDiagramList) {
                data.RemoveAll(point => point.time >= mMaxTime);

                foreach (DiagramPoint point in data) {
                    point.time += AnimTimer.Interval;
                    if (point.value > mMaxValue) {
                        mMaxValue = point.value;
                    }
                }
            }

            mScaling.SetScaleY(mMaxValue);
            mScaling.PushTime(AnimTimer.Interval);
            RedrawPlot();
            Invalidate();
        }
    }
}
