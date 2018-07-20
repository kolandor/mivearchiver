using System;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace MiveArchiver
{
    public class Archiver : IArchiver
    {
        private Thread compressing_thread;
        private Thread progress_thread;
        public void Compress(string sourceFile, string compressedFile, ProgressBar bar)
        {
            try
            {
                bar.Value = 0;
                compressing_thread = new Thread(() =>
               {
                   if (File.Exists(sourceFile))
                   {
                       using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
                       {
                           using (FileStream targetStream = File.Create(compressedFile))
                           {
                               using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                               {
                                   progress_thread = new Thread(() =>
                                   {
                                       long progress = 90;
                                       long source_length = new System.IO.FileInfo(sourceFile).Length;
                                       while (true)
                                       {
                                           if (progress > 0)
                                           {
                                               long compressed_length = new System.IO.FileInfo(compressedFile).Length;
                                               if ((source_length / progress) < compressed_length)
                                               {
                                                   bar.Value += 10;
                                                   progress -= 10;
                                               }
                                           }
                                           else
                                               break;
                                       }
                                   });
                                   progress_thread.Start();
                                   sourceStream.CopyToAsync(compressionStream).Wait();
                                   bar.Value += 10;
                                   MessageBox.Show("File has been compressed!");
                                   bar.Value = 0;
                               }
                           }
                       }
                   }
                   else
                   {
                       throw new Exception("Compressed file doesn't exist");
                   }
               });
               compressing_thread.Start();
            }
            catch (ThreadAbortException)
            {
                compressing_thread.Abort();
                compressing_thread = null;
                progress_thread.Abort();
                progress_thread = null;
                File.Delete(compressedFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bar.Value = 0;
            }
        }

        public void Decompress(string compressedFile, string targetFile, ProgressBar bar)
        {
            bar.Value = 0;
            try
            {
                new Task(() =>
                {
                    if (File.Exists(compressedFile))
                    {
                        using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
                        {
                            using (FileStream targetStream = File.Create(targetFile))
                            {
                                using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                                {
                                    new Task(() =>
                                    {
                                        long progress = 90;
                                        long compressed_length = new System.IO.FileInfo(compressedFile).Length;
                                        while (true)
                                        {
                                            if (progress > 0)
                                            {
                                                long target_length = new System.IO.FileInfo(targetFile).Length;
                                                if ((compressed_length / progress) < target_length)
                                                {
                                                    bar.Value += 10;
                                                    progress -= 10;
                                                }
                                            }
                                            else
                                                break;
                                        }
                                    }).Start();
                                    decompressionStream.CopyToAsync(targetStream).Wait();
                                    bar.Value += 10;
                                    MessageBox.Show("File has been decompressed!");
                                    bar.Value = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Compressed file doesn't exist");
                    }
                }).Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bar.Value = 0;
            }
        }
    }
}
