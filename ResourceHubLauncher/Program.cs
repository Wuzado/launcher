﻿using MetroFramework;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace ResourceHubLauncher {
    static class Program {
        [STAThread]
        static void Main() {

            WebRequest request = WebRequest.Create("https://raw.githubusercontent.com/DesktopGooseUnofficial/launcher-backend/master/data.json");
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            string html = String.Empty;
            using (StreamReader sr = new StreamReader(data))
            {
                html = sr.ReadToEnd();
            }

            dynamic array = JsonConvert.DeserializeObject(html);
            loadedData = array;
            
            if (!Directory.Exists("Disabled Mods"))
                Directory.CreateDirectory("Disabled Mods");

            if (!Directory.Exists("Mods"))
                Directory.CreateDirectory("Mods");

            string latest = data["app"]["md5"].ToString();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm form = new MainForm();
            form.results = data["mods"].Children().ToList();

            byte[] hash = MD5.Create().ComputeHash(File.ReadAllBytes(Application.ExecutablePath));

            StringBuilder md5 = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) {
                md5.Append(hash[i].ToString("X2"));
            }

            if(latest != md5.ToString()) {
                Form screen = new Form();
                screen.Size = Screen.PrimaryScreen.Bounds.Size;
                bool update = MetroMessageBox.Show(screen, $"App out of date.\nWould you like to update now?\n({md5} != {latest})", "Auto-Update", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes;
                if(update) {
                    return;
                }
            }

            Application.Run(form);
        }
    }
}
