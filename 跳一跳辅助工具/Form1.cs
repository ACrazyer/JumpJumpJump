using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 跳一跳辅助工具
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int AdbExec(String cmd)
        {
            Process compiler = new Process();
            compiler.StartInfo.FileName = ".\\adb\\adb.exe";
            compiler.StartInfo.Arguments = cmd;
            compiler.StartInfo.UseShellExecute = false;
            compiler.StartInfo.CreateNoWindow = true;
            compiler.Start();
            compiler.WaitForExit();
            int ret = compiler.ExitCode;
            return ret;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(AdbExec("shell touch /sdcard/test_connect") != 0)
            {
                MessageBox.Show("Adb连接失败，请安装Android驱动");
                Application.Exit();
            }
            AdbExec("shell screencap -p /sdcard/screencap.png");
            AdbExec("pull /sdcard/screencap.png .\\config\\screencap.png");
            System.Drawing.Image img = Image.FromFile(".\\config\\screencap.png");
            System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
            img.Dispose();
            pictureBox1.Image = bmp;
        }

        static Boolean firstFlag = true;

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            int x = mouse.X;
            int y = mouse.Y;
            if (firstFlag)
            {
                firstFlag = !firstFlag;
                labelX1.Text = x.ToString();
                labelY1.Text = y.ToString();
            }else
            {
                firstFlag = !firstFlag;
                labelX2.Text = x.ToString();
                labelY2.Text = y.ToString();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(labelX1.Text == ""||labelX2.Text == "")
            {
                return;
            }
            int x1, y1, x2, y2;
            x1 = int.Parse(labelX1.Text);
            y1 = int.Parse(labelY1.Text);
            x2 = int.Parse(labelX2.Text);
            y2 = int.Parse(labelY2.Text);
            int width = x2 - x1;
            int height = y2 - y1;
            double distance = Math.Sqrt(width*width+height*height);
            double value = 8.65;
            int touchTime = (int)(distance * value);
            AdbExec("shell input swipe 250 250 251 251 "+touchTime.ToString());

            //MessageBox.Show("distance:"+distance.ToString()+"touchTime:"+touchTime.ToString());
            Thread.Sleep(1000);

            AdbExec("shell screencap -p /sdcard/screencap.png");
            AdbExec("pull /sdcard/screencap.png .\\config\\screencap.png");
            System.Drawing.Image img = System.Drawing.Image.FromFile(".\\config\\screencap.png");
            System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
            img.Dispose();
            pictureBox1.Image = bmp;
        }
    }
}
