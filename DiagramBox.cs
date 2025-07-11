using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace NetworkDiagram
{
    public class DiagramData : List<DiagramPoint>
    {
        public SolidBrush mBrush;
        public Pen mPen;

        public DiagramData() {
            mBrush = new SolidBrush(Color.Gray);
            mPen = new Pen(new SolidBrush(Color.DarkGray));
        }

        public void setSolidColor(Color color) {
            mBrush = new SolidBrush(Color.FromArgb(75, color));
        }

        public void setPenColor(Color color) {
            mPen = new Pen(new SolidBrush(Color.FromArgb(150, color)));
        }
    }

    public class DiagramPoint
    {
        public int time { get; set; }
        public int value { get; set; }

        public DiagramPoint(int value) {
            this.value = value;
            this.time = 0;
        }
    }

    public class DiagramScaling
    {
        private bool mAutoScale = true;

        // Минимальный размер оси Y по высоте по умолчанию 100 KB/s
        private int mMinScaleY = 102400;

        // Текущий масштаб по оси Y
        private int mScaleYCurrent;

        private int mScaleYStart;

        private int mScaleYFinish;

        private int mScaleYStep = 0;

        // Время для плавного изменения масштаба
        private int mScaleYTime = 1000; // ms

        public DiagramScaling() {
            mScaleYCurrent = mMinScaleY;
            mScaleYFinish = mMinScaleY;
        }

        public void SetAutoScale(bool enabled) {
            mAutoScale = enabled;
            SetScaleY(mScaleYFinish);
        }

        public int GetScaleY() {
            return mScaleYCurrent;
        }

        public void SetScaleY(int scale)
        {
            if (scale < mMinScaleY) scale = mMinScaleY;

            // Принудительно сбросить, если слишком большой и новые значения маленькие
            if (mScaleYCurrent > mMinScaleY * 4 && scale == mMinScaleY) {
                // сбрасываем масштаб резко
                mScaleYCurrent = scale;
                mScaleYFinish = scale;
                return;
            }

            // Первая установка масштаба — без анимации
            if (mScaleYCurrent == mMinScaleY && mScaleYFinish == mMinScaleY) {
                mScaleYCurrent = scale;
                mScaleYFinish = scale;
                return;
            }

            if (scale == mScaleYFinish) return;

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
            if (mScaleYCurrent == mScaleYFinish) return;

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
        private List<DiagramData> mDiagramList = new List<DiagramData>();

        private DiagramScaling mScaling = new DiagramScaling();

        // Максимальное значение среди текущего списка точек
        private int mMaxValue;

        private int mMaxTime = 10000; // ms

        private int mPadding = 10;
         
        public DiagramBox() {
            InitializeComponent();
        }

        public void SetColor(int index, Color color) {
            InitDiagramData(index);
            DiagramData data = mDiagramList.ElementAt(index);
            data.setSolidColor(color);
            data.setPenColor(color);
        }

        public void AddValue(int index, int value)
        {
            InitDiagramData(index);
            DiagramData data = mDiagramList.ElementAt(index);

            // Новые значения добавляем в начало списка
            data.Insert(0, new DiagramPoint(value));
            if (value > mMaxValue) mMaxValue = value;
        }

        private void InitDiagramData(int index)
        {
            int add = index - mDiagramList.Count + 1;
            if (add > 0)
            {
                for (int i = 0; i < add; i++) {
                    mDiagramList.Add(new DiagramData());
                }
            }
        }

        public int GetMaxValue() {
            return mMaxValue;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (DiagramData data in mDiagramList) {
                DrawDiagram(graphics, data);
            }
        }

        private void DrawDiagram(Graphics graphics, DiagramData data) {
            if (data.Count < 2) return;

            if (data.All(p => p.value == 0)) return; // не рисуем, если все значения нули

            List<Point> points = new List<Point>();

            // Bottom points (для заполнения фона)
            points.Insert(0, new Point(ValueToX(0), ValueToY(0)));
            points.Insert(0, new Point(ValueToX(mMaxTime), ValueToY(0)));

            for (int i = 0; i < data.Count; i++) {
                int x = ValueToX(data[i].time);
                int y = ValueToY(data[i].value);

                // Плавный переход только если есть достаточно точек
                if (data.Count > 5) {
                    if (i == 0 && data.Count > 1) {
                        int timeStep = GetTimeStep(data);
                        int diffValue = data[i].value - data[i + 1].value;
                        int timePass = data[i + 1].time;
                        y = ValueToY(data[i + 1].value + diffValue * timePass / timeStep);
                    }

                    if (i == data.Count - 1 && data.Count > 1) {
                        int timeStep = GetTimeStep(data);
                        int diffValue = data[i].value - data[i - 1].value;
                        int timeLeft = data[i].time - data[i - 1].time;
                        y = ValueToY(data[i - 1].value + diffValue * timeLeft / timeStep);
                    }
                }

                points.Add(new Point(x, y));
            }

            graphics.FillPolygon(data.mBrush, points.ToArray());
            graphics.DrawPolygon(data.mPen, points.ToArray());
        }

        private int ValueToX(int time) {
            return GetDiagramWidth() - GetDiagramWidth() * time / mMaxTime + mPadding;
        }

        private int ValueToY(int value)
        {
            int y = GetDiagramHeight() - GetDiagramHeight() * value / mScaling.GetScaleY() + mPadding;

            // Защита от переполнения вверх
            if (y < mPadding) y = mPadding;

            // Защита от выхода за нижнюю границу
            int maxY = Height - mPadding;
            if (y > maxY) y = maxY;

            return y;
        }

        private int GetTimeStep(DiagramData data) {
            return mMaxTime / (data.Count - 1);
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            mMaxValue = 0;

            foreach (DiagramData data in mDiagramList) {
                // Удаляем устаревшие точки (время >= mMaxTime)
                data.RemoveAll(p => p.time >= mMaxTime);

                foreach (DiagramPoint point in data) {
                    // Сдвигаем время
                    point.time += AnimTimer.Interval;

                    // Обновляем максимум
                    if (point.value > mMaxValue) {
                        mMaxValue = point.value;
                    }
                }
            }

            mScaling.SetScaleY(mMaxValue);
            mScaling.PushTime(AnimTimer.Interval);
            Invalidate();
        }

        public int GetDiagramWidth() {
            return Width - 2 * mPadding;
        }

        public int GetDiagramHeight() {
            return Height - 2 * mPadding;
        }
    }
}
