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
    public partial class FormStock : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\DITP2123\LabTest2\d032310209\d032310209\AdmiralBookstoreDatabase.mdf;Integrated Security=True;Connect Timeout=30";

        public FormStock()
        {
            InitializeComponent();
            LoadStockData();
        }

        private void stockBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.stockBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.stockDataSet);

        }

        private void FormStock_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'stockDataSet.Stock' table. You can move, or remove it, as needed.
            this.stockTableAdapter.Fill(this.stockDataSet.Stock);

        }

        private void LoadStockData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Stock";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                stockDataGridView.DataSource = dt;
                stockDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void stockDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = stockDataGridView.Rows[e.RowIndex];
                stockIDTextBox.Text = row.Cells[0].Value.ToString();
                authorIDTextBox.Text = row.Cells[1].Value.ToString();
                iSBN_13TextBox.Text = row.Cells[2].Value.ToString();
                quantityInStockTextBox.Text = row.Cells[3].Value.ToString();
                dateRecordedDateTimePicker.Text = row.Cells[4].Value.ToString();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(stockIDTextBox.Text.Trim(), out int stockID))
            {
                MessageBox.Show("Please enter a valid Stock ID.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Stock WHERE StockID = @stockID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@stockID", stockID);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Stock record deleted.");
                    LoadStockData();
                }
                else
                {
                    MessageBox.Show("No record deleted.");
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAuthor f = new FormAuthor();
            f.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormBook f = new FormBook();
            f.Show();
            this.Hide();
        }
    }
}
