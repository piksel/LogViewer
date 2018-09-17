using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Piksel.LogViewer.Helpers
{
    public static class GeometryExtensions
    {
        public static Rectangle Inside(this Rectangle rect)
            => new Rectangle(rect.Location, rect.Size - new Size(1,1));

        public static Rectangle Inside(this Rectangle rect, int size)
            => new Rectangle(rect.Location.Add(1 - size), rect.Size.Add((size * 2) + 1));

        public static Size Add(this Size size, int addend)
            => size + new Size(addend, addend);

        public static Point Add(this Point point, int addend)
            => point + new Size(addend, addend);
    }
}
