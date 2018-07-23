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
    public interface IArchiver
    {
        void CancelWork();

        void Compress(ThreadCompressFileData threadCompressFileData);

        void Decompress(ThreadCompressFileData threadCompressFileData);
    }
}