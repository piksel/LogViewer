using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Piksel.LogViewer.Helpers
{
    public static class ToolStripItemCollectionExtensions
    {
        public static ToolStripItem AddChained(this ToolStripItemCollection collection, ToolStripItem item)
            => collection[collection.Add(item)];

        public static int AddItemAt(this ToolStrip toolStrip, ToolStripItem item, int col = -1, int row = -1)
        {
            var index = toolStrip.Items.Add(item);
            item.Dock = DockStyle.Fill;
            var tls = toolStrip.LayoutSettings as TableLayoutSettings;
            if (col >= 0) tls.SetColumn(item, col);
            if (row >= 0) tls.SetRow(item, row);
            return index;
        }
    }
}
