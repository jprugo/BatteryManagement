using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BatteryManagement
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            CreateHostBuilder(null).Build().Run();
            validateCheck(sender,"http://192.168.5.171/chargerOn",null);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            validateCheck(sender,"http://192.168.5.171/chargerOff", null);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            String postData = "seconds=" + Uri.EscapeDataString("3600");
            
            String data = Encoding.ASCII.GetBytes(postData).ToString();

            validateCheck(sender, "http://192.168.5.171/fanOn", data);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            validateCheck(sender,"http://192.168.5.171/fanOff", null);
        }

        private void validateCheck(object s, String url, String datos) {
            RadioButton rb = s as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                {
                    using (var wb = new WebClient())
                    {
                        connect(url, datos);
                    }
                }
            }
        }
        private void connect(String url, String postData) {

            if (postData != null)
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            else {
                var request = (HttpWebRequest)WebRequest.Create(url);

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddHostedService<Battery_Management>();
            });
    }

}
