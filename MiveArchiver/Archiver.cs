using System;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using System.Threading;
using static MiveArchiver.ThreadCompressFileData;

namespace MiveArchiver
{
    public class Archiver : IArchiver
    {
        private Thread thread;
        public void CancelWork()
        {
            if(thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }
        public void Compress(string sourceFile, string compressedFile, ProgressSet progressSet)
        {
            try
            {
                thread = new Thread(ThreadMethodCompress);
                thread.Start(new ThreadCompressFileData() { FileFrom = sourceFile, FileTo = compressedFile, CompressProgressSet = progressSet });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                progressSet(0);
            }
        }

        void ThreadMethodCompress(object compressFileData)
        {
            ThreadCompressFileData threadCompressData = (ThreadCompressFileData)compressFileData;

            try
            {
                threadCompressData.CompressProgressSet.Invoke(0);
                if (File.Exists(threadCompressData.FileFrom))
                {
                    using (FileStream sourceStream = new FileStream(threadCompressData.FileFrom, FileMode.OpenOrCreate))
                    {
                        threadCompressData.CompressProgressSet.Invoke(1);

                        using (FileStream targetStream = File.Create(threadCompressData.FileTo))
                        {
                            using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                            {
                                new Task(() =>
                                {
                                    long progress = 99;
                                    long progress_set = 1;
                                    long source_length = new System.IO.FileInfo(threadCompressData.FileFrom).Length;
                                    while (true)
                                    {
                                        if (progress > 0)
                                        {
                                            long compressed_length = new System.IO.FileInfo(threadCompressData.FileTo).Length;
                                            if ((source_length / progress) < compressed_length)
                                            {
                                                threadCompressData.CompressProgressSet.Invoke(++progress_set);
                                                --progress;
                                            }
                                        }
                                        else
                                            break;
                                    }
                                }).Start();
                                sourceStream.CopyToAsync(compressionStream).Wait();
                                threadCompressData.CompressProgressSet.Invoke(100);
                                MessageBox.Show("File has been compressed!");
                                threadCompressData.CompressProgressSet.Invoke(0);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Compressed file doesn't exist");
                }
            }
            catch(ThreadAbortException)
            {
                File.Delete(threadCompressData.FileTo);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                threadCompressData.CompressProgressSet.Invoke(0);
            }
        }

        void ThreadMethodDecompress(object compressFileData)
        {
            ThreadCompressFileData threadCompressData = (ThreadCompressFileData)compressFileData;

            try
            {
                threadCompressData.CompressProgressSet.Invoke(0);
                if (File.Exists(threadCompressData.FileFrom))
                {
                    using (FileStream sourceStream = new FileStream(threadCompressData.FileFrom, FileMode.OpenOrCreate))
                    {
                        threadCompressData.CompressProgressSet.Invoke(1);

                        using (FileStream targetStream = File.Create(threadCompressData.FileTo))
                        {
                            using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                            {
                                new Task(() =>
                                {
                                    long progress = 99;
                                    long progress_set = 1;
                                    long source_length = new System.IO.FileInfo(threadCompressData.FileFrom).Length;
                                    while (true)
                                    {
                                        if (progress > 0)
                                        {
                                            long decompressed_length = new System.IO.FileInfo(threadCompressData.FileTo).Length;
                                            if ((source_length / progress) > decompressed_length)
                                            {
                                                threadCompressData.CompressProgressSet.Invoke(++progress_set);
                                                --progress;
                                            }
                                        }
                                        else
                                            break;
                                    }
                                }).Start();
                                decompressionStream.CopyToAsync(targetStream).Wait();
                                threadCompressData.CompressProgressSet.Invoke(100);
                                MessageBox.Show("File has been decompressed!");
                                threadCompressData.CompressProgressSet.Invoke(0);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Compressed file doesn't exist");
                }
            }
            catch (ThreadAbortException)
            {
                File.Delete(threadCompressData.FileTo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                threadCompressData.CompressProgressSet.Invoke(0);
            }
        }

        public void Decompress(string compressedFile, string targetFile, ProgressSet progressSet)
        {
            try
            {
                thread = new Thread(ThreadMethodDecompress);
                thread.Start(new ThreadCompressFileData() { FileFrom = compressedFile, FileTo = targetFile, CompressProgressSet = progressSet });
                /* new Task(() =>
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
                }).Start(); */

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               progressSet(0);
            }
        }
    }
}