using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using PackageLibrary;
using SDP.Interfaces;

namespace SDP.Test.Controls
{
    public partial class ClientControl : UserControl
    {
        private readonly IAsyncClientSocket socket;
        private SocketCfg cfg;

        private List<string> times = new List<string>();

        public ClientControl(SocketCfg cfg)
        {
            this.cfg = cfg;
            InitializeComponent();
            times=new List<string>();
            //Console.Title = "Client Console";

            socket = SdpSocket.ClientFactory(cfg);
            socket.Connect += socket_Connect;
            socket.Receive += socket_Receive;
            socket.Disconnect += socket_Disconnect;
            socket.BeginConnect();
        }

        void socket_Connect(object sender, Events.ConnectionEventArgs e)
        {
            //  Console.WriteLine("Conectado ao servidor");
        }

        void socket_Receive(object sender, Events.ReceiveEventArgs e)
        {
            // Console.WriteLine("Recebi: " + e.ReceivedData.Length + " bytes");
        }

        void socket_Disconnect(object sender, Events.ConnectionEventArgs e)
        {
            // Console.WriteLine("Desconectado do servidor");
        }

        private void ClientControl_Load(object sender, EventArgs e)
        {

        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            lbTime.Text = "";
            Console.WriteLine("Tamanho do buffer");
            int buffer = int.Parse(txtTamanho.Text);
            Console.WriteLine("Quantidade de pacotes");
            int pacotes = int.Parse(txtQuantidade.Text);

            byte[] msg = new byte[buffer];

            Clock.Start();
            for (int i = 0; i < pacotes; i++)
            {
                socket.Send(msg);
            }
            Clock.Stop();

            var ticks = Clock.Ticks;
            lbTime.Text = ticks + " ms";
            string log = "Tamanho do buffer: " + buffer + " quantidade de pacotes: " + pacotes + " tempo transcorrido: " +
                         ticks + " ms";
            times.Add(log);
            //    MessageBox.Show(ticks.ToString());
        }

        private void Test1()
        {
            int tamanho = int.Parse(txtTamanho.Text);
            int quantidade = int.Parse(txtQuantidade.Text);

            string message = "";
            for (int i = 0; i < tamanho; i++)
            {
                message += "a";
            }
            if (rbSync.Checked)
            {
                for (int i = 0; i < quantidade; i++)
                {

                    socket.Send(new MessagePacket(message).Serialize());
                    Thread.Sleep(1);
                }
            }
            else
            {
                socket.SendAsync(new MessagePacket(message).Serialize());
            }
        }

        private void ClientControl_ControlRemoved(object sender, ControlEventArgs e)
        {
              //  File.WriteAllLines("log.txt", times);
        }
    }
}
