using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiveArchiver
{
    public class ThreadCompressFileData
    {
        public delegate void ProgressSet(long valueToUp);

        public delegate void Finish();

        public string FileFrom { get; set; }

        public string FileTo { get; set; }

        public ProgressSet CompressProgressSet { get; set; }

        public Finish CompressProgressFinish { get; set; }
    }
}