namespace SupportActivate.FormWindows
{
    partial class DataKey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataKey));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_SaveDBToFileText = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_BackupDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_RestoreDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_AddDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Action = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_AddKey = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_SearchKey = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_ShowKeyBlock = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_Refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_RefreshDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_RecheckTheSelectedKey = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_RecheckInformationTheSelectedKey = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_CopyTheSelectedKey = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_CopyTheSelectedKeyAndTheirInformation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_ChangeTheSelectedKeyToKeyblock = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_DeleteTheSelectedKey = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_RecoveryTheSelectedKey = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_SynchronizationPidkey = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_RegisterToken = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_UploadPidkey = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_DownloadPidkey = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_Timer = new System.Windows.Forms.Label();
            this.tbx_KeySearch = new System.Windows.Forms.TextBox();
            this.cbb_Description = new System.Windows.Forms.ComboBox();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.btn_DeleteKey = new System.Windows.Forms.Button();
            this.btn_KeyBlock = new System.Windows.Forms.Button();
            this.btn_Copy = new System.Windows.Forms.Button();
            this.btn_Search = new System.Windows.Forms.Button();
            this.dgv_Key = new System.Windows.Forms.DataGridView();
            this.STT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LicenseType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAKCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ErrorCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Getweb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenu_CopyTheSelectedKey = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu_CopyTheSelectedKeyAndTheirInformation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenu_RecheckTheSelectedKey = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu_RecheckInformationTheSelectedKey = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenu_DeleteTheSelectedKey = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu_ChangeTheSelectedKeyToKeyBlock = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu_RecoveryTheSelectedKey = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Key)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_File,
            this.menu_Action,
            this.menu_SynchronizationPidkey});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(919, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menu_File
            // 
            this.menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_SaveDBToFileText,
            this.toolStripSeparator1,
            this.menu_BackupDatabase,
            this.menu_RestoreDatabase,
            this.toolStripSeparator2,
            this.menu_AddDatabase,
            this.toolStripSeparator6,
            this.menu_Close});
            this.menu_File.Name = "menu_File";
            this.menu_File.Size = new System.Drawing.Size(37, 20);
            this.menu_File.Text = "File";
            // 
            // menu_SaveDBToFileText
            // 
            this.menu_SaveDBToFileText.Name = "menu_SaveDBToFileText";
            this.menu_SaveDBToFileText.Size = new System.Drawing.Size(243, 22);
            this.menu_SaveDBToFileText.Text = "Save DB to file text";
            this.menu_SaveDBToFileText.Click += new System.EventHandler(this.menu_SaveDBToFileText_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(240, 6);
            // 
            // menu_BackupDatabase
            // 
            this.menu_BackupDatabase.Name = "menu_BackupDatabase";
            this.menu_BackupDatabase.Size = new System.Drawing.Size(243, 22);
            this.menu_BackupDatabase.Text = "Backup Database";
            this.menu_BackupDatabase.Click += new System.EventHandler(this.menu_BackupDatabase_Click);
            // 
            // menu_RestoreDatabase
            // 
            this.menu_RestoreDatabase.Name = "menu_RestoreDatabase";
            this.menu_RestoreDatabase.Size = new System.Drawing.Size(243, 22);
            this.menu_RestoreDatabase.Text = "Restore Database";
            this.menu_RestoreDatabase.Click += new System.EventHandler(this.menu_RestoreDatabase_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(240, 6);
            // 
            // menu_AddDatabase
            // 
            this.menu_AddDatabase.Name = "menu_AddDatabase";
            this.menu_AddDatabase.Size = new System.Drawing.Size(243, 22);
            this.menu_AddDatabase.Text = "Add Database";
            this.menu_AddDatabase.Click += new System.EventHandler(this.menu_AddDatabase_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(240, 6);
            // 
            // menu_Close
            // 
            this.menu_Close.Name = "menu_Close";
            this.menu_Close.Size = new System.Drawing.Size(243, 22);
            this.menu_Close.Text = "Close                                   Alt+F4";
            this.menu_Close.Click += new System.EventHandler(this.menu_Close_Click);
            // 
            // menu_Action
            // 
            this.menu_Action.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_AddKey,
            this.menu_SearchKey,
            this.menu_ShowKeyBlock,
            this.toolStripSeparator3,
            this.menu_Refresh,
            this.menu_RefreshDatabase,
            this.menu_RecheckTheSelectedKey,
            this.menu_RecheckInformationTheSelectedKey,
            this.toolStripSeparator4,
            this.menu_CopyTheSelectedKey,
            this.menu_CopyTheSelectedKeyAndTheirInformation,
            this.toolStripSeparator5,
            this.menu_ChangeTheSelectedKeyToKeyblock,
            this.menu_DeleteTheSelectedKey,
            this.menu_RecoveryTheSelectedKey});
            this.menu_Action.Name = "menu_Action";
            this.menu_Action.Size = new System.Drawing.Size(54, 20);
            this.menu_Action.Text = "Action";
            // 
            // menu_AddKey
            // 
            this.menu_AddKey.Name = "menu_AddKey";
            this.menu_AddKey.Size = new System.Drawing.Size(378, 22);
            this.menu_AddKey.Text = "Add Key";
            this.menu_AddKey.Click += new System.EventHandler(this.menu_AddKey_Click);
            // 
            // menu_SearchKey
            // 
            this.menu_SearchKey.Name = "menu_SearchKey";
            this.menu_SearchKey.Size = new System.Drawing.Size(378, 22);
            this.menu_SearchKey.Text = "Search Key                                                               Ctrl+F";
            this.menu_SearchKey.Click += new System.EventHandler(this.menu_SearchKey_Click);
            // 
            // menu_ShowKeyBlock
            // 
            this.menu_ShowKeyBlock.Name = "menu_ShowKeyBlock";
            this.menu_ShowKeyBlock.Size = new System.Drawing.Size(378, 22);
            this.menu_ShowKeyBlock.Text = "Show key block";
            this.menu_ShowKeyBlock.Click += new System.EventHandler(this.menu_ShowKeyBlock_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(375, 6);
            // 
            // menu_Refresh
            // 
            this.menu_Refresh.Name = "menu_Refresh";
            this.menu_Refresh.Size = new System.Drawing.Size(378, 22);
            this.menu_Refresh.Text = "Refresh                                                                     F5";
            this.menu_Refresh.Click += new System.EventHandler(this.menu_Refresh_Click);
            // 
            // menu_RefreshDatabase
            // 
            this.menu_RefreshDatabase.Name = "menu_RefreshDatabase";
            this.menu_RefreshDatabase.Size = new System.Drawing.Size(378, 22);
            this.menu_RefreshDatabase.Text = "Refresh database";
            this.menu_RefreshDatabase.Click += new System.EventHandler(this.menu_RefreshDatabase_Click);
            // 
            // menu_RecheckTheSelectedKey
            // 
            this.menu_RecheckTheSelectedKey.Enabled = false;
            this.menu_RecheckTheSelectedKey.Name = "menu_RecheckTheSelectedKey";
            this.menu_RecheckTheSelectedKey.Size = new System.Drawing.Size(378, 22);
            this.menu_RecheckTheSelectedKey.Text = "Recheck the selected key";
            this.menu_RecheckTheSelectedKey.Click += new System.EventHandler(this.menu_RecheckTheSelectedKey_Click);
            // 
            // menu_RecheckInformationTheSelectedKey
            // 
            this.menu_RecheckInformationTheSelectedKey.Enabled = false;
            this.menu_RecheckInformationTheSelectedKey.Name = "menu_RecheckInformationTheSelectedKey";
            this.menu_RecheckInformationTheSelectedKey.Size = new System.Drawing.Size(378, 22);
            this.menu_RecheckInformationTheSelectedKey.Text = "Recheck information the selected key";
            this.menu_RecheckInformationTheSelectedKey.Click += new System.EventHandler(this.menu_RecheckInformationTheSelectedKey_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(375, 6);
            // 
            // menu_CopyTheSelectedKey
            // 
            this.menu_CopyTheSelectedKey.Enabled = false;
            this.menu_CopyTheSelectedKey.Name = "menu_CopyTheSelectedKey";
            this.menu_CopyTheSelectedKey.Size = new System.Drawing.Size(378, 22);
            this.menu_CopyTheSelectedKey.Text = "Copy the selected key                                            Ctrl+C";
            this.menu_CopyTheSelectedKey.Click += new System.EventHandler(this.menu_CopyTheSelectedKey_Click);
            // 
            // menu_CopyTheSelectedKeyAndTheirInformation
            // 
            this.menu_CopyTheSelectedKeyAndTheirInformation.Enabled = false;
            this.menu_CopyTheSelectedKeyAndTheirInformation.Name = "menu_CopyTheSelectedKeyAndTheirInformation";
            this.menu_CopyTheSelectedKeyAndTheirInformation.Size = new System.Drawing.Size(378, 22);
            this.menu_CopyTheSelectedKeyAndTheirInformation.Text = "Copy the selected key and their information     Ctrl+Alt+C";
            this.menu_CopyTheSelectedKeyAndTheirInformation.Click += new System.EventHandler(this.menu_CopyTheSelectedKeyAndTheirInformation_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(375, 6);
            // 
            // menu_ChangeTheSelectedKeyToKeyblock
            // 
            this.menu_ChangeTheSelectedKeyToKeyblock.Enabled = false;
            this.menu_ChangeTheSelectedKeyToKeyblock.Name = "menu_ChangeTheSelectedKeyToKeyblock";
            this.menu_ChangeTheSelectedKeyToKeyblock.Size = new System.Drawing.Size(378, 22);
            this.menu_ChangeTheSelectedKeyToKeyblock.Text = "Change the selected key to Keyblock                  Shift+Del";
            this.menu_ChangeTheSelectedKeyToKeyblock.Click += new System.EventHandler(this.menu_ChangeTheSelectedKeyToKeyblock_Click);
            // 
            // menu_DeleteTheSelectedKey
            // 
            this.menu_DeleteTheSelectedKey.Enabled = false;
            this.menu_DeleteTheSelectedKey.Name = "menu_DeleteTheSelectedKey";
            this.menu_DeleteTheSelectedKey.Size = new System.Drawing.Size(378, 22);
            this.menu_DeleteTheSelectedKey.Text = "Delete the selected key                                          Del";
            this.menu_DeleteTheSelectedKey.Click += new System.EventHandler(this.menu_DeleteTheSelectedKey_Click);
            // 
            // menu_RecoveryTheSelectedKey
            // 
            this.menu_RecoveryTheSelectedKey.Enabled = false;
            this.menu_RecoveryTheSelectedKey.Name = "menu_RecoveryTheSelectedKey";
            this.menu_RecoveryTheSelectedKey.Size = new System.Drawing.Size(378, 22);
            this.menu_RecoveryTheSelectedKey.Text = "Recovery the selected key";
            this.menu_RecoveryTheSelectedKey.Click += new System.EventHandler(this.menu_RecoveryTheSelectedKey_Click);
            // 
            // menu_SynchronizationPidkey
            // 
            this.menu_SynchronizationPidkey.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_RegisterToken,
            this.menu_UploadPidkey,
            this.menu_DownloadPidkey});
            this.menu_SynchronizationPidkey.Name = "menu_SynchronizationPidkey";
            this.menu_SynchronizationPidkey.Size = new System.Drawing.Size(142, 20);
            this.menu_SynchronizationPidkey.Text = "Synchronization Pidkey";
            // 
            // menu_RegisterToken
            // 
            this.menu_RegisterToken.Name = "menu_RegisterToken";
            this.menu_RegisterToken.Size = new System.Drawing.Size(166, 22);
            this.menu_RegisterToken.Text = "Register token";
            this.menu_RegisterToken.Click += new System.EventHandler(this.menu_RegisterToken_Click);
            // 
            // menu_UploadPidkey
            // 
            this.menu_UploadPidkey.Name = "menu_UploadPidkey";
            this.menu_UploadPidkey.Size = new System.Drawing.Size(166, 22);
            this.menu_UploadPidkey.Text = "Upload pidkey";
            this.menu_UploadPidkey.Click += new System.EventHandler(this.menu_UploadPidkey_Click);
            // 
            // menu_DownloadPidkey
            // 
            this.menu_DownloadPidkey.Name = "menu_DownloadPidkey";
            this.menu_DownloadPidkey.Size = new System.Drawing.Size(166, 22);
            this.menu_DownloadPidkey.Text = "Download pidkey";
            this.menu_DownloadPidkey.Click += new System.EventHandler(this.menu_DownloadPidkey_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(802, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Timer:";
            // 
            // lb_Timer
            // 
            this.lb_Timer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_Timer.AutoSize = true;
            this.lb_Timer.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lb_Timer.Location = new System.Drawing.Point(837, 4);
            this.lb_Timer.Name = "lb_Timer";
            this.lb_Timer.Size = new System.Drawing.Size(70, 13);
            this.lb_Timer.TabIndex = 0;
            this.lb_Timer.Text = "00:00:00:000";
            // 
            // tbx_KeySearch
            // 
            this.tbx_KeySearch.Location = new System.Drawing.Point(6, 29);
            this.tbx_KeySearch.Name = "tbx_KeySearch";
            this.tbx_KeySearch.Size = new System.Drawing.Size(312, 20);
            this.tbx_KeySearch.TabIndex = 4;
            this.tbx_KeySearch.Click += new System.EventHandler(this.tbx_KeySearch_Click);
            // 
            // cbb_Description
            // 
            this.cbb_Description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbb_Description.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_Description.FormattingEnabled = true;
            this.cbb_Description.Location = new System.Drawing.Point(520, 28);
            this.cbb_Description.Name = "cbb_Description";
            this.cbb_Description.Size = new System.Drawing.Size(394, 21);
            this.cbb_Description.TabIndex = 2;
            this.cbb_Description.SelectedIndexChanged += new System.EventHandler(this.cbb_Description_SelectedIndexChanged);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.BackgroundImage = global::SupportActivate.Properties.Resources.refresh_480px;
            this.btn_Refresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Refresh.Location = new System.Drawing.Point(442, 28);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(23, 23);
            this.btn_Refresh.TabIndex = 9;
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // btn_DeleteKey
            // 
            this.btn_DeleteKey.BackgroundImage = global::SupportActivate.Properties.Resources.delete_trash_500px;
            this.btn_DeleteKey.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_DeleteKey.Location = new System.Drawing.Point(412, 27);
            this.btn_DeleteKey.Name = "btn_DeleteKey";
            this.btn_DeleteKey.Size = new System.Drawing.Size(23, 23);
            this.btn_DeleteKey.TabIndex = 8;
            this.btn_DeleteKey.UseVisualStyleBackColor = true;
            this.btn_DeleteKey.Click += new System.EventHandler(this.btn_DeleteKey_Click);
            // 
            // btn_KeyBlock
            // 
            this.btn_KeyBlock.BackgroundImage = global::SupportActivate.Properties.Resources.unavailable_500px;
            this.btn_KeyBlock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_KeyBlock.Location = new System.Drawing.Point(383, 27);
            this.btn_KeyBlock.Name = "btn_KeyBlock";
            this.btn_KeyBlock.Size = new System.Drawing.Size(23, 23);
            this.btn_KeyBlock.TabIndex = 7;
            this.btn_KeyBlock.UseVisualStyleBackColor = true;
            this.btn_KeyBlock.Click += new System.EventHandler(this.btn_KeyBlock_Click);
            // 
            // btn_Copy
            // 
            this.btn_Copy.BackgroundImage = global::SupportActivate.Properties.Resources.copy_500px;
            this.btn_Copy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Copy.Location = new System.Drawing.Point(354, 27);
            this.btn_Copy.Name = "btn_Copy";
            this.btn_Copy.Size = new System.Drawing.Size(23, 23);
            this.btn_Copy.TabIndex = 6;
            this.btn_Copy.UseVisualStyleBackColor = true;
            this.btn_Copy.Click += new System.EventHandler(this.btn_Copy_Click);
            // 
            // btn_Search
            // 
            this.btn_Search.BackgroundImage = global::SupportActivate.Properties.Resources.search_480px;
            this.btn_Search.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Search.Location = new System.Drawing.Point(324, 27);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(23, 23);
            this.btn_Search.TabIndex = 5;
            this.btn_Search.UseVisualStyleBackColor = true;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // dgv_Key
            // 
            this.dgv_Key.AllowUserToAddRows = false;
            this.dgv_Key.AllowUserToDeleteRows = false;
            this.dgv_Key.AllowUserToResizeRows = false;
            this.dgv_Key.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_Key.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgv_Key.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_Key.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgv_Key.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Key.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.STT,
            this.Key,
            this.Description,
            this.SubType,
            this.LicenseType,
            this.MAKCount,
            this.ErrorCode,
            this.Getweb,
            this.Note});
            this.dgv_Key.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dgv_Key.Location = new System.Drawing.Point(0, 53);
            this.dgv_Key.Name = "dgv_Key";
            this.dgv_Key.RowHeadersVisible = false;
            this.dgv_Key.RowHeadersWidth = 51;
            this.dgv_Key.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv_Key.Size = new System.Drawing.Size(919, 396);
            this.dgv_Key.TabIndex = 3;
            this.dgv_Key.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_Key_CellMouseDown);
            this.dgv_Key.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Key_CellValueChanged);
            this.dgv_Key.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgv_Key_KeyDown);
            // 
            // STT
            // 
            this.STT.HeaderText = "STT";
            this.STT.MinimumWidth = 6;
            this.STT.Name = "STT";
            this.STT.ReadOnly = true;
            this.STT.Width = 40;
            // 
            // Key
            // 
            this.Key.HeaderText = "Key";
            this.Key.MinimumWidth = 6;
            this.Key.Name = "Key";
            this.Key.ReadOnly = true;
            this.Key.Width = 230;
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.MinimumWidth = 6;
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 220;
            // 
            // SubType
            // 
            this.SubType.HeaderText = "SubType";
            this.SubType.MinimumWidth = 6;
            this.SubType.Name = "SubType";
            this.SubType.ReadOnly = true;
            this.SubType.Width = 70;
            // 
            // LicenseType
            // 
            this.LicenseType.HeaderText = "LicenseType";
            this.LicenseType.MinimumWidth = 6;
            this.LicenseType.Name = "LicenseType";
            this.LicenseType.ReadOnly = true;
            this.LicenseType.Width = 73;
            // 
            // MAKCount
            // 
            this.MAKCount.HeaderText = "MAKCount";
            this.MAKCount.MinimumWidth = 6;
            this.MAKCount.Name = "MAKCount";
            this.MAKCount.ReadOnly = true;
            this.MAKCount.Width = 60;
            // 
            // ErrorCode
            // 
            this.ErrorCode.HeaderText = "ErrorCode";
            this.ErrorCode.MinimumWidth = 6;
            this.ErrorCode.Name = "ErrorCode";
            this.ErrorCode.ReadOnly = true;
            this.ErrorCode.Width = 60;
            // 
            // Getweb
            // 
            this.Getweb.HeaderText = "Getweb";
            this.Getweb.MinimumWidth = 6;
            this.Getweb.Name = "Getweb";
            this.Getweb.ReadOnly = true;
            this.Getweb.Width = 50;
            // 
            // Note
            // 
            this.Note.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Note.HeaderText = "Note";
            this.Note.MinimumWidth = 6;
            this.Note.Name = "Note";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Enabled = false;
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenu_CopyTheSelectedKey,
            this.contextMenu_CopyTheSelectedKeyAndTheirInformation,
            this.toolStripSeparator7,
            this.contextMenu_RecheckTheSelectedKey,
            this.contextMenu_RecheckInformationTheSelectedKey,
            this.toolStripSeparator8,
            this.contextMenu_DeleteTheSelectedKey,
            this.contextMenu_ChangeTheSelectedKeyToKeyBlock,
            this.contextMenu_RecoveryTheSelectedKey});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(382, 170);
            // 
            // contextMenu_CopyTheSelectedKey
            // 
            this.contextMenu_CopyTheSelectedKey.Enabled = false;
            this.contextMenu_CopyTheSelectedKey.Name = "contextMenu_CopyTheSelectedKey";
            this.contextMenu_CopyTheSelectedKey.Size = new System.Drawing.Size(381, 22);
            this.contextMenu_CopyTheSelectedKey.Text = "Copy the selected key                                             Ctrl+C";
            this.contextMenu_CopyTheSelectedKey.Click += new System.EventHandler(this.contextMenu_CopyTheSelectedKey_Click);
            // 
            // contextMenu_CopyTheSelectedKeyAndTheirInformation
            // 
            this.contextMenu_CopyTheSelectedKeyAndTheirInformation.Enabled = false;
            this.contextMenu_CopyTheSelectedKeyAndTheirInformation.Name = "contextMenu_CopyTheSelectedKeyAndTheirInformation";
            this.contextMenu_CopyTheSelectedKeyAndTheirInformation.Size = new System.Drawing.Size(381, 22);
            this.contextMenu_CopyTheSelectedKeyAndTheirInformation.Text = "Copy the selected key and their information      Ctrl+Alt+C";
            this.contextMenu_CopyTheSelectedKeyAndTheirInformation.Click += new System.EventHandler(this.contextMenu_CopyTheSelectedKeyAndTheirInformation_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(378, 6);
            // 
            // contextMenu_RecheckTheSelectedKey
            // 
            this.contextMenu_RecheckTheSelectedKey.Enabled = false;
            this.contextMenu_RecheckTheSelectedKey.Name = "contextMenu_RecheckTheSelectedKey";
            this.contextMenu_RecheckTheSelectedKey.Size = new System.Drawing.Size(381, 22);
            this.contextMenu_RecheckTheSelectedKey.Text = "Recheck the selected key";
            this.contextMenu_RecheckTheSelectedKey.Click += new System.EventHandler(this.contextMenu_RecheckTheSelectedKey_Click);
            // 
            // contextMenu_RecheckInformationTheSelectedKey
            // 
            this.contextMenu_RecheckInformationTheSelectedKey.Enabled = false;
            this.contextMenu_RecheckInformationTheSelectedKey.Name = "contextMenu_RecheckInformationTheSelectedKey";
            this.contextMenu_RecheckInformationTheSelectedKey.Size = new System.Drawing.Size(381, 22);
            this.contextMenu_RecheckInformationTheSelectedKey.Text = "Recheck information the selected key";
            this.contextMenu_RecheckInformationTheSelectedKey.Click += new System.EventHandler(this.contextMenu_RecheckInformationTheSelectedKey_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(378, 6);
            // 
            // contextMenu_DeleteTheSelectedKey
            // 
            this.contextMenu_DeleteTheSelectedKey.Enabled = false;
            this.contextMenu_DeleteTheSelectedKey.Name = "contextMenu_DeleteTheSelectedKey";
            this.contextMenu_DeleteTheSelectedKey.Size = new System.Drawing.Size(381, 22);
            this.contextMenu_DeleteTheSelectedKey.Text = "Delete the selected key                                           Del";
            this.contextMenu_DeleteTheSelectedKey.Click += new System.EventHandler(this.contextMenu_DeleteTheSelectedKey_Click);
            // 
            // contextMenu_ChangeTheSelectedKeyToKeyBlock
            // 
            this.contextMenu_ChangeTheSelectedKeyToKeyBlock.Enabled = false;
            this.contextMenu_ChangeTheSelectedKeyToKeyBlock.Name = "contextMenu_ChangeTheSelectedKeyToKeyBlock";
            this.contextMenu_ChangeTheSelectedKeyToKeyBlock.Size = new System.Drawing.Size(381, 22);
            this.contextMenu_ChangeTheSelectedKeyToKeyBlock.Text = "Change the selected key to KeyBlock                   Shift+Del";
            this.contextMenu_ChangeTheSelectedKeyToKeyBlock.Click += new System.EventHandler(this.contextMenu_ChangeTheSelectedKeyToKeyBlock_Click);
            // 
            // contextMenu_RecoveryTheSelectedKey
            // 
            this.contextMenu_RecoveryTheSelectedKey.Enabled = false;
            this.contextMenu_RecoveryTheSelectedKey.Name = "contextMenu_RecoveryTheSelectedKey";
            this.contextMenu_RecoveryTheSelectedKey.Size = new System.Drawing.Size(381, 22);
            this.contextMenu_RecoveryTheSelectedKey.Text = "Recovery the selected key";
            this.contextMenu_RecoveryTheSelectedKey.Click += new System.EventHandler(this.contextMenu_RecoveryTheSelectedKey_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DataKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 450);
            this.Controls.Add(this.dgv_Key);
            this.Controls.Add(this.btn_Refresh);
            this.Controls.Add(this.btn_DeleteKey);
            this.Controls.Add(this.btn_KeyBlock);
            this.Controls.Add(this.btn_Copy);
            this.Controls.Add(this.btn_Search);
            this.Controls.Add(this.cbb_Description);
            this.Controls.Add(this.tbx_KeySearch);
            this.Controls.Add(this.lb_Timer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DataKey";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataKey";
            this.Load += new System.EventHandler(this.DataKey_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Key)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.MenuStrip menuStrip1;
        public System.Windows.Forms.ToolStripMenuItem menu_File;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripMenuItem menu_Action;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        public System.Windows.Forms.ToolStripMenuItem menu_SynchronizationPidkey;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lb_Timer;
        public System.Windows.Forms.ToolStripMenuItem menu_SaveDBToFileText;
        public System.Windows.Forms.ToolStripMenuItem menu_BackupDatabase;
        public System.Windows.Forms.ToolStripMenuItem menu_RestoreDatabase;
        public System.Windows.Forms.ToolStripMenuItem menu_AddDatabase;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        public System.Windows.Forms.ToolStripMenuItem menu_Close;
        public System.Windows.Forms.ToolStripMenuItem menu_AddKey;
        public System.Windows.Forms.ToolStripMenuItem menu_SearchKey;
        public System.Windows.Forms.ToolStripMenuItem menu_ShowKeyBlock;
        public System.Windows.Forms.ToolStripMenuItem menu_Refresh;
        public System.Windows.Forms.ToolStripMenuItem menu_RefreshDatabase;
        public System.Windows.Forms.ToolStripMenuItem menu_RecheckTheSelectedKey;
        public System.Windows.Forms.ToolStripMenuItem menu_RecheckInformationTheSelectedKey;
        public System.Windows.Forms.ToolStripMenuItem menu_CopyTheSelectedKey;
        public System.Windows.Forms.ToolStripMenuItem menu_CopyTheSelectedKeyAndTheirInformation;
        public System.Windows.Forms.ToolStripMenuItem menu_ChangeTheSelectedKeyToKeyblock;
        public System.Windows.Forms.ToolStripMenuItem menu_DeleteTheSelectedKey;
        public System.Windows.Forms.ToolStripMenuItem menu_RecoveryTheSelectedKey;
        public System.Windows.Forms.ToolStripMenuItem menu_RegisterToken;
        public System.Windows.Forms.ToolStripMenuItem menu_UploadPidkey;
        public System.Windows.Forms.ToolStripMenuItem menu_DownloadPidkey;
        public System.Windows.Forms.TextBox tbx_KeySearch;
        public System.Windows.Forms.ComboBox cbb_Description;
        public System.Windows.Forms.Button btn_Search;
        public System.Windows.Forms.Button btn_Copy;
        public System.Windows.Forms.Button btn_KeyBlock;
        public System.Windows.Forms.Button btn_DeleteKey;
        public System.Windows.Forms.Button btn_Refresh;
        public System.Windows.Forms.DataGridView dgv_Key;
        public System.Windows.Forms.DataGridViewTextBoxColumn STT;
        public System.Windows.Forms.DataGridViewTextBoxColumn Key;
        public System.Windows.Forms.DataGridViewTextBoxColumn Description;
        public System.Windows.Forms.DataGridViewTextBoxColumn SubType;
        public System.Windows.Forms.DataGridViewTextBoxColumn LicenseType;
        public System.Windows.Forms.DataGridViewTextBoxColumn MAKCount;
        public System.Windows.Forms.DataGridViewTextBoxColumn ErrorCode;
        public System.Windows.Forms.DataGridViewTextBoxColumn Getweb;
        public System.Windows.Forms.DataGridViewTextBoxColumn Note;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        public System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public System.Windows.Forms.ToolStripMenuItem contextMenu_CopyTheSelectedKey;
        public System.Windows.Forms.ToolStripMenuItem contextMenu_CopyTheSelectedKeyAndTheirInformation;
        public System.Windows.Forms.ToolStripMenuItem contextMenu_RecheckTheSelectedKey;
        public System.Windows.Forms.ToolStripMenuItem contextMenu_RecheckInformationTheSelectedKey;
        public System.Windows.Forms.ToolStripMenuItem contextMenu_RecoveryTheSelectedKey;
        public System.Windows.Forms.ToolStripMenuItem contextMenu_DeleteTheSelectedKey;
        public System.Windows.Forms.ToolStripMenuItem contextMenu_ChangeTheSelectedKeyToKeyBlock;
    }
}