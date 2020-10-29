using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;

namespace neko_view
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (WebClient webClient = new WebClient())
            {
                string imageLocation = pictureBox1.ImageLocation;
                webClient.DownloadFile(imageLocation, imageLocation.Substring(28, 12));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (WebClient client = new WebClient())
            {
                var neko = client.DownloadString("https://nekos.life/api/v2/img/neko");
                JObject json = JObject.Parse(neko);
                var nekourl = json["url"].ToString();
                pictureBox1.LoadAsync(nekourl);
                //pictureBox1.ImageLocation = nekourl;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (WebClient client = new WebClient())
            {
                var neko = client.DownloadString("https://nekos.life/api/v2/img/neko");
                JObject json = JObject.Parse(neko);
                var nekourl = json["url"].ToString();
                pictureBox1.LoadAsync(nekourl);
                //pictureBox1.ImageLocation = nekourl;
            }
        }
    }
}
