using System;
using System.Windows.Forms;
using SDP.Test.Controls;

namespace SDP.Test.Forms
{
    public partial class FrmMain : Form
    {
        private Control mainControl;
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {   
            Show();

            var result = DialogResult.Cancel;

            while (result != DialogResult.OK)
            {
                using (var frmConfigure = new FrmConfigure())
                {
                    result = frmConfigure.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        switch (frmConfigure.ConnectionType)
                        {
                            case ConnectionType.Client:
                                mainControl = new ClientControl(frmConfigure.Cfg);
                                Controls.Add(mainControl);
                                break;
                            case ConnectionType.Server:
                                mainControl = new ServerControl(frmConfigure.Cfg);
                                Controls.Add(mainControl);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                       // MessageBox.Show("ERRO! configure novamente");
                        Environment.Exit(0);
                    }
                }
            }
        }

        private void FrmMain_Enter(object sender, EventArgs e)
        {
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(mainControl!=null)
            Controls.Remove(mainControl);
        }
    }
}
