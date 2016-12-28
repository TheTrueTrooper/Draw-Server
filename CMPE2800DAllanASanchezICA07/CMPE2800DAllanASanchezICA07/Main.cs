/************************************************
 * File: Main.cs                                *
 * Author: Angelo Sanchez & Dillon Allan        *
 * Description: This is the main UI for the Sox *
 *              Multi Draw client.              *
 *************************************************/
using mdtypes;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMPE2800DAllanASanchezICA07
{
    public partial class Main : Form
    {
        #region Delegates & Delegate Types
        /// <summary>
        /// Delegate type used for client updates.
        /// </summary>
        private delegate void delVoidVoid();

        /// <summary>
        /// Delegate type used for updating labels on the status bar.
        /// </summary>
        /// <param name="tsl">Status label.</param>
        /// <param name="str">Message.</param>
        private delegate void delVoidTSLStr(ToolStripStatusLabel tsl, string str);

        /// <summary>
        /// Delegate used to update the UI.
        /// </summary>
        private delVoidVoid UpdateUI = null;

        /// <summary>
        /// Delegate used for shutting down socket comms.
        /// </summary>
        private delVoidVoid Recycle = null;
        #endregion

        #region Fields
        /// <summary>
        /// Socket used to connect to the server, handling all
        /// send and receive operations.
        /// </summary>
        private Socket TxRxSocket;

        /// <summary>
        /// Thread used for sending line segments to the server.
        /// </summary>
        private Thread TxThread;

        /// <summary>
        /// Thread used for receiving frames from the server.
        /// </summary>
        private Thread RxThread;

        /// <summary>
        /// Thread used for rendering line segments
        /// to the client graphics.
        /// </summary>
        private Thread RenderThread;

        private object SyncLock = new object();

        // TEST //
        private MemoryStream DataStream = new MemoryStream();

        /// <summary>
        /// Queue for storing rx buffers.
        /// </summary>
        // private Queue<byte[]> RxBuffs = new Queue<byte[]>(); Try this later when there is time should be thread safe? <-----
        private Queue<byte[]> RxBuffs = new Queue<byte[]>();

        /// <summary>
        /// Queue for holding line segments to send to the server.
        /// </summary>
        private Queue<LineSegment> TxLineSegs = new Queue<LineSegment>();

        /// <summary>
        /// Queue for holding line segments to render.
        /// </summary>
        //private ConcurrentQueue<LineSegment> RxLineSegs = new ConcurrentQueue<LineSegment>(); Try this later when there is time should be thread safe? <-----
        private Queue<LineSegment> RxLineSegs = new Queue<LineSegment>();
        
        /// <summary>
        /// Flag used for signalling when the user is drawing.
        /// </summary>
        private bool IsDrawing = false;

        /// <summary>
        /// Flag used for changing alpha.
        /// </summary>
        private bool ShiftScrolling = false;

        /// <summary>
        /// Color used for drawing.
        /// </summary>
        private Color drawColor = Color.Black;

        /// <summary>
        /// Value for line segment thickness.
        /// </summary>
        private volatile ushort Thickness = 1;

        /// <summary>
        /// Value used for setting draw alpha/transparency.
        /// </summary>
        private volatile byte Alpha = 255;

        /// <summary>
        /// Maximum thickness of a line segment.
        /// </summary>
        private const int MaxThickness = 2000;

        /// <summary>
        /// Minimum thickness of a line segment
        /// </summary>
        private const int MinThickness = 1;

        /// <summary>
        /// MSDN standard delta value for mouse wheel scrolling.
        /// https://msdn.microsoft.com/en-us/library/system.windows.forms.control.mousewheel(v=vs.110).aspx
        /// </summary>
        private const int ScrollIncrement = 120;

        /// <summary>
        /// Number of receive events.
        /// </summary>
        private volatile int RxCount = 0;

        /// <summary>
        /// Number of complete frames received by the client.
        /// </summary>
        private volatile int RxFrameCount = 0;

        /// <summary>
        /// Number of times fragmented data/partial frame was received by the client.
        /// </summary>
        private volatile int FragmentsCount = 0;

        /// <summary>
        /// Average number of frames destacked per receive event.
        /// </summary>
        private volatile float DestackAvg = 0f;

        /// <summary>
        /// Total number of bytes received by the client.
        /// </summary>
        private volatile int RxBytesTotal = 0;

        /// <summary>
        /// Number of frames sent by the client.
        /// </summary>
        private volatile int TxCount = 0;

        /// <summary>
        /// Collection of common prefixes to use when formatting RxBytesTotal values.
        /// Note: 1024 bytes = 1KB
        /// </summary>
        private static readonly string[] sizes = { "B", "KB", "MB", "GB" };


        /// <summary>
        /// location of the last mouse to prevent redundency
        /// </summary>
        Point LastMouseLocation = new Point(0,0);
        #endregion

        #region Form Methods
        public Main()
        {
            InitializeComponent();
            
            // set up the UI
            InitUI();
        }

        /// <summary>
        /// Launches a connection dialog for connecting 
        /// to the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _TSL_Conn_Click(object sender, EventArgs e)
        {
            //instance of modal dialog
            Connect C = new Connect();

            //display modal dialog
            C.ShowDialog();
            
            //User clicked "Try to Connect" button.
            if (C.DialogResult == DialogResult.OK)
            {
                // pass C's connected socket on to the client socket
                TxRxSocket = C.returnSocket;

                // indicate connection success in the UI
                _TSL_Conn.ForeColor = Color.Green;
                _TSL_Conn.Text = "Connected";

                // start the tx thread
                TxThread = new Thread(new ParameterizedThreadStart(TxThreadFunc));
                TxThread.IsBackground = true;
                TxThread.Start(this);

                // start the rx thread
                RxThread = new Thread(new ParameterizedThreadStart(RxThreadFunc));
                RxThread.IsBackground = true;
                RxThread.Start(this);

                // start the DataHandling thread
                //DataProcThread = new Thread(new ParameterizedThreadStart(HandleDataThread));
                //DataProcThread.IsBackground = true;
                //DataProcThread.Start(this);

                // start the render thread
                RenderThread = new Thread(new ParameterizedThreadStart(RenderThreadFunc));
                RenderThread.IsBackground = true;
                RenderThread.Start(this);

                //Thread.Sleep(20); // <-- don we need this?
            }
        }

        /// <summary>
        /// Updates the draw color.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _TSL_Color_Click(object sender, EventArgs e)
        {
            // launch color picker for the user 
            // to choose a new draw color
            UpdateColor();
        }

        /// <summary>
        /// Updates the draw color.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _TSL_ColorSwatch_Click(object sender, EventArgs e)
        {
            // launch color picker for the user 
            // to choose a new draw color
            UpdateColor();
        }

        /// <summary>
        /// Raises shift scrolling flag 
        /// if the user presses the shift key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            // check for shift key
            if (e.KeyCode == Keys.ShiftKey)
            {
                // set the shift scroll flag
                ShiftScrolling = true;
            }

        }

        /// <summary>
        /// Lowers the shift scrolling flag
        /// if the user releases the shift key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            // check for shift key
            if (e.KeyCode == Keys.ShiftKey)
            {
                // reset the shift scroll flag
                ShiftScrolling = false;
            }
        }

        /// <summary>
        /// Stores the location of the mouse where its button was clicked,
        /// and raises the drawing flag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            // store initial location
            LastMouseLocation = e.Location;

            // set the drawing flag
            IsDrawing = true;
        }

        /// <summary>
        /// Lowers the drawing flag, signalling that 
        /// the user is done drawing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_MouseUp(object sender, MouseEventArgs e)
        {
            // reset the drawing flag
            IsDrawing = false;
        }

        /// <summary>
        /// If the drawing flag is raised, the user creates line segments by
        /// moving the mouse. The new location and last location are used to form
        /// a line segment, which is sent to the TxLineSegs queue for transmitting
        /// to the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            // don't do anything if not drawing
            if (!IsDrawing)
                return;

            //if they are the same just return
            if (e.Location == LastMouseLocation)
                return;

            //lame no constructor set it all maunualy
            LineSegment LS = new LineSegment();
            LS.Start = LastMouseLocation;
            LS.End = e.Location;
            LS.Alpha = Alpha;
            LS.Colour = drawColor;
            LS.Thickness = Thickness;

            // add to tx line segments queue
            lock (TxLineSegs)
                TxLineSegs.Enqueue(LS);

            // set last location to current location
            LastMouseLocation = e.Location;
        }

        /// <summary>
        /// Updates the drawing thickness when the user rolls the mouse wheel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_MouseWheel(object sender, MouseEventArgs e)
        {
            /////////////////////////////////////////////////////////////////
            // the mouse wheel delta value is either +/-120.
            // Therefore, let the user change thickness by +/-1 per scroll.
            /////////////////////////////////////////////////////////////////

            // check if the user is shift scrolling
            if (ShiftScrolling)
            {
                // don't let alpha go over 255, or below 1
                int newAlpha = Alpha + e.Delta / ScrollIncrement;
                if (newAlpha > 255 || newAlpha < 1)
                    return;

                // not out of range -- set new alpha
                Alpha = (byte)newAlpha;

                // update UI
                _TSL_Alpha.Text = $"Alpha: {Alpha}";
            }

            // regular scrolling -- set thickness
            else
            {
                // don't let thickness go over the max, or under 1
                int newThickness = Thickness + e.Delta / ScrollIncrement;
                if (newThickness > MaxThickness || newThickness < MinThickness)
                    return;

                // not out of range -- set new thickness
                Thickness = (ushort)newThickness;

                // update UI
                _TSL_Thickness.Text = $"Thickness: {Thickness}";
            }
        }
        #endregion

        #region Thread Methods
        /// <summary>
        /// Pulls line segments from the queue, and renders them 
        /// to the client graphics, using double buffered rendering.
        /// </summary>
        /// <param name="obj">The main client</param>
        private void RenderThreadFunc(object obj)
        {
            // cast a local copy of the main form
            Main client = (Main)obj;

            // line segments found -- start double buffered rendering
            using (BufferedGraphicsContext bgc = new BufferedGraphicsContext())
            {
                // use local client graphics to create back buffer
                using (BufferedGraphics bg = bgc.Allocate(client.CreateGraphics(), client.ClientRectangle))
                {
                    // clear the back buffer
                    bg.Graphics.Clear(Color.White);

                    // loop while the connection socket is alive
                    while (TxRxSocket != null)
                    {
                        // check if any line segments are available for rendering
                        int count = 0;

                        do
                        {
                            // pull a line segment from the front of the queue
                            LineSegment ls = null;

                            lock (client.RxLineSegs)
                            {
                                // update the count
                                count = client.RxLineSegs.Count;

                                if (count > 0)
                                {
                                    // pull off a line segment
                                    ls = client.RxLineSegs.Dequeue();
                                }
                                else
                                    break;
                            }

                            // render the line segment to the back buffer
                            ls.Render(bg.Graphics);

                            // flip the back buffer to Main's graphics
                            // do I need a lock here?
                            // i.e. does this conflict with the main thread?
                            //lock (client.SyncLock)
                            
                        }
                        while (count > 0);

                        bg.Render();

                        // try to update the UI
                        try
                        {
                            Invoke(UpdateUI);
                        }
                        catch (Exception)
                        {
                            //Trace.WriteLine($"UI Update error: {err.Message}");
                        }

                        // sleep to force an FPS of ~60fps NOPE
                        Thread.Sleep(0);
                    }
                }
            }
        }



        /// <summary>
        /// the thread that will listen and rec
        /// </summary>
        /// <param name="obj">this object to invok from</param>
        private void RxThreadFunc(object obj)
        {
            // create a local instance of the main form
            // using the obj parameter
            Main Client = (Main)obj;
            BinaryFormatter BinaryPacker = new BinaryFormatter();
            // create a memory stream for holding fragmented data
            //MemoryStream msRxStream = new MemoryStream();

            // create a buffer for storing rx frames
            byte[] rxBuff = new byte[2048];

            // loop while the socket is not null
            while (Client.TxRxSocket != null)
            {
                // try to receive data
                int rxBytesCount = 0;
                try
                {
                    // receive some data
                    rxBytesCount = Client.TxRxSocket.Receive(rxBuff);

                    // successful receive -- update count
                    /*lock (Client.SyncLock)*/ Client.RxCount++;

                    // check for a soft disconnect -- no bytes received
                    if (rxBytesCount == 0)
                    {
                        // send an error msg to the trace
                        Trace.WriteLine("Soft disconnect.");

                        // try to tell the main UI to disable the socket
                        try
                        {
                            Client.Recycle.Invoke();
                        }
                        catch (Exception err) {}

                        // exit thread
                        return;
                    }

                    // add number of bytes to total count
                    Client.RxBytesTotal += rxBytesCount;

                    // update UI
                    //Client.UpdateUI.Invoke(Client._TSL_RxBytes, $"Bytes Rx'd: {(Client.RxBytesTotal)}");
                }

                // receive failed -- hard disconnect
                catch (Exception err)
                {
                    // send error message to stack trace
                    Trace.WriteLine($"Hard disconnect: {err.Message}");

                    // tell the main UI to disable the socket
                    try
                    {
                        Client.Recycle.Invoke();
                    }
                    catch (Exception err2) {}

                    // exit thread
                    return;
                }

                /////////////////////////////////////////////
                // try to destack and deserialize rx data
                ////////////////////////////////////////////

                // the last postion to use;
                long LastPos = 0;
                // a serailization fail reset point
                long CurrentTryPos = 0;
                try
                {
                    LastPos = DataStream.Position;

                    DataStream.Seek(0, SeekOrigin.End);

                    DataStream.Write(rxBuff, 0, rxBytesCount);

                    DataStream.Position = LastPos;
                }
                catch (Exception err)
                {
                    Trace.WriteLine($"Datastream error: {err.Message}");
                }

                // loop over the newly written data
                do
                {
                    //store the postion for rewind
                    CurrentTryPos = DataStream.Position;
                    try
                    {
                        //de serializes
                        LineSegment LS = (LineSegment)BinaryPacker.Deserialize(DataStream);
                        lock (RxLineSegs)
                            RxLineSegs.Enqueue(LS);

                        //lock (Client.SyncLock)
                        
                            // a full frame was processed
                            // increment total received frames count
                            Client.RxFrameCount++;
                            //lock (Client.SyncLock)
                                // update the destack avg
                                Client.DestackAvg = (float)Client.RxFrameCount / Client.RxCount;


                    }// may loop for ever look int later <--------------------------------------------------------------------------
                    catch (SerializationException SE)
                    {
                        // if there is an error tell us about it but step back as it is prob fragmentaion
                        DataStream.Position = CurrentTryPos;

                        // update fragments count
                        Client.FragmentsCount++;

                        // leave the loop
                        break;
                    }

                } while (DataStream.Position < DataStream.Length);

                if (DataStream.Position == DataStream.Length)
                {
                    //resset all buffers if able or done
                    DataStream.Position = 0;
                    DataStream.SetLength(0);
                }
                
                Thread.Sleep(0);
            }
        }
        

        /// <summary>
        /// Pulls line segments from the tx queue, and 
        /// attempts to send it as a serialized byte stream.
        /// </summary>
        /// <param name="obj">Client form.</param>
        private void TxThreadFunc(object obj)
        {
            // cast a local copy of the client
            Main Client = (Main)obj;
            BinaryFormatter bf = new BinaryFormatter();
            // loop while the socket is up
            while (TxRxSocket != null)
            {
                // don't send anything if there's nothing to send
                lock (TxLineSegs)
                {
                    if (TxLineSegs.Count == 0)
                        continue;
                }

                // pull a line segment from the front of the queue
                LineSegment ls = null;
                lock (TxLineSegs)
                {
                    ls = TxLineSegs.Dequeue();
                }

                // create new BinaryFormatter and MemoryStream locals
                MemoryStream ms = new MemoryStream();

                // attempt to serialize ls using bf and ms
                try
                {
                    bf.Serialize(ms, ls);
                }
                catch (Exception)
                {
                    // make note of the error msg
                    //Trace.WriteLine(err.Message);

                    // inform the user that the serialization attempt failed
                    MessageBox.Show("Sorry, the line segment couldn't be packaged properly.\n" + 
                        "Please try reconnecting to the server.", "Serialization Attempt Error");

                    // leave
                    return;
                }

                // serialization was successful

                // try to send the serialized data with ms, and the socket
                try
                {
                    //lock (TxRxSocket)
                    TxRxSocket.Send(ms.GetBuffer(), (int)ms.Length, SocketFlags.None);

                    //lock (Client.SyncLock) 
                    Client.TxCount++;
                }
                catch (Exception err)
                {
                    // send failed -- make note of the err msg
                    Trace.WriteLine(err.Message);

                    // let the user know the Send failed
                    MessageBox.Show("Sorry, the guess couldn't be sent.\n" + 
                        "Please try reconnecting to the server.", "Send Attempt Error");

                    // leave
                    return;
                }

                Thread.Sleep(0);
            }
        }
