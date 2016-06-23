using System;
using System.Drawing;
using System.Windows.Forms;
using SDP.Interfaces;
using ZedGraph;

namespace SDP.Test.Controls
{
    public partial class ServerControl : UserControl
    {
        private readonly IAsyncServerSocket socket;
        private SocketCfg cfg;

        private PointPairList list;

        public ServerControl(SocketCfg cfg)
        {
            this.cfg = cfg;
            InitializeComponent();

          //  Console.Title = "Server Console";

            socket = SdpSocket.ServerFactory(cfg);
            socket.Connect += socket_Connect;
            socket.Receive += socket_Receive;
            socket.Disconnect += socket_Disconnect;
            socket.BeginAccept();
        }

        void socket_Connect(object sender, Events.ConnectionEventArgs e)
        {
         //   Console.WriteLine("Cliente conectado");
        }

        void socket_Receive(object sender, Events.ReceiveEventArgs e)
        {
          //  Console.WriteLine("Recebi: " + e.ReceivedData.Length + " bytes");
            //y += e.ReceivedData.Length;
            y = e.ReceivedData.Length;
            list.Add(x, y);

            zg1.AxisChange();
            zg1.Refresh();
        }

        void socket_Disconnect(object sender, Events.ConnectionEventArgs e)
        {
           // Console.WriteLine("Cliente desconectado");
        }

        private void ClientControl_Load(object sender, System.EventArgs e)
        {

        }


        private void ServerControl_Load(object sender, EventArgs e)
        {
            GraphPane myPane = zg1.GraphPane;

            myPane.Title.Text = "Bytes recebidos no tempo";
            myPane.XAxis.Title.Text = "Bytes, Tempo";
            myPane.YAxis.Title.Text = "Byte";
            myPane.Y2Axis.Title.Text = "Tempo(s)";

            list = new PointPairList();

            LineItem myCurve = myPane.AddCurve("Curva",
                list, Color.Red, SymbolType.Diamond);
            myCurve.Symbol.Fill = new Fill(Color.White);

            myPane.XAxis.MajorGrid.IsVisible = true;

            myPane.YAxis.Scale.FontSpec.FontColor = Color.Red;
            myPane.YAxis.Title.FontSpec.FontColor = Color.Red;
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;
            myPane.YAxis.MajorGrid.IsZeroLine = false;
            myPane.YAxis.Scale.Align = AlignP.Inside;
            myPane.YAxis.Scale.Min = -100;
            myPane.YAxis.Scale.Max = 1024; 

            myPane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);

         /*   TextObj text = new TextObj(
                "Zoom: left mouse & drag\nPan: middle mouse & drag\nContext Menu: right mouse",
                0.05f, 0.95f, CoordType.ChartFraction, AlignH.Left, AlignV.Bottom);
            text.FontSpec.StringAlignment = StringAlignment.Near;
            myPane.GraphObjList.Add(text);*/

            zg1.IsShowHScrollBar = true;
            zg1.IsShowVScrollBar = true;
            zg1.IsAutoScrollRange = true;
            zg1.IsScrollY2 = true;

            zg1.IsShowPointValues = true;
            zg1.PointValueEvent += MyPointValueHandler;

            zg1.ContextMenuBuilder += MyContextMenuBuilder;

            zg1.ZoomEvent += MyZoomEvent;

            SetSize();


            zg1.AxisChange();
            zg1.Invalidate();
        }

        /// <summary>
        /// On resize action, resize the ZedGraphControl to fill most of the Form, with a small
        /// margin around the outside
        /// </summary>
        private void Form1_Resize(object sender, EventArgs e)
        {
            SetSize();
        }

        private void SetSize()
        {
            zg1.Location = new Point(10, 10);
            // Leave a small margin around the outside of the control
            zg1.Size = new Size(this.ClientRectangle.Width - 20,
                    this.ClientRectangle.Height - 20);
        }

        /// <summary>
        /// Display customized tooltips when the mouse hovers over a point
        /// </summary>
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane,
                        CurveItem curve, int iPt)
        {
            // Get the PointPair that is under the mouse
            PointPair pt = curve[iPt];

            return curve.Label.Text + " is " + pt.Y.ToString("f2") + " units at " + pt.X.ToString("f1") + " days";
        }

        /// <summary>
        /// Customize the context menu by adding a new item to the end of the menu
        /// </summary>
        private void MyContextMenuBuilder(ZedGraphControl control, ContextMenuStrip menuStrip,
                        Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Name = "add-beta";
            item.Tag = "add-beta";
            item.Text = "Add a new Beta Point";
            item.Click += AddBetaPoint;

            menuStrip.Items.Add(item);
        }

        /// <summary>
        /// Handle the "Add New Beta Point" context menu item.  This finds the curve with
        /// the CurveItem.Label = "Beta", and adds a new point to it.
        /// </summary>
        private void AddBetaPoint(object sender, EventArgs args)
        {
            // Get a reference to the "Beta" curve IPointListEdit
            IPointListEdit ip = zg1.GraphPane.CurveList["Beta"].Points as IPointListEdit;
            if (ip != null)
            {
                double x = ip.Count * 5.0;
                double y = Math.Sin(ip.Count * Math.PI / 15.0) * 16.0 * 13.5;
                ip.Add(x, y);
                zg1.AxisChange();
                zg1.Refresh();
            }
        }

        // Respond to a Zoom Event
        private void MyZoomEvent(ZedGraphControl control, ZoomState oldState,
                    ZoomState newState)
        {
            // Here we get notification everytime the user zooms
        }

        private int x;
        private int y;
        private readonly object locker = new object();

        private void timer1_Tick(object sender, EventArgs e)
        {
            lock (locker)
            {
        
                x++;
                
            }
        }
    }
}