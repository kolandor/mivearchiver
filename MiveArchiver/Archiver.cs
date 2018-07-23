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
        private Thread archiverThread;
        public void CancelWork()
        {
            if(archiverThread != null)
            {
                archiverThread.Abort();
                archiverThread = null;
            }
        }
        public void Compress(ThreadCompressFileData threadCompressFileData)
        {
            try
            {
                archiverThread = new Thread(ThreadMethodCompress);
                archiverThread.Start(threadCompressFileData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                threadCompressFileData.CompressProgressSet.Invoke(0);
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
                                    long progressSet = 1;
                                    long sourceLength = new System.IO.FileInfo(threadCompressData.FileFrom).Length;
                                    while (true)
                                    {
                                        if (progress > 0)
                                        {
                                            long compressedLength = new System.IO.FileInfo(threadCompressData.FileTo).Length;
                                            if ((sourceLength / progress) < compressedLength)
                                            {
                                                threadCompressData.CompressProgressSet.Invoke(++progressSet);
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
                threadCompressData.CompressProgressFinish.Invoke();
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
                                    long progressSet = 1;
                                    long sourceLength = new System.IO.FileInfo(threadCompressData.FileFrom).Length;
                                    while (true)
                                    {
                                        if (progress > 0)
                                        {
                                            long decompressedLength = new System.IO.FileInfo(threadCompressData.FileTo).Length;
                                            if ((sourceLength / progress) > decompressedLength)
                                            {
                                                threadCompressData.CompressProgressSet.Invoke(++progressSet);
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
                threadCompressData.CompressProgressFinish.Invoke();
            }
        }

        public void Decompress(ThreadCompressFileData threadCompressFileData)
        {
            try
            {
                archiverThread = new Thread(ThreadMethodDecompress);
                archiverThread.Start(threadCompressFileData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                threadCompressFileData.CompressProgressSet.Invoke(0);
            }
        }
    }
}