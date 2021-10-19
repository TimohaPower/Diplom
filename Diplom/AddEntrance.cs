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
    public partial class AddEntrance : Form
    {
        private SQLiteConnection conn = new SQLiteConnection("Data Source=dpbd.db;");
        private int Id;
        private int IdProvider = 0;
        private int IdUser = 0;
        public AddEntrance(int id, int idu)
        {
            Id = id;
            IdUser = idu;
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
        }

        void LoadProviders()
        {
            try
            {
                DataTable dt = new DataTable();
                conn.Open();
                SQLiteCommand CMD = new SQLiteCommand("SELECT Providers.id AS '№', Providers.name AS 'Название' FROM Providers", conn);
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

        void LoadItem()
        {
            try
            {
                string cm = "SELECT Items.name FROM `Items` WHERE Items.id = @id";
                SQLiteCommand cmd = new SQLiteCommand(cm, conn);
                cmd.Parameters.Add("@id", DbType.Int32).Value = Id;
                conn.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    label2.Text = "Товар: " + reader.GetString(0);
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
        private void AddEntrance_Load(object sender, EventArgs e)
        {
            LoadItem();
            LoadProviders();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (IdProvider == 0)
                {
                    MessageBox.Show("Укажите поставщика двойным кликом", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SQLiteCommand command = new SQLiteCommand("UPDATE `Items` SET quantity = quantity + @n WHERE id = @id", conn);
                command.Parameters.Add("@n", DbType.Int32).Value = numericUpDown1.Value;
                command.Parameters.Add("@id", DbType.Int32).Value = Id;

                conn.Open();

                if (command.ExecuteNonQuery() == 1)
                {
                    command = new SQLiteCommand("INSERT INTO `Operations` (id_item, id_provider, id_user, quantity, sum, type, datetime) VALUES (@idi, @idp, @idu, @quan, @sum, 2, @date)", conn);

                    command.Parameters.Add("@idi", DbType.Int32).Value = Id;
                    command.Parameters.Add("@idp", DbType.Int32).Value = IdProvider;
                    command.Parameters.Add("@idu", DbType.Int32).Value = IdUser;
                    command.Parameters.Add("@quan", DbType.Int32).Value = numericUpDown1.Value;
                    command.Parameters.Add("@sum", DbType.Double).Value = numericUpDown2.Value;
                    command.Parameters.Add("@date", DbType.String).Value = DateTime.Now;

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Вы добавили поставку!", "Успех", MessageBoxButtons.OK);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка добавления поставки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка добавления поставки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows.Count > 1)
                {
                    return;
                }
                if (dataGridView1.SelectedRows[0].Cells[0].Value == null)
                {
                    return;
                }
                label6.Text = "Поставщик: " + dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                IdProvider = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            }
            catch { }
        }
    }
}
