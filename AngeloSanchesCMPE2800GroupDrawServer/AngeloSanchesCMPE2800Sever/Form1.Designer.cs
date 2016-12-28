namespace AngeloSanchesCMPE2800Sever
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._TB_ServerPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._Bu_connect = new System.Windows.Forms.Button();
            this._DGW_Stats = new System.Windows.Forms.DataGridView();
            this.StatsUpDater = new System.Windows.Forms.Timer(this.components);
            this._TB_Sent = new System.Windows.Forms.TextBox();
            this._TB_Recieved = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._DGW_Stats)).BeginInit();
            this.SuspendLayout();
            // 
            // _TB_ServerPort
            // 
            this._TB_ServerPort.Location = new System.Drawing.Point(12, 29);
            this._TB_ServerPort.Name = "_TB_ServerPort";
            this._TB_ServerPort.Size = new System.Drawing.Size(383, 20);
            this._TB_ServerPort.TabIndex = 0;
            this._TB_ServerPort.Text = "1666";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port";
            // 
            // _Bu_connect
            // 
            this._Bu_connect.Location = new System.Drawing.Point(320, 4);
            this._Bu_connect.Name = "_Bu_connect";
            this._Bu_connect.Size = new System.Drawing.Size(75, 23);
            this._Bu_connect.TabIndex = 2;
            this._Bu_connect.Text = "Start Server";
            this._Bu_connect.UseVisualStyleBackColor = true;
            this._Bu_connect.Click += new System.EventHandler(this._Bu_StartServer_Click);
            // 
            // _DGW_Stats
            // 
            this._DGW_Stats.AllowUserToAddRows = false;
            this._DGW_Stats.AllowUserToDeleteRows = false;
            this._DGW_Stats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._DGW_Stats.Location = new System.Drawing.Point(12, 107);
            this._DGW_Stats.Name = "_DGW_Stats";
            this._DGW_Stats.ReadOnly = true;
            this._DGW_Stats.Size = new System.Drawing.Size(383, 142);
            this._DGW_Stats.TabIndex = 3;
            // 
            // StatsUpDater
            // 
            this.StatsUpDater.Interval = 1000;
            this.StatsUpDater.Tick += new System.EventHandler(this.StatsUpDater_Tick);
            // 
            // _TB_Sent
            // 
            this._TB_Sent.Location = new System.Drawing.Point(127, 55);
            this._TB_Sent.Name = "_TB_Sent";
            this._TB_Sent.ReadOnly = true;
            this._TB_Sent.Size = new System.Drawing.Size(268, 20);
            this._TB_Sent.TabIndex = 4;
            // 
            // _TB_Recieved
            // 
            this._TB_Recieved.Location = new System.Drawing.Point(127, 81);
            this._TB_Recieved.Name = "_TB_Recieved";
            this._TB_Recieved.ReadOnly = true;
            this._TB_Recieved.Size = new System.Drawing.Size(268, 20);
            this._TB_Recieved.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Total Bytes Sent";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Total Bytes Recieved";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 261);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._TB_Recieved);
            this.Controls.Add(this._TB_Sent);
            this.Controls.Add(this._DGW_Stats);
            this.Controls.Add(this._Bu_connect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._TB_ServerPort);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this._DGW_Stats)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _TB_ServerPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _Bu_connect;
        private System.Windows.Forms.DataGridView _DGW_Stats;
        private System.Windows.Forms.Timer StatsUpDater;
        private System.Windows.Forms.TextBox _TB_Sent;
        private System.Windows.Forms.TextBox _TB_Recieved;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

