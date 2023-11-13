using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WindowsInput;
using WindowsInput.Native;

namespace win11_keysender
{
    public partial class Form1 : Form
    {
        private UdpClient udpClient;
        private string clipboard_ip = "";
        private string voiceboxclient_ip = "";

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
                // Clear the text box on the UI thread
                this.Invoke((MethodInvoker)delegate
                {
                    textBox1.Text = "";
                });
                var simulator = new InputSimulator();
                simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_H);
            }
            else if (inputText.StartsWith("set_clipboard_ip:"))
            {
                string[] parts = inputText.Split(':');
                if (parts.Length == 2)
                {
                    clipboard_ip = parts[1];
                    Debug.Print(clipboard_ip);
                }
            }
            else if (inputText.StartsWith("set_client_ip:"))
            {
                string[] parts = inputText.Split(':');
                if (parts.Length == 2)
                {
                    voiceboxclient_ip = parts[1];
                    Debug.Print(voiceboxclient_ip);
                }
            }
            else if (inputText == "stop")
            {
                // Send the escape key
                var simulator = new InputSimulator();
                simulator.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);

                // This is just an initial hack so I can use dictation at all
                if (clipboard_ip != "none")
                {
                    var selectedText = textBox1.Text;
                    UdpClient udpClient = new UdpClient();
                    byte[] bytesToSend = Encoding.UTF8.GetBytes(selectedText);
                    udpClient.Send(bytesToSend, bytesToSend.Length, clipboard_ip, 1339);

                    // Add a small delay to allow the clipboard to be updated
                    Thread.Sleep(300);

                    bytesToSend = Encoding.UTF8.GetBytes("25,0");
                    udpClient.Send(bytesToSend, bytesToSend.Length, voiceboxclient_ip, 5000);

                    bytesToSend = Encoding.UTF8.GetBytes("37,0");
                    udpClient.Send(bytesToSend, bytesToSend.Length, voiceboxclient_ip, 5000);

                    bytesToSend = Encoding.UTF8.GetBytes("37,1");
                    udpClient.Send(bytesToSend, bytesToSend.Length, voiceboxclient_ip, 5000);

                    bytesToSend = Encoding.UTF8.GetBytes("25,1");
                    udpClient.Send(bytesToSend, bytesToSend.Length, voiceboxclient_ip, 5000);
                }
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