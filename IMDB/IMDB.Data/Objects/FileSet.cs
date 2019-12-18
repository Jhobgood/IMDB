using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IMDB.Data.Objects
{
    public class FileSet
    {
        public FileInfo titleCrew, titleEpisode, titleRating, titlePrincipals, titleAkas, titleBasics, nameBasics;

        public const string TITLE_CREW_FILE_NAME = "titleCrew.tsv";
        public const string TITLE_EPISODE_FILE_NAME = "titleEpisode.tsv";
        public const string TITLE_RATING_FILE_NAME = "titleRating.tsv";
        public const string TITLE_PRINCIPALS_FILE_NAME = "titlePrincipals.tsv";
        public const string TITLE_AKAS_FILE_NAME = "titleAkas.tsv";
        public const string TITLE_BASICS_FILE_NAME = "titleBasics.tsv";
        public const string NAME_BASICS_FILE_NAME = "nameBasics.tsv";

        public string RootDir { get; private set; }

        public FileSet(string rootDir)
        {
            RootDir = rootDir;
        }

        public bool AnyFileIsNew
        {
            get => Files.Any(f => DeliveredNew(f));
        }

        public bool DeliveredNew(FileInfo file)
        {
            if (file.LastWriteTimeUtc > DateTime.UtcNow.AddHours(-25))
            {
                return true;
            }
            return false;
        }

        private string GetFullFilePath(string fileName)
        {
            return Path.Combine(RootDir, fileName);
        }


        public List<FileInfo> Files
        {
            get
            {
                // Always get the latest state of the files
                titleCrew = new FileInfo(GetFullFilePath(TITLE_CREW_FILE_NAME));
                titleEpisode = new FileInfo(GetFullFilePath(TITLE_EPISODE_FILE_NAME));
                titleRating = new FileInfo(GetFullFilePath(TITLE_RATING_FILE_NAME));
                titlePrincipals = new FileInfo(GetFullFilePath(TITLE_PRINCIPALS_FILE_NAME));
                titleAkas = new FileInfo(GetFullFilePath(TITLE_AKAS_FILE_NAME));
                titleBasics = new FileInfo(GetFullFilePath(TITLE_BASICS_FILE_NAME));
                nameBasics = new FileInfo(GetFullFilePath(NAME_BASICS_FILE_NAME));

                return new List<FileInfo>()
                {
                    titleCrew,
                    titleEpisode,
                    titleRating,
                    titlePrincipals,
                    titleAkas,
                    titleBasics,
                    nameBasics
                };
            }
        }
    }
}
