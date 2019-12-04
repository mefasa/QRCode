using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Aztec;
using System.Data.OleDb;




namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        FilterInfoCollection webcam;
        VideoCaptureDevice cam;
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=bilgiler.mdb");

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        
        DataTable tablo = new DataTable();
        private void listele()
        {
            baglanti.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter("Select *From kayitbilgileri",baglanti);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach(FilterInfo dev in webcam)
            {
                comboBox1.Items.Add(dev.Name);
            }
            comboBox1.SelectedIndex = 0;
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("Delete *From kayitbilgileri", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            tablo.Clear();
            listele();
            textBox2.Text = ssc.ToString();
            BarcodeWriter writer = new BarcodeWriter() { Format = BarcodeFormat.CODE_128 };
            pic.Image = writer.Write(textBox2.Text);


        }
        private void cam_NewCam(object sender,NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = ((Bitmap)eventArgs.Frame.Clone());

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cam = new VideoCaptureDevice(webcam[comboBox1.SelectedIndex].MonikerString);
            cam.NewFrame += new NewFrameEventHandler(cam_NewCam);
            cam.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            BarcodeReader barkod = new BarcodeReader();
            if (pictureBox1.Image!=null)
            {
                Result res = barkod.Decode((Bitmap)pictureBox1.Image);
                try
                {
                    string dec = res.ToString().Trim();
                    if (dec != "")
                    {
                        timer1.Stop();
                        textBox1.Text = dec;

                    }
                }
                catch(Exception ex)
                {


                }
               

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 ekle = new Form2();
            ekle.Show();
            this.Hide();

        }
        int sayi =0;
        long ssc = 376123450000000000;
        
        private void button4_Click(object sender, EventArgs e)
        {
            
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("insert into kayitbilgileri(QRNo,SSCCNo) values('" + textBox1.Text + "','" + textBox2.Text + "')", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kayıt Eklendi", "Kayıt");
            tablo.Clear();
            listele();
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] is TextBox)
                {
                    Controls[i].Text = "";

                }

            }
            sayi++;
            textBox3.Text = sayi.ToString();
            
            if (sayi==5)
            {
                
                sayi = 0;
                ssc = ssc + 1;
               textBox2.Text = ssc.ToString();
                BarcodeWriter writer = new BarcodeWriter() { Format = BarcodeFormat.CODE_128 };
                pic.Image = writer.Write(textBox2.Text);


            }
            textBox2.Text = ssc.ToString();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("Delete *From kayitbilgileri", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kayıtlar Silindi", "Kayıt");
            tablo.Clear();
            listele();
            
            
        }
        

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
