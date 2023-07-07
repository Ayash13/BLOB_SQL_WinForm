using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace coba_blob
{
    public partial class DataMahasiswa : Form
    {
        private string imagePath;
        public DataMahasiswa()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.gif) | *.jpg; *.jpeg; *.png; *.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imagePath = openFileDialog.FileName;
                textBox1.Text = imagePath;
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                MessageBox.Show("Please select an image first.");
                return;
            }

            byte[] imageData;
            using (MemoryStream ms = new MemoryStream())
            {
                using (Image image = Image.FromFile(imagePath))
                {
                    image.Save(ms, image.RawFormat);
                    imageData = ms.ToArray();
                }
            }

            string connectionString = "Data Source=DESKTOP-AJFQKR8\\AYASH;Initial Catalog=FotoMahasiswa;Persist Security Info=True;User ID=sa;Password=Rahasiatau123";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Images (NIM, Nama, Foto) VALUES (@NIM, @Nama, @Foto)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NIM", textBox3.Text);
                command.Parameters.AddWithValue("@Nama", textBox2.Text);
                command.Parameters.AddWithValue("@Foto", imageData);
                command.ExecuteNonQuery();
            }

            MessageBox.Show("Image uploaded successfully.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-AJFQKR8\\AYASH;Initial Catalog=FotoMahasiswa;Persist Security Info=True;User ID=sa;Password=Rahasiatau123";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Foto FROM Images WHERE NIM = @NIM";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NIM", textBox3.Text);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        byte[] imageData = (byte[])reader["Foto"];
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            pictureBox1.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Image not found for the specified NIM.");
                    }
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