#endregion

#region Call Backs
        /// <summary>
        /// Call back function used for dismantling
        /// the client socket.
        /// </summary>
        private void CBDismantleSocket()
        {
            // try to shutdown the socket
            try
            {
                Trace.Write("Attempting to shutdown the socket...");
                TxRxSocket?.Shutdown(SocketShutdown.Both);
                Trace.WriteLine("success.");
                TxRxSocket.Close();
            }

            // shutdown failed
            catch (Exception err)
            {
                // print error msg to trace
                Trace.WriteLine($"Socket shutdown failed: {err.Message}");
            }

            // nullify the socket
            TxRxSocket = null;

            // update the UI
            _TSL_Conn.Text = "No Connection";
        }
#endregion

#region Helpers -- Add me to a separate Utils.cs partial class!
        /// <summary>
        /// Initializes the UI.
        /// </summary>
        private void InitUI()
        {
            // UI inits
            _TSL_Conn.Text = "No Connection";
            _TSL_Conn.ForeColor = Color.Black;
            _TSL_ColorSwatch.ForeColor = drawColor;
            _TSL_Thickness.Text = $"Thickness: {Thickness}";
            _TSL_Alpha.Text = $"Alpha: {Alpha}";
            _TSL_RxFrames.Text = $"Frames Rx'd: {RxFrameCount}";
            _TSL_Fragments.Text = $"Fragments: {FragmentsCount}";
            _TSL_DestackAvg.Text = $"Destack Avg: {DestackAvg}";
            _TSL_RxBytes.Text = $"Bytes Rx'd: {RxBytesTotal}";

            // delegate inits
            Recycle = new delVoidVoid(CBDismantleSocket);
            UpdateUI = new delVoidVoid(UpdateStatus);
        }

        /// <summary>
        /// Updates the UI status labels.
        /// </summary>
        private void UpdateStatus()
        {
            // update status labels with current values
            _TSL_ColorSwatch.BackColor = drawColor;
            _TSL_Thickness.Text = $"Thickness: {Thickness}";
            _TSL_Alpha.Text = $"Alpha: {Alpha}";
            _TSL_RxFrames.Text = $"Frames Rx'd: {RxFrameCount}";
            _TSL_Fragments.Text = $"Fragments: {FragmentsCount}";
            _TSL_DestackAvg.Text = $"Destack Avg: {DestackAvg.ToString("f2")}";
            _TSL_RxBytes.Text = $"Bytes Rx'd: {FormatBytes(RxBytesTotal)}";
        }

        /// <summary>
        /// Launches _CD_ColorPicker, which allows the user to
        /// choose a draw color.
        /// </summary>
        private void UpdateColor()
        {
            // show color picker modeless dlg
            DialogResult dr = _CD_ColorPicker.ShowDialog();

            // check for valid color pick
            if (dr == DialogResult.OK)
            {
                // assign new color
                drawColor = _CD_ColorPicker.Color;

                // set prog bar to new color
                _TSL_ColorSwatch.BackColor = drawColor;
            }
        }

        /// <summary>
        /// Accepts a byte value, and returns a string 
        /// formatted using K, M, and G prefixes.
        /// </summary>
        /// <param name="bytesVal">Byte value.</param>
        /// <returns></returns>
        private static string FormatBytes(int bytesVal)
        {
            // format the byte count to K, M, G style
            double bytes = bytesVal;

            // create a counter to set which prefix to use
            int order = 0;

            // keep dividing the byte count by 1024 bytes to get the prefix
            while (bytes >= 1024 && ++order < sizes.Length)
            {
                bytes /= 1024;
            }

            // format the byte count with a prefix
            string formattedBytes = string.Format("{0:0.##}{1}", bytes, sizes[order]);

            return formattedBytes;
        }
#endregion
    }
}
