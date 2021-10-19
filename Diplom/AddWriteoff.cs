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
    public partial class AddWriteoff : Form
    {
        private SQLiteConnection conn = new SQLiteConnection("Data Source=dpbd.db;");
        private int Id;
        private int IdUser = 0;

        private int quan;
        private double cost;
        public AddWriteoff(int id, int idu)
        {
            InitializeComponent();
            Id = id;
            IdUser = idu;
        }

        void LoadItem()
        {
            try
            {
                string cm = "SELECT Items.name, Items.quantity, Items.cost FROM `Items` WHERE Items.id = @id";
                SQLiteCommand cmd = new SQLiteCommand(cm, conn);
                cmd.Parameters.Add("@id", DbType.Int32).Value = Id;
                conn.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    label2.Text = "Товар: " + reader.GetString(0);
                    quan = reader.GetInt32(1);
                    cost = reader.GetDouble(2);
                }
                reader.Close();
                conn.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if ((quan - numericUpDown1.Value) < 0)
                {
                    MessageBox.Show("Товара на складе недостаточно!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                SQLiteCommand command = new SQLiteCommand("UPDATE `Items` SET quantity = quantity - @n WHERE id = @id", conn);
                command.Parameters.Add("@n", DbType.Int32).Value = numericUpDown1.Value;
                command.Parameters.Add("@id", DbType.Int32).Value = Id;

                conn.Open();

                if (command.ExecuteNonQuery() == 1)
                {
                    command = new SQLiteCommand("INSERT INTO `Operations` (id_item, id_user, quantity, sum, type, datetime) VALUES (@idi, @idu, @quan, @sum, 3, @date)", conn);

                    command.Parameters.Add("@idi", DbType.Int32).Value = Id;
                    command.Parameters.Add("@idu", DbType.Int32).Value = IdUser;
                    command.Parameters.Add("@quan", DbType.Int32).Value = numericUpDown1.Value;
                    command.Parameters.Add("@sum", DbType.Double).Value = cost * Convert.ToInt32(numericUpDown1.Value);
                    command.Parameters.Add("@date", DbType.String).Value = DateTime.Now;

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Вы добавили списание!", "Успех", MessageBoxButtons.OK);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка добавления списания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка добавления списания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                conn.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddWriteoff_Load(object sender, EventArgs e)
        {
            LoadItem();
        }
    }
}
