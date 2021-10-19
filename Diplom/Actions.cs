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
    public partial class Actions : Form
    {
        private SQLiteConnection conn = new SQLiteConnection("Data Source=dpbd.db;");
        private int IdUser;
        public Actions(int id)
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            IdUser = id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadActions(string cmd)
        {
            try
            {
                DataTable dt = new DataTable();
                conn.Open();
                SQLiteCommand CMD = new SQLiteCommand(cmd, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(CMD);
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                conn.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cmd = "";
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    cmd = "SELECT Operations.id AS '№', Items.name AS 'Товар', Users.surname || ' ' || Users.name AS 'Сотрудник', Operations.quantity AS 'Количество', Operations.sum AS 'Сумма', Operations.datetime AS 'Дата и время' FROM Operations JOIN Items ON Operations.id_item = Items.id JOIN Users ON Operations.id_user = Users.id WHERE Operations.type = 1";
                    break;
                case 1:
                    cmd = "SELECT Operations.id AS '№', Items.name AS 'Товар', Providers.name AS 'Поставщик', Users.surname || ' ' || Users.name AS 'Сотрудник', Operations.quantity AS 'Количество', Operations.sum AS 'Сумма', Operations.datetime AS 'Дата и время' FROM Operations JOIN Items ON Operations.id_item = Items.id JOIN Users ON Operations.id_user = Users.id JOIN Providers ON Operations.id_provider = Providers.id WHERE Operations.type = 2 ";
                    break;
                case 2:
                    cmd = "SELECT Operations.id AS '№', Items.name AS 'Товар', Users.surname || ' ' || Users.name AS 'Сотрудник', Operations.quantity AS 'Количество', Operations.sum AS 'Сумма', Operations.datetime AS 'Дата и время' FROM Operations JOIN Items ON Operations.id_item = Items.id JOIN Users ON Operations.id_user = Users.id WHERE Operations.type = 3";
                    break;

            }
            LoadActions(cmd);
        }

        private void Actions_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string cmd = "";
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    cmd = "SELECT Operations.id AS '№', Items.name AS 'Товар', Users.surname || ' ' || Users.name AS 'Сотрудник', Operations.quantity AS 'Количество', Operations.sum AS 'Сумма', Operations.datetime AS 'Дата и время' FROM Operations JOIN Items ON Operations.id_item = Items.id JOIN Users ON Operations.id_user = Users.id WHERE Operations.type = 1 AND Operations.datetime LIKE '" + dateTimePicker1.Value.ToString("d") + "%'";
                    break;
                case 1:
                    cmd = "SELECT Operations.id AS '№', Items.name AS 'Товар', Providers.name AS 'Поставщик', Users.surname || ' ' || Users.name AS 'Сотрудник', Operations.quantity AS 'Количество', Operations.sum AS 'Сумма', Operations.datetime AS 'Дата и время' FROM Operations JOIN Items ON Operations.id_item = Items.id JOIN Users ON Operations.id_user = Users.id JOIN Providers ON Operations.id_provider = Providers.id WHERE Operations.type = 2 AND Operations.datetime LIKE '" + dateTimePicker1.Value.ToString("d") + "%'";
                    break;
                case 2:
                    cmd = "SELECT Operations.id AS '№', Items.name AS 'Товар', Users.surname || ' ' || Users.name AS 'Сотрудник', Operations.quantity AS 'Количество', Operations.sum AS 'Сумма', Operations.datetime AS 'Дата и время' FROM Operations JOIN Items ON Operations.id_item = Items.id JOIN Users ON Operations.id_user = Users.id WHERE Operations.type = 3 AND Operations.datetime LIKE '" + dateTimePicker1.Value.ToString("d") + "%'";
                    break;

            }
            LoadActions(cmd);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            comboBox1_SelectedIndexChanged(null, null);
        }

        private void Actions_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form form = Application.OpenForms[1];
            form.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Doc doc = new Doc(IdUser);
            doc.ShowDialog();
        }
    }
}
