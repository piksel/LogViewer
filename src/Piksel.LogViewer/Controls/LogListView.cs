using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Piksel.LogViewer.Logging;

namespace Piksel.LogViewer.Controls
{
    public partial class LogListView : UserControl
    {
        private int itemsBeforeUpdate;
        private bool scrollAfterUpdate = true;
        private bool updating;
        private bool _configVisible;

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        public LogListView()
        {
            InitializeComponent();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = Items;

            dataGridView1.SelectionChanged += (s, e) =>
            {
                var index = dataGridView1.SelectedRows.Count > 0 ? dataGridView1.SelectedRows[0].Index : -1;
                SelectionChanged?.Invoke(s, new SelectionChangedEventArgs(index));
            };
        }

        public bool AutoScrollToBottom { get; set; } = true;

        public LogBindingList Items { get; } = new LogBindingList();
        public bool ConfigBarVisible
        {
            get => _configVisible;
            set
            {
                _configVisible = value;
                if (!value)
                {
                    toolStripContainer1.TopToolStripPanelVisible = false;
                    toolStripContainer1.BottomToolStripPanelVisible = false;
                }
                else
                {
                    toolStripContainer1.TopToolStripPanelVisible 
                        = toolStripContainer1.TopToolStripPanel.Controls.Count > 0;
                    toolStripContainer1.BottomToolStripPanelVisible 
                        = toolStripContainer1.BottomToolStripPanel.Controls.Count > 0;
                }
            }
        }

        private void LogListView_Resize(object sender, EventArgs e)
        {

        }

        private void logMessageBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= Items.Count) return;

            var lm = Items[e.RowIndex] as LogMessage;
            if (e.DesiredType == typeof(Image))
            {
                e.Value = imageList1.Images[lm.LogLevel.ToString()];
            }
            else
            {

                var col = dataGridView1.Columns[e.ColumnIndex];
                var prop = typeof(LogMessage).GetProperty(col.DataPropertyName);

                e.Value = prop?.GetValue(lm);



                //var b = dataGridView1.Rows[e.RowIndex]
            }
        }

        internal void ScrollToBottom()
        {
            var lastRowIndex = dataGridView1.RowCount - 1;
            dataGridView1.FirstDisplayedScrollingRowIndex = lastRowIndex;
            var currentRow = dataGridView1.Rows[lastRowIndex].Selected = true;
        }

        internal void BeginUpdate()
        {
            itemsBeforeUpdate = Items.Count;
            var rows = dataGridView1.Rows;
            /*
            if (rows.GetLastRow(DataGridViewElementStates.Displayed) == rows.GetLastRow(DataGridViewElementStates.None))
            {
                scrollAfterUpdate = true;
            }
            else
            {
                scrollAfterUpdate = false;
            }
            */
            updating = true;
            dataGridView1.SuspendLayout();
        }

        internal void EndUpdate()
        {
            var itemsAfterUpdate = Items.Count;
            updating = false;
            dataGridView1.ResumeLayout();
            if(itemsAfterUpdate > itemsBeforeUpdate)
            {
                ScrollToBottom();
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (!updating && e.RowIndex >= 0 && scrollAfterUpdate)
            {
                /*
                var lastRowIndex = dataGridView1.RowCount - 1;// (e.RowIndex + e.RowCount)-1;
                if (dataGridView1.RowCount <= lastRowIndex)
                    return;

                //dataGridView1.ClearSelection();
                
                dataGridView1.Rows[lastRowIndex].Selected = true;
                dataGridView1.FirstDisplayedScrollingRowIndex = lastRowIndex;
                */
                ScrollToBottom();
            }
        }

        internal void UpdateVisibleLogLevels(LogLevel enabledLevels)
        {
            Items.Filter = enabledLevels.ToString();
        }

        internal void SetErrorFields(LogField[] errorFields)
        {
            ColLevel.CellTemplate.Style.BackColor = errorFields.Contains(LogField.Level)
                ? Color.MistyRose
                : SystemColors.Window;
            ColTime.CellTemplate.Style.BackColor = errorFields.Contains(LogField.Time)
                ? Color.MistyRose
                : SystemColors.Window;
            ColSource.CellTemplate.Style.BackColor = errorFields.Contains(LogField.Source)
                ? Color.MistyRose
                : SystemColors.Window;
            //if(errorFields.Contains(LogField.Level)) 
        }

        internal void SetToolStrips(ToolStrip top, ToolStrip bottom)
        {
            if(top != null)
            {
                toolStripContainer1.TopToolStripPanel.Controls.Add(top);
                toolStripContainer1.TopToolStripPanelVisible = _configVisible;
            }
            else
            {
                toolStripContainer1.TopToolStripPanelVisible = false;
            }

            if (bottom != null)
            {
                toolStripContainer1.BottomToolStripPanel.Controls.Add(bottom);
                toolStripContainer1.BottomToolStripPanelVisible = _configVisible;
            }
            else
            {
                toolStripContainer1.BottomToolStripPanelVisible = false;
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
