using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MiveArchiver.Program;

namespace MiveArchiver
{
    public partial class ArchiverForm : Form
    {
        private Thread ExecutionThread;
        public ArchiverForm()
        {
            InitializeComponent();
        }

        void btnZip_Click(object sender, EventArgs e)
        {
            try
            {
                if (opdFileDialogZip.ShowDialog() == DialogResult.Cancel)
                    return;
                string path_to_file = opdFileDialogZip.FileName;

                // Filling the text-box with the path to the file to be zipped:
                tbZip.Text = path_to_file;

                // Folder Selection:
                var folder_browser_dialogue = new FolderBrowserDialog();
                if (folder_browser_dialogue.ShowDialog() == DialogResult.Cancel)
                    return;

                // Creating the zipped file:
                IArchiver arch = new Archiver();
                ExecutionThread = new Thread(()=> {
                    arch.Compress(path_to_file, $"{folder_browser_dialogue.SelectedPath}\\{opdFileDialogZip.SafeFileName}.zip", progressBar);
                });
                ExecutionThread.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.StackTrace, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void btnUnzip_Click(object sender, EventArgs e)
        {
            try
            {
                if (opdFileDialogUnzip.ShowDialog() == DialogResult.Cancel)
                    return;
                string path_to_file = opdFileDialogUnzip.FileName;

                // Filling the text-box with the path to the file to be unzipped:
                tbUnzip.Text = path_to_file;

                // Folder Selection:
                var folder_browser_dialogue = new FolderBrowserDialog();
                if (folder_browser_dialogue.ShowDialog() == DialogResult.Cancel)
                    return;
                string new_name = Path.GetFileNameWithoutExtension(Path.Combine(folder_browser_dialogue.SelectedPath, opdFileDialogUnzip.SafeFileName));

                // Creating the unzipped file:
                IArchiver arch = new Archiver();
                arch.Decompress(path_to_file, Path.Combine(folder_browser_dialogue.SelectedPath, new_name), progressBar);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.StackTrace, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            if (ExecutionThread != null)
            {
                ExecutionThread.Abort();
                ExecutionThread = null;
            }
            progressBar.Value = 0;
        }

        private void ArchiverForm_Load(object sender, EventArgs e)
        {

        }
    }
}
