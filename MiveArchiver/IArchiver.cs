using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MiveArchiver.ThreadCompressFileData;

namespace MiveArchiver
{
    interface IArchiver
    {
        void CancelWork();

        void Compress(string sourceFile, string compressedFile, ProgressSet progressSet);

        void Decompress(string compressedFile, string targetFile, ProgressSet progressSet);
    }
}