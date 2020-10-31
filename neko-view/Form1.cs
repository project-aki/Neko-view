using System;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Drawing;
using System.Drawing.Imaging;
using neko_view_settings;
using System.Diagnostics;
using System.Data;
using System.Data.SQLite;

namespace neko_view
{
    public partial class Form1 : Form
    {
        private SQLiteConnection conn = null;
        public Form1()
        {
            InitializeComponent();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (WebClient webClient = new WebClient())
            {
                string imageLocation = pictureBox1.ImageLocation; //imageLocation.Substring(28, 11)
                if (Settings.select_path == null) { return; }
                else
                {
                    try
                    {
                        string connStr = @"Data Source=" + Application.StartupPath + "/user.db;Version=3;";
                        using (var conn = new SQLiteConnection(connStr))
                        {
                            conn.Open();
                            string sql = "SELECT * FROM user";
                            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                            SQLiteDataReader rdr = cmd.ExecuteReader();
                            while (rdr.Read())
                            {
                                pictureBox1.Image.Save(rdr["path"].ToString() + "\\" + Path.GetFileName(imageLocation));
                            }
                            rdr.Close();
                            conn.Close();
                        }
                    } catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string databaseFileName = "./user.db";
            if (!File.Exists(databaseFileName))
            {
                SQLiteConnection.CreateFile(databaseFileName);
                conn = new SQLiteConnection("Data Source=" + Application.StartupPath + "/user.db;Version=3;");
                string strsql = "create table user (path text, tags text, nsfw integer);";
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(strsql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                conn = new SQLiteConnection("Data Source=./user.db;Version=3;");
                string sql = "insert into user values ('NULL', 'neko', 'NULL');";
                conn.Open();
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
            }
            string connStr = @"Data Source=" + Application.StartupPath + "/user.db;Version=3;";

            using (var conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT * FROM user";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    using (WebClient client = new WebClient())
                    {
                        // 다른 url 지원
                       var neko = client.DownloadString("https://nekos.life/api/v2/img/" + rdr["tags"].ToString());
                        JObject json = JObject.Parse(neko);
                        var nekourl = json["url"].ToString();
                        pictureBox1.LoadAsync(nekourl);
                    }
                }
                rdr.Close();
                conn.Close();
            }
        }
        private void form_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
        private void button1_Click(object sender, EventArgs e) 
            {
            string connStr = @"Data Source=" + Application.StartupPath + "/user.db;Version=3;";

            using (var conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT * FROM user";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    using (WebClient client = new WebClient())
                    {
                        var neko = client.DownloadString("https://nekos.life/api/v2/img/" + rdr["tags"].ToString());
                        JObject json = JObject.Parse(neko);
                        var nekourl = json["url"].ToString();
                        pictureBox1.LoadAsync(nekourl);
                    }
                }
                rdr.Close();
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Settings settings = new Settings();
            settings.ShowDialog();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            Application.Exit();
        }
    }
}
