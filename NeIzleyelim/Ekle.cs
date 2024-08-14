using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace NeIzleyelim
{
    public partial class Ekle : Form
    {
        string _filePath;
        public Ekle()
        {
            InitializeComponent();
            label6.Visible = false;
            textBoxBolumSayisi.Visible = false;
            label4.Visible = false;
            textBoxSure.Visible = false;
        }
        private void buttonKaydet_Click(object sender, EventArgs e)
        {
            if(radioButtonFilm.Checked)
            {
                _filePath = ConfigurationManager.AppSettings["FilmlerJsonPath"];
                if (Kontrol())
                {
                    Dizi_Film_Ekle("Film");
                }
                
            }
            if(radioButtonDizi.Checked)
            {
                _filePath = ConfigurationManager.AppSettings["DizilerJsonPath"];
                if (Kontrol())
                {
                    Dizi_Film_Ekle("Dizi");
                }
            }
            Temizle();
        }

        private void Dizi_Film_Ekle(string type)
        {
            try
            {
                string name = textBoxAd.Text;
                List<string> categories = new List<string>();

                foreach (Control control in this.Controls)
                {
                    if (control is CheckBox checkBox && checkBox.Checked)
                    {
                        categories.Add(checkBox.Text);
                    }
                }
                string country = "";
                try
                {
                    country = comboBoxDil.SelectedItem.ToString();
                }
                catch (Exception)
                {
                }

                string year = "";
                try
                {
                    year = textBoxYil.Text.Trim();
                }
                catch (Exception)
                {
                }

                string imageURL = "";
                try
                {
                    imageURL = textBoxURL.Text.Trim();
                }
                catch (Exception)
                {
                }


                string explanation = "";
                try
                {
                    explanation = richTextBoxAciklama.Text.Trim();
                }
                catch (Exception)
                {
                }


                string jsonData = File.ReadAllText(_filePath);
                if(type == "Dizi")
                {
                    string episode = "";
                    try
                    {
                        episode = textBoxBolumSayisi.Text.Trim();
                    }
                    catch (Exception)
                    {
                    }

                    List<DiziData> dataList = JsonConvert.DeserializeObject<List<DiziData>>(jsonData);
                    DiziData newData = new DiziData
                    {
                        Name = textBoxAd.Text,
                        Category = categories,
                        Country = country,
                        Year = year,
                        Episode = episode,
                        ImageURL = imageURL,
                        Explanation = richTextBoxAciklama.Text.Trim(),
                        Status = "0"
                    };

                    dataList.Add(newData);
                    string updatedJsonData = JsonConvert.SerializeObject(dataList, Formatting.Indented);
                    File.WriteAllText(_filePath, updatedJsonData);
                    MessageBox.Show("Dizi Ekleme Başarılı", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    string duration = "";
                    try
                    {
                        duration = textBoxSure.Text;
                    }
                    catch (Exception)
                    {
                    }

                    List<FilmData> dataList = JsonConvert.DeserializeObject<List<FilmData>>(jsonData);
                    FilmData newData = new FilmData
                    {
                        Name = textBoxAd.Text,
                        Category = categories,
                        Country = country,
                        Year = year,
                        Duration = duration,
                        ImageURL = imageURL,
                        Explanation = richTextBoxAciklama.Text.Trim(),
                        Status = "0"
                    };

                    dataList.Add(newData);
                    string updatedJsonData = JsonConvert.SerializeObject(dataList, Formatting.Indented);
                    File.WriteAllText(_filePath, updatedJsonData);
                    MessageBox.Show("Film Ekleme Başarılı", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Dizi Ekleme Başarısız Oldu", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxSure_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxBolumSayisi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void radioButtonDizi_Click(object sender, EventArgs e)
        {
            label6.Visible = true;
            textBoxBolumSayisi.Visible = true;
            label4.Visible = false;
            textBoxSure.Visible = false;
            this.Text = "Dizi Ekle";
        }

        private void radioButtonFilm_Click(object sender, EventArgs e)
        {
            label6.Visible = false;
            textBoxBolumSayisi.Visible = false;
            label4.Visible = true;
            textBoxSure.Visible = true;
            this.Text = "Film Ekle";
        }

        private void Temizle()
        {
            textBoxBolumSayisi.Text = "";
            textBoxURL.Text = "";
            richTextBoxAciklama.Text = "";
            textBoxAd.Text = "";
            textBoxYil.Text = "";
            textBoxSure.Text = "";
        }

        private bool Kontrol()
        {
            string jsonContent = File.ReadAllText(_filePath);

            // JSON verisini JArray olarak parse et
            JArray jsonArray = JArray.Parse(jsonContent);

            // Her bir elemanı kontrol et
            foreach (JObject item in jsonArray)
            {
                if ((string)item["Name"].ToString().ToLower() == textBoxAd.Text.ToLower())
                {
                    MessageBox.Show(textBoxAd.Text + " Zaten Mevcut!", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private void richTextBoxAciklama_MouseEnter(object sender, EventArgs e)
        {
            if (richTextBoxAciklama.Text == "Açıklama")
            {
                richTextBoxAciklama.Text = "";
            }
        }

        private void richTextBoxAciklama_MouseLeave(object sender, EventArgs e)
        {
            if (richTextBoxAciklama.Text.Trim() == "")
            {
                richTextBoxAciklama.Text = "Açıklama";
            }
        }
    }
}
