using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace win11_keysender
{
    public partial class Form1 : Form
    {
        private UdpClient udpClient;
        public Form1()
        {
            InitializeComponent();
            udpClient = new UdpClient(5000);
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 0);
            byte[] bytes = udpClient.EndReceive(ar, ref ip);
            string inputText = Encoding.ASCII.GetString(bytes);
            Debug.Print(inputText);
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string inputText = textBox1.Text;
            Debug.Print(inputText);
        }
    }
}