using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using SDP.FEC;
using SDP.LowLevelAPI;

namespace Raw.TestServer
{
    public partial class FrmMain : Form
    {
        private readonly IPAddress sourceAddress = IPAddress.Parse("127.0.0.1");
        private IPAddress destAddress = IPAddress.Parse("127.0.0.1");
        private readonly IPAddress bindAddress = IPAddress.Any;

        private const ushort SOURCE_PORT = 9958;
        private const ushort DEST_PORT = 9959;

        private readonly byte[] builtPacket = new byte[1024];

        private readonly Socket rawSocket;

        public new event EventHandler<byte[]> Receive;

        public FrmMain()
        {
            InitializeComponent();

            const SocketOptionLevel socketLevel = SocketOptionLevel.IP;
            rawSocket = new Socket(sourceAddress.AddressFamily, SocketType.Raw, ProtocolType.Udp);
            rawSocket.Bind(new IPEndPoint(bindAddress, 0));
            rawSocket.SetSocketOption(socketLevel, SocketOptionName.HeaderIncluded, 1);

            Receive += FrmMain_Receive;

            lbStatus.Text = "Online";
            lbStatus.ForeColor=Color.Green;

            BeginReceive();
        }

        void FrmMain_Receive(object sender, byte[] e)
        {
            byte[] newBytes = new byte[e.Length - 20];
            Buffer.BlockCopy(builtPacket, 20, newBytes, 0, newBytes.Length);
            e = newBytes;

            int x = 0;
            var udpHeader = UdpHeader.Create(e, ref x);

            newBytes = new byte[e.Length - 8];
            Buffer.BlockCopy(e, 8, newBytes, 0, newBytes.Length);
            e = newBytes;

            if (udpHeader.SourcePort != 9959) return;

            lbTamanho.Invoke(new Action(() =>
            {
                txtConteudo.Clear();

                lbTamanho.Text = e.Length.ToString();
                lbCrc.Text = udpHeader.Checksum.ToString();

                txtConteudo.Text += "Valor recebido: ";
                txtConteudo.Text += Encoding.Default.GetString(e, 1, e.Length - 1 - e[0]);
                txtConteudo.Text += Environment.NewLine;

                byte ecBytes = e[0];
                txtConteudo.Text += "Bytes de correção: " + ecBytes;
                txtConteudo.Text += Environment.NewLine;

                int[] realPayloadInt = e.Select(d => (int) d).ToArray();
                ReedSolomonDecoder decoder = new ReedSolomonDecoder(GenericGf.QrCodeField256);
                if (decoder.Decode(realPayloadInt, ecBytes))
                {
                    byte[] data = realPayloadInt.Select(d => (byte) d).ToArray();
                    byte[] message = new byte[data.Length - 1 - ecBytes];
                    Buffer.BlockCopy(data, 1, message, 0, message.Length);
                    txtConteudo.Text += "Mensagem decodificada: ";
                    txtConteudo.Text += Encoding.Default.GetString(message);
                }
                else
                {
                    txtConteudo.Text += "Não foi possivel recuperar a mensagem original";
                }
            }));
        }

        private void BeginReceive()
        {
            EndPoint remoteEnd = new IPEndPoint(IPAddress.Any, 0);
            rawSocket.BeginReceiveFrom(builtPacket, 0, 1024, SocketFlags.None, ref remoteEnd, DoReceiveFrom, null);
        }

        private void DoReceiveFrom(IAsyncResult iar)
        {
            EndPoint clientEp = new IPEndPoint(IPAddress.Any, 0);
            int msgLen = rawSocket.EndReceiveFrom(iar, ref clientEp);
            byte[] receivedBuffer = new byte[msgLen];
            Buffer.BlockCopy(builtPacket, 0,receivedBuffer, 0, msgLen);

            OnReceive(receivedBuffer);

            BeginReceive();
        }

        public void OnReceive(byte[] e)
        {
            var handler = Receive;
            if (handler != null) handler(this, e);
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            lbTamanho.Text = "NULL";
            lbCrc.Text = "NULL";
            txtConteudo.Text = "";
        }
    }
}
