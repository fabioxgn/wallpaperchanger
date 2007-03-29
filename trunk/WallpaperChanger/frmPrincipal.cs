using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace WallpaperChanger
{
    public partial class frmPrincipal : Form
    {
        private List<string> wallpapers;
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                this.Hide();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
        }

        private void TrocarWallpaper()
        {
            Random r = new Random();            
            string wallpaper = wallpapers[r.Next(0, wallpapers.Count)];
            string bmpFilename = Directory.GetCurrentDirectory() + "\\wallpaper.bmp";
            Image.FromFile(wallpaper).Save(bmpFilename, System.Drawing.Imaging.ImageFormat.Bmp);
            WinAPI.SystemParametersInfo(WinAPI.SPI_SETDESKWALLPAPER, 0, bmpFilename, WinAPI.SPIF_UPDATEINIFILE | WinAPI.SPIF_SENDWININICHANGE);
        }
        
        private void timer_Tick(object sender, EventArgs e)
        {
            TrocarWallpaper();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(txtPasta.Text == string.Empty)
            {
                MessageBox.Show("Selecione uma pasta contendo imagens .jpg", "Pasta inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtIntervalo.Text == string.Empty)
            {
                MessageBox.Show("Digite um intervalo de tempo", "Intervalo inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            wallpapers = new List<string>();
            foreach (string s in Directory.GetFiles(txtPasta.Text))
            {
                if (s.ToUpper().EndsWith(".JPG"))
                {
                    wallpapers.Add(s);
                }
            }

            if (wallpapers.Count > 0)
            {
                TrocarWallpaper();
                timer.Interval = Convert.ToInt32(txtIntervalo.Text) * 60000;
                timer.Enabled = true;
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                MessageBox.Show("Selecione uma pasta contendo imagens .jpg", "Nenhuma imagem foi encontrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnPasta_Click(object sender, EventArgs e)
        {
            fdbPasta.ShowDialog();
            txtPasta.Text = fdbPasta.SelectedPath;
        }

    }
    public class WinAPI
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPIF_UPDATEINIFILE = 0x01;
        public const int SPIF_SENDWININICHANGE = 0x02;
    }  
}