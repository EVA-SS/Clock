using System.Drawing.Drawing2D;

namespace Clock
{
    public static class Helper
    {
        #region 圆角

        public static GraphicsPath RoundPath(this RectangleF rect, float radius)
        {
            var path = new GraphicsPath();
            if (radius > 0F)
            {
                // If the corner radius is greater than or equal to
                // half the width, or height (whichever is shorter)
                // then return a capsule instead of a lozenge
                if (radius >= (Math.Min(rect.Width, rect.Height) / 2F)) AddCapsule(path, rect);
                else
                {
                    // Create the arc for the rectangle sides and declare
                    // a graphics path object for the drawing
                    float diameter = radius * 2F;
                    SizeF size = new SizeF(diameter, diameter);
                    RectangleF arc = new RectangleF(rect.Location, size);

                    // Top left arc
                    path.AddArc(arc, 180, 90);

                    // Top right arc
                    arc.X = rect.Right - diameter;
                    path.AddArc(arc, 270, 90);

                    // Bottom right arc
                    arc.Y = rect.Bottom - diameter;
                    path.AddArc(arc, 0, 90);

                    // Bottom left arc
                    arc.X = rect.Left;
                    path.AddArc(arc, 90, 90);

                    path.CloseFigure();
                }
            }
            else path.AddRectangle(rect);
            return path;
        }

        static void AddCapsule(GraphicsPath path, RectangleF rect)
        {
            float diameter;
            RectangleF arc;
            try
            {
                if (rect.Width > rect.Height)
                {
                    // Horizontal capsule
                    diameter = rect.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(rect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = rect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (rect.Width < rect.Height)
                {
                    // Vertical capsule
                    diameter = rect.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(rect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = rect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else
                {
                    // Circle
                    path.AddEllipse(rect);
                }
            }
            catch
            {
                path.AddEllipse(rect);
            }
            path.CloseFigure();
        }

        #endregion
    }
}