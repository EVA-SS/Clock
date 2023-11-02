using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Clock
{
    /// <summary>
    /// Clock 时钟
    /// </summary>
    /// <remarks>精致时钟</remarks>
    [Description("Clock 时钟")]
    public class Clock : Control
    {
        public Clock()
        {
            SetStyle(
               ControlStyles.AllPaintingInWmPaint |
               ControlStyles.OptimizedDoubleBuffer |
               ControlStyles.ResizeRedraw |
               ControlStyles.DoubleBuffer |
               ControlStyles.SupportsTransparentBackColor |
               ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        #region 属性

        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(typeof(Color), "Black")]
        public Color Back { get; set; } = Color.Black;

        /// <summary>
        /// 颜色
        /// </summary>
        [Description("颜色"), Category("外观"), DefaultValue(typeof(Color), "White")]
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// 秒表颜色
        /// </summary>
        [Description("秒表颜色"), Category("外观"), DefaultValue(typeof(Color), "250, 159, 34")]
        public Color ColorSecond { get; set; } = Color.FromArgb(250, 159, 34);

        #endregion

        StringFormat format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

        string font_family = "Arial";
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            var _rect = ClientRectangle;
            float clock_size = _rect.Width > _rect.Height ? _rect.Height : _rect.Width;
            var rect = new RectangleF((_rect.Width - clock_size) / 2, (_rect.Height - clock_size) / 2, clock_size, clock_size);

            var now = DateTime.Now;

            float dpi = g.DpiX / 96F, font_size = clock_size * 0.03F / dpi;
            using (var fontMinute = new Font(font_family, font_size))
            using (var fontHour = new Font(font_family, font_size * 3.33F))
            {
                float radius = rect.Width / 2F; //圆的半径
                if (font_minute_size == 0)
                {
                    font_minute_size = GetFontSizeMax(g, "60", fontMinute);
                    font_hour_size = GetFontSizeMax(g, "60", fontHour);
                }

                //默认坐标系统原点是左上角，现在把原点移到屏幕中心, 右下左上对应的轴：x,y,-x,-y
                g.TranslateTransform(rect.X + radius, rect.Y + radius);

                var state = g.Save();

                #region 刻度

                float font_minute_size_half = font_minute_size / 2F, dodd = font_minute_size_half * 0.56F;
                using (var pen_scale = new Pen(Color.FromArgb(60, Color), 1F))
                {
                    var scale_y = -radius + font_minute_size_half + dodd;
                    for (int i = 0; i < 60; i++)
                    {
                        if (i % 5 != 0) g.DrawLine(pen_scale, 0, scale_y, 0, scale_y - (dodd * 2F) + font_minute_size);
                        //围绕指定点按照顺时针方向旋转角度360 / 60 = 6度
                        g.RotateTransform(6);
                    }
                }

                #endregion

                g.Restore(state); state = g.Save();

                using (var brush = new SolidBrush(Color))
                {
                    #region 字体

                    float font_hour_size_half = font_hour_size / 2F;
                    int num = 12;
                    double pi = Math.PI / 180F, num_360 = 360D / 12;
                    int txtMinute = 15;
                    for (int i = 0; i < num; i++)
                    {
                        double d = pi * i * num_360; //每次转360/12度
                        float cos = (float)Math.Cos(d), sin = (float)Math.Sin(d);

                        //因为是从顺时钟3点钟开始画，所以索引i需要加上3

                        g.DrawString(txtMinute.ToString().PadLeft(2, '0'), fontMinute, brush, new RectangleF(-font_minute_size_half + (radius - font_minute_size) * cos, -font_minute_size_half + (radius - font_minute_size) * sin, font_minute_size, font_minute_size), format);
                        txtMinute += 5;
                        if (txtMinute > 60) txtMinute = 5;

                        int txtHour = i + 3;
                        if (txtHour > 12) txtHour = txtHour - 12;
                        g.DrawString(txtHour.ToString(), fontHour, brush, new RectangleF(-font_hour_size_half + (radius - font_hour_size) * cos, -font_hour_size_half + (radius - font_hour_size) * sin, font_hour_size, font_hour_size), format);
                    }

                    #endregion

                    g.Restore(state); state = g.Save();

                    float dot_centre_size = clock_size * 0.04F, dot_centre_size_half = dot_centre_size / 2F;

                    g.FillEllipse(brush, new RectangleF(-dot_centre_size_half, -dot_centre_size_half, dot_centre_size, dot_centre_size));

                    float line_base = clock_size * 0.06F, line_hour = clock_size * 0.24F, line_minute = clock_size * 0.42F;

                    // 画时钟的图形
                    float line_hour_size = dot_centre_size * 0.3F, line_minute_size = dot_centre_size * 0.6F;
                    float line_hour_size_half_ = -(line_hour_size / 2F), line_minute_size_half_ = -(line_minute_size / 2F);

                    g.RotateTransform((now.Hour - 12 + now.Minute / 60f) * 360F / 12F);
                    using (var path = new RectangleF(line_hour_size_half_, -line_hour, line_hour_size, line_hour).RoundPath(line_hour_size))
                    {
                        g.FillPath(brush, path);
                    }
                    using (var path = new RectangleF(line_minute_size_half_, -line_hour, line_minute_size, line_hour - line_base).RoundPath(line_minute_size))
                    {
                        g.FillPath(brush, path);
                    }
                    g.Restore(state); state = g.Save();

                    float d_60 = 360F / 60F;
                    // 画分钟的图形
                    g.RotateTransform((now.Minute + now.Second / 60F) * d_60);
                    using (var path = new RectangleF(line_hour_size_half_, -line_minute, line_hour_size, line_minute).RoundPath(line_hour_size))
                    {
                        g.FillPath(brush, path);
                    }
                    using (var path = new RectangleF(line_minute_size_half_, -line_minute, line_minute_size, line_minute - line_base).RoundPath(line_minute_size))
                    {
                        g.FillPath(brush, path);
                    }

                    g.Restore(state); state = g.Save();

                    // 画秒钟的图形
                    g.RotateTransform((now.Second + now.Millisecond / 1000F) * d_60);
                    using (var brushSecond = new SolidBrush(ColorSecond))
                    {
                        float second_outer_circle = clock_size * 0.04F * 0.7F, second_outer_circle_half = second_outer_circle / 2F, dot_wh3 = second_outer_circle_half * 0.46F;
                        var rect_second = new RectangleF(-second_outer_circle_half, -second_outer_circle_half, second_outer_circle, second_outer_circle);
                        g.FillEllipse(brushSecond, rect_second);
                        g.FillRectangle(brushSecond, new RectangleF(-(dot_wh3 / 2F), -radius + font_minute_size_half, dot_wh3, radius + line_base));
                        using (var brush_back = new SolidBrush(Back))
                        {
                            g.FillEllipse(brush_back, GetRect(rect_second, dot_wh3 * 2));
                        }
                    }
                }
            }
            base.OnPaint(e);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(50);
                    Invalidate();
                }
            });
        }

        float font_minute_size = 0, font_hour_size = 0;
        protected override void OnSizeChanged(EventArgs e)
        {
            font_minute_size = font_hour_size = 0;
            base.OnSizeChanged(e);
        }

        float GetFontSizeMax(Graphics g, string text, Font font)
        {
            var size = g.MeasureString(text, font);
            return Math.Max(size.Width, size.Height);
        }
        RectangleF GetRect(RectangleF rect, float size)
        {
            float w = rect.Width - size, h = rect.Height - size;
            return new RectangleF(-(w / 2), -(h / 2), w, h);
        }
    }
}