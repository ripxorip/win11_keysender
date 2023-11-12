using System.Diagnostics;

namespace win11_keysender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string inputText = textBox1.Text;
            Debug.Print(inputText);
        }
    }
}