namespace Raw.TestClient
{
    partial class FrmMain
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.txtCorrompido = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbAutomatica = new System.Windows.Forms.RadioButton();
            this.rbManual = new System.Windows.Forms.RadioButton();
            this.nudEcBytes = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEcBytes)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mensagem:";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(110, 22);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(247, 26);
            this.txtMessage.TabIndex = 2;
            this.txtMessage.TextChanged += new System.EventHandler(this.txtMessage_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Location = new System.Drawing.Point(12, 216);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(394, 100);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Envio";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(71, 31);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(270, 58);
            this.button2.TabIndex = 7;
            this.button2.Text = "CRC Nulo";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtCorrompido
            // 
            this.txtCorrompido.Location = new System.Drawing.Point(196, 62);
            this.txtCorrompido.Name = "txtCorrompido";
            this.txtCorrompido.Size = new System.Drawing.Size(191, 26);
            this.txtCorrompido.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Mensagem Corrompida:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.nudEcBytes);
            this.groupBox2.Controls.Add(this.rbManual);
            this.groupBox2.Controls.Add(this.rbAutomatica);
            this.groupBox2.Location = new System.Drawing.Point(12, 107);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(394, 100);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Redundância";
            // 
            // rbAutomatica
            // 
            this.rbAutomatica.AutoSize = true;
            this.rbAutomatica.Checked = true;
            this.rbAutomatica.Location = new System.Drawing.Point(6, 34);
            this.rbAutomatica.Name = "rbAutomatica";
            this.rbAutomatica.Size = new System.Drawing.Size(108, 24);
            this.rbAutomatica.TabIndex = 0;
            this.rbAutomatica.Text = "Automática";
            this.rbAutomatica.UseVisualStyleBackColor = true;
            this.rbAutomatica.CheckedChanged += new System.EventHandler(this.rbAutomatica_CheckedChanged);
            // 
            // rbManual
            // 
            this.rbManual.AutoSize = true;
            this.rbManual.Location = new System.Drawing.Point(6, 64);
            this.rbManual.Name = "rbManual";
            this.rbManual.Size = new System.Drawing.Size(79, 24);
            this.rbManual.TabIndex = 1;
            this.rbManual.Text = "Manual";
            this.rbManual.UseVisualStyleBackColor = true;
            this.rbManual.CheckedChanged += new System.EventHandler(this.rbManual_CheckedChanged);
            // 
            // nudEcBytes
            // 
            this.nudEcBytes.Enabled = false;
            this.nudEcBytes.Location = new System.Drawing.Point(98, 64);
            this.nudEcBytes.Name = "nudEcBytes";
            this.nudEcBytes.Size = new System.Drawing.Size(88, 26);
            this.nudEcBytes.TabIndex = 2;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 328);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtCorrompido);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(426, 210);
            this.Name = "FrmMain";
            this.Text = "SDP - Raw Socket Client";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEcBytes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtCorrompido;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nudEcBytes;
        private System.Windows.Forms.RadioButton rbManual;
        private System.Windows.Forms.RadioButton rbAutomatica;
    }
}

