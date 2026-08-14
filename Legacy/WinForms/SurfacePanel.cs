using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NetworkDiagram
{
    public class SurfacePanel : Panel
    {
        private int mCornerRadius = 12;
        private Color mFillColor = Color.White;
        private Color mBorderColor = Color.Gainsboro;

        public int CornerRadius
        {
            get { return mCornerRadius; }
            set
            {
                mCornerRadius = value < 0 ? 0 : value;
                UpdateRegion();
                Invalidate();
            }
        }

        public Color FillColor
        {
            get { return mFillColor; }
            set
            {
                mFillColor = value;
                Invalidate();
            }
        }

        public Color BorderColor
        {
            get { return mBorderColor; }
            set
            {
                mBorderColor = value;
                Invalidate();
            }
        }

        public SurfacePanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
        }

        protected override void OnResize(EventArgs eventArgs)
        {
            base.OnResize(eventArgs);
            UpdateRegion();
        }

        protected override void OnPaintBackground(PaintEventArgs eventArgs)
        {
            Color parentColor = Parent == null ? SystemColors.Control : Parent.BackColor;
            eventArgs.Graphics.Clear(parentColor);

            using (GraphicsPath path = CreatePath(ClientRectangle, mCornerRadius))
            using (SolidBrush brush = new SolidBrush(mFillColor)) {
                eventArgs.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                eventArgs.Graphics.FillPath(brush, path);
            }
        }

        protected override void OnPaint(PaintEventArgs eventArgs)
        {
            base.OnPaint(eventArgs);

            Rectangle borderRectangle = new Rectangle(0, 0, Math.Max(Width - 1, 0), Math.Max(Height - 1, 0));
            using (GraphicsPath path = CreatePath(borderRectangle, mCornerRadius))
            using (Pen pen = new Pen(mBorderColor)) {
                eventArgs.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                eventArgs.Graphics.DrawPath(pen, path);
            }
        }

        private void UpdateRegion()
        {
            if (Width <= 0 || Height <= 0) {
                return;
            }

            using (GraphicsPath path = CreatePath(ClientRectangle, mCornerRadius)) {
                Region previousRegion = Region;
                Region = new Region(path);
                if (previousRegion != null) {
                    previousRegion.Dispose();
                }
            }
        }

        private static GraphicsPath CreatePath(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0) {
                path.AddRectangle(rectangle);
                path.CloseFigure();
                return path;
            }

            int diameter = radius * 2;
            Rectangle bounds = new Rectangle(rectangle.X, rectangle.Y, Math.Max(rectangle.Width - 1, 1), Math.Max(rectangle.Height - 1, 1));

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
