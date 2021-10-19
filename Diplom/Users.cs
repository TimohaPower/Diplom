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
    public partial class Users : Form
    {
        private SQLiteConnection conn = new SQLiteConnection("Data Source=dpbd.db;");

        public Users(int id)
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
        }

        private void LoadUsers()
        {
            try
            {
                DataTable dt = new DataTable();
                conn.Open();
                SQLiteCommand CMD = new SQLiteCommand("SELECT Users.id AS '№', Users.surname || ' ' || Users.name || ' ' || Users.patronymic AS 'ФИО', Genders.name AS 'Пол' FROM Users JOIN Genders ON Users.gender = Genders.id", conn);
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

        private void Users_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void Users_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form form = Application.OpenForms[1];
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                conn.Open();
                SQLiteCommand CMD = new SQLiteCommand("SELECT Users.id AS '№', Users.surname || ' ' || Users.name || ' ' || Users.patronymic AS 'ФИО', Genders.name AS 'Пол' FROM Users JOIN Genders ON Users.gender = Genders.id WHERE Users.surname || ' ' || Users.name || ' ' || Users.patronymic LIKE '%" + textBox1.Text + "%'", conn);
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

        private void button3_Click(object sender, EventArgs e)
        {
            LoadUsers();
            textBox1.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddUser addUser = new AddUser();
            addUser.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dataGridView1.SelectedRows[0].Cells[0].Value == null)
            {
                MessageBox.Show("Выберите сотрудника", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DialogResult dialogResult = MessageBox.Show("Вы уверены что хотите удалить сотрудника?", "Удалить сотрудника", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return;
            }
            try
            {
                SQLiteCommand command = new SQLiteCommand("DELETE FROM `Users` WHERE `Users`.`id` = @id", conn);
                command.Parameters.Add("@id", DbType.Int32).Value = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                conn.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Вы успешно удалили сотрудника", "Успех", MessageBoxButtons.OK);
                    
                }
                else
                {
                    MessageBox.Show("Ошибка");
                }
                conn.Close();
                LoadUsers();
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
