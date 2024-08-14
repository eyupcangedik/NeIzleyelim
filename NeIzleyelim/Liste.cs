using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NeIzleyelim
{
    public partial class Liste : Form
    {
        string _filePath = "";
        string _type;
        public Liste()
        {
            InitializeComponent();
            radioButtonDizi.Checked = true;
        }

        private void buttonListele_Click(object sender, EventArgs e)
        {
            if(radioButtonDizi.Checked)
            {
                _type = "Dizi";
                _filePath = ConfigurationManager.AppSettings["DizilerJsonPath"];
                var jsonData = File.ReadAllText(_filePath);
                var json = JsonConvert.DeserializeObject<List<DiziData>>(jsonData);

                var sortedJson = json.OrderBy(x => x.Name).OrderBy(x => x.Status).ToList();
                dataGridView1.DataSource = sortedJson;

                dataGridView1.Columns[3].HeaderText = "Bölüm Sayısı";
                label1.Text = "Toplam Dizi Sayısı: " + dataGridView1.Rows.Count.ToString();
            }
            if(radioButtonFilm.Checked)
            {
                _type = "Film";
                _filePath = ConfigurationManager.AppSettings["FilmlerJsonPath"];
                var jsonData = File.ReadAllText(_filePath);
                var json = JsonConvert.DeserializeObject<List<FilmData>>(jsonData);

                var sortedJson = json.OrderBy(x => x.Name).OrderBy(x => x.Status).ToList();
                dataGridView1.DataSource = sortedJson;

                dataGridView1.Columns[3].HeaderText = "Süre(dk)";
                label1.Text = "Toplam Film Sayısı: " + dataGridView1.Rows.Count.ToString();
            }
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.RowPrePaint += dataGridView_Liste_RowPrePaint;
            dataGridView1.ClearSelection();
            dataGridView1.Columns[0].HeaderText = "İsim";
            dataGridView1.Columns[1].HeaderText = "Yerli/Yabancı";
            dataGridView1.Columns[2].HeaderText = "Yapım Yılı";
            dataGridView1.Columns[5].HeaderText = "İzlendi Mi";
            dataGridView1.Columns[6].HeaderText = "Açıklama";

            dataGridView1.AutoResizeColumns();
            this.Text = _type + " Listesi";
        }

        public void dataGridView_Liste_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex % 2 != 0)
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            }
            else
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Silver;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string type = "";
            if (radioButtonDizi.Checked)
            {
                type = "Dizi";
                _filePath = ConfigurationManager.AppSettings["DizilerJsonPath"];
            }

            if (radioButtonFilm.Checked)
            {
                type = "Film";
                _filePath = ConfigurationManager.AppSettings["FilmlerJsonPath"];
            }

            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                string name = selectedRow.Cells["Name"].Value.ToString();

                ListeDetay listeDetay = new ListeDetay(name, type, _filePath);
                listeDetay.Show();
            }
            if (checkBox1.Checked)
            {
                buttonListele.PerformClick();
            }       
        }

        private void radioButtonDizi_Click(object sender, EventArgs e)
        {
            buttonListele.PerformClick();
        }

        private void radioButtonFilm_Click(object sender, EventArgs e)
        {
            buttonListele.PerformClick();
        }

        private void Liste_Load(object sender, EventArgs e)
        {
            buttonListele.PerformClick();
        }
    }
}
