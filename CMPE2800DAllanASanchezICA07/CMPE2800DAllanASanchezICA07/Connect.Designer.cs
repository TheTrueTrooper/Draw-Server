﻿namespace CMPE2800DAllanASanchezICA07
{
    partial class Connect
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
            this._TB_Host = new System.Windows.Forms.TextBox();
            this._TB_Port = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._Bu_TryConnect = new System.Windows.Forms.Button();
            this._Bu_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _TB_Host
            // 
            this._TB_Host.Location = new System.Drawing.Point(12, 25);
            this._TB_Host.Name = "_TB_Host";
            this._TB_Host.Size = new System.Drawing.Size(305, 20);
            this._TB_Host.TabIndex = 0;
            this._TB_Host.Text = "bits.net.nait.ca";
            // 
            // _TB_Port
            // 
            this._TB_Port.Location = new System.Drawing.Point(12, 64);
            this._TB_Port.Name = "_TB_Port";
            this._TB_Port.Size = new System.Drawing.Size(305, 20);
            this._TB_Port.TabIndex = 1;
            this._TB_Port.Text = "1666";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Host";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Port";
            // 
            // _Bu_TryConnect
            // 
            this._Bu_TryConnect.Location = new System.Drawing.Point(12, 90);
            this._Bu_TryConnect.Name = "_Bu_TryConnect";
            this._Bu_TryConnect.Size = new System.Drawing.Size(101, 23);
            this._Bu_TryConnect.TabIndex = 4;
            this._Bu_TryConnect.Text = "Try to Connect";
            this._Bu_TryConnect.UseVisualStyleBackColor = true;
            this._Bu_TryConnect.Click += new System.EventHandler(this._Bu_TryConnect_Click);
            // 
            // _Bu_Cancel
            // 
            this._Bu_Cancel.Location = new System.Drawing.Point(216, 90);
            this._Bu_Cancel.Name = "_Bu_Cancel";
            this._Bu_Cancel.Size = new System.Drawing.Size(101, 23);
            this._Bu_Cancel.TabIndex = 5;
            this._Bu_Cancel.Text = "Cancel";
            this._Bu_Cancel.UseVisualStyleBackColor = true;
            this._Bu_Cancel.Click += new System.EventHandler(this._Bu_Cancel_Click);
            // 
            // Connect
            // 
            this.AcceptButton = this._Bu_TryConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 125);
            this.Controls.Add(this._Bu_Cancel);
            this.Controls.Add(this._Bu_TryConnect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._TB_Port);
            this.Controls.Add(this._TB_Host);
            this.Name = "Connect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _TB_Host;
        private System.Windows.Forms.TextBox _TB_Port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button _Bu_TryConnect;
        private System.Windows.Forms.Button _Bu_Cancel;
    }
}