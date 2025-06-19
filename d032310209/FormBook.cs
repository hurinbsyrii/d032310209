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
using d032310209;

namespace d032310209
{
    public partial class FormBook : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\DITP2123\LabTest2\d032310209\d032310209\AdmiralBookstoreDatabase.mdf;Integrated Security=True;Connect Timeout=30";

        public FormBook()
        {
            InitializeComponent();
            LoadBookData();
        }

        private void bookBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bookBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.bookDataSet);

        }

        private void FormBook_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'bookDataSet.Book' table. You can move, or remove it, as needed.
            this.bookTableAdapter.Fill(this.bookDataSet.Book);

        }

        private void LoadBookData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Book";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                bookDataGridView.DataSource = dt;

                bookDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void bookDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = bookDataGridView.Rows[e.RowIndex];
                iSBN_13TextBox.Text = row.Cells["ISBN_13"].Value.ToString();
                titleTextBox.Text = row.Cells["Title"].Value.ToString();
                publisherTextBox.Text = row.Cells["Publisher"].Value.ToString();
                publishDateDateTimePicker.Text = row.Cells["PublishDate"].Value.ToString();
            }

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            string isbn = iSBN_13TextBox.Text;
            string title = titleTextBox.Text;
            string publisher = publisherTextBox.Text;
            DateTime publishDate = publishDateDateTimePicker.Value;


            // Check if date is valid
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Book SET Title = @title, Publisher = @publisher, PublishDate = @pdate WHERE [ISBN_13] = @isbn";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@isbn", isbn);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@publisher", publisher);
                cmd.Parameters.AddWithValue("@pdate", publishDate);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Book updated successfully!");
                    LoadBookData();
                }
                else
                {
                    MessageBox.Show("No record updated.");
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAuthor formAuthor = new FormAuthor();
            formAuthor.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormStock formStock = new FormStock();
            formStock.Show();
            this.Hide();
        }
    }
}

