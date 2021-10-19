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
    public partial class AddProvider : Form
    {
        private SQLiteConnection conn = new SQLiteConnection("Data Source=dpbd.db;");
        private int Type;
        private int Id;
        public AddProvider(int type, int id)
        {
            InitializeComponent();
            if (type == 0)
            {
                Type = 0;
            }
            else
            {
                Type = 1;
                Id = id;
                label1.Text = "Редактировать информацию";
                button1.Text = "Сохранить";
                this.Text = "Редактировать информацию";
                string cm = "SELECT Providers.name, Providers.phone_number, Providers.address, Providers.email FROM `Providers` WHERE Providers.id = @id";
                SQLiteCommand cmd = new SQLiteCommand(cm, conn);
                cmd.Parameters.Add("@id", DbType.Int32).Value = id;
                conn.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    textBox1.Text = reader.GetString(0);
                    maskedTextBox1.Text = reader.GetString(1);
                    textBox3.Text = reader.GetString(2);
                    textBox4.Text = reader.GetString(3);
                }
                reader.Close();
                conn.Close();
            }
        }
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return false;
            }
            catch
            {
                return true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "" || textBox4.Text == "" || textBox3.Text == "" || !maskedTextBox1.MaskCompleted)
                {
                    MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (IsValidEmail(textBox4.Text))
                {
                    MessageBox.Show("Введите корректный Email", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Type == 0)
                {
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO `Providers` (name, phone_number, address, email) VALUES (@name, @phone_number, @address, @email)", conn);

                    command.Parameters.Add("@name", DbType.String).Value = textBox1.Text;
                    command.Parameters.Add("@phone_number", DbType.String).Value = maskedTextBox1.Text;
                    command.Parameters.Add("@address", DbType.String).Value = textBox3.Text;
                    command.Parameters.Add("@email", DbType.String).Value = textBox4.Text;

                    conn.Open();

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Вы добавили поставщика!", "Успех", MessageBoxButtons.OK);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка добавления поставщика", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    conn.Close();
                }
                else
                {
                    SQLiteCommand command = new SQLiteCommand("UPDATE `Providers` SET name = @name, phone_number = @phone_number, address = @address, email = @email WHERE id = @id", conn);

                    command.Parameters.Add("@name", DbType.String).Value = textBox1.Text;
                    command.Parameters.Add("@phone_number", DbType.String).Value = maskedTextBox1.Text;
                    command.Parameters.Add("@address", DbType.String).Value = textBox3.Text;
                    command.Parameters.Add("@email", DbType.String).Value = textBox4.Text;
                    command.Parameters.Add("@id", DbType.Int32).Value = Id;

                    conn.Open();

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Вы изменили информацию о поставщике!", "Успех", MessageBoxButtons.OK);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка изменения информации о поставщике", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    conn.Close();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к БД", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }
    }
}

