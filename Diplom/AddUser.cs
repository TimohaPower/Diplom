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
    public partial class AddUser : Form
    {
        private SQLiteConnection conn = new SQLiteConnection("Data Source=dpbd.db;");
        public AddUser()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox3.TextLength < 5)
            {
                MessageBox.Show("Логин не может быть меньше 5-ти знаков!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox5.TextLength <= 5)
            {
                MessageBox.Show("Пароль должен быть длинее 5-ти знаков", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (isUserExists()) { }
            else
            {
                try
                {
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO `Users` (`login`, `password`, `name`, `surname`, `patronymic`, `gender`) VALUES (@login, @password, @name, @surname, @middlename, @floor)", conn);

                    command.Parameters.Add("@login", DbType.String).Value = textBox3.Text;
                    command.Parameters.Add("@password", DbType.String).Value = textBox5.Text;
                    command.Parameters.Add("@name", DbType.String).Value = textBox1.Text;
                    command.Parameters.Add("@surname", DbType.String).Value = textBox2.Text;
                    command.Parameters.Add("@middlename", DbType.String).Value = textBox4.Text;
                    if (comboBox1.SelectedIndex == 0)
                    {
                        command.Parameters.Add("@floor", DbType.Int32).Value = 1;
                    }
                    else
                    {
                        command.Parameters.Add("@floor", DbType.Int32).Value = 2;
                    }

                    conn.Open();

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Вы добавили сотрудника!", "Успех", MessageBoxButtons.OK);
                        this.Close();

                    }
                    else
                    {
                        MessageBox.Show("Ошибка добавления");
                    }
                    conn.Close();
                }
                catch (Exception) 
                {
                    MessageBox.Show("Ошибка подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public bool isUserExists()
        {

            try
            {
                DataTable table = new DataTable();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter();
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM `Users` WHERE `login` = @uL", conn);
                command.Parameters.Add("@uL", DbType.String).Value = textBox3.Text;

                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    MessageBox.Show("Такой логин уже занят!");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return true;
            }


        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }
    }
}
