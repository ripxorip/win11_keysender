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
            // If the string is equalt to start, then start the key sending
            if (inputText == "start")
            {
                Debug.Print("Got the start");
            }
            else if (inputText == "stop")
            {
                Debug.Print("Got the stop");
                // Clear the text box on the UI thread
                this.Invoke((MethodInvoker)delegate
                {
                    textBox1.Text = "";
                });
            }
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string inputText = textBox1.Text;
            Debug.Print(inputText);
        }
    }
}