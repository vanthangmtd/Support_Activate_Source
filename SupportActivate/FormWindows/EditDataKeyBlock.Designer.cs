
namespace SupportActivate.FormWindows
{
    partial class EditDataKeyBlock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditDataKeyBlock));
            this.lb_XuLy = new System.Windows.Forms.Label();
            this.btn_File = new System.Windows.Forms.Button();
            this.tbx_KeyInput = new System.Windows.Forms.TextBox();
            this.btn_DeleteKeyBlock = new System.Windows.Forms.Button();
            this.btn_AddKeyBlock = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_XuLy
            // 
            this.lb_XuLy.AutoSize = true;
            this.lb_XuLy.Location = new System.Drawing.Point(93, 124);
            this.lb_XuLy.Name = "lb_XuLy";
            this.lb_XuLy.Size = new System.Drawing.Size(24, 13);
            this.lb_XuLy.TabIndex = 4;
            this.lb_XuLy.Text = "0/0";
            // 
            // btn_File
            // 
            this.btn_File.Location = new System.Drawing.Point(12, 67);
            this.btn_File.Name = "btn_File";
            this.btn_File.Size = new System.Drawing.Size(202, 23);
            this.btn_File.TabIndex = 1;
            this.btn_File.Text = "Add keys from file to database";
            this.btn_File.UseVisualStyleBackColor = true;
            this.btn_File.Click += new System.EventHandler(this.btn_File_Click);
            // 
            // tbx_KeyInput
            // 
            this.tbx_KeyInput.Location = new System.Drawing.Point(12, 98);
            this.tbx_KeyInput.Multiline = true;
            this.tbx_KeyInput.Name = "tbx_KeyInput";
            this.tbx_KeyInput.Size = new System.Drawing.Size(202, 20);
            this.tbx_KeyInput.TabIndex = 0;
            this.tbx_KeyInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbx_KeyInput.Click += new System.EventHandler(this.tbx_KeyInput_Click);
            this.tbx_KeyInput.TextChanged += new System.EventHandler(this.tbx_KeyInput_TextChanged);
            // 
            // btn_DeleteKeyBlock
            // 
            this.btn_DeleteKeyBlock.Enabled = false;
            this.btn_DeleteKeyBlock.Location = new System.Drawing.Point(12, 38);
            this.btn_DeleteKeyBlock.Name = "btn_DeleteKeyBlock";
            this.btn_DeleteKeyBlock.Size = new System.Drawing.Size(202, 23);
            this.btn_DeleteKeyBlock.TabIndex = 3;
            this.btn_DeleteKeyBlock.Text = "Delete this key from the blocked ones";
            this.btn_DeleteKeyBlock.UseVisualStyleBackColor = true;
            this.btn_DeleteKeyBlock.Click += new System.EventHandler(this.btn_DeleteKeyBlock_Click);
            // 
            // btn_AddKeyBlock
            // 
            this.btn_AddKeyBlock.Enabled = false;
            this.btn_AddKeyBlock.Location = new System.Drawing.Point(12, 9);
            this.btn_AddKeyBlock.Name = "btn_AddKeyBlock";
            this.btn_AddKeyBlock.Size = new System.Drawing.Size(202, 23);
            this.btn_AddKeyBlock.TabIndex = 2;
            this.btn_AddKeyBlock.Text = "Add this key to the blocked ones";
            this.btn_AddKeyBlock.UseVisualStyleBackColor = true;
            this.btn_AddKeyBlock.Click += new System.EventHandler(this.btn_AddKeyBlock_Click);
            // 
            // EditDataKeyBlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 147);
            this.Controls.Add(this.lb_XuLy);
            this.Controls.Add(this.btn_File);
            this.Controls.Add(this.tbx_KeyInput);
            this.Controls.Add(this.btn_DeleteKeyBlock);
            this.Controls.Add(this.btn_AddKeyBlock);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditDataKeyBlock";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Data Key Block";
            this.Load += new System.EventHandler(this.EditDataKeyBlock_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lb_XuLy;
        internal System.Windows.Forms.Button btn_File;
        internal System.Windows.Forms.TextBox tbx_KeyInput;
        internal System.Windows.Forms.Button btn_DeleteKeyBlock;
        internal System.Windows.Forms.Button btn_AddKeyBlock;
    }
}