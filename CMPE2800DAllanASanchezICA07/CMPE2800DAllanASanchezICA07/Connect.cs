﻿//*****************************************************************************
//ICA 6, Socket assignment 1
//Nov 15 2016
//By Angelo Sanches and Justin Trudel
//Print format: Landscape
//*****************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;   //needed for type Socket
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMPE2800DAllanASanchezICA07
{
    public partial class Connect : Form
    {
        //client socket to establish connection with server
        public Socket returnSocket { private set; get; }

        public Connect()
        {
            InitializeComponent();
            //*****************************************************************
            //Position the Connect dialog over the Client form.
            //We used the designer to set the "StartPosition" property of
            //  the Connect dialog.
            //*************************
            //Form.StartPosition property
            //  https://msdn.microsoft.com/en-us/library/system.windows.forms.form.startposition(v=vs.110).aspx
            //FormStartPosition enumeration
            //  https://msdn.microsoft.com/en-us/library/system.windows.forms.formstartposition(v=vs.110).aspx
            //*****************************************************************
        }

        private void _Bu_TryConnect_Click(object sender, EventArgs e)
        {
            //server IP address
            string strHost = "";
            int IPort;

            if ((_TB_Port.Text.Length < 1) || (!int.TryParse(_TB_Port.Text, out IPort)))
            {
                MessageBox.Show("Please enter a port number that is numerical.", "Client error");
                return;
            }
            //Get the server IP address specified by the user.
            strHost = _TB_Host.Text;
            try
            {
                //Create client socket (use IP v4, send/receive socket, use TCP).
                returnSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}\nAn exception occurred in the"
                    + " Try to Connect button event handler, when trying to"
                    + " create the client"
                    + " socket.", ex.Message), "Client Exept");
                //We cannot continue.
                return;
            }
            try
            {
                //Try to connect to the server, at the address and port specified.
                //Check for invalid data.
                returnSocket.Connect(strHost, IPort);
            }
            catch
            {
                MessageBox.Show("Server not found or unresponsive.\nPlease check your Server address or port number.\nOr contact your host.", "Client Exept");
                return;
            }
            //So that the main form understands.
            DialogResult = DialogResult.OK;

        }

        private void _Bu_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}