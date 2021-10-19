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
    public partial class Providers : Form
    {
        private SQLiteConnection conn = new SQLiteConnection("Data Source=dpbd.db;");
        public Providers(int id)
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            
        }

        private void Providers_Load(object sender, EventArgs e)
        {
            ProvidersLoad();
        }

        void ProvidersLoad()
        {
            try
            {
                DataTable dt = new DataTable();
                conn = new SQLiteConnection("Data Source=dpbd.db;");
                conn.Open();
                SQLiteCommand CMD = new SQLiteCommand("SELECT Providers.id AS '№', Providers.name AS 'Название', Providers.address AS 'Адрес', Providers.phone_number AS 'Телефон', Providers.email AS 'Email' FROM Providers", conn);
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
            private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddProvider addProvider = new AddProvider(0, 0);
            addProvider.ShowDialog();
            ProvidersLoad();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ProvidersLoad();
            textBox1.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите поставщика", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dataGridView1.SelectedRows[0].Cells[0].Value == null)
            {
                MessageBox.Show("Выберите поставщика", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DialogResult dialogResult = MessageBox.Show("Вы уверены что хотите удалить поставщика?", "Удалить поставщика", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return;
            }
            try
            {
                SQLiteCommand command = new SQLiteCommand("DELETE FROM `Providers` WHERE `Providers`.`id` = @id", conn);
                command.Parameters.Add("@id", DbType.Int32).Value = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                conn.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Вы успешно удалили поставщика", "Успех", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Ошибка");
                }
                conn.Close();
                ProvidersLoad();
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
                MessageBox.Show("Выберите поставщика", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dataGridView1.SelectedRows[0].Cells[0].Value == null)
            {
                MessageBox.Show("Выберите поставщика", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            AddProvider addProvider = new AddProvider(1, Convert.ToInt32(dataGridView1.SelectedCells[0].Value));
            addProvider.ShowDialog();
            ProvidersLoad();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("Поле поиска пустое", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try {
                string cm = "";
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        cm = "SELECT Providers.id AS '№', Providers.name AS 'Название', Providers.address AS 'Адрес', Providers.phone_number AS 'Телефон', Providers.email AS 'Email' FROM Providers WHERE Providers.name LIKE '%" + textBox1.Text + "%'";
                        break;
                    case 1:
                        cm = "SELECT Providers.id AS '№', Providers.name AS 'Название', Providers.address AS 'Адрес', Providers.phone_number AS 'Телефон', Providers.email AS 'Email' FROM Providers WHERE Providers.phone_number LIKE '%" + textBox1.Text + "%'";
                        break;
                    case 2:
                        cm = "SELECT Providers.id AS '№', Providers.name AS 'Название', Providers.address AS 'Адрес', Providers.phone_number AS 'Телефон', Providers.email AS 'Email' FROM Providers WHERE Providers.address LIKE '%" + textBox1.Text + "%'";
                        break;
                    case 3:
                        cm = "SELECT Providers.id AS '№', Providers.name AS 'Название', Providers.address AS 'Адрес', Providers.phone_number AS 'Телефон', Providers.email AS 'Email' FROM Providers WHERE Providers.email LIKE '%" + textBox1.Text + "%'";
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

        private void Providers_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form form = Application.OpenForms[1];
            form.Show();
        }
    }
}
