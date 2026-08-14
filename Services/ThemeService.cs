using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Linq;
using System.Windows.Forms;

namespace NetworkDiagram
{
    public sealed class ThemePalette
    {
        public Color BackgroundColor { get; set; }
        public Color SurfaceColor { get; set; }
        public Color SurfaceRaisedColor { get; set; }
        public Color TextColor { get; set; }
        public Color MutedTextColor { get; set; }
        public Color SoftTextColor { get; set; }
        public Color DisabledTextColor { get; set; }
        public Color BorderColor { get; set; }
        public Color PrimaryColor { get; set; }
        public Color PrimaryHoverColor { get; set; }
        public Color SuccessColor { get; set; }
        public Color WarningColor { get; set; }
        public Color ErrorColor { get; set; }
        public Color TitleBarBackgroundColor { get; set; }
        public Color TitleBarBorderColor { get; set; }
    }

    internal static class ThemeService
    {
        public const string LightTheme = "Light";
        public const string DarkTheme = "Dark";

        private const int WmNclButtonDown = 0xA1;
        private const int HtCaption = 0x2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr handle, int message, int wParam, int lParam);

        public static event EventHandler ThemeChanged;

        public static string CurrentTheme
        {
            get { return NormalizeTheme(Properties.Settings.Default.Theme); }
            set
            {
                string normalized = NormalizeTheme(value);
                if (string.Equals(CurrentTheme, normalized, StringComparison.Ordinal)) {
                    return;
                }

                Properties.Settings.Default.Theme = normalized;
                Properties.Settings.Default.Save();
                OnThemeChanged();
            }
        }

        public static ThemePalette CurrentPalette
        {
            get { return GetPalette(CurrentTheme); }
        }

        public static string NormalizeTheme(string theme)
        {
            return string.Equals(theme, DarkTheme, StringComparison.OrdinalIgnoreCase)
                ? DarkTheme
                : LightTheme;
        }

        public static bool IsDarkTheme(string theme)
        {
            return string.Equals(NormalizeTheme(theme), DarkTheme, StringComparison.Ordinal);
        }

        public static void ToggleTheme()
        {
            CurrentTheme = IsDarkTheme(CurrentTheme) ? LightTheme : DarkTheme;
        }

        public static ThemePalette GetPalette(string theme)
        {
            return IsDarkTheme(theme) ? CreateDarkPalette() : CreateLightPalette();
        }

        public static void BeginWindowDrag(Form form)
        {
            if (form == null) {
                return;
            }

            ReleaseCapture();
            SendMessage(form.Handle, WmNclButtonDown, HtCaption, 0);
        }

        public static void ApplyFormStyle(Form form)
        {
            ThemePalette palette = CurrentPalette;

            form.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            form.BackColor = palette.BorderColor;
            form.ForeColor = palette.TextColor;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Padding = new Padding(1);
        }

