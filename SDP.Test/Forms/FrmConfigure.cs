using System;
using System.Windows.Forms;
using SDP.Enums;

namespace SDP.Test.Forms
{
    public enum ConnectionType
    {
        Client,
        Server
    }

    public partial class FrmConfigure : Form
    {
        public SocketCfg Cfg { get; private set; }
        public ConnectionType ConnectionType { get; private set; }

        public FrmConfigure()
        {
            InitializeComponent();

            cbProtocolo.DataSource = Enum.GetValues(typeof(ProtocolType));
            cbTipo.DataSource = Enum.GetValues(typeof(ConnectionType));

            txtIP.Text = "127.0.0.1";
            txtPort.Text = "9958";
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            if (txtIP.Text == string.Empty || txtPort.Text == string.Empty || cbProtocolo.SelectedIndex == -1 ||
                cbTipo.SelectedIndex == -1)
            {
                MessageBox.Show("Preencha todos os campos");
                return;
            }
            if (txtIP.TextLength < 5)
            {
                MessageBox.Show("Preencha o IP corretamente");
                return;
            }

            string ip = txtIP.Text;
            int port = int.Parse(txtPort.Text);
            ProtocolType protocolType = (ProtocolType) cbProtocolo.SelectedItem;
            Cfg = new SocketCfg(ip, port, protocolType);
            ConnectionType = (ConnectionType) cbTipo.SelectedItem;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void txtIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtIP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (IpKeyHandle(e))
            {
                e.Handled = true;
                return;
            }

            char lastCharacter = '0';
            if (txtIP.Text.Length > 0)
                lastCharacter = txtIP.Text[txtIP.TextLength - 1];

            if (e.KeyChar == (char) Keys.Back && lastCharacter == '.')
            {
                txtIP.Text = txtIP.Text.Remove(txtIP.TextLength - 1);
                txtIP.Select(txtIP.Text.Length, 0);
                return;
            }

            if (char.IsDigit(e.KeyChar))
            {
                if (txtIP.TextLength >= 3)
                {
                    if (!HaveComaInBlock(txtIP.Text))
                    {
                        txtIP.Text += ".";
                        txtIP.Select(txtIP.Text.Length, 0);
                    }
                }
            }
        }

        private bool HaveComaInBlock(string txt)
        {
            if (txt.Length >= 3)
            {
                for (int i = txt.Length - 1; i > txt.Length - 4; i--)
                {
                    if (txt[i] == '.')
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        private bool IpKeyHandle(KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != (char) Keys.Back)
                return true;
            if (txtIP.Text.Length == 0 && e.KeyChar == '.')
                return true;
            if (txtIP.Text.Length > 0)
                if (txtIP.Text[txtIP.TextLength - 1] == '.' && e.KeyChar == '.')
                    return true;

            int comas = 0;
            foreach (char c in txtIP.Text)
            {
                if (c == '.')
                    comas++;
                if (comas >= 3)
                {
                    var splitComa = txtIP.Text.Split('.');
                    if ((splitComa[splitComa.Length - 1].Length > 2 || e.KeyChar == '.') && e.KeyChar!=(char)Keys.Back)
                        return true;
                }
            }

            return false;
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char) Keys.Back)
                e.Handled = true;

            if (txtPort.Text == string.Empty) return;
            var value = int.Parse(txtPort.Text);
            if (value > 65535)
            {
                txtPort.Text = "65535";
            }
        }
    }
}