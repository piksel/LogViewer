using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Piksel.LogViewer.Controls
{
    [DesignerCategory("code")]
    [ToolboxItemFilter("System.Windows.Forms")]
    [ProvideProperty("HideCaret", typeof(TextBoxBase))]
    public class TextBoxExtender: Component, IExtenderProvider
    {
        private HashSet<Control> boundControls = new HashSet<Control>();
        private Hashtable hideCaret = new Hashtable();

        public bool CanExtend(object extendee)
            => extendee is TextBoxBase;

        private void UpdateEventBinding(TextBoxBase textBox)
        {
            var hide = GetHideCaret(textBox);
            if (hide && !boundControls.Contains(textBox))
            {
                textBox.MouseDown += HandleMouseEvent;
                textBox.MouseUp += HandleMouseEvent;
                textBox.GotFocus += HandleFocusEvent;
                textBox.ReadOnly = true;
                boundControls.Add(textBox);
            }
            else if (!hide&& boundControls.Contains(textBox))
            {
                textBox.MouseDown -= HandleMouseEvent;
                textBox.MouseUp -= HandleMouseEvent;
                textBox.GotFocus -= HandleFocusEvent;
                boundControls.Remove(textBox);
            }
            else
            {
                // No change is needed
                return;
            }

            textBox.TabStop = !hide;
        }

        #region Event methods

        private void HandleFocusEvent(object sender, EventArgs e)
            => HideCaret(sender as Control);

        private void HandleMouseEvent(object sender, MouseEventArgs e)
            => HideCaret(sender as Control);

        #endregion

        #region Provided Properties

        public virtual bool GetHideCaret(Control ctl)
            => (Boolean)(hideCaret[ctl] ?? false);

        public virtual void SetHideCaret(Control ctl, bool value)
        {
            hideCaret[ctl] = value;
            UpdateEventBinding(ctl as TextBoxBase);
        }

        internal virtual bool ShouldSerializeHideCaret(Control ctl)
            => hideCaret.ContainsKey(ctl);

        public virtual void ResetHideCaret(Control ctl)
            => hideCaret.Remove(ctl);

        #endregion

        [DllImport("user32.dll")]
        private static extern int HideCaret(IntPtr hwnd);

        public bool HideCaret(Control control)
            => control != null &&  !DesignMode&&  HideCaret(control.Handle) != 0;

        internal static TextBoxExtender staticInstance;
        internal static TextBoxExtender StaticInstance
            => staticInstance ?? (staticInstance = new TextBoxExtender());

    }

    public static class TextBoxExtenderExtensions
    {
        public static void HideCaret(this TextBoxBase textBox, bool hide = true)
            => TextBoxExtender.StaticInstance.SetHideCaret(textBox, hide);
    }

}
