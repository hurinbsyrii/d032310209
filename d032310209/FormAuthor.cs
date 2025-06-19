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

namespace d032310209
{
    public partial class FormAuthor : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\DITP2123\LabTest2\d032310209\d032310209\AdmiralBookstoreDatabase.mdf;Integrated Security=True;Connect Timeout=30";

        public FormAuthor()
        {
            InitializeComponent();
            LoadAuthorData(); // load data bila form dibuka
        }

        private void LoadAuthorData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Author";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt); // <-- sini error akan ditangkap
                    authorDataGridView.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void authorBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.authorBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.authorDataSet);

        }

        private void FormAuthor_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'authorDataSet.Author' table. You can move, or remove it, as needed.
            this.authorTableAdapter.Fill(this.authorDataSet.Author);

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int authorID = int.Parse(authorIDTextBox.Text);
                string name = nameTextBox.Text;
                int birthYear = int.Parse(birthYearTextBox.Text);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Author (AuthorID, Name, BirthYear) VALUES (@id, @name, @year)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", authorID);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@year", birthYear);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Author added successfully!");

                    LoadAuthorData(); // refresh DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormBook bookForm = new FormBook();
            bookForm.Show();
            this.Hide(); // kalau nak tutup form sekarang
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormStock stockForm = new FormStock();
            stockForm.Show();
            this.Hide();
        }
    }
}
