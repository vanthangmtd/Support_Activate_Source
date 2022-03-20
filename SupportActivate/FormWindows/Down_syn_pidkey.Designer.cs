
namespace SupportActivate.FormWindows
{
    partial class Down_syn_pidkey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Down_syn_pidkey));
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_Syn = new System.Windows.Forms.Button();
            this.tbx_Token = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lb_Timer = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.Location = new System.Drawing.Point(188, 76);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 3;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_Syn
            // 
            this.btn_Syn.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Syn.Location = new System.Drawing.Point(63, 76);
            this.btn_Syn.Name = "btn_Syn";
            this.btn_Syn.Size = new System.Drawing.Size(102, 23);
            this.btn_Syn.TabIndex = 2;
            this.btn_Syn.Text = "Synchronization";
            this.btn_Syn.UseVisualStyleBackColor = true;
            this.btn_Syn.Click += new System.EventHandler(this.btn_Syn_Click);
            // 
            // tbx_Token
            // 
            this.tbx_Token.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbx_Token.Location = new System.Drawing.Point(63, 44);
            this.tbx_Token.MaxLength = 10;
            this.tbx_Token.Name = "tbx_Token";
            this.tbx_Token.Size = new System.Drawing.Size(278, 22);
            this.tbx_Token.TabIndex = 1;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(16, 47);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(43, 15);
            this.Label2.TabIndex = 0;
            this.Label2.Text = "Token:";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(123, 12);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(125, 19);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Download Pidkey";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lb_Timer
            // 
            this.lb_Timer.AutoSize = true;
            this.lb_Timer.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Timer.Location = new System.Drawing.Point(271, 80);
            this.lb_Timer.Name = "lb_Timer";
            this.lb_Timer.Size = new System.Drawing.Size(70, 15);
            this.lb_Timer.TabIndex = 0;
            this.lb_Timer.Text = "00:00:00:000";
            // 
            // Down_syn_pidkey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(357, 110);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Syn);
            this.Controls.Add(this.tbx_Token);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lb_Timer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Down_syn_pidkey";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Down_syn_pidkey";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btn_Cancel;
        internal System.Windows.Forms.Button btn_Syn;
        internal System.Windows.Forms.TextBox tbx_Token;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Timer timer1;
        internal System.Windows.Forms.Label lb_Timer;
    }
}