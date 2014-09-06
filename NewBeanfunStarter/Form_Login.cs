using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace NewBeanfunStarter
{
    public partial class Form_Login : Form
    {
        ImageMarquee im;
        Timer tmrMarquee = new Timer() { Interval = 100 };
        BackgroundWorker bgwkr = new BackgroundWorker();
        public Form_Login()
        {
            InitializeComponent();
            im = new ImageMarquee(Properties.Resources.gama0,
                                  Properties.Resources.gama1,
                                  pbCaptcha.BackColor,
                                  pbCaptcha.Width, pbCaptcha.Height, -3);
            tmrMarquee.Tick += tmrMarquee_Tick;
            tmrMarquee.Start();
        }

        private void Form_Login_Load(object sender, EventArgs e)
        {
        }

        void tmrMarquee_Tick(object sender, EventArgs e)
        {
            pbCaptcha.BackgroundImage = im.Next();
        }

        private void Form_Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                btnLogin_Click(sender, e);
            }
            else if (e.KeyCode == Keys.R && e.Control)
            {
                btnRefreshCaptcha_Click(sender, e);
            }
        }

        private void btnRefreshCaptcha_Click(object sender, EventArgs e)
        {
            if (!btnRefreshCaptcha.Enabled) return;
            btnRefreshCaptcha.Enabled = false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!btnLogin.Enabled) return;
            btnLogin.Enabled = false;
        }

        private void Status(string text, Color cl)
        {
            this.lblStatus.Text = text;
            this.lblStatus.ForeColor = cl;
        }

        private void Status(string text)
        {
            this.Status(text, Color.Black);
        }

        private void Error(string text)
        {
            this.Status(text, Color.Red);
        }
    }
}
