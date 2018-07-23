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
using static MiveArchiver.ThreadCompressFileData;

namespace MiveArchiver
{
    public partial class ArchiverForm : Form
    {
        public ArchiverForm()
        {
            InitializeComponent();
        }
        
        void btnZip_Click(object sender, EventArgs e)
        {
            ZipProcessForm zipProcess = new ZipProcessForm(UserZipAction.Zip);

            if (zipProcess != null)
            {
                zipProcess.Show();
            }
        }

        void btnUnzip_Click(object sender, EventArgs e)
        {
            ZipProcessForm zipProcess = new ZipProcessForm(UserZipAction.Unzip);

            if (zipProcess != null)
            {
                zipProcess.Show();
            }
        }
    }
}
