using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MiveArchiver.ThreadCompressFileData;

namespace MiveArchiver
{
    public partial class ZipProcessForm : Form
    {
        OpenFileDialog opdFileDialogZip;
        FolderBrowserDialog folderBrowserDialogue;

        private IArchiver arch;
        private string pathToTargetFile;
        private string pathToNewFile;
        UserZipAction zipAction;

        public ZipProcessForm(UserZipAction userAction)
        {
            InitializeComponent();

            zipAction = userAction;
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            arch.CancelWork();
            progressBar.Value = 0;
            this.Close();
        }

        public void SetBarValue(long set_value)
        {
            if (set_value >= 0 && set_value <= 100)
                progressBar.Value = (int)set_value;
        }

        void Zip()
        {
            try
            {
                opdFileDialogZip = new OpenFileDialog();

                if (opdFileDialogZip.ShowDialog() == DialogResult.Cancel)
                    throw new CancelException();

                pathToTargetFile = opdFileDialogZip.FileName;

                // Filling the text-box with the path to the file to be zipped:
                labelZipFile.Text = pathToTargetFile;

                // Folder Selection:
                folderBrowserDialogue = new FolderBrowserDialog();
                if (folderBrowserDialogue.ShowDialog() == DialogResult.Cancel)
                    throw new CancelException();

                pathToNewFile = $"{folderBrowserDialogue.SelectedPath}\\{opdFileDialogZip.SafeFileName}.zip";

                // Creating the zipped file:
                arch = new Archiver();

                arch.Compress(new ThreadCompressFileData() { FileFrom = pathToTargetFile, FileTo = pathToNewFile, CompressProgressSet = SetBarValue, CompressProgressFinish = FinishAction });
            }
            catch (CancelException)
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.StackTrace, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Close();
            }
        }

        private void FinishAction()
        {
            this.Close();
        }

        void Unzip()
        {
            try
            {
                opdFileDialogZip = new OpenFileDialog();

                if (opdFileDialogZip.ShowDialog() == DialogResult.Cancel)
                    throw new CancelException();

                pathToTargetFile = opdFileDialogZip.FileName;

                // Filling the text-box with the path to the file to be zipped:
                labelZipFile.Text = pathToTargetFile;

                // Folder Selection:
                folderBrowserDialogue = new FolderBrowserDialog();
                if (folderBrowserDialogue.ShowDialog() == DialogResult.Cancel)
                    throw new CancelException();

                string newName = Path.GetFileNameWithoutExtension(Path.Combine(folderBrowserDialogue.SelectedPath, opdFileDialogZip.SafeFileName));

                pathToNewFile = Path.Combine(folderBrowserDialogue.SelectedPath, newName);

                // Creating the unzipped file:
                IArchiver arch = new Archiver();

                arch.Decompress(new ThreadCompressFileData() { FileFrom = pathToTargetFile, FileTo = pathToNewFile, CompressProgressSet = SetBarValue, CompressProgressFinish = FinishAction });
            }
            catch (CancelException)
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.StackTrace, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ZipProcessForm_Shown(object sender, EventArgs e)
        {
            if (zipAction == UserZipAction.Zip)
            {
                Zip();
            }
            else if (zipAction == UserZipAction.Unzip)
            {
                Unzip();
            }
        }
    }
}
