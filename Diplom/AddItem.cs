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
    public partial class AddItem : Form
    {
        private SQLiteConnection conn = new SQLiteConnection("Data Source=dpbd.db;");
        private int Type;
        private int Id;

        public AddItem(int type, int id)
        {
            InitializeComponent();
            if(type == 0)
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
                string cm = "SELECT Items.name, Items.barcode, Items.cost FROM `Items` WHERE Items.id = @id";
                SQLiteCommand cmd = new SQLiteCommand(cm, conn);
                cmd.Parameters.Add("@id", DbType.Int32).Value = id;
                conn.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    textBox1.Text = reader.GetString(0);
                    textBox2.Text = reader.GetString(1);
                    textBox3.Text = reader.GetDouble(2).ToString();
                }
                reader.Close();
                conn.Close();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
                {
                    MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    double.Parse(textBox3.Text);
                }
                catch
                {
                    MessageBox.Show("Пожалуйста, введите корректную цену!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Type == 0)
                {
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO `Items` (name, barcode, cost, quantity) VALUES (@name, @barcode, @cost, 0)", conn);

                    command.Parameters.Add("@name", DbType.String).Value = textBox1.Text;
                    command.Parameters.Add("@barcode", DbType.String).Value = textBox2.Text;
                    command.Parameters.Add("@cost", DbType.Double).Value = double.Parse(textBox3.Text);
                    conn.Open();

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Вы добавили товар!", "Успех", MessageBoxButtons.OK);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка добавления товара" ,"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    conn.Close();
                }
                else
                {
                    SQLiteCommand command = new SQLiteCommand("UPDATE `Items` SET name = @name, barcode = @barcode, cost = @cost WHERE id = @id", conn);

                    command.Parameters.Add("@name", DbType.String).Value = textBox1.Text;
                    command.Parameters.Add("@barcode", DbType.String).Value = textBox2.Text;
                    command.Parameters.Add("@cost", DbType.Double).Value = double.Parse(textBox3.Text);
                    command.Parameters.Add("@id", DbType.Int32).Value = Id;
                    conn.Open();

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Вы изменили информацию о товаре!", "Успех", MessageBoxButtons.OK);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка изменения информации о товаре", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 44)
            {
                e.Handled = true;
            }
        }
    }
}
