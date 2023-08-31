using System;
using System.Windows.Forms;

namespace TetrisGame
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //시작하기 (Form1 열기)
            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //종료
            Application.Exit();
        }
    }
}
