/*
 * This code is provided under the Code Project Open Licence (CPOL)
 * See http://www.codeproject.com/info/cpol10.aspx for details
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace Piksel.LogViewer.Controls
{
    [DesignerCategory("Code")]
    [ToolboxBitmap(typeof(TabControl))]
    [ProvideProperty("Persistant", typeof(TabPage))]
    public partial class CustomTabControl : TabControl, IExtenderProvider
    {

        #region	Construction

        public CustomTabControl()
        {

            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw, true);

            _BackBuffer = new Bitmap(Width, Height);
            _BackBufferGraphics = Graphics.FromImage(_BackBuffer);
            _TabBuffer = new Bitmap(Width, Height);
            _TabBufferGraphics = Graphics.FromImage(_TabBuffer);

            DisplayStyle = TabStyle.Default;

        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            OnFontChanged(EventArgs.Empty);
        }


        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                CreateParams cp = base.CreateParams;
                if (RightToLeftLayout)
                    cp.ExStyle = cp.ExStyle | NativeMethods.WS_EX_LAYOUTRTL | NativeMethods.WS_EX_NOINHERITLAYOUT;
                return cp;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_BackImage != null)
                {
                    _BackImage.Dispose();
                }
                if (_BackBufferGraphics != null)
                {
                    _BackBufferGraphics.Dispose();
                }
                if (_BackBuffer != null)
                {
                    _BackBuffer.Dispose();
                }
                if (_TabBufferGraphics != null)
                {
                    _TabBufferGraphics.Dispose();
                }
                if (_TabBuffer != null)
                {
                    _TabBuffer.Dispose();
                }

                if (_StyleProvider != null)
                {
                    _StyleProvider.Dispose();
                }
            }
        }

        #endregion

        #region Private variables

        private Bitmap _BackImage;
        private Bitmap _BackBuffer;
        private Graphics _BackBufferGraphics;
        private Bitmap _TabBuffer;
        private Graphics _TabBufferGraphics;

        private int _oldValue;
        private Point _dragStartPosition = Point.Empty;

        private TabStyle _Style;
        private TabStyleProvider _StyleProvider;

        private List<TabPage> _TabPages;

        #endregion

        #region Public properties

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TabStyleProvider DisplayStyleProvider
        {
            get
            {
                if (_StyleProvider == null)
                {
                    DisplayStyle = TabStyle.Default;
                }

                return _StyleProvider;
            }
            set
            {
                _StyleProvider = value;
            }
        }

        [Category("Appearance"), DefaultValue(typeof(TabStyle), "Default"), RefreshProperties(RefreshProperties.All)]
        public TabStyle DisplayStyle
        {
            get { return _Style; }
            set
            {
                if (_Style != value)
                {
                    _Style = value;
                    _StyleProvider = TabStyleProvider.CreateProvider(this);
                    Invalidate();
                }
            }
        }

        [Category("Appearance"), RefreshProperties(RefreshProperties.All)]
        public new bool Multiline
        {
            get
            {
                return base.Multiline;
            }
            set
            {
                base.Multiline = value;
                Invalidate();
            }
        }


        //	Hide the Padding attribute so it can not be changed
        //	We are handling this on the Style Provider
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Point Padding
        {
            get { return DisplayStyleProvider.Padding; }
            set
            {
                DisplayStyleProvider.Padding = value;
            }
        }

        public override bool RightToLeftLayout
        {
            get { return base.RightToLeftLayout; }
            set
            {
                base.RightToLeftLayout = value;
                UpdateStyles();
            }
        }


        //	Hide the HotTrack attribute so it can not be changed
        //	We are handling this on the Style Provider
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool HotTrack
        {
            get { return DisplayStyleProvider.HotTrack; }
            set
            {
                DisplayStyleProvider.HotTrack = value;
            }
        }

        [Category("Appearance")]
        public new TabAlignment Alignment
        {
            get { return base.Alignment; }
            set
            {
                base.Alignment = value;
                switch (value)
                {
                    case TabAlignment.Top:
                    case TabAlignment.Bottom:
                        Multiline = false;
                        break;
                    case TabAlignment.Left:
                    case TabAlignment.Right:
                        Multiline = true;
                        break;
                }

            }
        }

        //	Hide the Appearance attribute so it can not be changed
        //	We don't want it as we are doing all the painting.
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        public new TabAppearance Appearance
        {
            get
            {
                return base.Appearance;
            }
            set
            {
                //	Don't permit setting to other appearances as we are doing all the painting
                base.Appearance = TabAppearance.Normal;
            }
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                //	Special processing to hide tabs
                if (_Style == TabStyle.None)
                {
                    return new Rectangle(0, 0, Width, Height);
                }
                else
                {
                    int tabStripHeight = 0;
                    int itemHeight = 0;

                    if (Alignment <= TabAlignment.Bottom)
                    {
                        itemHeight = ItemSize.Height;
                    }
                    else
                    {
                        itemHeight = ItemSize.Width;
                    }

                    tabStripHeight = 5 + (itemHeight * RowCount);

                    Rectangle rect = new Rectangle(4, tabStripHeight, Width - 8, Height - tabStripHeight - 4);
                    switch (Alignment)
                    {
                        case TabAlignment.Top:
                            rect = new Rectangle(4, tabStripHeight, Width - 8, Height - tabStripHeight - 4);
                            break;
                        case TabAlignment.Bottom:
                            rect = new Rectangle(4, 4, Width - 8, Height - tabStripHeight - 4);
                            break;
                        case TabAlignment.Left:
                            rect = new Rectangle(tabStripHeight, 4, Width - tabStripHeight - 4, Height - 8);
                            break;
                        case TabAlignment.Right:
                            rect = new Rectangle(4, 4, Width - tabStripHeight - 4, Height - 8);
                            break;
                    }
                    return rect;
                }
            }
        }

        [Browsable(false)]
        public int ActiveIndex
        {
            get
            {
                NativeMethods.TCHITTESTINFO hitTestInfo = new NativeMethods.TCHITTESTINFO(PointToClient(Control.MousePosition));
                int index = NativeMethods.SendMessage(Handle, NativeMethods.TCM_HITTEST, IntPtr.Zero, NativeMethods.ToIntPtr(hitTestInfo)).ToInt32();
                if (index == -1)
                {
                    return -1;
                }
                else
                {
                    if (TabPages[index].Enabled)
                    {
                        return index;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }

        [Browsable(false)]
        public TabPage ActiveTab
        {
            get
            {
                int activeIndex = ActiveIndex;
                if (activeIndex > -1)
                {
                    return TabPages[activeIndex];
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region	Extension methods

        public void HideTab(TabPage page)
        {
            if (page != null && TabPages.Contains(page))
            {
                BackupTabPages();
                TabPages.Remove(page);
            }
        }

        public void HideTab(int index)
        {
            if (IsValidTabIndex(index))
            {
                HideTab(_TabPages[index]);
            }
        }

        public void HideTab(string key)
        {
            if (TabPages.ContainsKey(key))
            {
                HideTab(TabPages[key]);
            }
        }

        public void ShowTab(TabPage page)
        {
            if (page != null)
            {
                if (_TabPages != null)
                {
                    if (!TabPages.Contains(page)
                        && _TabPages.Contains(page))
                    {

                        //	Get insert point from backup of pages
                        int pageIndex = _TabPages.IndexOf(page);
                        if (pageIndex > 0)
                        {
                            int start = pageIndex - 1;

                            //	Check for presence of earlier pages in the visible tabs
                            for (int index = start; index >= 0; index--)
                            {
                                if (TabPages.Contains(_TabPages[index]))
                                {

                                    //	Set insert point to the right of the last present tab
                                    pageIndex = TabPages.IndexOf(_TabPages[index]) + 1;
                                    break;
                                }
                            }
                        }

                        //	Insert the page, or add to the end
                        if ((pageIndex >= 0) && (pageIndex < TabPages.Count))
                        {
                            TabPages.Insert(pageIndex, page);
                        }
                        else
                        {
                            TabPages.Add(page);
                        }
                    }
                }
                else
                {

                    //	If the page is not found at all then just add it
                    if (!TabPages.Contains(page))
                    {
                        TabPages.Add(page);
                    }
                }
            }
        }

        public void ShowTab(int index)
        {
            if (IsValidTabIndex(index))
            {
                ShowTab(_TabPages[index]);
            }
        }

        public void ShowTab(string key)
        {
            if (_TabPages != null)
            {
                TabPage tab = _TabPages.Find(delegate (TabPage page) { return page.Name.Equals(key, StringComparison.OrdinalIgnoreCase); });
                ShowTab(tab);
            }
        }

        private bool IsValidTabIndex(int index)
        {
            BackupTabPages();
            return ((index >= 0) && (index < _TabPages.Count));
        }

        private void BackupTabPages()
        {
            if (_TabPages == null)
            {
                _TabPages = new List<TabPage>();
                foreach (TabPage page in TabPages)
                {
                    _TabPages.Add(page);
                }
            }
        }

        #endregion

        #region Drag 'n' Drop

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (AllowDrop)
            {
                _dragStartPosition = new Point(e.X, e.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (AllowDrop)
            {
                _dragStartPosition = Point.Empty;
            }
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            base.OnDragOver(drgevent);

            if (drgevent.Data.GetDataPresent(typeof(TabPage)))
            {
                drgevent.Effect = DragDropEffects.Move;
            }
            else
            {
                drgevent.Effect = DragDropEffects.None;
            }
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            if (drgevent.Data.GetDataPresent(typeof(TabPage)))
            {
                drgevent.Effect = DragDropEffects.Move;

                TabPage dragTab = (TabPage)drgevent.Data.GetData(typeof(TabPage));

                if (ActiveTab == dragTab)
                {
                    return;
                }

                //	Capture insert point and adjust for removal of tab
                //	We cannot assess this after removal as differeing tab sizes will cause
                //	inaccuracies in the activeTab at insert point.
                int insertPoint = ActiveIndex;
                if (dragTab.Parent.Equals(this) && TabPages.IndexOf(dragTab) < insertPoint)
                {
                    insertPoint--;
                }
                if (insertPoint < 0)
                {
                    insertPoint = 0;
                }

              //	Remove from current position (could be another tabcontrol)
              ((TabControl)dragTab.Parent).TabPages.Remove(dragTab);

                //	Add to current position
                TabPages.Insert(insertPoint, dragTab);
                SelectedTab = dragTab;

                //	deal with hidden tab handling?
            }
        }

        private void StartDragDrop()
        {
            if (!_dragStartPosition.IsEmpty)
            {
                TabPage dragTab = SelectedTab;
                if (dragTab != null)
                {
                    //	Test for movement greater than the drag activation trigger area
                    Rectangle dragTestRect = new Rectangle(_dragStartPosition, Size.Empty);
                    dragTestRect.Inflate(SystemInformation.DragSize);
                    Point pt = PointToClient(Control.MousePosition);
                    if (!dragTestRect.Contains(pt))
                    {
                        DoDragDrop(dragTab, DragDropEffects.All);
                        _dragStartPosition = Point.Empty;
                    }
                }
            }
        }

        #endregion

        #region Events

        [Category("Action")] public event ScrollEventHandler HScroll;
        [Category("Action")] public event EventHandler<TabControlEventArgs> TabImageClick;
        [Category("Action")] public event EventHandler<TabControlCancelEventArgs> TabClosing;

        #endregion

        #region	Base class event processing

        protected override void OnFontChanged(EventArgs e)
        {
            IntPtr hFont = Font.ToHfont();
            NativeMethods.SendMessage(Handle, NativeMethods.WM_SETFONT, hFont, (IntPtr)(-1));
            NativeMethods.SendMessage(Handle, NativeMethods.WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero);
            UpdateStyles();
            if (Visible)
            {
                Invalidate();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            //	Recreate the buffer for manual double buffering
            if (Width > 0 && Height > 0)
            {
                if (_BackImage != null)
                {
                    _BackImage.Dispose();
                    _BackImage = null;
                }
                if (_BackBufferGraphics != null)
                {
                    _BackBufferGraphics.Dispose();
                }
                if (_BackBuffer != null)
                {
                    _BackBuffer.Dispose();
                }

                _BackBuffer = new Bitmap(Width, Height);
                _BackBufferGraphics = Graphics.FromImage(_BackBuffer);

                if (_TabBufferGraphics != null)
                {
                    _TabBufferGraphics.Dispose();
                }
                if (_TabBuffer != null)
                {
                    _TabBuffer.Dispose();
                }

                _TabBuffer = new Bitmap(Width, Height);
                _TabBufferGraphics = Graphics.FromImage(_TabBuffer);

                if (_BackImage != null)
                {
                    _BackImage.Dispose();
                    _BackImage = null;
                }

            }
            base.OnResize(e);
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            if (_BackImage != null)
            {
                _BackImage.Dispose();
                _BackImage = null;
            }
            base.OnParentBackColorChanged(e);
        }

        protected override void OnParentBackgroundImageChanged(EventArgs e)
        {
            if (_BackImage != null)
            {
                _BackImage.Dispose();
                _BackImage = null;
            }
            base.OnParentBackgroundImageChanged(e);
        }

        private void OnParentResize(object sender, EventArgs e)
        {
            if (Visible)
            {
                Invalidate();
            }
        }


        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
            {
                Parent.Resize += OnParentResize;
            }
        }

        protected override void OnSelecting(TabControlCancelEventArgs e)
        {
            base.OnSelecting(e);

            //	Do not allow selecting of disabled tabs
            if (e.Action == TabControlAction.Selecting && e.TabPage != null && !e.TabPage.Enabled)
            {
                e.Cancel = true;
            }
        }

        protected override void OnMove(EventArgs e)
        {
            if (Width > 0 && Height > 0)
            {
                if (_BackImage != null)
                {
                    _BackImage.Dispose();
                    _BackImage = null;
                }
            }
            base.OnMove(e);
            Invalidate();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (Visible)
            {
                Invalidate();
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (Visible)
            {
                Invalidate();
            }
        }


        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessMnemonic(char charCode)
        {
            foreach (TabPage page in TabPages)
            {
                if (IsMnemonic(charCode, page.Text))
                {
                    SelectedTab = page;
                    return true;
                }
            }
            return base.ProcessMnemonic(charCode);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        [System.Diagnostics.DebuggerStepThrough()]
        protected override void WndProc(ref Message m)
        {

            switch (m.Msg)
            {
                case NativeMethods.WM_HSCROLL:

                    //	Raise the scroll event when the scroller is scrolled
                    base.WndProc(ref m);
                    OnHScroll(new ScrollEventArgs(((ScrollEventType)NativeMethods.LoWord(m.WParam)), _oldValue, NativeMethods.HiWord(m.WParam), ScrollOrientation.HorizontalScroll));
                    break;
                //				case NativeMethods.WM_PAINT:
                //					
                //					//	Handle painting ourselves rather than call the base OnPaint.
                //					CustomPaint(ref m);
                //					break;

                default:
                    base.WndProc(ref m);
                    break;

            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            int index = ActiveIndex;

            //	If we are clicking on an image then raise the ImageClicked event before raising the standard mouse click event
            //	if there if a handler.
            if (index > -1 && TabImageClick != null
                && (TabPages[index].ImageIndex > -1 || !string.IsNullOrEmpty(TabPages[index].ImageKey))
                && GetTabImageRect(index).Contains(MousePosition))
            {
                OnTabImageClick(new TabControlEventArgs(TabPages[index], index, TabControlAction.Selected));

                //	Fire the base event
                base.OnMouseClick(e);

            }
            else if (!DesignMode && index > -1 && _StyleProvider.ShowTabCloser && GetTabCloserRect(index).Contains(MousePosition))
            {
                //	If we are clicking on a closer then remove the tab instead of raising the standard mouse click event
                //	But raise the tab closing event first
                TabPage tab = ActiveTab;
                TabControlCancelEventArgs args = new TabControlCancelEventArgs(tab, index, false, TabControlAction.Deselecting);
                OnTabClosing(args);

                if (!args.Cancel)
                {
                    TabPages.Remove(tab);
                    tab.Dispose();
                }
            }
            else
            {
                //	Fire the base event
                base.OnMouseClick(e);
            }
        }

        protected virtual void OnTabImageClick(TabControlEventArgs e) => TabImageClick?.Invoke(this, e);

        protected virtual void OnTabClosing(TabControlCancelEventArgs e) => TabClosing?.Invoke(this, e);

        protected virtual void OnHScroll(ScrollEventArgs e)
        {
            //	repaint the moved tabs
            Invalidate();

            //	Raise the event
            HScroll?.Invoke(this, e);

            if (e.Type == ScrollEventType.EndScroll)
            {
                _oldValue = e.NewValue;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_StyleProvider.ShowTabCloser)
            {
                Rectangle tabRect = _StyleProvider.GetTabRect(ActiveIndex);
                if (tabRect.Contains(MousePosition))
                {
                    Invalidate();
                }
            }

            //	Initialise Drag Drop
            if (AllowDrop && e.Button == MouseButtons.Left)
            {
                StartDragDrop();
            }
        }

        #endregion

        #region String formatting

        private StringFormat GetStringFormat()
        {
            StringFormat format = null;

            //	Rotate Text by 90 degrees for left and right tabs
            switch (Alignment)
            {
                case TabAlignment.Top:
                case TabAlignment.Bottom:
                    format = new StringFormat();
                    break;
                case TabAlignment.Left:
                case TabAlignment.Right:
                    format = new StringFormat(StringFormatFlags.DirectionVertical);
                    break;
            }
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            if (FindForm() != null && FindForm().KeyPreview)
            {
                format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
            }
            else
            {
                format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;
            }
            if (RightToLeft == RightToLeft.Yes)
            {
                format.FormatFlags = format.FormatFlags | StringFormatFlags.DirectionRightToLeft;
            }
            return format;
        }
        #endregion

        #region Extender properties

        // Backing fields

        HashSet<TabPage> persistantTabs = new HashSet<TabPage>();

        // IExtenderProvider implementation

        public bool CanExtend(object extendee)
            => extendee is TabPage;

        // Persistant Property

        [DefaultValue(false)]
        [Description("Prevents closing the tab page and hides the closer on its tab")]
        public virtual bool GetPersistant(Control ctl)
            => ctl is TabPage tab && persistantTabs.Contains(tab);

        public virtual bool GetPersistant(int tabIndex)
            => GetPersistant(TabPages[tabIndex]);

        public virtual bool SetPersistant(TabPage tab, bool value)
            => (value ? persistantTabs.Add(tab) : persistantTabs.Remove(tab)) && InvalidateTab(tab);

        public virtual void SetPersistant(Control ctl, bool value)
            => SetPersistant(ctl as TabPage, value);

        internal virtual bool ShouldSerializePersistant(Control ctl)
            => GetPersistant(ctl);

        public virtual void ResetPersistant(Control ctl)
            => SetPersistant(ctl, false);

        #endregion

    }

    public static class TabPageExtensions
    {
        public static bool SetPersistant(this TabPage tab, bool value = true)
            => tab.Parent is CustomTabControl ctc && ctc.SetPersistant(tab, value);

    }
}
