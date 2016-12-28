//// by Angelo Sanches
//// Multi draw Server 
//// Sunday 9 2016
using mdtypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AngeloSanchesCMPE2800Sever
{
    public partial class Form1 : Form
    {
        #region CallBacks
        /// <summary>
        /// The Disconect call back sig 
        /// </summary>
        /// <param name="ToDisconnect">a socket</param>
        internal delegate void VoidServerClient(ServerClient ToDisconnect);
        /// <summary>
        /// Call back to let the guys diconnect properly
        /// </summary>
        internal VoidServerClient CallDisconnectClient { private set; get; }
        #endregion

        #region vars
        /// <summary>
        /// Client List to maintain clients
        /// </summary>
        List<ServerClient> Clients = new List<ServerClient>();

        /// <summary>
        /// a que to send
        /// </summary>
        internal Queue<LineSegment> SendQue = new Queue<LineSegment>();
        //internal Queue<byte[]> SendQue = new Queue<byte[]>(); an exp i though better of

        /// <summary>
        /// the Read total  for line segs
        /// </summary>
        internal volatile int RxSegCount = 0;
        /// <summary>
        /// the read total .
        /// </summary>
        internal volatile int RxBytCount = 0;

        /// <summary>
        /// a bool for breaking for transmiting
        /// </summary>
        volatile bool Sending = false;
        /// <summary>
        /// the transimit thread
        /// </summary>
        Thread TxThread;

        /// <summary>
        /// listening socket to wait for incoming connection requests
        /// </summary>
        Socket Listener;
        /// <summary>
        /// binds data to grid view
        /// </summary>
        BindingSource Binder;
        #endregion

        public Form1()
        {
            InitializeComponent();
            CallDisconnectClient = new VoidServerClient(DisconnectClient);
            StatsUpDater.Enabled = true;

            //start a Send thread 
            TxThread = new Thread(new ParameterizedThreadStart(TxThreadFunk));
            TxThread.IsBackground = true;
            Sending = true;
            TxThread.Start(this);

            //bind the thing for the links
            Binder = new BindingSource();

            //bind the bindingsource to the gridview
            _DGW_Stats.DataSource = Binder;
        }

        //All the disconnects
        #region Diconnects
        /// <summary>
        /// Disconnets the client is also used to disconnect from clients side on Call Back
        /// </summary>
        /// <param name="ToDisconnect">the Client To Remove and disconnect</param>
        void DisconnectClient(ServerClient ToDisconnect)
        {
            if (Clients.Contains(ToDisconnect))
            {
                lock(Clients)
                    Clients.Remove(ToDisconnect);
                ToDisconnect.Dispose();
            }
        }

        /// <summary>
        /// Disconnets all the clients. used on formclosing
        /// </summary>
        void MassDisconnectClient()
        {
            while (Clients.Count > 0)
                DisconnectClient(Clients[0]);
        }
        #endregion



        #region StartHerUp
        private void _Bu_StartServer_Click(object sender, EventArgs e)
        {
            //true, if port number was valid
            bool blnResult = false;
            //port number to listen on
            int iPort = 0;

            //Ensure that a valid port number was received.
            blnResult = int.TryParse(_TB_ServerPort.Text, out iPort);

            if (!blnResult || iPort < 1)
            {
                MessageBox.Show("Port number is invalid.  Please provide a"
                    + " whole number greater than 0.");
                return;
            }
            //Disable the button after press.
            _Bu_connect.Enabled = false;
            _TB_ServerPort.Enabled = false;

            //create a listening socket to wait for connection requests
            try
            {
                Listener = new Socket(
                AddressFamily.InterNetwork, //IP V4 address scheme
                SocketType.Stream,          //streaming socket (connection based)
                ProtocolType.Tcp);          //use TCP as protocol
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}\nAn exception occurred in the Start"
                    + " Server button event handler, when initializing"
                    + " the listening socket.", ex.Message), "Server Exept");
            }
            //listen for incoming connections on the specified port
            try
            {
                Listener.Bind(new IPEndPoint(IPAddress.Any, iPort));
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}\nAn exception occurred in the Start"
                    + " Server button event handler, when trying to bind the"
                    + " listening socket to port {1}.", ex.Message, iPort), "Server Exept");
            }
            //set the maximum length of the pending connections queue
            try
            {
                Listener.Listen(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}\nAn exception occurred in the Start"
                    + " Server button event handler, when setting the pending"
                    + " connections queue to 1.", ex.Message), "Server Exept");
            }
            //invoke the asynchronous made to wait for incoming connections
            try
            {
                Listener.BeginAccept(cbAccept, Listener);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}\nAn exception occurred in the Start"
                    + " Server button event handler, when starting an"
                    + " asynchronous accept.", ex.Message), "Server Exept");
            }
        }




        /// <summary>
        /// Asynchronous method to handle incoming connection requests.
        /// </summary>
        /// <param name="ar"></param>
        void cbAccept(IAsyncResult ar)
        {
            Socket IsOk = (Socket)(ar.AsyncState);

            try
            {
                Socket connsok = IsOk.EndAccept(ar);
                Accept(connsok);
                Listener.BeginAccept(cbAccept, Listener);
            }
            catch (SocketException error)
            {
                Listener.BeginAccept(cbAccept, Listener);
                Error(error.Message);
            }
        }

        /// <summary>
        /// Update the server form to show that a connection has been made.
        /// </summary>
        /// <param name="sok">connection socket</param>
        void Accept(Socket sok)
        {
            Clients.Add(new ServerClient(this, sok));
        }
        

        /// <summary>
        /// Update the server form to indicate that an error has occurred.
        /// </summary>
        /// <param name="error">error message to display in message box</param>
        void Error(string error)
        {
            MessageBox.Show(error, "Socket Error");
        }

        #endregion


        #region TransmitORTAALKTALKStuff
        static void TxThreadFunk(object OwnerIn)
        {
            Form1 Owner = (Form1)OwnerIn;
            Queue<LineSegment> SendQue = Owner.SendQue;
            //Queue<byte[]> SendQue = Owner.SendQue;
            List<ServerClient> Clients = Owner.Clients;

            BinaryFormatter BFormat = new BinaryFormatter();

            while (Owner.Sending)
            {
                

                

                int Count = 0;


                lock (SendQue)
                    Count = SendQue.Count;
                while (Count > 0)
                {
                    LineSegment LS = null;
                    //byte[] LS = null;

                    lock (SendQue)
                    {
                        LS = SendQue.Dequeue();
                        Count = SendQue.Count;
                    }
                    ServerClient ServerC = null;
                    try
                    {
                        lock(Clients)
                        foreach (ServerClient SC in Clients)
                        {
                            ServerC = SC;
                            using (MemoryStream MS = new MemoryStream())
                            {
                                BFormat.Serialize(MS, LS);
                                SC.CSocket.Send(MS.GetBuffer(), (int)MS.Length, SocketFlags.None);
                            }
                        }
                    }
#if DEBUG
                    catch (Exception ex)
#else
                    catch
#endif
                    {
#if DEBUG
                        Console.WriteLine("Failed to send to Client : Disconnecting\n" + ex);
#endif
                        Owner.DisconnectClient(ServerC);
                    }
                }

                Thread.Sleep(0);
            }


        }

        #endregion

        #region FormEventsLikeClosing+Tick
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StatsUpDater.Enabled = false;
            Sending = false;
            if(TxThread != null && TxThread.IsAlive)
                TxThread.Join();
            MassDisconnectClient();

        }

        private void StatsUpDater_Tick(object sender, EventArgs e)
        {
            lock (Clients)
            {


                    Binder.DataSource = from CL in Clients
                                        select new
                                        {
                                            FramesReceived = CL.FrameCount,
                                            AverageFramesDestacked = CL.RxCount == 0 ? 0.0f : (float)CL.FrameCount / CL.RxCount,
                                            FragmentationCount = CL.FragmentCount
                                        };

            }
            _TB_Sent.Text = RxSegCount.ToString();
            _TB_Recieved.Text = RxBytCount.ToString();
        }
        #endregion
    }
}
