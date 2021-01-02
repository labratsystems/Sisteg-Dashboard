using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class Form_splashScreen : Form
    {
        public Form_splashScreen()
        {
            InitializeComponent();
            this.timer_splashScreen.Start();
            string caminhoVideo = Path.Combine(Application.StartupPath, "splashScreen.mp4");
            File.WriteAllBytes(caminhoVideo, Properties.Resources.splashScreen);
            axWindowsMediaPlayer.URL = caminhoVideo;
        }

        private void timer_splashScreen_Tick(object sender, EventArgs e)
        {
            this.timer_splashScreen.Stop();
            if (Application.OpenForms.OfType<Main>().Count() == 0)
            {
                Main main = new Main();
                main.Opacity = 0;
                main.Show();
                do
                {
                    System.Threading.Thread.Sleep(25);
                    this.Opacity -= 0.025;
                    main.Opacity += 0.050;
                } while ((this.Opacity > 0) && (main.Opacity < 1));
                this.Hide();
                typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, main, new object[] { true });
            }
        }
    }
}