        public static void ApplyToolStrip(ToolStrip toolStrip)
        {
            if (toolStrip == null) {
                return;
            }

            ThemePalette palette = CurrentPalette;
            toolStrip.Renderer = new ToolStripProfessionalRenderer(new ThemeColorTable(palette));
            toolStrip.BackColor = palette.SurfaceRaisedColor;
            toolStrip.ForeColor = palette.TextColor;
            toolStrip.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            toolStrip.Padding = new Padding(6);

            foreach (ToolStripItem item in toolStrip.Items) {
                item.ForeColor = palette.TextColor;

                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem != null) {
                    menuItem.Padding = new Padding(8, 7, 8, 7);
                    menuItem.ImageScaling = ToolStripItemImageScaling.None;
                }

                ToolStripSeparator separator = item as ToolStripSeparator;
                if (separator != null) {
                    separator.Margin = new Padding(6, 4, 6, 4);
                }
            }
        }

        public static void ApplySelectionMenu(ContextMenuStrip menu)
        {
            if (menu == null) {
                return;
            }

            ApplyToolStrip(menu);

            menu.ShowCheckMargin = false;
            menu.ShowImageMargin = false;
            menu.AutoSize = false;

            int itemWidth = GetSelectionMenuWidth(menu);
            int totalHeight = menu.Padding.Vertical;

            foreach (ToolStripItem item in menu.Items) {
                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem != null) {
                    menuItem.AutoSize = false;
                    menuItem.Size = new Size(itemWidth - menu.Padding.Horizontal, 34);
                    menuItem.Margin = new Padding(0, 0, 0, 2);
                    menuItem.Padding = Padding.Empty;
                    menuItem.DisplayStyle = ToolStripItemDisplayStyle.None;
                    menuItem.Paint -= SelectionMenuItem_Paint;
                    menuItem.Paint += SelectionMenuItem_Paint;
                    totalHeight += menuItem.Height + menuItem.Margin.Vertical;
                    continue;
                }

                ToolStripSeparator separator = item as ToolStripSeparator;
                if (separator != null) {
                    separator.AutoSize = false;
                    separator.Height = 10;
                    separator.Margin = new Padding(0, 2, 0, 4);
                    separator.Paint -= SelectionMenuSeparator_Paint;
                    separator.Paint += SelectionMenuSeparator_Paint;
                    totalHeight += separator.Height + separator.Margin.Vertical;
                }
            }

            menu.Size = new Size(itemWidth, totalHeight);
        }

        public static void StyleComboBox(ComboBox comboBox)
        {
            if (comboBox == null) {
                return;
            }

            ThemePalette palette = CurrentPalette;

            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.IntegralHeight = false;
            comboBox.ItemHeight = 28;
            comboBox.MaxDropDownItems = 12;
            comboBox.BackColor = palette.SurfaceRaisedColor;
            comboBox.ForeColor = palette.TextColor;

            comboBox.DrawItem -= ComboBox_DrawItem;
            comboBox.DrawItem += ComboBox_DrawItem;
        }

        public static void StyleSecondaryButton(Button button)
        {
            if (button == null) {
                return;
            }

            ThemePalette palette = CurrentPalette;

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = palette.BorderColor;
            button.FlatAppearance.MouseOverBackColor = Blend(palette.PrimaryColor, palette.SurfaceRaisedColor, 0.12f);
            button.FlatAppearance.MouseDownBackColor = Blend(palette.PrimaryColor, palette.SurfaceRaisedColor, 0.18f);
            button.BackColor = palette.SurfaceRaisedColor;
            button.ForeColor = palette.TextColor;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
        }

        public static void StyleTitleBarButton(Button button, bool danger = false)
        {
            if (button == null) {
                return;
            }

            ThemePalette palette = CurrentPalette;

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = danger
                ? palette.ErrorColor
                : Blend(palette.PrimaryColor, palette.TitleBarBackgroundColor, 0.16f);
            button.FlatAppearance.MouseDownBackColor = danger
                ? Blend(Color.Black, palette.ErrorColor, 0.14f)
                : Blend(palette.PrimaryColor, palette.TitleBarBackgroundColor, 0.22f);
            button.BackColor = Color.Transparent;
            button.ForeColor = danger ? Color.White : palette.TextColor;
            button.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
        }

        public static void StyleCheckBox(CheckBox checkBox)
        {
            if (checkBox == null) {
                return;
            }

            ThemePalette palette = CurrentPalette;

            checkBox.FlatStyle = FlatStyle.Flat;
            checkBox.ForeColor = palette.TextColor;
            checkBox.BackColor = Color.Transparent;
            checkBox.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
        }

        public static void UpdateRoundedRegion(Control control, int radius)
        {
            if (control == null || control.Width <= 0 || control.Height <= 0 || radius <= 0) {
                return;
            }

            Rectangle bounds = new Rectangle(0, 0, control.Width, control.Height);
            using (GraphicsPath path = CreateRoundedPath(bounds, radius)) {
                Region previousRegion = control.Region;
                control.Region = new Region(path);
                if (previousRegion != null) {
                    previousRegion.Dispose();
                }
            }
        }

        public static Color GetContrastTextColor(Color backgroundColor)
        {
            int brightness = (backgroundColor.R * 299) + (backgroundColor.G * 587) + (backgroundColor.B * 114);
            return brightness / 1000 >= 140 ? Color.FromArgb(23, 25, 28) : Color.White;
        }

        private static void OnThemeChanged()
        {
            EventHandler handler = ThemeChanged;
            if (handler != null) {
                handler(null, EventArgs.Empty);
            }
        }

        private static int GetSelectionMenuWidth(ContextMenuStrip menu)
        {
            int maxTextWidth = 0;
            foreach (ToolStripMenuItem item in menu.Items.OfType<ToolStripMenuItem>()) {
                int width = TextRenderer.MeasureText(item.Text, menu.Font).Width;
                if (width > maxTextWidth) {
                    maxTextWidth = width;
                }
            }

            return Math.Max(220, maxTextWidth + 72);
        }

        private static void SelectionMenuItem_Paint(object sender, PaintEventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item == null) {
                return;
            }

            ThemePalette palette = CurrentPalette;
            Rectangle bounds = new Rectangle(Point.Empty, item.Size);
            Rectangle surfaceBounds = Rectangle.Inflate(bounds, -1, -1);
            bool selected = item.Selected;
            Color foreground = item.Enabled ? palette.TextColor : palette.DisabledTextColor;
            Color background = selected
                ? Blend(palette.PrimaryColor, palette.SurfaceRaisedColor, 0.12f)
                : palette.SurfaceRaisedColor;
            Color border = selected
                ? Blend(palette.PrimaryColor, palette.SurfaceRaisedColor, 0.28f)
                : palette.SurfaceRaisedColor;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(palette.SurfaceRaisedColor);

            using (GraphicsPath path = CreateRoundedPath(surfaceBounds, 8))
            using (SolidBrush backgroundBrush = new SolidBrush(background))
            using (Pen borderPen = new Pen(border))
            {
                e.Graphics.FillPath(backgroundBrush, path);
                e.Graphics.DrawPath(borderPen, path);
            }

            int checkLeft = 10;
            int checkWidth = 12;
            if (item.Checked) {
                TextRenderer.DrawText(
                    e.Graphics,
                    "\u2713",
                    new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 204),
                    new Rectangle(checkLeft, 0, checkWidth, bounds.Height),
                    palette.PrimaryColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            Image image = item.Image;
            int contentLeft = checkLeft + checkWidth + 8;
            if (image != null) {
                Rectangle imageBounds = new Rectangle(contentLeft, (bounds.Height - image.Height) / 2, image.Width, image.Height);
                e.Graphics.DrawImage(image, imageBounds);
                contentLeft += image.Width + 10;
            }

            Rectangle textBounds = new Rectangle(contentLeft, 0, bounds.Width - contentLeft - 12, bounds.Height);
            TextRenderer.DrawText(
                e.Graphics,
                item.Text,
                item.Font,
                textBounds,
                foreground,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private static void SelectionMenuSeparator_Paint(object sender, PaintEventArgs e)
        {
            ThemePalette palette = CurrentPalette;
            e.Graphics.Clear(palette.SurfaceRaisedColor);
            using (Pen pen = new Pen(palette.BorderColor))
            {
                int y = e.ClipRectangle.Height / 2;
                e.Graphics.DrawLine(pen, 12, y, e.ClipRectangle.Width - 12, y);
            }
        }

        private static void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            ThemePalette palette = CurrentPalette;
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) {
                return;
            }

            using (SolidBrush backgroundBrush = new SolidBrush(comboBox.BackColor))
            {
                e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
            }

            if (e.Index < 0 || e.Index >= comboBox.Items.Count) {
                return;
            }

            object item = comboBox.Items[e.Index];
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color background = selected
                ? Blend(palette.PrimaryColor, palette.SurfaceRaisedColor, 0.18f)
                : palette.SurfaceRaisedColor;
            Color foreground = palette.TextColor;

            using (SolidBrush brush = new SolidBrush(background)) {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            Image image = GetComboItemImage(item, selected, palette);
            Rectangle contentBounds = Rectangle.Inflate(e.Bounds, -8, 0);
            int textOffset = 0;
            if (image != null) {
                Rectangle imageBounds = new Rectangle(
                    contentBounds.X,
                    contentBounds.Y + (contentBounds.Height - image.Height) / 2,
                    image.Width,
                    image.Height);
                e.Graphics.DrawImage(image, imageBounds);
                textOffset = image.Width + 10;
            }

            Rectangle textBounds = new Rectangle(
                contentBounds.X + textOffset,
                contentBounds.Y,
                contentBounds.Width - textOffset,
                contentBounds.Height);
            TextRenderer.DrawText(
                e.Graphics,
                GetComboItemText(item),
                comboBox.Font,
                textBounds,
                foreground,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            e.DrawFocusRectangle();
        }

        private static string GetComboItemText(object item)
        {
            NetworkAdapter adapter = item as NetworkAdapter;
            if (adapter != null) {
                return adapter.DisplayName;
            }

            return item == null ? string.Empty : item.ToString();
        }

        private static Image GetComboItemImage(object item, bool selected, ThemePalette palette)
        {
            NetworkAdapter adapter = item as NetworkAdapter;
            if (adapter == null) {
                return null;
            }

            Color strokeColor = selected ? palette.PrimaryColor : palette.SoftTextColor;
            Color accentColor = selected ? palette.PrimaryColor : palette.MutedTextColor;
            return NetworkInterfaceIconService.GetIcon(adapter, new Size(16, 16), strokeColor, accentColor);
        }

        private static Color Blend(Color overlay, Color background, float amount)
        {
            amount = Math.Max(0F, Math.Min(1F, amount));

            int r = (int)(background.R + (overlay.R - background.R) * amount);
            int g = (int)(background.G + (overlay.G - background.G) * amount);
            int b = (int)(background.B + (overlay.B - background.B) * amount);

            return Color.FromArgb(r, g, b);
        }

        private static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        private static ThemePalette CreateLightPalette()
        {
            return new ThemePalette
            {
                BackgroundColor = ColorTranslator.FromHtml("#FAFAFA"),
                SurfaceColor = ColorTranslator.FromHtml("#F7F7F8"),
                SurfaceRaisedColor = ColorTranslator.FromHtml("#FFFFFF"),
                TextColor = ColorTranslator.FromHtml("#17191C"),
                MutedTextColor = ColorTranslator.FromHtml("#73777F"),
                SoftTextColor = ColorTranslator.FromHtml("#454951"),
                DisabledTextColor = ColorTranslator.FromHtml("#ADB1B8"),
                BorderColor = ColorTranslator.FromHtml("#E2E4E8"),
                PrimaryColor = ColorTranslator.FromHtml("#00ACF7"),
                PrimaryHoverColor = ColorTranslator.FromHtml("#0099DF"),
                SuccessColor = ColorTranslator.FromHtml("#2FAE62"),
                WarningColor = ColorTranslator.FromHtml("#F59E0B"),
                ErrorColor = ColorTranslator.FromHtml("#D94A4A"),
                TitleBarBackgroundColor = ColorTranslator.FromHtml("#EFF2F5"),
                TitleBarBorderColor = ColorTranslator.FromHtml("#D8DEE6")
            };
        }

        private static ThemePalette CreateDarkPalette()
        {
            return new ThemePalette
            {
                BackgroundColor = ColorTranslator.FromHtml("#111418"),
                SurfaceColor = ColorTranslator.FromHtml("#171B20"),
                SurfaceRaisedColor = ColorTranslator.FromHtml("#1D232A"),
                TextColor = ColorTranslator.FromHtml("#F2F5F8"),
                MutedTextColor = ColorTranslator.FromHtml("#9FA8B3"),
                SoftTextColor = ColorTranslator.FromHtml("#D5DAE0"),
                DisabledTextColor = ColorTranslator.FromHtml("#6F7883"),
                BorderColor = ColorTranslator.FromHtml("#2A323B"),
                PrimaryColor = ColorTranslator.FromHtml("#38BDF8"),
                PrimaryHoverColor = ColorTranslator.FromHtml("#18A9EC"),
                SuccessColor = ColorTranslator.FromHtml("#45C97A"),
                WarningColor = ColorTranslator.FromHtml("#FFB020"),
                ErrorColor = ColorTranslator.FromHtml("#FF6B6B"),
                TitleBarBackgroundColor = ColorTranslator.FromHtml("#252D36"),
                TitleBarBorderColor = ColorTranslator.FromHtml("#34404B")
            };
        }

        private sealed class ThemeColorTable : ProfessionalColorTable
        {
            private readonly ThemePalette mPalette;

            public ThemeColorTable(ThemePalette palette)
            {
                mPalette = palette;
                UseSystemColors = false;
            }

            public override Color ToolStripDropDownBackground { get { return mPalette.SurfaceRaisedColor; } }
            public override Color MenuBorder { get { return mPalette.BorderColor; } }
            public override Color MenuItemBorder { get { return mPalette.BorderColor; } }
            public override Color MenuItemSelected { get { return Blend(mPalette.PrimaryColor, mPalette.SurfaceRaisedColor, 0.18f); } }
            public override Color MenuItemSelectedGradientBegin { get { return MenuItemSelected; } }
            public override Color MenuItemSelectedGradientEnd { get { return MenuItemSelected; } }
            public override Color MenuItemPressedGradientBegin { get { return mPalette.SurfaceColor; } }
            public override Color MenuItemPressedGradientMiddle { get { return mPalette.SurfaceColor; } }
            public override Color MenuItemPressedGradientEnd { get { return mPalette.SurfaceColor; } }
            public override Color ImageMarginGradientBegin { get { return mPalette.SurfaceRaisedColor; } }
            public override Color ImageMarginGradientMiddle { get { return mPalette.SurfaceRaisedColor; } }
            public override Color ImageMarginGradientEnd { get { return mPalette.SurfaceRaisedColor; } }
            public override Color SeparatorDark { get { return mPalette.BorderColor; } }
            public override Color SeparatorLight { get { return mPalette.BorderColor; } }
        }
    }
}
