using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Piksel.LogViewer.Controls
{
    public partial class CustomTabControl: TabControl
    {

        private bool _painting = false;

        protected override void OnPaint(PaintEventArgs e)
        {
            //	We must always paint the entire area of the tab control
            if (e.ClipRectangle.Equals(ClientRectangle) || _painting)
            {
                _painting = false;
                CustomPaint(e.Graphics);
            }
            else
            {
                //	it is less intensive to just reinvoke the paint with the whole surface available to draw on.
                _painting = true;
                Invalidate();
            }
        }

        private void CustomPaint(Graphics screenGraphics)
        {
            //	We render into a bitmap that is then drawn in one shot rather than using
            //	double buffering built into the control as the built in buffering
            // 	messes up the background painting.
            //	Equally the .Net 2.0 BufferedGraphics object causes the background painting
            //	to mess up, which is why we use this .Net 1.1 buffering technique.

            //	Buffer code from Gil. Schmidt http://www.codeproject.com/KB/graphics/DoubleBuffering.aspx

            if (Width > 0 && Height > 0)
            {
                if (_BackImage == null)
                {
                    //	Cached Background Image
                    _BackImage = new Bitmap(Width, Height);
                    Graphics backGraphics = Graphics.FromImage(_BackImage);
                    backGraphics.Clear(Color.Transparent);
                    PaintTransparentBackground(backGraphics, ClientRectangle);
                }

                _BackBufferGraphics.Clear(Color.Transparent);
                _BackBufferGraphics.DrawImageUnscaled(_BackImage, 0, 0);

                _TabBufferGraphics.Clear(Color.Transparent);

                if (TabCount > 0)
                {

                    //	When top or bottom and scrollable we need to clip the sides from painting the tabs.
                    //	Left and right are always multiline.
                    if (Alignment <= TabAlignment.Bottom && !Multiline)
                    {
                        _TabBufferGraphics.Clip = new Region(new RectangleF(ClientRectangle.X + 3, ClientRectangle.Y, ClientRectangle.Width - 6, ClientRectangle.Height));
                    }

                    //	Draw each tabpage from right to left.  We do it this way to handle
                    //	the overlap correctly.
                    if (Multiline)
                    {
                        for (int row = 0; row < RowCount; row++)
                        {
                            for (int index = TabCount - 1; index >= 0; index--)
                            {
                                if (index != SelectedIndex && (RowCount == 1 || GetTabRow(index) == row))
                                {
                                    DrawTabPage(index, _TabBufferGraphics);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int index = TabCount - 1; index >= 0; index--)
                        {
                            if (index != SelectedIndex)
                            {
                                DrawTabPage(index, _TabBufferGraphics);
                            }
                        }
                    }

                    //	The selected tab must be drawn last so it appears on top.
                    if (SelectedIndex > -1)
                    {
                        DrawTabPage(SelectedIndex, _TabBufferGraphics);
                    }
                }
                _TabBufferGraphics.Flush();

                //	Paint the tabs on top of the background

                // Create a new color matrix and set the alpha value to 0.5
                var alphaMatrix = new ColorMatrix();
                alphaMatrix.Matrix00 = alphaMatrix.Matrix11 = alphaMatrix.Matrix22 = alphaMatrix.Matrix44 = 1;
                alphaMatrix.Matrix33 = _StyleProvider.Opacity;

                // Create a new image attribute object and set the color matrix to
                // the one just created
                using (ImageAttributes alphaAttributes = new ImageAttributes())
                {

                    alphaAttributes.SetColorMatrix(alphaMatrix);

                    // Draw the original image with the image attributes specified
                    _BackBufferGraphics.DrawImage(_TabBuffer,
                                                       new Rectangle(0, 0, _TabBuffer.Width, _TabBuffer.Height),
                                                       0, 0, _TabBuffer.Width, _TabBuffer.Height, GraphicsUnit.Pixel,
                                                       alphaAttributes);
                }

                _BackBufferGraphics.Flush();

                //	Now paint this to the screen


                //	We want to paint the whole tabstrip and border every time
                //	so that the hot areas update correctly, along with any overlaps

                //	paint the tabs etc.
                if (RightToLeftLayout)
                {
                    screenGraphics.DrawImageUnscaled(_BackBuffer, -1, 0);
                }
                else
                {
                    screenGraphics.DrawImageUnscaled(_BackBuffer, 0, 0);
                }
            }
        }

        protected void PaintTransparentBackground(Graphics graphics, Rectangle clipRect)
        {

            if ((Parent != null))
            {

                //	Set the cliprect to be relative to the parent
                clipRect.Offset(Location);

                //	Save the current state before we do anything.
                var state = graphics.Save();

                //	Set the graphicsobject to be relative to the parent
                graphics.TranslateTransform(-Location.X, -Location.Y);
                graphics.SmoothingMode = SmoothingMode.HighSpeed;

                //	Paint the parent
                var e = new PaintEventArgs(graphics, clipRect);
                try
                {
                    InvokePaintBackground(Parent, e);
                    InvokePaint(Parent, e);
                }
                finally
                {
                    //	Restore the graphics state and the clipRect to their original locations
                    graphics.Restore(state);
                    clipRect.Offset(-Location.X, -Location.Y);
                }
            }
        }

        private void DrawTabPage(int index, Graphics graphics)
        {
            graphics.SmoothingMode = SmoothingMode.HighSpeed;

            //	Get TabPageBorder
            using (var tabPageBorderPath = GetTabPageBorder(index))
            {

                //	Paint the background
                using (var fillBrush = _StyleProvider.GetPageBackgroundBrush(index))
                {
                    graphics.FillPath(fillBrush, tabPageBorderPath);
                }

                if (_Style != TabStyle.None)
                {

                    //	Paint the tab
                    _StyleProvider.PaintTab(index, graphics);

                    //	Draw any image
                    DrawTabImage(index, graphics);

                    //	Draw the text
                    DrawTabText(index, graphics);

                }

                //	Paint the border
                DrawTabBorder(tabPageBorderPath, index, graphics);

            }
        }

        private void DrawTabBorder(GraphicsPath path, int index, Graphics graphics)
        {
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            Color borderColor;
            if (index == SelectedIndex)
            {
                borderColor = _StyleProvider.BorderColorSelected;
            }
            else if (_StyleProvider.HotTrack && index == ActiveIndex)
            {
                borderColor = _StyleProvider.BorderColorHot;
            }
            else
            {
                borderColor = _StyleProvider.BorderColor;
            }

            using (Pen borderPen = new Pen(borderColor))
            {
                graphics.DrawPath(borderPen, path);
            }
        }

        private void DrawTabText(int index, Graphics graphics)
        {
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            Rectangle tabBounds = GetTabTextRect(index);

            if (SelectedIndex == index)
            {
                using (Brush textBrush = new SolidBrush(_StyleProvider.TextColorSelected))
                {
                    graphics.DrawString(TabPages[index].Text, Font, textBrush, tabBounds, GetStringFormat());
                }
            }
            else
            {
                if (TabPages[index].Enabled)
                {
                    using (Brush textBrush = new SolidBrush(_StyleProvider.TextColor))
                    {
                        graphics.DrawString(TabPages[index].Text, Font, textBrush, tabBounds, GetStringFormat());
                    }
                }
                else
                {
                    using (Brush textBrush = new SolidBrush(_StyleProvider.TextColorDisabled))
                    {
                        graphics.DrawString(TabPages[index].Text, Font, textBrush, tabBounds, GetStringFormat());
                    }
                }
            }
        }

        private void DrawTabImage(int index, Graphics graphics)
        {
            Image tabImage = null;
            if (TabPages[index].ImageIndex > -1 && ImageList != null && ImageList.Images.Count > TabPages[index].ImageIndex)
            {
                tabImage = ImageList.Images[TabPages[index].ImageIndex];
            }
            else if ((!string.IsNullOrEmpty(TabPages[index].ImageKey) && !TabPages[index].ImageKey.Equals("(none)", StringComparison.OrdinalIgnoreCase))
                     && ImageList != null && ImageList.Images.ContainsKey(TabPages[index].ImageKey))
            {
                tabImage = ImageList.Images[TabPages[index].ImageKey];
            }

            if (tabImage != null)
            {
                if (RightToLeftLayout)
                {
                    tabImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                Rectangle imageRect = GetTabImageRect(index);
                if (TabPages[index].Enabled)
                {
                    graphics.DrawImage(tabImage, imageRect);
                }
                else
                {
                    ControlPaint.DrawImageDisabled(graphics, tabImage, imageRect.X, imageRect.Y, Color.Transparent);
                }
            }
        }

        public bool InvalidateTab(TabPage tab)
        {
            //tab.Invalidate();
            Invalidate();
            return true;
        }
    }
}
