using neko_view;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace neko_view_settings
{
    public partial class Settings : Form
    {
        private SQLiteConnection conn = null;
        public Settings()
        {
            InitializeComponent();
        }

        private Point mousePoint;
        private int nsfw;
        public static string select_path;
        private string tags;

        private void form_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void form_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }
        // path 더 보기 쉅게 만들기
        private void Settings_Load(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.AutoScroll = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn = new SQLiteConnection("Data Source=" + Application.StartupPath + "/user.db;Version=3;");
            if (!checkBox1.Checked) { nsfw = 0; } else { nsfw = 1; }
            if (comboBox1.SelectedItem == null) { tags = "neko"; } else { tags = comboBox1.SelectedItem.ToString(); } 
            if (select_path == null) { select_path = Application.StartupPath.ToString(); } else { select_path = select_path.ToString(); }
            string sql = "UPDATE user SET path = '" + select_path.ToString() + "', tags = '" + tags + "', nsfw = '" + nsfw + "';";
            conn.Open();
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
            }
            catch (SQLiteException sqle)
            {
                MessageBox.Show(sqle.ToString());
            }
            conn.Close();
            this.Visible = false;
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                String[] nsfw = { "hass", "hmidriff", "pgif", "4k", "hentai", "hneko", "hkitsune", "anal", "hanal", "gonewild", "ass", "pussy", "thigh", "hthigh"  };
                comboBox1.Items.AddRange(nsfw);
            }
            else
            {
                comboBox1.Items.Clear();
                String[] notnsfw = { "holo", "neko", "kemonomimi", "kanna", "gah", "coffee", "food" };
                comboBox1.Items.AddRange(notnsfw);
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            select_path = dialog.SelectedPath;
            textBox1.Text = select_path;
        }
    }
}
