using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace IMDBDownloaderAndExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            DownloadFiles();
        }

        public static void DownloadFiles()
        {
            string localFile = @"C:\Test";
            if (!Directory.Exists(localFile))
            {
                Directory.CreateDirectory(localFile);
            }

            DirectoryInfo directorySelected = new DirectoryInfo(localFile);
            foreach (FileInfo file in directorySelected.GetFiles())
            {
                file.Delete();
            }
            WebClient Client = new WebClient();
            Client.DownloadFile("https://datasets.imdbws.com/name.basics.tsv.gz", @"C:\Test\nameBasics.tsv.gz");
            Client.DownloadFile("https://datasets.imdbws.com/title.akas.tsv.gz", @"C:\Test\titleAkas.tsv.gz");
            Client.DownloadFile("https://datasets.imdbws.com/title.basics.tsv.gz", @"C:\Test\titleBasics.tsv.gz");
            Client.DownloadFile("https://datasets.imdbws.com/title.crew.tsv.gz", @"C:\Test\titleCrew.tsv.gz");
            Client.DownloadFile("https://datasets.imdbws.com/title.episode.tsv.gz", @"C:\Test\titleEpisode.tsv.gz");
            Client.DownloadFile("https://datasets.imdbws.com/title.principals.tsv.gz", @"C:\Test\titlePrincipals.tsv.gz");
            Client.DownloadFile("https://datasets.imdbws.com/title.ratings.tsv.gz", @"C:\Test\titleRatings.gz");

            foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*.gz"))
            {
                Decompress(fileToDecompress);
                fileToDecompress.Delete();
            }
        }

        public static void Decompress(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
            }
        }
    }
}
