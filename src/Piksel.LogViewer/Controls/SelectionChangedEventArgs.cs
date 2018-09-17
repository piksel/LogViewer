using System;

namespace Piksel.LogViewer.Controls
{
    public class SelectionChangedEventArgs: EventArgs
    {
        public int Index { get; }

        public SelectionChangedEventArgs(int index)
        {
            Index = index;
        }
    }
}