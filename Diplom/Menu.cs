using System;
using System.Windows.Forms;

namespace Diplom
{
    public partial class Menu : Form
    {
        private int Id;
        public Menu(int id)
        {
            InitializeComponent();
            Id = id;
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Items items = new Items(Id);
            items.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Cart cart = new Cart(Id);
            cart.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Actions actions = new Actions(Id);
            actions.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Users users = new Users(Id);
            users.Show();
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Providers providers = new Providers(Id);
            providers.Show();
        }
    }
}
