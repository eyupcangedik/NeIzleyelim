using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace NeIzleyelim
{
    public partial class YerimeSec : Form
    {
        string _filePath;
        string _type;
        public YerimeSec()
        {
            InitializeComponent();
            radioButtonDizi.Checked = true;
            pictureBox1.Visible = false;
            label1.Visible = false;
            label2.Visible = false;    
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            richTextBox1.Visible = false;
            button1.Visible = false;
            button2.Visible = false;

        }

        private void buttonListele_Click(object sender, EventArgs e)
        {
            if(radioButtonFilm.Checked)
            {
                _filePath = ConfigurationManager.AppSettings["FilmlerJsonPath"];
                _type = "Film";             
            }
            if(radioButtonDizi.Checked)
            {
                _filePath = ConfigurationManager.AppSettings["DizilerJsonPath"];
                _type = "Dizi";
            }

            Dizi_Film_Sec();

            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            richTextBox1.Visible= true;
            pictureBox1.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
        }
        private void Dizi_Film_Sec()
        {
            // JSON dosyasını oku
            string jsonContent = File.ReadAllText(_filePath);
            Random random = new Random();
            int randomIndex;

            // Type = Film ise 
            if (_type == "Film")
            {
                List<FilmData> items = JsonConvert.DeserializeObject<List<FilmData>>(jsonContent);
                randomIndex = random.Next(items.Count);

                // Rastgele elemanı seç
                FilmData selectedItem = items[randomIndex];

                string categories = "";
                foreach (var item in items[randomIndex].Category)
                {
                    categories += item.ToString() + ", ";
                }

                categories = categories.Substring(0, categories.Length - 2);
                LoadImageFromUrlAsync(selectedItem.ImageURL);
                label1.Text = selectedItem.Name;
                label2.Text = "Kategori: " + categories;
                label3.Text = "Süre(dk): " + selectedItem.Duration;

                if (selectedItem.Status != "0")
                {
                    label4.Text = "Durum: İzlendi";
                }
                else
                {
                    label4.Text = "Durum: İzlenmedi";
                }
                label5.Text = "Yapım Yılı: " + selectedItem.Year;
                richTextBox1.Text = selectedItem.Explanation;
                
            }

            // Type = Dizi ise 
            if (_type == "Dizi")
            {
                List<DiziData> items = JsonConvert.DeserializeObject<List<DiziData>>(jsonContent);
                randomIndex = random.Next(items.Count);

                // Rastgele elemanı seç
                DiziData selectedItem = items[randomIndex];

                string categories = "";
                foreach (var item in items[randomIndex].Category)
                {
                    categories += item.ToString() + ", ";
                }

                categories = categories.Substring(0, categories.Length - 2);
                LoadImageFromUrlAsync(selectedItem.ImageURL);
                label1.Text = selectedItem.Name;
                label2.Text = "Kategori: " + categories;
                label3.Text = "Bölüm Sayısı: " + selectedItem.Episode;

                if (selectedItem.Status != "0")
                {
                    label4.Text = "Durum: İzlendi";
                }
                else
                {
                    label4.Text = "Durum: İzlenmedi";
                }
                label5.Text = "Yapım Yılı: " + selectedItem.Year;
                richTextBox1.Text = selectedItem.Explanation;
            }
        }
        private async void LoadImageFromUrlAsync(string url)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    // WebClient kullanarak resmi asenkron olarak indir
                    byte[] imageBytes = await webClient.DownloadDataTaskAsync(new Uri(url));
                    using (var ms = new System.IO.MemoryStream(imageBytes))
                    {
                        // MemoryStream'den resmi yükle ve PictureBox'a ata
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Guncelle("1");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Guncelle("0");
        }
        private void Guncelle(string durum)
        {
            string filePath = "";
            if (radioButtonFilm.Checked)
            {
                filePath = ConfigurationManager.AppSettings["FilmlerJsonPath"];
            }
            if (radioButtonDizi.Checked)
            {
                filePath = ConfigurationManager.AppSettings["DizilerJsonPath"];
            }
            string jsonContent = File.ReadAllText(filePath);

            // JSON verisini JArray olarak parse et
            JArray jsonArray = JArray.Parse(jsonContent);

            // Her bir elemanı kontrol et
            foreach (JObject item in jsonArray)
            {
                if ((string)item["Name"] == label1.Text)
                {
                    // Status alanını güncelle
                    item["Status"] = durum;
                    if(durum == "1")
                    {
                        label4.Text = "Durum: İzlendi";
                    }
                    if(durum == "0")
                    {
                        label4.Text = "Durum: İzlenmedi";
                    }
                }
            }
            // Güncellenmiş JSON verisini string olarak al
            string updatedJson = JsonConvert.SerializeObject(jsonArray, Formatting.Indented);

            // Güncellenmiş JSON verisini bir dosyaya kaydet
            File.WriteAllText(filePath, updatedJson);
        }
        private void radioButtonFilm_Click(object sender, EventArgs e)
        {
            this.Text = "Rastgele Film Seç";
        }

        private void radioButtonDizi_Click(object sender, EventArgs e)
        {
            this.Text = "Rastgele Dizi Seç";
        }
    }
}
