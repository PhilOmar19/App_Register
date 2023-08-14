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
using static App_Register.Program;

namespace App_Register
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            dataGridView1.DataSource = ShowAsignatura();
            dataGridView2.DataSource = ShowAula();
        }

        public DataTable ShowAsignatura()
        {
            using (SqlConnection connection = new DatabaseConnection("Server=DESKTOP-TFNBFUK;database=DCU;integrated security=true").GetConnection())
            {
                SqlCommand cmd = new SqlCommand("ShowAsignatura", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                // La conexión se cerrará automáticamente al salir del bloque using
                return dt;
            }
        }

        public DataTable ShowAula()
        {
            using (SqlConnection connection = new DatabaseConnection("Server=DESKTOP-TFNBFUK;database=DCU;integrated security=true").GetConnection())
            {
                SqlCommand cmd = new SqlCommand("ShowAula", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                // La conexión se cerrará automáticamente al salir del bloque using
                return dt;
            }
        }

        void InsertRegister()
        {
            string nombre = textBox1.Text;
            string apellido = textBox2.Text;
            string telefono = textBox3.Text;
            string asignatura = textBox4.Text;
            string aula = textBox5.Text;
            string correo = textBox6.Text;

            if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(apellido) && !string.IsNullOrWhiteSpace(telefono))
            {
                if (!string.IsNullOrWhiteSpace(asignatura) && !string.IsNullOrWhiteSpace(aula) &&
                int.TryParse(asignatura, out int parsedAsignatura) && int.TryParse(aula, out int parsedAula))
                {
                    Insert(nombre, apellido, telefono, asignatura, aula, correo);
                }
            }
        }

        void Insert(string nombre, string apellido, string telefono, string asignatura, string aula, string correo)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection("Server=DESKTOP-TFNBFUK;database=DCU;integrated security=true"))
                {
                    conexion.Open();

                    using (SqlCommand cmd = new SqlCommand("InsertRegister", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nombre", nombre);
                        cmd.Parameters.AddWithValue("@Apellido", apellido);
                        cmd.Parameters.AddWithValue("@Telefono", telefono);
                        cmd.Parameters.AddWithValue("@Asignatura", asignatura);
                        cmd.Parameters.AddWithValue("@Aula", aula);
                        cmd.Parameters.AddWithValue("@Correo", correo);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar el error de alguna manera, como mostrar un mensaje al usuario o registrar el error.
                MessageBox.Show("Ha ocurrido un error: " + ex.Message);
            }
        }
        

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close(); // Cierra el formulario actual (Form2)
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            InsertRegister();
            if (pictureBox1.Image != null)
            {
                InsertFoto(pictureBox1.Image);
            }
            MessageBox.Show("El registro se ha insertado exitosamente");
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            bool allFieldsFilled = new[] { textBox1, textBox2, textBox3, textBox4, textBox5 }
            .All(textBox => !string.IsNullOrWhiteSpace(textBox.Text));

            button2.Enabled = allFieldsFilled;
        }

        void InsertFoto(Image foto)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection("Server=DESKTOP-TFNBFUK;database=DCU;integrated security=true"))
                {
                    conexion.Open();

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Register (Foto) VALUES (@Foto)", conexion))
                    {
                        // Convierte la imagen en un array de bytes
                        using (MemoryStream ms = new MemoryStream())
                        {
                            foto.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); // Puedes ajustar el formato de la imagen según tus necesidades
                            byte[] imageData = ms.ToArray();

                            // Agrega el parámetro para la imagen
                            cmd.Parameters.AddWithValue("@Foto", imageData);

                            // Ejecuta la consulta
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar el error de alguna manera, como mostrar un mensaje al usuario o registrar el error.
                MessageBox.Show("Ha ocurrido un error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.png;*.bmp|Todos los archivos|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
            }
        }
    }
}
