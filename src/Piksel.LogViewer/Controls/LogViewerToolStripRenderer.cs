using Piksel.LogViewer.Helpers;
using System.Drawing;
using System.Windows.Forms;

namespace Piksel.LogViewer.Controls
{
    public class LogViewerToolStripRenderer : ToolStripProfessionalRenderer
    {
        public bool NoBackground { get; set; }
        public bool NoBorder { get; set; }
        public Color? SeparatorColor { get; internal set; }
        public bool TopBorder { get; set; }
        public bool NoTextboxBackground { get; internal set; }

        public LogViewerToolStripRenderer()
        {
            RoundedEdges = false;
        }

        protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
        {
            if(!NoBackground)
                base.OnRenderToolStripPanelBackground(e);
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (!NoBackground)
                base.OnRenderToolStripBackground(e);
        }

        protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (!NoTextboxBackground)
                base.OnRenderItemBackground(e);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (NoBorder) return;

            var ab = e.AffectedBounds;
            if (TopBorder)
            {
                e.Graphics.DrawLine(new Pen(SystemColors.ActiveBorder), ab.X, 0, ab.Right - 1, 0);
            }
            else
            {
                e.Graphics.DrawLine(new Pen(SystemColors.ActiveBorder), ab.X, ab.Bottom - 2, ab.Right - 1, ab.Bottom - 2);
            }

            base.OnRenderToolStripBorder(e);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (!(e.Item is ToolStripButton item) || item.CheckState != CheckState.Checked || item.Selected)
            {
                base.OnRenderButtonBackground(e);
                return;
            }

            if (!e.Item.Enabled) return;

            var bounds = new Rectangle(Point.Empty, e.Item.Size);
            var brush = new SolidBrush(Color.FromArgb(128, ColorTable.ButtonCheckedHighlight));
            e.Graphics.FillRectangle(brush, bounds);

            using (Pen p = new Pen(ColorTable.ButtonSelectedBorder))
            {
                e.Graphics.DrawRectangle(p, bounds.Inside());
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            // Just use space as the separator
            return;

            /*
            if (SeparatorColor.HasValue)
            {
                var rect = new RectangleF(PointF.Empty, e.Item.Size);
                rect.X = rect.Width / 2;
                rect.Width = 1;
                e.Graphics.FillRectangle(new SolidBrush(SeparatorColor.Value), rect);
            }
            else
            {
                base.OnRenderSeparator(e);
            }
            */
        }

    }
}
