using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Ambiesoft;
using System.Diagnostics;
using Microsoft.Win32;
namespace defaultffp
{
    public partial class FormMain : Form
    {
        string ffpath_;
        string iniFile_;
        string IniFile
        {
            get
            {
                if(iniFile_==null)
                    iniFile_=Path.ChangeExtension(Application.ExecutablePath, "ini");
                return iniFile_;
            }
        }
        readonly string runpro_;
        public FormMain(string runpro)
        {
            runpro_=runpro;
            InitializeComponent();

            // FileInfo fi = new FileInfo(Application.ExecutablePath);
            // string inipath = Path.ChangeExtension(Application.ExecutablePath, ".ini");
            // string inipath = Path.Combine(fi.DirectoryName, fi.Name.Replace(fi.Extension, "ini"));
            Profile.GetString("option", "firefox", "", out ffpath_, IniFile);
        }

        int originalDefault_ = -1;
        int currentDefault_ = -1;
        HashIni ffini_;
        private string FFPPath
        {
            get
            {
                string inipath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                inipath += @"\Mozilla\Firefox\profiles.ini";
                return inipath;
            }
        }

        private void updateList()
        {
            if (!File.Exists(FFPPath))
            {
                MessageBox.Show(FFPPath + " not found", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            listMain.Items.Clear();
            ffini_ = Profile.ReadAll(FFPPath);
            for (int i = 0; ; ++i)
            {
                String app = "Profile" + i.ToString();
                String name;
                Profile.GetString(app, "Name", "", out name, ffini_);
                if (string.IsNullOrEmpty(name))
                    break;

                listMain.Items.Add(name);

                int ndef;
                Profile.GetInt(app, "Default", 0, out ndef, ffini_);
                if (ndef != 0)
                {
                    txtCurDef.Text = name;
                    if (originalDefault_ < 0)
                        originalDefault_ = i;
                }
            }
        }
        
        private void FormMain_Load(object sender, EventArgs e)
        {
            updateList();
            if (string.IsNullOrEmpty(runpro_))
            {
                int index;
                Profile.GetInt("option", "index", -1, out index, IniFile);
                if (0 <= index && index < listMain.Items.Count)
                {
                    listMain.SelectedIndex = index;
                }
            }
            else
            {
                int index = listMain.FindStringExact(runpro_);
                if (index < 0)
                {
                    CppUtils.Alert(string.Format(Properties.Resources.PROFILE_NOT_FOUND, runpro_));
                    Close();
                    return;
                }
                listMain.SelectedIndex = index;
                this.doSSR();
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool doWrite(int nDefault)
        {
            if (nDefault < 0)
            {
                if (listMain.SelectedIndex < 0)
                    return false;

                nDefault = listMain.SelectedIndex;
            }

            for(int i=0 ; i < listMain.Items.Count ; ++i)
            {
                String app = "Profile" + i.ToString();
                Profile.WriteInt(app, "Default", (i == nDefault ? 1 : 0), ffini_);
            }

            if (!Profile.WriteAll(ffini_, FFPPath))
                return false;

            currentDefault_ = nDefault;
            return true;

        }
        private bool doWrite()
        {
            return doWrite(-1);
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (listMain.SelectedIndex >= 0)
            {
                if (!doWrite())
                {
                    MessageBox.Show("Write failed", Application.ProductName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            Close();
        }

        private void listMain_DoubleClick(object sender, EventArgs e)
        {
            if (!doWrite())
            {
                MessageBox.Show("Write failed", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            updateList();
        }

        private void btnSRR_Click(object sender, EventArgs e)
        {
            doSSR();
        }
        internal void doSSR()
        {
            try
            {
                // this.UseWaitCursor = true;
                Cursor.Current = Cursors.WaitCursor;

                if (listMain.SelectedIndex < 0)
                    return;


                int current = currentDefault_ >= 0 ? currentDefault_ : originalDefault_;

                string filefoxpath = findFirefox();
                if (string.IsNullOrEmpty(filefoxpath))
                {
                    MessageBox.Show(Properties.Resources.FIREFOX_NOT_FOUND,
                        Application.ProductName,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                Process[] allproc = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(filefoxpath));
                List<Process> alltokill = new List<Process>();
                foreach (Process proc in allproc)
                {
                    if (0 == string.Compare(proc.MainModule.FileName, filefoxpath, true))
                    {
                        alltokill.Add(proc);
                    }
                }

                if (alltokill.Count != 0)
                {
                    DialogResult result = MessageBox.Show(Properties.Resources.FIREFOX_IS_ALREADY_RUNNING,
                        Application.ProductName,
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button3);
                    if(result==DialogResult.Cancel)
                    {
                        return;
                    }
                    if (result == DialogResult.Yes)
                    {
                        foreach (Process p in alltokill)
                        {
                            try
                            {
                                p.Kill();
                            }
                            catch (Exception)
                            { 
                            }
                        }
                    }
                }

                if (!doWrite())
                {
                    MessageBox.Show("Write failed", Application.ProductName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = filefoxpath;
                    psi.UseShellExecute = false;
                    Process firefox;
                    try
                    {
                        firefox = Process.Start(psi);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (firefox == null)
                    {
                        System.Threading.Thread.Sleep(5000);
                    }
                    else
                    {
                        firefox.WaitForInputIdle();
                    }
                }
                finally
                {
                    if (!doWrite(current))
                    {
                        MessageBox.Show("Write failed", Application.ProductName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                    }
                }
            }
            finally
            {
                // this.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
            }
            Close();
        }

        private string findFirefoxFromReg()
        {
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"Software\Mozilla\Mozilla Firefox", false);
            if (regkey == null)
                return null;

            foreach (string sub in regkey.GetSubKeyNames())
            {
                RegistryKey k = regkey.OpenSubKey(sub);
                if (k != null)
                {
                    RegistryKey main = k.OpenSubKey("Main");
                    if (main != null)
                    {
                        object o = main.GetValue("PathToExe");
                        if (o != null)
                        {
                            return o.ToString();
                        }
                    }
                }
            }
            return null;
        }
        private string findFirefox()
        {
            if (File.Exists(ffpath_))
                return ffpath_;

            string tryffpath = findFirefoxFromReg();

            //OpenFileDialogクラスのインスタンスを作成
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                
                //はじめのファイル名を指定する
                //はじめに「ファイル名」で表示される文字列を指定する
                ofd.FileName = Path.GetFileName(tryffpath) ?? "firefox.exe";
                
                //はじめに表示されるフォルダを指定する
                //指定しない（空の文字列）の時は、現在のディレクトリが表示される
                ofd.InitialDirectory = Path.GetDirectoryName(tryffpath);
                
                //[ファイルの種類]に表示される選択肢を指定する
                //指定しないとすべてのファイルが表示される
                ofd.Filter = "Firefox Executable (*.exe)|*.exe";
                
                //[ファイルの種類]ではじめに選択されるものを指定する
                //2番目の「すべてのファイル」が選択されているようにする
                //ofd.FilterIndex = 2;
                //タイトルを設定する
                ofd.Title = "Choose firefox.exe";
                
                //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
                ofd.RestoreDirectory = true;
                
                //存在しないファイルの名前が指定されたとき警告を表示する
                //デフォルトでTrueなので指定する必要はない
                ofd.CheckFileExists = true;
                
                //存在しないパスが指定されたとき警告を表示する
                //デフォルトでTrueなので指定する必要はない
                ofd.CheckPathExists = true;

                //ダイアログを表示する
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return string.Empty;
                }
                ffpath_ = ofd.FileName;
            }

            
            if (!Profile.WriteString("option", "firefox", ffpath_, IniFile))
            {
                MessageBox.Show("save failed",
                    Application.ProductName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            // return @"C:\LegacyPrograms\Mozilla Firefox 32\firefox.exe";
            // return "firefox.lnk";
            return ffpath_;
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!Profile.WriteInt("option", "index", listMain.SelectedIndex, IniFile))
            {
                MessageBox.Show("save failed",
                  Application.ProductName,
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
            }
        }


    }
}
