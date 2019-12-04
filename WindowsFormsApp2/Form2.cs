using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessagingToolkit.QRCode;
using MessagingToolkit.QRCode.Codec;
using ZXing;
using ZXing.Aztec;
using System.Data.OleDb;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=bilgiler.mdb");
        public Form2()
        {
            InitializeComponent();
        }

        DataTable tablo = new DataTable();
        private void listele()
        {
            baglanti.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter("Select *From kayitbilgileri", baglanti);
            adtr.Fill(tablo);
            data.DataSource = tablo;
            baglanti.Close();

        }
        BarcodeWriter writer = new BarcodeWriter() { Format = BarcodeFormat.CODE_128 };
        
        private void Form2_Load(object sender, EventArgs e)
        {
            listele();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            pictur.Image = writer.Write(arama.Text);
            baglanti.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter("Select *From kayitbilgileri where SSCCNo like '" + arama.Text + "'", baglanti);
            DataTable tablo2 = new DataTable();
            adtr.Fill(tablo2);
            data.DataSource = tablo2;
            baglanti.Close();
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 ekle = new Form1();
            ekle.Show();
            this.Hide();

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
            e.Graphics.DrawString(richTextBox1.Text, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(5, 5));
            e.Graphics.DrawString(richTextBox2.Text, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(5, 5));
            Image ss=pictur.Image;
            e.Graphics.DrawImage(ss,new PointF(20,20));
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
            
            

        }


        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("insert into barkod values('" + arama.Text + "','" + richTextBox1.Text + "','" + richTextBox2.Text + "')", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kayıt Eklendi", "Kayıt");
        }

        
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
