using System.Windows.Forms;

namespace Piksel.LogViewer.Controls.Tabs.ToolStrips
{
    internal interface ITabPageToolStripContainer
    {
        ToolStrip ToolStripTop { get; }
        ToolStrip ToolStripBottom { get; }

        bool TopVisible { get; }
        bool BottomVisible { get; }
    }
}