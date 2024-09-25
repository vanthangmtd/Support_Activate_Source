using SupportActivate.Common;
using SupportActivate.ProcessTabControl;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SupportActivate.FormWindows
{
    public partial class DataKey : Form
    {
        public static DataKey formDataKey;
        ProcessDataKey processDataKey;

        public string BoxDescription;
        public string optionVerKey;
        public string selectVerKey, keySearch;
        //public List<dataKey> listKey = new List<dataKey>();
        public string optionKey, optionCbx;
        public DateTime dateTimeCheckKey;
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DataKey));

        public DataKey()
        {
            InitializeComponent();
            formDataKey = this;
            processDataKey = new ProcessDataKey();
        }

        private void DataKey_Load(object sender, EventArgs e)
        {
            try
            {
                label1.Hide();
                lb_Timer.Hide();
                new Thread(() =>
                {
                    processDataKey.tongKey();
                    processDataKey.loadAllDescription();
                })
                { IsBackground = true }.Start();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void cbb_Description_SelectedIndexChanged(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                processDataKey.loadDataKeyCurrent();
            })
            { IsBackground = true }.Start();
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                processDataKey.loadDataKeyCurrent();
            })
            { IsBackground = true }.Start();
        }

        private void dgv_Key_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            var rowIndexes = dgv_Key.SelectedCells.Cast<DataGridViewCell>().Where(x => x.RowIndex == e.RowIndex)
                                  .Select(cell => cell.RowIndex)
                                  .ToList();
            if (e.Button == MouseButtons.Right & rowIndexes.Count() > 0)
            {
                bool recoverKey = false;
                DataGridViewSelectedCellCollection selectedCells = formDataKey.dgv_Key.SelectedCells;
                foreach (DataGridViewCell row in selectedCells)
                {
                    var selectedRow = row.RowIndex;
                    recoverKey = dgv_Key[5, selectedRow].Value.ToString() == "-2" ? true :
                                 dgv_Key[6, selectedRow].Value.ToString() == ContantResource.KeyRetaiBlock ? true : false;
                }

                contextMenuStrip1.Enabled = true;
                contextMenu_CopyTheSelectedKey.Enabled = true;
                contextMenu_CopyTheSelectedKeyAndTheirInformation.Enabled = true;
                contextMenu_RecheckTheSelectedKey.Enabled = true;
                contextMenu_RecheckInformationTheSelectedKey.Enabled = true;
                contextMenu_DeleteTheSelectedKey.Enabled = true;
                if (recoverKey == true)
                {
                    contextMenu_RecoveryTheSelectedKey.Enabled = true;
                    menu_RecoveryTheSelectedKey.Enabled = true;
                }
                else
                {
                    contextMenu_RecoveryTheSelectedKey.Enabled = false;
                    menu_RecoveryTheSelectedKey.Enabled = false;
                }
                contextMenu_ChangeTheSelectedKeyToKeyBlock.Enabled = true;
                menu_RecheckTheSelectedKey.Enabled = true;
                menu_RecheckInformationTheSelectedKey.Enabled = true;
                menu_ChangeTheSelectedKeyToKeyblock.Enabled = true;
                menu_CopyTheSelectedKey.Enabled = true;
                menu_CopyTheSelectedKeyAndTheirInformation.Enabled = true;
                menu_DeleteTheSelectedKey.Enabled = true;
                btn_Copy.Enabled = true;
                btn_DeleteKey.Enabled = true;
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }


        private void dgv_Key_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgv_Key.RowCount > 0)
                {
                    DataGridViewRow dgvRow = dgv_Key.CurrentRow;
                    processDataKey.addNoteDataKey(dgvRow.Cells[1].Value.ToString(), dgvRow.Cells[8].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                MessageBox.Show(MessagesResource.AddNoteError, MessagesResource.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_Key_KeyDown(object sender, KeyEventArgs e)
        {
            switch (true)
            {
                case true when e.Control & e.Alt & e.KeyCode == Keys.C:
                    var threadCopyInforKey = new Thread(() =>
                    {
                        processDataKey.copyInforKey();
                    });
                    threadCopyInforKey.TrySetApartmentState(ApartmentState.STA);
                    threadCopyInforKey.IsBackground = true;
                    threadCopyInforKey.Start();
                    break;
                case true when e.Control & e.KeyCode == Keys.F:
                    tbx_KeySearch.Clear();
                    tbx_KeySearch.Focus();
                    break;
                case true when e.Control & e.KeyCode == Keys.C:
                    processDataKey.getValuesDgvKey();
                    break;
                /*case true when e.Control & e.KeyCode == Keys.A:
                    processDataKey.selectAllDgvKey();
                    break;*/
                case true when e.Shift & e.KeyCode == Keys.Delete:
                    new Thread(() => { processDataKey.changeToKeyBlock(); }) { IsBackground = true }.Start();
                    break;
                case true when e.KeyCode == Keys.Delete:
                    new Thread(() => { processDataKey.deleteDgvKey(); }) { IsBackground = true }.Start();
                    break;
                case true when e.KeyCode == Keys.F5:
                    new Thread(() => { processDataKey.loadDataKeyCurrent(); }) { IsBackground = true }.Start();
                    break;
                case true when e.Alt & e.KeyCode == Keys.F4:
                    DataKey.ActiveForm.Close();
                    break;
                case true when e.KeyCode == Keys.Enter | e.KeyCode == Keys.Return:
                    new Thread(() => { processDataKey.searchDataKey(); }) { IsBackground = true }.Start();
                    break;
            }
        }

        private void contextMenu_CopyTheSelectedKey_Click(object sender, EventArgs e)
        {
            btn_Copy_Click(sender, e);
        }

        private void contextMenu_CopyTheSelectedKeyAndTheirInformation_Click(object sender, EventArgs e)
        {
            var threadCopyInforKey = new Thread(() =>
            {
                processDataKey.copyInforKey();
            });
            threadCopyInforKey.TrySetApartmentState(ApartmentState.STA);
            threadCopyInforKey.IsBackground = true;
            threadCopyInforKey.Start();
        }

        private void contextMenu_RecheckTheSelectedKey_Click(object sender, EventArgs e)
        {
            processDataKey.checkKey();
        }

        private void contextMenu_RecheckInformationTheSelectedKey_Click(object sender, EventArgs e)
        {
            btn_Search.Enabled = false;
            btn_Copy.Enabled = false;
            btn_KeyBlock.Enabled = false;
            btn_DeleteKey.Enabled = false;
            btn_Refresh.Enabled = false;
            cbb_Description.Enabled = false;
            dgv_Key.Enabled = false;
            menuStrip1.Enabled = false;
            label1.Show();
            lb_Timer.Show();
            lb_Timer.Text = "00:00:00:000";
            dateTimeCheckKey = DateTime.Now;
            processDataKey.reCheckInforDataKey();
        }

        private void contextMenu_DeleteTheSelectedKey_Click(object sender, EventArgs e)
        {
            btn_DeleteKey_Click(sender, e);
        }

        private void contextMenu_ChangeTheSelectedKeyToKeyBlock_Click(object sender, EventArgs e)
        {
            var ask = MessageBox.Show(MessagesResource.ChangeKeyToKeyBlocked, MessagesResource.success, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask == DialogResult.Yes)
                new Thread(() => { processDataKey.changeToKeyBlock(); }) { IsBackground = true }.Start();
        }

        private void contextMenu_RecoveryTheSelectedKey_Click(object sender, EventArgs e)
        {
            var ask = MessageBox.Show(MessagesResource.RecoverKey, MessagesResource.success, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask == DialogResult.Yes)
                new Thread(() => { processDataKey.recoveryKeyBlock(); }) { IsBackground = true }.Start();
        }

        private void menu_SaveDBToFileText_Click(object sender, EventArgs e)
        {
            var threadSave = new Thread(() =>
            {
                processDataKey.saveKey();
            });
            threadSave.TrySetApartmentState(ApartmentState.STA);
            threadSave.IsBackground = true;
            threadSave.Start();
        }

        private void menu_BackupDatabase_Click(object sender, EventArgs e)
        {
            string part = Application.StartupPath + @"\pkeyconfig\data_new.db";
            if (File.Exists(part) == true)
            {
                using (SaveFileDialog SaveFileDialog1 = new SaveFileDialog())
                {
                    SaveFileDialog1.Filter = "SQLite Files (*.db)|*.db";
                    if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string file = SaveFileDialog1.FileName;
                        File.Copy(part, file, true);
                        MessageBox.Show("Backup success", MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void menu_RestoreDatabase_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "SQLite Files (*.db)|*.db";
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                fileName = dialog.FileName;
                var ask = MessageBox.Show("Do you want to restore databse?", MessagesResource.success, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask == DialogResult.Yes)
                {
                    new Thread(() => { processDataKey.readDatabaseBK(fileName); }) { IsBackground = true }.Start();
                }
            }
        }

        private void menu_AddDatabase_Click(object sender, EventArgs e)
        {
            var ask = MessageBox.Show("The application can only read the database of PIDKey Lite!\r\nDo you want to add a product key?", MessagesResource.success, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask == DialogResult.Yes)
            {
                string fileName = string.Empty;
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;
                    fileName = dialog.FileName;
                }
                new Thread(() =>
                {
                    processDataKey.readDatabasePIDKeyLite(fileName);
                })
                { IsBackground = true }.Start();
            }
        }

        private void menu_Close_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
        }

        private void menu_AddKey_Click(object sender, EventArgs e)
        {
            AddKey addProductKey = new AddKey();
            addProductKey.ShowDialog();
        }

        private void menu_SearchKey_Click(object sender, EventArgs e)
        {
            tbx_KeySearch.Clear();
            tbx_KeySearch.Focus();
        }

        private void menu_Refresh_Click(object sender, EventArgs e)
        {
            new Thread(() => { processDataKey.loadDataKeyCurrent(); }) { IsBackground = true }.Start();
        }

        private void menu_RecheckTheSelectedKey_Click(object sender, EventArgs e)
        {
            contextMenu_RecheckTheSelectedKey_Click(sender, e);
        }

        private void menu_RecheckInformationTheSelectedKey_Click(object sender, EventArgs e)
        {
            contextMenu_RecheckInformationTheSelectedKey_Click(sender, e);
        }

        private void menu_ChangeTheSelectedKeyToKeyblock_Click(object sender, EventArgs e)
        {
            contextMenu_ChangeTheSelectedKeyToKeyBlock_Click(sender, e);
        }

        private void menu_ShowKeyBlock_Click(object sender, EventArgs e)
        {
            cbb_Description.SelectedItem = "Select Version Key";
            new Thread(() => { processDataKey.loadAllDataKeyBlock(); }) { IsBackground = true }.Start();
        }

        private void menu_DeleteTheSelectedKey_Click(object sender, EventArgs e)
        {
            btn_DeleteKey_Click(sender, e);
        }

        private void menu_RecoveryTheSelectedKey_Click(object sender, EventArgs e)
        {
            contextMenu_RecoveryTheSelectedKey_Click(sender, e);
        }

        private void menu_RefreshDatabase_Click(object sender, EventArgs e)
        {
            var ask = MessageBox.Show("Do you want to refresh database?", MessagesResource.success, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask == DialogResult.Yes)
            {
                new Thread(() => { processDataKey.refreshDatabase(); }) { IsBackground = true }.Start();
            }
        }

        private void menu_CopyTheSelectedKey_Click(object sender, EventArgs e)
        {
            btn_Copy_Click(sender, e);
        }

        private void menu_CopyTheSelectedKeyAndTheirInformation_Click(object sender, EventArgs e)
        {
            contextMenu_CopyTheSelectedKeyAndTheirInformation_Click(sender, e);
        }

        private void menu_RegisterToken_Click(object sender, EventArgs e)
        {
            Register_token register_token = new Register_token();
            register_token.ShowDialog();
        }

        private void menu_UploadPidkey_Click(object sender, EventArgs e)
        {
            Up_syn_pidkey up_synpidkey = new Up_syn_pidkey();
            up_synpidkey.ShowDialog();
        }

        private void menu_DownloadPidkey_Click(object sender, EventArgs e)
        {
            Down_syn_pidkey down_synpidkey = new Down_syn_pidkey();
            down_synpidkey.ShowDialog();
        }

        private void tbx_KeySearch_Click(object sender, EventArgs e)
        {
            string key = Clipboard.GetText();
            if (key.Length == 29)
                tbx_KeySearch.Clear();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            new Thread(() => { processDataKey.searchDataKey(); }) { IsBackground = true }.Start();
        }

        private void btn_Copy_Click(object sender, EventArgs e)
        {
            processDataKey.getValuesDgvKey();
        }

        private void btn_KeyBlock_Click(object sender, EventArgs e)
        {
            EditDataKeyBlock editDataKeyBlock = new EditDataKeyBlock();
            editDataKeyBlock.ShowDialog();
        }

        private void btn_DeleteKey_Click(object sender, EventArgs e)
        {
            var ask = MessageBox.Show(MessagesResource.DeleteKey, MessagesResource.success, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask == DialogResult.Yes)
                new Thread(() => { processDataKey.deleteDgvKey(); }) { IsBackground = true }.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan diff = DateTime.Now.Subtract(dateTimeCheckKey);
            lb_Timer.Text = diff.ToString(@"hh\:mm\:ss\:fff");
        }
    }
}
