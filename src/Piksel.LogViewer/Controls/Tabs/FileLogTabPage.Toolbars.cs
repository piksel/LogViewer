using Piksel.LogViewer.Helpers;
using Piksel.LogViewer.Logging;
using System.Drawing;
using System.Windows.Forms;

namespace Piksel.LogViewer.Controls.Tabs
{
    public partial class FileLogTabPage
    {
        ToolStrip topToolStrip;
        ToolStrip bottomToolStrip;

        public void InitializeToolStrips()
        {
            var cbDelims = new ToolStripComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cbDelims.SelectedIndexChanged += (s, e) =>
            {
                if (cbDelims.SelectedItem is Delimiter delim)
                {
                    Delimiter = delim;
                }
            };

            cbDelims.Items.AddRange(delimiters);
            if (_delimiter == null)
            {
                cbDelims.SelectedIndex = 0;
            }
            else
            {
                cbDelims.SelectedItem = _delimiter;
            }


            var tbOrder = new ToolStripTextBox()
            {
                Text = FieldOrder.ToString(),
            };
            tbOrder.TextChanged += (s, e) =>
            {
                if (!FieldOrder.TrySetFromString(tbOrder.Text))
                {
                    tbOrder.BackColor = Color.MistyRose;
                }
                else
                {
                    tbOrder.BackColor = SystemColors.Window;
                    ReparseLines();
                }
            };

            tbOrder.AutoSize = false;
            tbOrder.Width = 200;

            topToolStrip = new ToolStrip()
            {
                LayoutStyle = ToolStripLayoutStyle.Table,
                Stretch = true,
                Renderer = new LogViewerToolStripRenderer()
                {
                    NoBackground = false,
                    NoBorder = false
                },
                Height = 30
            };

            var topSettings = topToolStrip.LayoutSettings as TableLayoutSettings;
            topSettings.ColumnCount = 4;
            topSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            topSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            topSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            topSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            //tableSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            topToolStrip.AddItemAt(new ToolStripLabel("Delimiters:", null, true, (s, e) =>
            {
                FormMain.Config.Update(c => c.FileLog.PathParserOptions, ppo =>
                {
                    var globConf = new Configuration.FileLogConfig.ParserOptions()
                    {
                        FieldOrder = tbOrder.Text,
                        PrimaryDelimiter = Delimiter.StartString,
                        SecondaryDelimiter = Delimiter.EndString
                    };
                    var glob = GetGlobString(fileName);
                    if (ppo.ContainsKey(glob))
                    {
                        ppo[glob] = globConf;
                    }
                    else
                    {
                        ppo.Add(glob, globConf);
                    }
                });
            })
            {
                TextAlign = ContentAlignment.MiddleRight
            }, 0);
            topToolStrip.AddItemAt(cbDelims, 1);

            topToolStrip.AddItemAt(new ToolStripLabel("Field order:")
            {
                TextAlign = ContentAlignment.MiddleRight
            }, 2);
            topToolStrip.AddItemAt(tbOrder, 3);

            textBoxRawLine = new ToolStripTextBox();
            textBoxRawLine.Font = new Font("Consolas", 9);
            

            bottomToolStrip = new ToolStrip()
            {
                AutoSize = false,
                LayoutStyle = ToolStripLayoutStyle.Table,
                Stretch = true,
                Renderer = new LogViewerToolStripRenderer()
                {
                    NoBackground = false,
                    NoBorder = false,
                    TopBorder = true,
                    NoTextboxBackground = true,
                },
                MinimumSize = new Size(30, 30)
            };

            var bottomSettings = bottomToolStrip.LayoutSettings as TableLayoutSettings;
            bottomSettings.ColumnCount = 2;
            bottomSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            bottomSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            bottomToolStrip.AddItemAt(new ToolStripLabel("Raw line:")
            {
                TextAlign = ContentAlignment.MiddleRight
            }, 0);
            bottomToolStrip.AddItemAt(textBoxRawLine, 0);
        }
    }
}
