using System;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MiveArchiver
{
    public class Archiver : IArchiver
    {
        public void Compress(string sourceFile, string compressedFile, ProgressBar bar)
        {
            try
            {
                bar.Value = 0; 
                new Task( () =>
                {
                    if (File.Exists(sourceFile))
                    {
                        using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
                        {
                            using (FileStream targetStream = File.Create(compressedFile))
                            {
                                using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                                {

                                    new Task(() =>
                                    {
                                        long source_length = new System.IO.FileInfo(sourceFile).Length;
                                        long compressed_length = new System.IO.FileInfo(compressedFile).Length;
                                        if ((source_length / 2) > compressed_length)
                                            bar.Value += 100;
                                    }).Start();
                                    sourceStream.CopyToAsync(compressionStream).Wait();
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
                                        long compressed_length = new System.IO.FileInfo(compressedFile).Length;
                                        long target_length = new System.IO.FileInfo(targetFile).Length;
                                        if ((target_length / 2) < compressed_length)
                                            bar.Value += 100;
                                    }).Start();
                                    decompressionStream.CopyToAsync(targetStream).Wait();
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
                //bar.Value = 0;
            }
        }
    }
}
