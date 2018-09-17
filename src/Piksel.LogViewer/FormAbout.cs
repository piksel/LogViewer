using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Piksel.LogViewer
{
    public partial class FormAbout : Form
    {
        private string sourceUrl;

        public FormAbout(Assembly versionAssembly) :
            this(versionAssembly.GetName().Version, 
                versionAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description) { }

        public FormAbout(Version version, string productName)
        {
            var v = version;
            InitializeComponent();

            label1.Text = $"Version {v.Major}.{v.Minor}.{v.Revision}  build {v.Build}";
            label2.Text = productName;
            Text = "About " + productName;

            linkLabel1.Visible = false;
            linkLabel1.LinkClicked += (s, e) => Process.Start(sourceUrl);
        }

        public FormAbout UsingSourceUrl(string githubUser, string githubRepo)
            => UsingSourceUrl(new Uri($"https://github.com/{githubUser}/{githubRepo}"));

        public FormAbout UsingSourceUrl(Uri url)
        {
            sourceUrl = url.ToString();
            linkLabel1.Text = "Source is available at " + sourceUrl;
            linkLabel1.LinkArea = new LinkArea(linkLabel1.Text.Length - sourceUrl.Length, sourceUrl.Length);
            linkLabel1.Visible = true;
            return this;
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {

        }

        internal static FormAbout WithVersionFrom<TVersionSource>(TVersionSource source)
            => new FormAbout(typeof(TVersionSource).Assembly);
    }

}
