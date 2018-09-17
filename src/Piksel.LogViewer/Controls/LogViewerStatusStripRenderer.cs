using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Piksel.LogViewer.Controls
{
    public class LogViewerStatusStripRenderer: ToolStripProfessionalRenderer
    {
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            var ab = e.AffectedBounds;
            e.Graphics.DrawLine(new Pen(SystemColors.ActiveBorder), ab.X, ab.Top+1, ab.Right - 1, ab.Top+1);

            base.OnRenderToolStripBorder(e);
        }
    }
}
