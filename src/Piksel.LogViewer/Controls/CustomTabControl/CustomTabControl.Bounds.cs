using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Piksel.LogViewer.Controls
{
    public partial class CustomTabControl
    {
        private GraphicsPath GetTabPageBorder(int index)
        {

            var path = new GraphicsPath();
            var pageBounds = GetPageBounds(index);
            var tabBounds = _StyleProvider.GetTabRect(index);
            _StyleProvider.AddTabBorder(path, tabBounds);
            AddPageBorder(path, pageBounds, tabBounds);

            path.CloseFigure();
            return path;
        }

        public Rectangle GetPageBounds(int index)
        {
            var pageBounds = TabPages[index].Bounds;
            pageBounds.Width += 1;
            pageBounds.Height += 1;
            pageBounds.X -= 1;
            pageBounds.Y -= 1;

            if (pageBounds.Bottom > Height - 4)
            {
                pageBounds.Height -= (pageBounds.Bottom - Height + 4);
            }
            return pageBounds;
        }

        private Rectangle GetTabTextRect(int index)
        {
            Rectangle textRect = new Rectangle();
            using (GraphicsPath path = _StyleProvider.GetTabBorder(index))
            {
                RectangleF tabBounds = path.GetBounds();

                textRect = new Rectangle((int)tabBounds.X, (int)tabBounds.Y, (int)tabBounds.Width, (int)tabBounds.Height);

                //	Make it shorter or thinner to fit the height or width because of the padding added to the tab for painting
                switch (Alignment)
                {
                    case TabAlignment.Top:
                        textRect.Y += 4;
                        textRect.Height -= 6;
                        break;
                    case TabAlignment.Bottom:
                        textRect.Y += 2;
                        textRect.Height -= 6;
                        break;
                    case TabAlignment.Left:
                        textRect.X += 4;
                        textRect.Width -= 6;
                        break;
                    case TabAlignment.Right:
                        textRect.X += 2;
                        textRect.Width -= 6;
                        break;
                }

                //	If there is an image allow for it
                if (ImageList != null && (TabPages[index].ImageIndex > -1
                                               || (!string.IsNullOrEmpty(TabPages[index].ImageKey)
                                                   && !TabPages[index].ImageKey.Equals("(none)", StringComparison.OrdinalIgnoreCase))))
                {
                    Rectangle imageRect = GetTabImageRect(index);
                    if ((_StyleProvider.ImageAlign & NativeMethods.AnyLeftAlign) != ((ContentAlignment)0))
                    {
                        if (Alignment <= TabAlignment.Bottom)
                        {
                            textRect.X = imageRect.Right + 4;
                            textRect.Width -= (textRect.Right - (int)tabBounds.Right);
                        }
                        else
                        {
                            textRect.Y = imageRect.Y + 4;
                            textRect.Height -= (textRect.Bottom - (int)tabBounds.Bottom);
                        }
                        //	If there is a closer allow for it
                        if (_StyleProvider.ShowTabCloser)
                        {
                            Rectangle closerRect = GetTabCloserRect(index);
                            if (Alignment <= TabAlignment.Bottom)
                            {
                                if (RightToLeftLayout)
                                {
                                    textRect.Width -= (closerRect.Right + 4 - textRect.X);
                                    textRect.X = closerRect.Right + 4;
                                }
                                else
                                {
                                    textRect.Width -= ((int)tabBounds.Right - closerRect.X + 4);
                                }
                            }
                            else
                            {
                                if (RightToLeftLayout)
                                {
                                    textRect.Height -= (closerRect.Bottom + 4 - textRect.Y);
                                    textRect.Y = closerRect.Bottom + 4;
                                }
                                else
                                {
                                    textRect.Height -= ((int)tabBounds.Bottom - closerRect.Y + 4);
                                }
                            }
                        }
                    }
                    else if ((_StyleProvider.ImageAlign & NativeMethods.AnyCenterAlign) != ((ContentAlignment)0))
                    {
                        //	If there is a closer allow for it
                        if (_StyleProvider.ShowTabCloser)
                        {
                            Rectangle closerRect = GetTabCloserRect(index);
                            if (Alignment <= TabAlignment.Bottom)
                            {
                                if (RightToLeftLayout)
                                {
                                    textRect.Width -= (closerRect.Right + 4 - textRect.X);
                                    textRect.X = closerRect.Right + 4;
                                }
                                else
                                {
                                    textRect.Width -= ((int)tabBounds.Right - closerRect.X + 4);
                                }
                            }
                            else
                            {
                                if (RightToLeftLayout)
                                {
                                    textRect.Height -= (closerRect.Bottom + 4 - textRect.Y);
                                    textRect.Y = closerRect.Bottom + 4;
                                }
                                else
                                {
                                    textRect.Height -= ((int)tabBounds.Bottom - closerRect.Y + 4);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Alignment <= TabAlignment.Bottom)
                        {
                            textRect.Width -= ((int)tabBounds.Right - imageRect.X + 4);
                        }
                        else
                        {
                            textRect.Height -= ((int)tabBounds.Bottom - imageRect.Y + 4);
                        }
                        //	If there is a closer allow for it
                        if (_StyleProvider.ShowTabCloser)
                        {
                            Rectangle closerRect = GetTabCloserRect(index);
                            if (Alignment <= TabAlignment.Bottom)
                            {
                                if (RightToLeftLayout)
                                {
                                    textRect.Width -= (closerRect.Right + 4 - textRect.X);
                                    textRect.X = closerRect.Right + 4;
                                }
                                else
                                {
                                    textRect.Width -= ((int)tabBounds.Right - closerRect.X + 4);
                                }
                            }
                            else
                            {
                                if (RightToLeftLayout)
                                {
                                    textRect.Height -= (closerRect.Bottom + 4 - textRect.Y);
                                    textRect.Y = closerRect.Bottom + 4;
                                }
                                else
                                {
                                    textRect.Height -= ((int)tabBounds.Bottom - closerRect.Y + 4);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //	If there is a closer allow for it
                    if (_StyleProvider.ShowTabCloser)
                    {
                        Rectangle closerRect = GetTabCloserRect(index);
                        if (Alignment <= TabAlignment.Bottom)
                        {
                            if (RightToLeftLayout)
                            {
                                textRect.Width -= (closerRect.Right + 4 - textRect.X);
                                textRect.X = closerRect.Right + 4;
                            }
                            else
                            {
                                textRect.Width -= ((int)tabBounds.Right - closerRect.X + 4);
                            }
                        }
                        else
                        {
                            if (RightToLeftLayout)
                            {
                                textRect.Height -= (closerRect.Bottom + 4 - textRect.Y);
                                textRect.Y = closerRect.Bottom + 4;
                            }
                            else
                            {
                                textRect.Height -= ((int)tabBounds.Bottom - closerRect.Y + 4);
                            }
                        }
                    }
                }


                //	Ensure it fits inside the path at the centre line
                if (Alignment <= TabAlignment.Bottom)
                {
                    while (!path.IsVisible(textRect.Right, textRect.Y) && textRect.Width > 0)
                    {
                        textRect.Width -= 1;
                    }
                    while (!path.IsVisible(textRect.X, textRect.Y) && textRect.Width > 0)
                    {
                        textRect.X += 1;
                        textRect.Width -= 1;
                    }
                }
                else
                {
                    while (!path.IsVisible(textRect.X, textRect.Bottom) && textRect.Height > 0)
                    {
                        textRect.Height -= 1;
                    }
                    while (!path.IsVisible(textRect.X, textRect.Y) && textRect.Height > 0)
                    {
                        textRect.Y += 1;
                        textRect.Height -= 1;
                    }
                }
            }
            return textRect;
        }

        public int GetTabRow(int index)
        {
            //	All calculations will use this rect as the base point
            //	because the itemsize does not return the correct width.
            Rectangle rect = GetTabRect(index);

            int row = -1;

            switch (Alignment)
            {
                case TabAlignment.Top:
                    row = (rect.Y - 2) / rect.Height;
                    break;
                case TabAlignment.Bottom:
                    row = ((Height - rect.Y - 2) / rect.Height) - 1;
                    break;
                case TabAlignment.Left:
                    row = (rect.X - 2) / rect.Width;
                    break;
                case TabAlignment.Right:
                    row = ((Width - rect.X - 2) / rect.Width) - 1;
                    break;
            }
            return row;
        }

        public Point GetTabPosition(int index)
        {

            //	If we are not multiline then the column is the index and the row is 0.
            if (!Multiline)
            {
                return new Point(0, index);
            }

            //	If there is only one row then the column is the index
            if (RowCount == 1)
            {
                return new Point(0, index);
            }

            //	We are in a true multi-row scenario
            int row = GetTabRow(index);
            Rectangle rect = GetTabRect(index);
            int column = -1;

            //	Scan from left to right along rows, skipping to next row if it is not the one we want.
            for (int testIndex = 0; testIndex < TabCount; testIndex++)
            {
                Rectangle testRect = GetTabRect(testIndex);
                if (Alignment <= TabAlignment.Bottom)
                {
                    if (testRect.Y == rect.Y)
                    {
                        column += 1;
                    }
                }
                else
                {
                    if (testRect.X == rect.X)
                    {
                        column += 1;
                    }
                }

                if (testRect.Location.Equals(rect.Location))
                {
                    return new Point(row, column);
                }
            }

            return new Point(0, 0);
        }

        public bool IsFirstTabInRow(int index)
        {
            if (index < 0)
            {
                return false;
            }
            bool firstTabinRow = (index == 0);
            if (!firstTabinRow)
            {
                if (Alignment <= TabAlignment.Bottom)
                {
                    if (GetTabRect(index).X == 2)
                    {
                        firstTabinRow = true;
                    }
                }
                else
                {
                    if (GetTabRect(index).Y == 2)
                    {
                        firstTabinRow = true;
                    }
                }
            }
            return firstTabinRow;
        }

        private void AddPageBorder(GraphicsPath path, Rectangle pageBounds, Rectangle tabBounds)
        {
            switch (Alignment)
            {
                case TabAlignment.Top:
                    path.AddLine(tabBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Y);
                    path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Bottom);
                    path.AddLine(pageBounds.Right, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom);
                    path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Y);
                    path.AddLine(pageBounds.X, pageBounds.Y, tabBounds.X, pageBounds.Y);
                    break;
                case TabAlignment.Bottom:
                    path.AddLine(tabBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom);
                    path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Y);
                    path.AddLine(pageBounds.X, pageBounds.Y, pageBounds.Right, pageBounds.Y);
                    path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Bottom);
                    path.AddLine(pageBounds.Right, pageBounds.Bottom, tabBounds.Right, pageBounds.Bottom);
                    break;
                case TabAlignment.Left:
                    path.AddLine(pageBounds.X, tabBounds.Y, pageBounds.X, pageBounds.Y);
                    path.AddLine(pageBounds.X, pageBounds.Y, pageBounds.Right, pageBounds.Y);
                    path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Bottom);
                    path.AddLine(pageBounds.Right, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom);
                    path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, tabBounds.Bottom);
                    break;
                case TabAlignment.Right:
                    path.AddLine(pageBounds.Right, tabBounds.Bottom, pageBounds.Right, pageBounds.Bottom);
                    path.AddLine(pageBounds.Right, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom);
                    path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Y);
                    path.AddLine(pageBounds.X, pageBounds.Y, pageBounds.Right, pageBounds.Y);
                    path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, tabBounds.Y);
                    break;
            }
        }

        private Rectangle GetTabImageRect(int index)
        {
            using (GraphicsPath tabBorderPath = _StyleProvider.GetTabBorder(index))
            {
                return GetTabImageRect(tabBorderPath);
            }
        }

        private Rectangle GetTabImageRect(GraphicsPath tabBorderPath)
        {
            Rectangle imageRect = new Rectangle();
            RectangleF rect = tabBorderPath.GetBounds();

            //	Make it shorter or thinner to fit the height or width because of the padding added to the tab for painting
            switch (Alignment)
            {
                case TabAlignment.Top:
                    rect.Y += 4;
                    rect.Height -= 6;
                    break;
                case TabAlignment.Bottom:
                    rect.Y += 2;
                    rect.Height -= 6;
                    break;
                case TabAlignment.Left:
                    rect.X += 4;
                    rect.Width -= 6;
                    break;
                case TabAlignment.Right:
                    rect.X += 2;
                    rect.Width -= 6;
                    break;
            }

            //	Ensure image is fully visible
            if (Alignment <= TabAlignment.Bottom)
            {
                if ((_StyleProvider.ImageAlign & NativeMethods.AnyLeftAlign) != ((ContentAlignment)0))
                {
                    imageRect = new Rectangle((int)rect.X, (int)rect.Y + (int)Math.Floor((double)((int)rect.Height - 16) / 2), 16, 16);
                    while (!tabBorderPath.IsVisible(imageRect.X, imageRect.Y))
                    {
                        imageRect.X += 1;
                    }
                    imageRect.X += 4;

                }
                else if ((_StyleProvider.ImageAlign & NativeMethods.AnyCenterAlign) != ((ContentAlignment)0))
                {
                    imageRect = new Rectangle((int)rect.X + (int)Math.Floor((double)(((int)rect.Right - (int)rect.X - (int)rect.Height + 2) / 2)), (int)rect.Y + (int)Math.Floor((double)((int)rect.Height - 16) / 2), 16, 16);
                }
                else
                {
                    imageRect = new Rectangle((int)rect.Right, (int)rect.Y + (int)Math.Floor((double)((int)rect.Height - 16) / 2), 16, 16);
                    while (!tabBorderPath.IsVisible(imageRect.Right, imageRect.Y))
                    {
                        imageRect.X -= 1;
                    }
                    imageRect.X -= 4;

                    //	Move it in further to allow for the tab closer
                    if (_StyleProvider.ShowTabCloser && !RightToLeftLayout)
                    {
                        imageRect.X -= 10;
                    }
                }
            }
            else
            {
                if ((_StyleProvider.ImageAlign & NativeMethods.AnyLeftAlign) != ((ContentAlignment)0))
                {
                    imageRect = new Rectangle((int)rect.X + (int)Math.Floor((double)((int)rect.Width - 16) / 2), (int)rect.Y, 16, 16);
                    while (!tabBorderPath.IsVisible(imageRect.X, imageRect.Y))
                    {
                        imageRect.Y += 1;
                    }
                    imageRect.Y += 4;
                }
                else if ((_StyleProvider.ImageAlign & NativeMethods.AnyCenterAlign) != ((ContentAlignment)0))
                {
                    imageRect = new Rectangle((int)rect.X + (int)Math.Floor((double)((int)rect.Width - 16) / 2), (int)rect.Y + (int)Math.Floor((double)(((int)rect.Bottom - (int)rect.Y - (int)rect.Width + 2) / 2)), 16, 16);
                }
                else
                {
                    imageRect = new Rectangle((int)rect.X + (int)Math.Floor((double)((int)rect.Width - 16) / 2), (int)rect.Bottom, 16, 16);
                    while (!tabBorderPath.IsVisible(imageRect.X, imageRect.Bottom))
                    {
                        imageRect.Y -= 1;
                    }
                    imageRect.Y -= 4;

                    //	Move it in further to allow for the tab closer
                    if (_StyleProvider.ShowTabCloser && !RightToLeftLayout)
                    {
                        imageRect.Y -= 10;
                    }
                }
            }
            return imageRect;
        }

        const int CloserSize = 24;
        const float CloserHalf = (float)CloserSize / 2;

        public Rectangle GetTabCloserRect(int index)
        {
            Rectangle closerRect = new Rectangle();
            using (GraphicsPath path = _StyleProvider.GetTabBorder(index))
            {
                RectangleF rect = path.GetBounds();


                //	Make it shorter or thinner to fit the height or width because of the padding added to the tab for painting
                switch (Alignment)
                {
                    case TabAlignment.Top:
                        rect.Y += 4;
                        rect.Height -= 6;
                        break;
                    case TabAlignment.Bottom:
                        rect.Y += 2;
                        rect.Height -= 6;
                        break;
                    case TabAlignment.Left:
                        rect.X += 4;
                        rect.Width -= 6;
                        break;
                    case TabAlignment.Right:
                        rect.X += 2;
                        rect.Width -= 6;
                        break;
                }

                if (Alignment <= TabAlignment.Bottom)
                {
                    // Horizontal tabs
                    int offsetTop = (int)rect.Y + (int)Math.Floor((double)((int)rect.Height - CloserSize) / 2);

                    if (RightToLeftLayout)
                    {
                        closerRect = new Rectangle((int)(rect.Left + 6), offsetTop, CloserSize, CloserSize);
                        /*
                        while (!path.IsVisible(closerRect.Left, closerRect.Y) && closerRect.Right < Width) closerRect.X++;
                        */
                        closerRect.X += 4;
                    }
                    else // LeftToRight
                    {
                        closerRect = new Rectangle((int)(rect.Right - CloserSize - 6), offsetTop, CloserSize, CloserSize);

                        /*
                        while (!path.IsVisible(closerRect.Right, closerRect.Y) && closerRect.Right > -CloserSize) closerRect.X--;
                        */

                        closerRect.X -= 4;
                    }
                }
                else
                {
                    // Vertical tabs
                    if (RightToLeftLayout)
                    {
                        closerRect = new Rectangle((int)rect.X + (int)Math.Floor((double)((int)rect.Width - 6) / 2), (int)rect.Top, 6, 6);
                        while (!path.IsVisible(closerRect.X, closerRect.Top) && closerRect.Bottom < Height)
                        {
                            closerRect.Y += 1;
                        }
                        closerRect.Y += 4;
                    }
                    else // LeftToRight
                    {
                        closerRect = new Rectangle((int)rect.X + (int)Math.Floor((double)((int)rect.Width - 6) / 2), (int)rect.Bottom, 6, 6);
                        while (!path.IsVisible(closerRect.X, closerRect.Bottom) && closerRect.Top > -6)
                        {
                            closerRect.Y -= 1;
                        }
                        closerRect.Y -= 4;
                    }
                }
            }

            return closerRect;
        }

        public new Point MousePosition
        {
            get
            {
                Point loc = PointToClient(Control.MousePosition);
                if (RightToLeftLayout)
                {
                    loc.X = (Width - loc.X);
                }
                return loc;
            }
        }
    }
}
