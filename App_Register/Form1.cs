using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static App_Register.Program;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace App_Register
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.DataSource = ShowRegister();
            dataGridView1.DefaultCellStyle.BackColor = Color.White;  // Fondo blanco
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;  // Texto negro
        }
        public DataTable ShowRegister()
        {
            using (SqlConnection connection = new DatabaseConnection("Server=DESKTOP-TFNBFUK;database=DCU;integrated security=true").GetConnection())
            {
                SqlCommand cmd = new SqlCommand("ShowRegister", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                // La conexión se cerrará automáticamente al salir del bloque using
                return dt;
            }
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(); // Crear instancia del Form2
            form2.Show(); // Mostrar el Form2
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close(); // Cierra el formulario actual (Form1)
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && int.TryParse(textBox1.Text, out int result))
            {
                button3.Enabled = true;
                button4.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
                button4.Enabled = false;
            }
            if (textBox1.Text == "") 
            {
                dataGridView1.DataSource = ShowRegister();
            }
        }
        public DataTable FindRegister(string id)
        {
            using (SqlConnection connection = new DatabaseConnection("Server=DESKTOP-TFNBFUK;database=DCU;integrated security=true").GetConnection())
            {
                SqlCommand cmd = new SqlCommand("FindRegister", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_register", id);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                // La conexión se cerrará automáticamente al salir del bloque using
                return dt;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string idToSearch = textBox1.Text;
            dataGridView1.DataSource = FindRegister(idToSearch);

        }
        public DataTable DropRegister(string id)
        {
            using (SqlConnection connection = new DatabaseConnection("Server=DESKTOP-TFNBFUK;database=DCU;integrated security=true").GetConnection())
            {
                SqlCommand cmd = new SqlCommand("DropRegister", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_register", id);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                // La conexión se cerrará automáticamente al salir del bloque using
                return dt;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string idToSearch = textBox1.Text;
            dataGridView1.DataSource = DropRegister(idToSearch);
            MessageBox.Show("El registro ha sido eliminado");
            dataGridView1.DataSource = ShowRegister();
        }
    }
}
