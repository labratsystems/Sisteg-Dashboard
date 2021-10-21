using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class Form_splashScreen : Form
    {
        public Form_splashScreen()
        {
            InitializeComponent();
            this.timer_splashScreen.Start();
            string videoPath = Globals.path + "assets\\video\\splashScreen.mp4";
            axWindowsMediaPlayer.URL = videoPath;
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
                    System.Threading.Thread.Sleep(20);
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
