/*
 * This code is provided under the Code Project Open Licence (CPOL)
 * See http://www.codeproject.com/info/cpol10.aspx for details
 */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Piksel.LogViewer.Controls
{
	[ToolboxItem(false)]
	public class TabStyleChromeProvider : TabStyleProvider
	{
		public TabStyleChromeProvider(CustomTabControl tabControl) : base(tabControl){
            _Overlap = 16;
            _Radius = 16;
            _ShowTabCloser = true;
            _CloserColorActive = Color.White;

            //	Must set after the _Radius as this is used in the calculations of the actual padding
            Padding = new Point(7, 5);
		}

        /* Curvy border, ew
		public override void AddTabBorder(GraphicsPath path, Rectangle tabBounds){

			int spread;
			int eigth;
			int sixth;
			int quarter;

			if (_TabControl.Alignment <= TabAlignment.Bottom){
				spread = (int)Math.Floor((decimal)tabBounds.Height * 2/3);
				eigth = (int)Math.Floor((decimal)tabBounds.Height * 1/8);
				sixth = (int)Math.Floor((decimal)tabBounds.Height * 1/6);
				quarter = (int)Math.Floor((decimal)tabBounds.Height * 1/4);
			} else {
				spread = (int)Math.Floor((decimal)tabBounds.Width * 2/3);
				eigth = (int)Math.Floor((decimal)tabBounds.Width * 1/8);
				sixth = (int)Math.Floor((decimal)tabBounds.Width * 1/6);
				quarter = (int)Math.Floor((decimal)tabBounds.Width * 1/4);
			}

			switch (_TabControl.Alignment) {
				case TabAlignment.Top:

					path.AddCurve(new Point[] {  new Point(tabBounds.X, tabBounds.Bottom)
					              		,new Point(tabBounds.X + sixth, tabBounds.Bottom - eigth)
					              		,new Point(tabBounds.X + spread - quarter, tabBounds.Y + eigth)
					              		,new Point(tabBounds.X + spread, tabBounds.Y)});
					path.AddLine(tabBounds.X + spread, tabBounds.Y, tabBounds.Right - spread, tabBounds.Y);
					path.AddCurve(new Point[] {  new Point(tabBounds.Right - spread, tabBounds.Y)
					              		,new Point(tabBounds.Right - spread + quarter, tabBounds.Y + eigth)
					              		,new Point(tabBounds.Right - sixth, tabBounds.Bottom - eigth)
					              		,new Point(tabBounds.Right, tabBounds.Bottom)});
					break;
				case TabAlignment.Bottom:
					path.AddCurve(new Point[] {  new Point(tabBounds.Right, tabBounds.Y)
					              		,new Point(tabBounds.Right - sixth, tabBounds.Y + eigth)
					              		,new Point(tabBounds.Right - spread + quarter, tabBounds.Bottom - eigth)
					              		,new Point(tabBounds.Right - spread, tabBounds.Bottom)});
					path.AddLine(tabBounds.Right - spread, tabBounds.Bottom, tabBounds.X + spread, tabBounds.Bottom);
					path.AddCurve(new Point[] {  new Point(tabBounds.X + spread, tabBounds.Bottom)
					              		,new Point(tabBounds.X + spread - quarter, tabBounds.Bottom - eigth)
					              		,new Point(tabBounds.X + sixth, tabBounds.Y + eigth)
					              		,new Point(tabBounds.X, tabBounds.Y)});
					break;
				case TabAlignment.Left:
					path.AddCurve(new Point[] {  new Point(tabBounds.Right, tabBounds.Bottom)
					              		,new Point(tabBounds.Right - eigth, tabBounds.Bottom - sixth)
					              		,new Point(tabBounds.X + eigth, tabBounds.Bottom - spread + quarter)
					              		,new Point(tabBounds.X, tabBounds.Bottom - spread)});
					path.AddLine(tabBounds.X, tabBounds.Bottom - spread, tabBounds.X ,tabBounds.Y + spread);
					path.AddCurve(new Point[] {  new Point(tabBounds.X, tabBounds.Y + spread)
					              		,new Point(tabBounds.X + eigth, tabBounds.Y + spread - quarter)
					              		,new Point(tabBounds.Right - eigth, tabBounds.Y + sixth)
					              		,new Point(tabBounds.Right, tabBounds.Y)});

					break;
				case TabAlignment.Right:
					path.AddCurve(new Point[] {  new Point(tabBounds.X, tabBounds.Y)
					              		,new Point(tabBounds.X + eigth, tabBounds.Y + sixth)
					              		,new Point(tabBounds.Right - eigth, tabBounds.Y + spread - quarter)
					              		,new Point(tabBounds.Right, tabBounds.Y + spread)});
					path.AddLine(tabBounds.Right, tabBounds.Y + spread, tabBounds.Right, tabBounds.Bottom - spread);
					path.AddCurve(new Point[] {  new Point(tabBounds.Right, tabBounds.Bottom - spread)
					              		,new Point(tabBounds.Right - eigth, tabBounds.Bottom - spread + quarter)
					              		,new Point(tabBounds.X + eigth, tabBounds.Bottom - sixth)
					              		,new Point(tabBounds.X, tabBounds.Bottom)});
					break;
			}
		}
        */

        public override void AddTabBorder(GraphicsPath path, Rectangle tabBounds)
        {
            switch (_TabControl.Alignment)
            {
                case TabAlignment.Top:
                    path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X + _Radius - 2, tabBounds.Y + 2);
                    path.AddLine(tabBounds.X + _Radius, tabBounds.Y, tabBounds.Right - _Radius, tabBounds.Y);
                    path.AddLine(tabBounds.Right - _Radius + 2, tabBounds.Y + 2, tabBounds.Right, tabBounds.Bottom);
                    break;
                case TabAlignment.Bottom:
                    path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right - _Radius + 2, tabBounds.Bottom - 2);
                    path.AddLine(tabBounds.Right - _Radius, tabBounds.Bottom, tabBounds.X + _Radius, tabBounds.Bottom);
                    path.AddLine(tabBounds.X + _Radius - 2, tabBounds.Bottom - 2, tabBounds.X, tabBounds.Y);
                    break;
                case TabAlignment.Left:
                    path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X + 2, tabBounds.Bottom - _Radius + 2);
                    path.AddLine(tabBounds.X, tabBounds.Bottom - _Radius, tabBounds.X, tabBounds.Y + _Radius);
                    path.AddLine(tabBounds.X + 2, tabBounds.Y + _Radius - 2, tabBounds.Right, tabBounds.Y);
                    break;
                case TabAlignment.Right:
                    path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right - 2, tabBounds.Y + _Radius - 2);
                    path.AddLine(tabBounds.Right, tabBounds.Y + _Radius, tabBounds.Right, tabBounds.Bottom - _Radius);
                    path.AddLine(tabBounds.Right - 2, tabBounds.Bottom - _Radius + 2, tabBounds.X, tabBounds.Bottom);
                    break;
            }
        }

        protected override void DrawTabCloser(int index, Graphics graphics){
			if (_ShowTabCloser)
            {
				Rectangle closerRect = _TabControl.GetTabCloserRect(index);
				graphics.SmoothingMode = SmoothingMode.AntiAlias;

                //graphics.FillRectangle(new SolidBrush(Color.Cyan), closerRect);

                Image closeImage = closerRect.Contains(_TabControl.MousePosition)
                    ? Properties.Resources.cancel_24px
                    : Properties.Resources.close_nocircle_24px;
                graphics.DrawImage(closeImage, closerRect.Location);

                /*
                if (closerRect.Contains(_TabControl.MousePosition)){
                    
                    /*
					using (GraphicsPath closerPath = GetCloserButtonPath(closerRect)){
						using (SolidBrush closerBrush = new SolidBrush(Color.FromArgb(193, 53, 53))){
							graphics.FillPath(closerBrush, closerPath);
						}
					}
					using (GraphicsPath closerPath = GetCloserPath(closerRect)){
						using (Pen closerPen = new Pen(_CloserColorActive)){
							graphics.DrawPath(closerPen, closerPath);
						}
					}
                    
				} else {
                    
					using (GraphicsPath closerPath = GetCloserPath(closerRect)){
						using (Pen closerPen = new Pen(_CloserColor)){
							graphics.DrawPath(closerPen, closerPath);
						}
					}
				}
            */

			}
		}

		private static GraphicsPath GetCloserButtonPath(Rectangle closerRect){
			GraphicsPath closerPath = new GraphicsPath();
			closerPath.AddEllipse(new Rectangle(closerRect.X - 2, closerRect.Y - 2, closerRect.Width + 4, closerRect.Height + 4));
			closerPath.CloseFigure();
			return closerPath;
		}

        protected override Brush GetTabBackgroundBrush(int index)
        {

            LinearGradientBrush fillBrush = null;

            //	Capture the colours dependant on selection state of the tab
            Color dark = Color.FromArgb(220, 220, 220);
            Color light = Color.FromArgb(230, 230, 230);

            if (_TabControl.SelectedIndex == index)
            {
                dark = Color.Red;
                light = Color.White;
            }
            else if (!_TabControl.TabPages[index].Enabled)
            {
                light = dark;
            }
            else if (_HotTrack && index == _TabControl.ActiveIndex)
            {
                //	Enable hot tracking
                dark = light;
            }

            //	Get the correctly aligned gradient
            Rectangle tabBounds = GetTabRect(index);
            tabBounds.Inflate(3, 3);
            tabBounds.X -= 1;
            tabBounds.Y -= 1;
            switch (_TabControl.Alignment)
            {
                case TabAlignment.Top:
                    if (_TabControl.SelectedIndex == index)
                    {
                        dark = light;
                    }
                    fillBrush = new LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Vertical);
                    break;
                case TabAlignment.Bottom:
                    fillBrush = new LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Vertical);
                    break;
                case TabAlignment.Left:
                    fillBrush = new LinearGradientBrush(tabBounds, dark, light, LinearGradientMode.Horizontal);
                    break;
                case TabAlignment.Right:
                    fillBrush = new LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Horizontal);
                    break;
            }

            //	Add the blend
            //fillBrush.Blend = GetBackgroundBlend();

            return fillBrush;
        
        }
    }
}
