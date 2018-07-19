using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiveArchiver
{
    interface IArchiver
    {
        void Compress(string sourceFile, string compressedFile, ProgressBar bar);

        void Decompress(string compressedFile, string targetFile, ProgressBar bar);
    }
}
