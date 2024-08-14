using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace NeIzleyelim
{
    public partial class ListeDetay : Form
    {
        string _name;
        string _type;
        string _filePath;

        public ListeDetay(string name, string type, string filePath)
        {
            InitializeComponent();
            _name = name;
            _type = type;
            _filePath = filePath;

            this.Text = name + " Detay";
            DetayGoster();
        }

        public void DetayGoster()
        {
            string jsonContent = File.ReadAllText(_filePath);

            // JSON verisini JArray olarak parse et
            JArray jsonArray = JArray.Parse(jsonContent);

            // Her bir elemanı kontrol et
            foreach (JObject item in jsonArray)
            {
                if ((string)item["Name"].ToString().ToLower() == _name.ToLower())
                {
                    label1.Text = item["Name"].ToString();

                    string categories = "";
                    foreach (var data in item["Category"])
                    {
                        categories += data.ToString() + ", ";
                    }
                    categories = categories.Substring(0, categories.Length - 2);

                    label2.Text = "Kategori: " + categories;

                    if(_type == "Dizi")
                    {
                        label3.Text = "Bölüm Sayısı: " + item["Episode"].ToString();
                    }
                    if(_type == "Film")
                    {
                        label3.Text = "Süre(dk): " + item["Duration"].ToString();
                    }
                    

                    if (item["Status"].ToString() == "0")
                    {
                        label4.Text = "Durum: İzlenmedi";
                    }
                    else
                    {
                        label4.Text = "Durum: İzlendi";
                    }

                    label5.Text = "Yapım Yılı: " + item["Year"].ToString();

                    richTextBox1.Text = item["Explanation"].ToString();
                    LoadImageFromUrlAsync(item["ImageURL"].ToString());
                }
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
        private void Guncelle(string durum)
        {
            string jsonContent = File.ReadAllText(_filePath);

            // JSON verisini JArray olarak parse et
            JArray jsonArray = JArray.Parse(jsonContent);

            // Her bir elemanı kontrol et
            foreach (JObject item in jsonArray)
            {
                if ((string)item["Name"] == label1.Text)
                {
                    // Status alanını güncelle
                    item["Status"] = durum;
                    if (durum == "1")
                    {
                        label4.Text = "Durum: İzlendi";
                    }
                    if (durum == "0")
                    {
                        label4.Text = "Durum: İzlenmedi";
                    }
                }
            }
            // Güncellenmiş JSON verisini string olarak al
            string updatedJson = JsonConvert.SerializeObject(jsonArray, Formatting.Indented);

            // Güncellenmiş JSON verisini bir dosyaya kaydet
            File.WriteAllText(_filePath, updatedJson);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Guncelle("1");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Guncelle("0");
        }
    }
}
