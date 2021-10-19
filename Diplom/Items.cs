using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diplom
{
    public partial class Items : Form
    {
        private SQLiteConnection conn = new SQLiteConnection("Data Source=dpbd.db;");
        private int Id;
        public Items(int id)
        {
            InitializeComponent();
            Id = id;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Items_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form form = Application.OpenForms[1];
            form.Show();
        }

        void LoadItems()
        {
            try
            {
                DataTable dt = new DataTable();
                conn.Open();
                SQLiteCommand CMD = new SQLiteCommand("SELECT Items.id AS 'Номер', Items.name AS 'Наименование', Items.barcode AS 'Штрих-код', Items.cost AS 'Цена', Items.quantity AS 'Количество' FROM Items ", conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(CMD);
                da.Fill(dt); ;

                dataGridView1.DataSource = dt;

                conn.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Items_Load(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            AddItem addItem = new AddItem(0,0);
            addItem.ShowDialog();
            LoadItems();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadItems();
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Поле поиска пустое", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string cm = "";
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        cm = "SELECT Items.id AS 'Номер', Items.name AS 'Наименование', Items.barcode AS 'Штрих-код', Items.cost AS 'Цена', Items.quantity AS 'Количество' FROM Items WHERE Items.name LIKE '%" + textBox1.Text + "%'";
                        break;
                    case 1:
                        cm = "SELECT Items.id AS 'Номер', Items.name AS 'Наименование', Items.barcode AS 'Штрих-код', Items.cost AS 'Цена', Items.quantity AS 'Количество' FROM Items WHERE Items.barcode LIKE '%" + textBox1.Text + "%'";
                        break;
                }
                DataTable dt = new DataTable();
                conn.Open();
                SQLiteCommand CMD = new SQLiteCommand(cm, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(CMD);
                da.Fill(dt); ;

                dataGridView1.DataSource = dt;

                conn.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e)
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
            DialogResult dialogResult = MessageBox.Show("Вы уверены что хотите удалить товар?", "Удалить товар", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return;
            }
            try
            {
                SQLiteCommand command = new SQLiteCommand("DELETE FROM `Items` WHERE `Items`.`id` = @id", conn);
                command.Parameters.Add("@id", DbType.Int32).Value = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                conn.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Вы успешно удалили товар", "Успех", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Ошибка");
                }
                conn.Close();
                LoadItems();
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button8_Click(object sender, EventArgs e)
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
            AddItem addItem = new AddItem(1, int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()));
            addItem.ShowDialog();
            LoadItems();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddEntrance addEntrance = new AddEntrance(int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()), Id);
            addEntrance.ShowDialog();
            LoadItems();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddWriteoff addWriteoff = new AddWriteoff(int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()), Id);
            addWriteoff.ShowDialog();
        }
    }
}
