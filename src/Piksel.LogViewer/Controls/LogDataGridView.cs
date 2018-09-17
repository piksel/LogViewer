using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Piksel.LogViewer.Controls
{
    public class LogDataGridView: DataGridView
    {
        public void ScrollToBottom()
        {
            FirstDisplayedScrollingRowIndex = RowCount - 1;

            UpdateRowHeightInfo(FirstDisplayedScrollingRowIndex, true);

            FirstDisplayedScrollingRowIndex = RowCount - 1;

            /*
            var ti = typeof(DataGridView).GetTypeInfo();
            //VerticalScrollBar.Value = VerticalScrollBar.Maximum;
            //var a = VerticalScrollBar.Value;
            var gridViewVScrolled = ti.GetDeclaredMethod("DataGridViewVScrolled");
            var getTrailHeight = ti.GetDeclaredMethod("ComputeHeightOfFittingTrailingScrollingRows");

            //var methods = typeof(DataGridView).GetTypeInfo().DeclaredMethods;
            //var methodtext = string.Join("\n", methods.Select(m => m.ToString()));

            int totalVisibleFrozenHeight = Rows.GetRowsHeight(DataGridViewElementStates.Visible | DataGridViewElementStates.Frozen);

            //if (totalVisibleFrozenHeight <= 0) return;

            int heightOffset = (int)getTrailHeight.Invoke(this, new object[] { totalVisibleFrozenHeight });


            var sea = new ScrollEventArgs(ScrollEventType.Last,
                VerticalScrollBar.Maximum, ScrollOrientation.VerticalScroll);

            var sea2 = new ScrollEventArgs(ScrollEventType.LargeIncrement,
    VerticalScrollBar.Maximum, ScrollOrientation.VerticalScroll);

            //try
            //{
            gridViewVScrolled.Invoke(this, new[] { null, sea });
                gridViewVScrolled.Invoke(this, new[] { null, sea2 });
                gridViewVScrolled.Invoke(this, new[] { null, sea2 });
            //}
            //catch (Exception) {}
            */
        }
    }
}
