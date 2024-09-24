using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class Form1 : Form
    {
        private Client client;
        private TextBox txtBox;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtBox = new TextBox();
            txtBox.Text = "Hello";
            txtBox.Multiline = true;
            txtBox.Location = new Point(15, 15);
            txtBox.Size = new Size(100, 100);

            client = new Client("127.0.0.1", 5555);
            this.Controls.Add(txtBox);
            this.Click += new EventHandler(ClickHandler);
        }

        public void ClickHandler(object ob, EventArgs e)
        {
            txtBox.Text = client.ReadMessage();
            //Form1.ActiveForm.Refresh();
            MessageBox.Show("You clicked on Form Area");
        }
    }
}
