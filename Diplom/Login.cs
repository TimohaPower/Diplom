using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SQLite;


namespace Diplom
{
    public partial class Login : Form
    {
        private SQLiteConnection conn = new SQLiteConnection("Data Source=dpbd.db;");

        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                DataTable table = new DataTable();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter();
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM `Users` WHERE `login` = @uL AND `password` = @uP", conn);
                command.Parameters.Add("@uL", DbType.String).Value = textBox1.Text;
                command.Parameters.Add("@uP", DbType.String).Value = textBox2.Text;

                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    textBox1.Clear();
                    textBox2.Clear();
                    Menu menu = new Menu(int.Parse(table.Rows[0][0].ToString()));
                    this.Hide();
                    menu.Show();
                }
                else
                {
                    textBox1.Clear();
                    textBox2.Clear();
                    MessageBox.Show("Пожалуйста, проверьте правильность введенных данных!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка подключение к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
