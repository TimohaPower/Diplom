using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Globalization;

namespace Diplom
{
    public partial class Cart : Form
    {
        private SQLiteConnection conn = new SQLiteConnection("Data Source=dpbd.db;");
        DataTable DTcart = new DataTable();
        private double cost = 0;
        private double sum = 0;
        private double change = 0;
        private int IdUser;
        public Cart(int id)
        {
            InitializeComponent();
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            IdUser = id;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void AddItem(string barcord, int n)
        {
            try
            {

                if (dataGridView1.Rows.Count == 0)
                {
                    conn.Open();
                    SQLiteCommand CMD = new SQLiteCommand("SELECT Items.id AS '№', Items.name AS 'Название', Items.cost AS 'Цена', @qual AS 'Количество' FROM Items WHERE Items.barcode = @barcode", conn);
                    CMD.Parameters.Add("@barcode", System.Data.DbType.String).Value = barcord;
                    CMD.Parameters.Add("@qual", System.Data.DbType.Int32).Value = n;
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(CMD);
                    adapter.Fill(DTcart);
                    conn.Close();
                    if (DTcart.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = DTcart;
                    }
                    else
                    {
                        MessageBox.Show("Такого товара не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox1.Clear();
                        return;
                    }
                }
                else
                {
                    DataTable dt = new DataTable();
                    conn.Open();
                    SQLiteCommand CMD = new SQLiteCommand("SELECT Items.id AS '№', Items.name AS 'Название', Items.cost AS 'Цена', @qual AS 'Количество' FROM Items WHERE Items.barcode = @barcode", conn);
                    CMD.Parameters.Add("@barcode", System.Data.DbType.String).Value = barcord;
                    CMD.Parameters.Add("@qual", System.Data.DbType.Int32).Value = n;

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(CMD);
                    adapter.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                        {

                            if (dataGridView1.Rows[i].Cells[0].Value.ToString() == dt.Rows[0][0].ToString())
                            {
                                dataGridView1.Rows[i].Cells[3].Value = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value) + n;
                                textBox1.Clear();
                                SumItems();
                                return;
                            }
                        }
                        DTcart.Merge(dt);
                    }
                    else
                    {
                        MessageBox.Show("Такого товара не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox1.Clear();
                        return;
                    }
                }
                SumItems();
                textBox1.Clear();
            }
            catch
            {
                textBox1.Clear();
            }

        }
        void SumItems()
        {
            double sum1 = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                sum1 += Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value) * Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);
            }
            cost = sum1;
            labelCost.Text = String.Format("{0:f2}", cost);
            if (sum != 0)
            {
                labelSum.Text = String.Format("{0:f2}", sum);
                change = sum - cost;
                labelChange.Text = String.Format("{0:f2}", change);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddItem(textBox1.Text, int.Parse(numericUpDown1.Value.ToString()));
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dataGridView1.SelectedRows[0].Cells[0].Value == null)
            {
                MessageBox.Show("Выберите товар", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(Convert.ToInt32(dataGridView1.SelectedCells[3].Value) > 0)
            {
                dataGridView1.SelectedCells[3].Value = Convert.ToInt32(dataGridView1.SelectedCells[3].Value) - 1;
                SumItems();
            }

        }

        private void buttonEnd_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("Отсутствуют товары", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    SQLiteCommand command = new SQLiteCommand("UPDATE `Items` SET quantity = quantity - 1 WHERE id = @id", conn);
                    command.Parameters.Add("@id", DbType.Int32).Value = dataGridView1.Rows[i].Cells[0].Value;

                    conn.Open();

                    if (command.ExecuteNonQuery() == 1)
                    {
                        command = new SQLiteCommand("INSERT INTO `Operations` (id_item, id_user, quantity, sum, type, datetime) VALUES (@idi, @idu, @quan, @sum, 1, @date)", conn);

                        command.Parameters.Add("@idi", DbType.Int32).Value = dataGridView1.Rows[i].Cells[0].Value;
                        command.Parameters.Add("@idu", DbType.Int32).Value = IdUser;
                        command.Parameters.Add("@quan", DbType.Int32).Value = dataGridView1.Rows[i].Cells[3].Value;
                        command.Parameters.Add("@sum", DbType.Double).Value = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value) * Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);
                        command.Parameters.Add("@date", DbType.String).Value = DateTime.Now;
                        dataGridView1.Rows.RemoveAt(i);
                        if (command.ExecuteNonQuery() == 1)
                        {

                        }
                        else
                        {
                            MessageBox.Show("Ошибка продажи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ошибка продажи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    conn.Close();
                }
                labelCost.Text = "000,00";
                labelChange.Text = "000,00";
                labelSum.Text = "000,00";
                cost = 0;
                change = 0;
                sum = 0;
                textBox1.Select();
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sum = Convert.ToDouble(numericUpDown2.Value);
            numericUpDown2.Value = 1;
            textBox1.Select();
            SumItems();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dataGridView1.SelectedRows[0].Cells[0].Value == null)
            {
                MessageBox.Show("Выберите товар", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
            SumItems();

        }

        private void Cart_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form form = Application.OpenForms[1];
            form.Show();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                buttonAdd.PerformClick();
            }
        }

        private void numericUpDown2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                button2.PerformClick();            
            }
        }
    }
}
