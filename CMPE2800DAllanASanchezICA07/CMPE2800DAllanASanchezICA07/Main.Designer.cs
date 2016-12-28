using System;
using System.Windows.Forms;

namespace CMPE2800DAllanASanchezICA07
{
    partial class Main
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
            this._SS_Status = new System.Windows.Forms.StatusStrip();
            this._TSL_Conn = new System.Windows.Forms.ToolStripStatusLabel();
            this._TSL_Color = new System.Windows.Forms.ToolStripStatusLabel();
            this._TSL_ColorSwatch = new System.Windows.Forms.ToolStripStatusLabel();
            this._TSL_Thickness = new System.Windows.Forms.ToolStripStatusLabel();
            this._TSL_Alpha = new System.Windows.Forms.ToolStripStatusLabel();
            this._TSL_RxFrames = new System.Windows.Forms.ToolStripStatusLabel();
            this._TSL_Fragments = new System.Windows.Forms.ToolStripStatusLabel();
            this._TSL_DestackAvg = new System.Windows.Forms.ToolStripStatusLabel();
            this._TSL_RxBytes = new System.Windows.Forms.ToolStripStatusLabel();
            this._CD_ColorPicker = new System.Windows.Forms.ColorDialog();
            this._SS_Status.SuspendLayout();
            this.SuspendLayout();
            // 
            // _SS_Status
            // 
            this._SS_Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._TSL_Conn,
            this._TSL_Color,
            this._TSL_ColorSwatch,
            this._TSL_Thickness,
            this._TSL_Alpha,
            this._TSL_RxFrames,
            this._TSL_Fragments,
            this._TSL_DestackAvg,
            this._TSL_RxBytes});
            this._SS_Status.Location = new System.Drawing.Point(0, 937);
            this._SS_Status.Name = "_SS_Status";
            this._SS_Status.Size = new System.Drawing.Size(984, 24);
            this._SS_Status.TabIndex = 0;
            this._SS_Status.Text = "statusStrip1";
            // 
            // _TSL_Conn
            // 
            this._TSL_Conn.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this._TSL_Conn.Name = "_TSL_Conn";
            this._TSL_Conn.Size = new System.Drawing.Size(127, 19);
            this._TSL_Conn.Spring = true;
            this._TSL_Conn.Text = "No Connection";
            this._TSL_Conn.Click += new System.EventHandler(this._TSL_Conn_Click);
            // 
            // _TSL_Color
            // 
            this._TSL_Color.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this._TSL_Color.Name = "_TSL_Color";
            this._TSL_Color.Size = new System.Drawing.Size(36, 19);
            this._TSL_Color.Text = "Color";
            this._TSL_Color.Click += new System.EventHandler(this._TSL_Color_Click);
            // 
            // _TSL_ColorSwatch
            // 
            this._TSL_ColorSwatch.AutoSize = false;
            this._TSL_ColorSwatch.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._TSL_ColorSwatch.Margin = new System.Windows.Forms.Padding(0, 3, 10, 2);
            this._TSL_ColorSwatch.Name = "_TSL_ColorSwatch";
            this._TSL_ColorSwatch.Size = new System.Drawing.Size(19, 19);
            this._TSL_ColorSwatch.Click += new System.EventHandler(this._TSL_ColorSwatch_Click);
            // 
            // _TSL_Thickness
            // 
            this._TSL_Thickness.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this._TSL_Thickness.Name = "_TSL_Thickness";
            this._TSL_Thickness.Size = new System.Drawing.Size(127, 19);
            this._TSL_Thickness.Spring = true;
            this._TSL_Thickness.Text = "Thickness: -1";
            // 
            // _TSL_Alpha
            // 
            this._TSL_Alpha.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this._TSL_Alpha.Name = "_TSL_Alpha";
            this._TSL_Alpha.Size = new System.Drawing.Size(127, 19);
            this._TSL_Alpha.Spring = true;
            this._TSL_Alpha.Text = "Alpha: -1";
            // 
            // _TSL_RxFrames
            // 
            this._TSL_RxFrames.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this._TSL_RxFrames.Name = "_TSL_RxFrames";
            this._TSL_RxFrames.Size = new System.Drawing.Size(127, 19);
            this._TSL_RxFrames.Spring = true;
            this._TSL_RxFrames.Text = "Frames Rx\'d: -1";
            // 
            // _TSL_Fragments
            // 
            this._TSL_Fragments.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this._TSL_Fragments.Name = "_TSL_Fragments";
            this._TSL_Fragments.Size = new System.Drawing.Size(127, 19);
            this._TSL_Fragments.Spring = true;
            this._TSL_Fragments.Text = "Fragments: -1";
            // 
            // _TSL_DestackAvg
            // 
            this._TSL_DestackAvg.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this._TSL_DestackAvg.Name = "_TSL_DestackAvg";
            this._TSL_DestackAvg.Size = new System.Drawing.Size(127, 19);
            this._TSL_DestackAvg.Spring = true;
            this._TSL_DestackAvg.Text = "Destack Avg: -1";
            // 
            // _TSL_RxBytes
            // 
            this._TSL_RxBytes.Name = "_TSL_RxBytes";
            this._TSL_RxBytes.Size = new System.Drawing.Size(127, 19);
            this._TSL_RxBytes.Spring = true;
            this._TSL_RxBytes.Text = "Bytes Rx\'d: -1";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 961);
            this.Controls.Add(this._SS_Status);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.KeyPreview = true;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sox Multi Draw Client";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Main_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Main_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Main_MouseUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Main_MouseWheel);
            this._SS_Status.ResumeLayout(false);
            this._SS_Status.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.StatusStrip _SS_Status;
        private System.Windows.Forms.ToolStripStatusLabel _TSL_Conn;
        private System.Windows.Forms.ToolStripStatusLabel _TSL_Color;
        private System.Windows.Forms.ToolStripStatusLabel _TSL_Thickness;
        private System.Windows.Forms.ColorDialog _CD_ColorPicker;
        private System.Windows.Forms.ToolStripStatusLabel _TSL_RxFrames;
        private System.Windows.Forms.ToolStripStatusLabel _TSL_Fragments;
        private System.Windows.Forms.ToolStripStatusLabel _TSL_DestackAvg;
        private System.Windows.Forms.ToolStripStatusLabel _TSL_RxBytes;
        private ToolStripStatusLabel _TSL_ColorSwatch;
        private ToolStripStatusLabel _TSL_Alpha;
    }
}

