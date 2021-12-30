using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Data.SqlClient;
using xClasses;
using System.Management;
using System.Diagnostics;
using PopUpNotification.Properties;

namespace PopUpNotification
{
    public partial class Form1 : Form
    {
        String connetionString = @"Data Source=192.168.2.9\SQLEXPRESS;Initial Catalog=hr_bak;User ID=sa;Password=Nescafe3in1;MultipleActiveResultSets=true";
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;

        etc123 qwe = new etc123();
        private string ip = "";
        private int timerTick = 30;
        DateTime addDT = DateTime.Now.AddMinutes(30);
        private string systemName = "";
        private string folderName = "";
        public Form1()
        {
            InitializeComponent(); 
            this.StartPosition = FormStartPosition.Manual;
            foreach (var scrn in Screen.AllScreens)
            {
                if (scrn.Bounds.Contains(this.Location))
                {
                    this.Location = new Point(scrn.Bounds.Right - this.Width, scrn.Bounds.Top);
                    return;
                }
            }
        }
        void ActivitiesLogs()
        {
            string logs = "Notification Module Popped Up " + systemName;

            try
            {
                const string location = @"ponLogs";

                if (!File.Exists(location))
                {
                    var createText = "New Activities Logs" + Environment.NewLine;
                    File.WriteAllText(location, createText);
                }
                var appendLogs = "Activities Logs: " + logs + " " + DateTime.Now + Environment.NewLine;
                File.AppendAllText(location, appendLogs);
            }
            catch (Exception ex)
            {
                const string location = @"ponLogs";
                if (!File.Exists(location))
                {
                    TextWriter file = File.CreateText(@"C:\ponLogs");
                    var createText = "New Activities Logs" + Environment.NewLine;

                    File.WriteAllText(location, createText);

                }
                var appendLogs = ex.Message + logs + DateTime.Now + Environment.NewLine;
                File.AppendAllText(location, appendLogs);

            }

        }

        private void con_on()
        {
            con = new SqlConnection();
            con.ConnectionString = connetionString;
            con.Open();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            ip = qwe.GetLocalIPv4();
            string uix = File.ReadLines("C:\\type.txt").First();
            
            if (uix.Trim() == "hr")
            {
                systemName = "HR_APPLICATION";
                folderName = "HR-System";
                pictureBox1.Image = Properties.Resources.HR_POP;
                label2.BackColor = Color.FromArgb(2, 119, 189);
                button1.BackColor = Color.FromArgb(2, 119, 189);
                this.Icon = Icon.FromHandle(((Bitmap)imageList1.Images[0]).GetHicon());
            }
            else if (uix.Trim() == "mftg")
            {
                systemName = "Mftg_System";
                folderName = "MFTG-System";
                pictureBox1.Image = imageList1.Images[1];
                pictureBox1.Image = Properties.Resources.MFTG_POP;
                label2.BackColor = Color.Maroon;
                button1.BackColor = Color.Maroon;
                this.Icon = Icon.FromHandle(((Bitmap)imageList1.Images[1]).GetHicon());
            }


            label1.Text = addDT.ToString("hh:mm:ss tt"); // MMMM/dd/yyyy hh:mm:ss tt
            label5.Text = folderName + " will automatically closed in";
            timer1.Start();


            ActivitiesLogs();


            ///
            ///
            var fileStream = new FileStream("C:\\"+ folderName +"\\Utilities\\PON\\UpdateConcern.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    // process the line
                    if (string.IsNullOrEmpty(textBox1.Text))
                    {
                        textBox1.Text = line;
                    }
                    else
                    {
                        textBox1.Text = textBox1.Text + Environment.NewLine + line;
                    }
                }
            }



            timer2.Start();
        }

        public async Task<string> timer()
        {
                await Task.Delay(1000);
            return "";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan span = addDT - DateTime.Now;
            //int totalMinutes = Convert.ToInt32(span.TotalMinutes);
            int totalSecs = Convert.ToInt32(span.TotalSeconds);
            label3.Text = "Seconds Left : " + totalSecs.ToString();

            if (DateTime.Now.ToString("hh:mm:ss") == addDT.ToString("hh:mm:ss"))
            {
                Process[] processes = Process.GetProcessesByName(systemName);
                foreach (var process in processes)
                {
                    process.Kill();
                }

                exit_log("Force Exit");

                Thread.Sleep(2000);
                Application.Exit();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        void exit_log(string action)
        {
            using (SqlConnection con = new SqlConnection(connetionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("sp_philip", con);
                cmd.Parameters.AddWithValue("@mode", "insert_hru");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ipAdd", ip);
                cmd.Parameters.AddWithValue("@action", action);

                cmd.ExecuteNonQuery();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Exit now?", "O&G | HR SYSTEM", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.Yes)
            {
                Process[] processes = Process.GetProcessesByName(systemName);
                foreach (var process in processes)
                {
                    process.Kill();
                }

                exit_log("Exited Gracefully");
                Thread.Sleep(2000);
                Application.Exit();

            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (pictureBox1.Visible)
                pictureBox1.Visible = false;
            else
                pictureBox1.Visible = true;
        }
    }
}
